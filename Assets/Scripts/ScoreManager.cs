using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    public TMP_Text nameText;
    public TMP_Text winsText;

    public ArrayList nameList = new ArrayList();
    public Scoreboard scoreboard;
    //ArrayList scoreList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo player1 = new PlayerInfo();
        player1.SetName("Nick");
        AddPlayer(player1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInfo player)
    {
        Debug.Log(player.GetName()); ;
        nameList.Add(player);
        Debug.Log("player added to list");
        AddToScoreBoard();

        
    }

    public void AddToScoreBoard()
    {
        Debug.Log("iamrunning");
        foreach (PlayerInfo player in nameList)
        {
            scoreboard.AddScoreBoardItem(player);
            Debug.Log("we are trying to add");
            Debug.Log(player.GetName());

        }
    }

    /*[ServerRpc]
    public void SendNameServerRpc(string name)
    {
        AddPlayer(name);
    }*/

}

public class PlayerInfo
{
    private string name;
    private int wins;

    public void SetName(string n)
    {
        name = n;
    }

    public void AddWin()
    {
        wins += 1;
    }

    public void ResetWins()
    {
        wins = 0;
    }

    public int GetWins()
    {
        return wins;
    }

    public string GetName()
    {
        return name;
    }
}
