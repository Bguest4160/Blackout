using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Scoreboard : NetworkBehaviour
{
    ScoreManager scoreManager;
    

    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;

    void Update()
    {

    }

    

    
}
