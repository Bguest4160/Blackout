using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    ScoreManager scoreManager;

    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;

    void Start()
    {
        foreach(PlayerInfo player in scoreManager.nameList)
        {
            AddScoreBoardItem(player);
        }
    }

    void AddScoreBoardItem(PlayerInfo player)
    {
        scoreBoardItem item = Instantiate(scoreBoardItemPrefab, container).GetComponent<scoreBoardItem>();
    }
}
