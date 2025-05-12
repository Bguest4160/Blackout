using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance;

    public List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Transform GetSpawnPoint(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
            return spawnPoints[index];

        Debug.LogWarning("Spawn index out of bounds. Using default.");
        return spawnPoints[0]; // fallback
    }
}
