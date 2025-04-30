using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "Character";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_START_GAME = "StartGame";

    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;
    public event EventHandler<LobbyEventArgs> OnGameStarted;

    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    public ScoreManager scoreManager;
    public Scoreboard scoreboard;

    public enum GameMode
    {
        CaptureTheFlag,
        Conquest
    }

    public enum PlayerCharacter
    {
        Marine,
        Ninja,
        Zombie
    }

    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private Lobby joinedLobby;
    private string playerName;
    public NetworkVariable<int> playersReady = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private async void Awake()
    {
        Debug.Log(playersReady);
        Instance = this;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Maybe remove this from here:
        // await UnityServices.InitializeAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
        HandleRefreshLobbyList(); // This is defined but never called
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayersReadyServerRpc(int num)
    {
        playersReady.Value += num;
        Debug.Log("add one to playerReady");
    }


    public async void Authenticate(string playerName)
    {
        if (string.IsNullOrEmpty(playerName) || playerName.Length > 30)
        {
            Debug.LogError("Invalid player name. It must not be empty and must be 30 characters or less.");
            return; // Exit if the player name is invalid
        }

        string validPlayerName = new string(playerName.Where(c => Char.IsLetterOrDigit(c) || c == '-' || c == '_').ToArray());

        if (validPlayerName.Length != playerName.Length)
        {
            Debug.LogWarning("Player name contained invalid characters. They have been removed.");
        }

        this.playerName = validPlayerName;

        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(validPlayerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
            RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void HandleRefreshLobbyList()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn)
        {
            refreshLobbyListTimer -= Time.deltaTime;
            if (refreshLobbyListTimer < 0f)
            {
                float refreshLobbyListTimerMax = 5f;
                refreshLobbyListTimer = refreshLobbyListTimerMax;

                RefreshLobbyList();
            }
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }

        }
    }

    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                if (!IsPlayerInLobby())
                {
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                    joinedLobby = null;
                }
                if (joinedLobby.Data.TryGetValue(KEY_START_GAME, out var startGameData))
                {
                    string relayCode = startGameData.Value;

                    if (relayCode != "0")
                    {
                        if (!IsLobbyHost())
                        {
                            Debug.Log("Client detected game start, joining Relay with code: " + relayCode);
                            RelayTest.Instance.JoinRelay(relayCode);
                        }

                        joinedLobby = null;
                        OnGameStarted?.Invoke(this, new LobbyEventArgs { lobby = null });
                    }
                }

            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            { KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerCharacter.Marine.ToString()) }
        });
    }

    public void ChangeGameMode()
    {
        if (IsLobbyHost())
        {
            GameMode gameMode = Enum.Parse<GameMode>(joinedLobby.Data[KEY_GAME_MODE].Value);

            switch (gameMode)
            {
                default:
                case GameMode.CaptureTheFlag:
                    gameMode = GameMode.Conquest;
                    break;
                case GameMode.Conquest:
                    gameMode = GameMode.CaptureTheFlag;
                    break;
            }

            UpdateLobbyGameMode(gameMode);
        }
    }

    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate, GameMode gameMode)
    {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject> {
                { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString()) },
                { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0" ) }
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

        Debug.Log("Created Lobby " + lobby.Name);
        PlayerInfo player1 = new PlayerInfo();
        player1.SetName(playerName);
        Debug.Log(player1.GetName() + "create lobby area");
        scoreManager.AddPlayer(player1);
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            options.Filters = new List<QueryFilter> {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            options.Order = new List<QueryOrder> {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        Player player = GetPlayer();

        Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions
        {
            Player = player
        });

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async Task JoinLobby(Lobby lobby)
    {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
        {
            Player = player
        });

        Debug.Log("Joined Lobby: " + (joinedLobby != null ? joinedLobby.Id : "null"));

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async void UpdatePlayerName(string playerName)
    {
        this.playerName = playerName;

        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_NAME, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerName)
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void UpdatePlayerCharacter(PlayerCharacter playerCharacter)
    {
        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_CHARACTER, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerCharacter.ToString())
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            joinedLobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void UpdateLobbyGameMode(GameMode gameMode)
    {
        try
        {
            Debug.Log("UpdateLobbyGameMode " + gameMode);

            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString()) }
            }
            });

            joinedLobby = lobby;
            OnLobbyGameModeChanged?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to update lobby game mode: {e}");
        }
    }


    public async void StartGame()
    {
        if (!IsLobbyHost()) return;
        scoreManager.AddToScoreBoard();

        try
        {
            Debug.Log("Starting game...");

            // Create Relay Server
            string relayCode = await RelayTest.Instance.CreateRelay();

            if (string.IsNullOrEmpty(relayCode))
            {
                Debug.LogError("Failed to create relay.");
                return;
            }

            // Update Lobby with Relay Code
            if (joinedLobby != null && !string.IsNullOrEmpty(joinedLobby.Id))
            {
                // Only update if the visibility has not been set yet
                // Alternatively, remove or modify the data on initial lobby creation to avoid visibility issues
                var dataObject = new DataObject(DataObject.VisibilityOptions.Member, relayCode);
                var lobbyData = new Dictionary<string, DataObject>
            {
                { KEY_START_GAME, dataObject }
            };

                // Update the lobby with the relay code (keeping the same visibility)
                Lobby lobby = await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = lobbyData
                });

                while(!playersReady.Equals(1))
                {
                    Debug.Log("waiting for ready up, " + playersReady.Value + " players ready");
                    await Task.Delay(1000);
                }
                Debug.Log("starting host proccess");
                joinedLobby = lobby;
                Debug.Log("Lobby updated with relay code. Game starting...");

                // Start the host and load the game scene
                NetworkManager.Singleton.StartHost();

                // Add a short delay before loading the scene (sometimes network initialization can take time)
                await Task.Delay(1000); // Adjust as needed, can be a short delay to let the host start

                Debug.Log("Host started, now loading scene...");
                NetworkManager.Singleton.SceneManager.LoadScene("Actual merge scene", LoadSceneMode.Single);



            }
            else
            {
                Debug.LogError("Lobby is null or invalid.");
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error starting game: " + e.Message);
        }
    }





}