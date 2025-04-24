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

        // Handle spawn logic here
        Transform spawnPoint = GetNextSpawnPoint();

        // Instantiate the player prefab and assign it the spawn position
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject playerNetworkObject = playerInstance.GetComponent<NetworkObject>();

        // Make sure the NetworkObject is assigned and spawning correctly
        if (playerNetworkObject != null)
        {
            // Spawn the player object as a networked player object
            playerNetworkObject.SpawnAsPlayerObject(clientId, true);

            // Get PlayerSpawner and set the spawn index from a possible player selection
            PlayerSpawner playerSpawner = playerInstance.GetComponent<PlayerSpawner>();
            if (playerSpawner != null)
            {
                // Set the spawn index according to player selection
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
