using Unity.Netcode;
using Unity.Netcode.Components;  // Correct namespace for ClientNetworkTransform
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class SpawnManager : NetworkBehaviour
{
    private List<Transform> spawnPoints = new();
    private int nextSpawnIndex = 0;

    private void Awake()
    {
        // Get all child transforms as spawn points
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Wait until the player's object is ready
        StartCoroutine(AssignSpawn(clientId));
    }

    private IEnumerator AssignSpawn(ulong clientId)
    {
        // Wait one frame for the player object to be fully spawned
        yield return null;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            Debug.LogWarning($"Client {clientId} not found.");
            yield break;
        }

        NetworkObject playerObject = client.PlayerObject;
        if (playerObject == null)
        {
            Debug.LogWarning("Player object is null.");
            yield break;
        }

        if (nextSpawnIndex >= spawnPoints.Count)
        {
            Debug.LogWarning("No available spawn points!");
            yield break;
        }

        Transform spawnPoint = spawnPoints[nextSpawnIndex++];
        var netTransform = playerObject.GetComponent<ClientNetworkTransform>();

        if (netTransform != null)
        {
//            netTransform.Teleport(spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            playerObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            Debug.LogWarning("ClientNetworkTransform not found on player — teleport may not sync correctly.");
        }
    }

    private void OnDestroy()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
