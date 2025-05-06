using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private Button joinButton;

    private Lobby lobby;

    private void Awake()
    {
        if (joinButton == null)
        {
            Debug.LogError("Join button not assigned in the inspector!");
            return;
        }

        joinButton.onClick.AddListener(() => {
            joinButton.interactable = false;
            JoinLobbyAsync();
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;
    }

    private async void JoinLobbyAsync()
    {
        try
        {
            // Call the JoinLobby method from LobbyManager
            await LobbyManager.Instance.JoinLobby(lobby);

            // Handle success, enable button if needed or show a success message
            Debug.Log("Successfully joined the lobby!");

        }
        catch (System.Exception e)
        {
            // Handle failure, show error message or retry logic
            Debug.LogError("Failed to join lobby: " + e.Message);

            // Optionally, enable the button again after failure
            joinButton.interactable = true;
        }
    }
}