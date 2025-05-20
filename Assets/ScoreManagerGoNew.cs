using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScoreManagerGoNew : MonoBehaviour
{
    public GameObject scoreManagerPrefab;
    private GameObject scoreManagerInstance;


    void Start()
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            scoreManagerInstance = Instantiate(scoreManagerPrefab);
            scoreManagerInstance.GetComponent<NetworkObject>().Spawn();

            Debug.Log("ScoreManager spawned as networked object.");
        }
    }
}
