using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : MonoBehaviour
{
    void Start()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            // Try to start as Host
            bool startedHost = NetworkManager.Singleton.StartHost();

            // If Host fails, start as Client
            if (!startedHost)
            {
                Debug.Log("Host failed, starting as Client...");
                NetworkManager.Singleton.StartClient();
            }
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log($"Client {clientId} connected.");

            // Find the player object
            NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            if (playerObject != null)
            {
                SetupPlayer(playerObject.gameObject, clientId);
            }
        }
    }

    private void SetupPlayer(GameObject player, ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                player.name = "Host_Player";
                player.GetComponent<MeshRenderer>().material.color = Color.red; // Example visual change
            }
            else
            {
                player.name = "Client_Player";
                player.GetComponent<MeshRenderer>().material.color = Color.blue; // Example visual change
            }
        }
    }
}
