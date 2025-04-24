using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public Transform GetSpawnPoint(int index)
    {
        if (index < 0 || index >= spawnPoints.Length)
        {
            Debug.LogWarning($"Spawn index {index} is out of range. Returning default spawn point.");
            return spawnPoints[0];
        }

        return spawnPoints[index];
    }
}
