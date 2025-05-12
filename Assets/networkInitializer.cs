using Unity.Netcode;
using UnityEngine;

public class NetworkInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Make sure this runs before starting the server/host
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApproveConnection;
        };

        // Optional: Start server/host from here if this is your entry point
        // NetworkManager.Singleton.StartHost(); 
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = false; //  Prevent automatic player spawn
        response.Position = Vector3.zero;
        response.Rotation = Quaternion.identity;
    }
}
