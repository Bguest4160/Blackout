using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

public class RelayTest : MonoBehaviour
{
    public LobbyManager lobbyManager;

    // Singleton Instance
    public static RelayTest Instance { get; private set; }

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Relay Created. Join Code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            //NetworkManager.Singleton.StartHost();

            //  This is the critical line that ensures scene sync for all clients
            //NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error in CreateRelay: " + e.Message);
        }
        return null;
    }

    public async Task JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("JoinRelay with code: " + joinCode);

            if (string.IsNullOrEmpty(joinCode) || joinCode.Length != 6)
            {
                Debug.LogError("JoinRelay failed: Invalid join code format");
                return;
            }

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();

            Debug.Log("Successfully joined relay, waiting for host to load scene...");
            lobbyManager.SetPlayersReadyClientrRpc(1);
            Debug.Log("runs server rpc method");
        }
        catch (Exception e)
        {
            Debug.LogError("Error in JoinRelay: " + e);
        }
    }
}
