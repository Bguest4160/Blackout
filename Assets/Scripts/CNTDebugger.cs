using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class CNTDebugger : MonoBehaviour
{
    private ClientNetworkTransform cnt;

    void Start()
    {
        cnt = GetComponent<ClientNetworkTransform>();
    }

    void Update()
    {
        if (cnt != null && cnt.IsOwner)
        {
            Debug.Log($"[CNT Debug] Sending Position: {transform.position}");
        }
    }
}
