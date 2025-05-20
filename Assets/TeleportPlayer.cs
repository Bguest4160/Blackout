using Unity.Netcode;
using UnityEngine;

public class PlayerAutoTeleport : NetworkBehaviour
{
    [SerializeField] private string spawnPointName = "SpawnPoint"; // Change as needed

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return; // Only teleport the local player

        StartCoroutine(TeleportAfterDelay());
    }

    private System.Collections.IEnumerator TeleportAfterDelay()
    {
        // Wait for scene to fully load and for target to exist
        yield return new WaitForSeconds(0.1f);

        GameObject target = GameObject.Find(spawnPointName);
        if (target == null)
        {
            Debug.LogWarning($"Spawn target '{spawnPointName}' not found.");
            yield break;
        }

        Vector3 position = target.transform.position;
        Quaternion rotation = target.transform.rotation;

        // Ask the server to teleport this player
        TeleportRequestServerRpc(position, rotation);
    }

    [ServerRpc]
    private void TeleportRequestServerRpc(Vector3 position, Quaternion rotation)
    {
        // Use TrySetPositionAndRotation to force-update the ClientNetworkTransform
//        NetworkObject.TrySetPositionAndRotation(position, rotation);
    }
}
