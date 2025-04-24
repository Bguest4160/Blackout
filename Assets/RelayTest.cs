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
using System;
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

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error in CreateRelay: " + e.Message);
        }
        return null;
    }

    public async Task JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("JoinRelay with code: " + joinCode); // Add this line for debugging

            // Validate the join code format
            if (string.IsNullOrEmpty(joinCode) || joinCode.Length != 6)
            {
                Debug.LogError("JoinRelay failed: Invalid join code format");
                return;
            }

            // Join the relay with the provided join code
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // Setup the transport and start client
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            // Start the client
            NetworkManager.Singleton.StartClient();

            // Load the game scene once the client starts
            Debug.Log("Successfully joined relay, loading game scene...");
             // Replace "GameScene" with the actual name of your game scene
        }
        catch (Exception e)
        {
            Debug.LogError("Error in JoinRelay: " + e);
        }
    }


}