using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab; // Ensure this is assigned in Inspector
    [SerializeField] private List<Transform> spawnPoints;

    private int nextSpawnIndex = 0;

    public static HostManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += HandleClientConnected;
        }
    }

    private void OnDestroy()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");

        // Get the next spawn point for the client
        Transform spawnPoint = GetNextSpawnPoint();

        // Spawn the player on the server
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject playerNetworkObject = playerInstance.GetComponent<NetworkObject>();

        if (playerNetworkObject != null)
        {
            // Spawn the player object as a networked player object
            playerNetworkObject.SpawnAsPlayerObject(clientId, true);

            // Now, change the ownership to the client
            playerNetworkObject.ChangeOwnership(clientId);

            // Get the PlayerSpawner component and set the spawn index for the client
            PlayerSpawner playerSpawner = playerInstance.GetComponent<PlayerSpawner>();
            if (playerSpawner != null)
            {
                playerSpawner.SetSpawnIndex(nextSpawnIndex); // This could be player-selected
            }
        }
        else
        {
            Debug.LogError("Player prefab is missing NetworkObject component.");
        }
    }

    private Transform GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned in HostManager.");
            return new GameObject("FallbackSpawnPoint").transform;
        }

        Transform spawn = spawnPoints[nextSpawnIndex];
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Count;
        return spawn;
    }
}
