using Unity.Netcode;  // Add this to use Netcode features
using UnityEngine;

public class humanSpawner : NetworkBehaviour
{
    public GameObject playerPrefab;  // Your player prefab
    public Transform spawnPoint;     // Spawn location for the player

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnStartHost()  // This is called when the server starts
    {
     
        SpawnPlayer();
    }

    public void OnStartClient()  // This is called when the server starts
    {

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Instantiate and spawn the player
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        // Ensure the player is networked
        player.GetComponent<NetworkObject>().Spawn();
    }
}
