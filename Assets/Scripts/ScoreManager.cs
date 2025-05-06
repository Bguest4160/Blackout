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
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;

    public ArrayList nameList = new ArrayList();
    public scoreBoardItem scoreBoardItem;
    //ArrayList scoreList = new ArrayList();
    public GameObject scoreBoard;
    bool scoreboardCanBeActive = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreboardCanBeActive && Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else
        {
            scoreBoard.SetActive(false);
        }
    }

    public void AddPlayer(PlayerInfo player)
    {
        Debug.Log(player.GetName()); ;
        nameList.Add(player);
        Debug.Log("player added to list");
        foreach (PlayerInfo p in nameList)
        {
            Debug.Log(p.GetName());
        }
        Debug.Log("done");
        //AddToScoreBoard();

        
    }

    public void AddToScoreBoard()
    {
        Debug.Log("iamrunning");
        foreach (PlayerInfo player in nameList)
        {
            AddScoreBoardItem(player);
            Debug.Log("we are trying to add");
            Debug.Log(player.GetName());

        }
    }

    public void AddScoreBoardItem(PlayerInfo player)
    {
        scoreBoardItem item = Instantiate(scoreBoardItemPrefab, container).GetComponent<scoreBoardItem>();
        item.Initialize(player);
        Debug.Log(player.GetName() + "intancinating");
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
