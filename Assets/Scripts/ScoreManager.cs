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
    //ArrayList scoreList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInfo player)
    {
        Debug.Log(player.GetName()); ;
        nameList.Add(player);
        
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
