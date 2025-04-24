using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkVariable<int> SpawnIndex = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner && IsClient)
        {
            Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(SpawnIndex.Value);
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }

    // Call this method when the player selects their spawn point
    public void SetSpawnIndex(int index)
    {
        if (IsOwner)
        {
            SpawnIndex.Value = index;
        }
    }
}
