using Unity.Netcode;
using UnityEngine;

public class NetworkInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Register approval callback BEFORE starting host/server
        NetworkManager.Singleton.ConnectionApprovalCallback = ApproveConnection;

        // Optionally start the host here
        // NetworkManager.Singleton.StartHost();
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("ApproveConnection called");

        response.Approved = true;
        response.CreatePlayerObject = false; // Prevent auto-spawn
        response.Position = Vector3.zero;
        response.Rotation = Quaternion.identity;
    }
}
