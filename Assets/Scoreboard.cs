using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Scoreboard : NetworkBehaviour
{
    ScoreManager scoreManager;
    scoreBoardItem scoreBoardItem;

    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;

    void Update()
    {
        Debug.Log(scoreManager.nameList);
    }

    public void AddScoreBoardItem(PlayerInfo player)
    {
        scoreBoardItem item = Instantiate(scoreBoardItemPrefab, container).GetComponent<scoreBoardItem>();
        item.Initialize(player);
        Debug.Log(player.GetName() + "intancinating");
    }

    
}
