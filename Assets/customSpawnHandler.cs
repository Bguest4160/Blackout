using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSpawnHandler : MonoBehaviour
{
    private Dictionary<ulong, int> assignedSpawns = new();

    private void OnEnable()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoadComplete;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoadComplete;
    }

    private void OnSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        if (!assignedSpawns.ContainsKey(clientId))
        {
            int spawnIndex = assignedSpawns.Count % SpawnPointManager.Instance.spawnPoints.Count;
            assignedSpawns[clientId] = spawnIndex;
        }

        int index = assignedSpawns[clientId];
        Transform spawnPoint = SpawnPointManager.Instance.GetSpawnPoint(index);

        GameObject playerPrefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
