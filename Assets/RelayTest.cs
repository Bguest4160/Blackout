using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class RelayTest : MonoBehaviour
{
    // Singleton Instance
    public static RelayTest Instance { get; private set; }

    private async void Awake()
    {
        // Ensure only one instance of RelayTest exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this instance between scene loads
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            // Create relay allocation for up to 3 players
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            // Get the join code for this allocation
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Relay Created. Join Code: " + joinCode);

            // Set relay server data
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            // Start hosting the network session
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Actual merge scene", LoadSceneMode.Single);

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error in CreateRelay: " + e.Message);
        }
        return null;
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // Set relay server data for the client
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            Debug.Log("1");

            // Start client
            NetworkManager.Singleton.StartClient();
            Debug.Log("2");

        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error in JoinRelay: " + e.Message);
        }
    }
}
