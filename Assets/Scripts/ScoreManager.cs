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
    [SerializeField] private GameObject scoreBoard;
    bool scoreboardCanBeActive = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreBoard.SetActive(true);
        scoreboardCanBeActive = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(scoreboardCanBeActive && Input.GetKey(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }

        if (scoreboardCanBeActive == false || !Input.GetKey(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }

    public void AddPlayer(PlayerInfo player)
    {
        bool alrExists = false;
        foreach (PlayerInfo p in nameList)
        {
            if (player.GetName().Equals(p.GetName()))
            {
                alrExists = true;
                Debug.Log("name already found");
            }
        }
        if (alrExists == false)
        {
            nameList.Add(player);
            Debug.Log("askedtoPingPong");
            SendPlayerServerRpc(player.GetName(), player.GetWins());
        }
        AddToScoreBoard();   
    }

    public void NewAddPlayerMethod(ArrayList players)
    {
        foreach(PlayerInfo player in players)
        {
            AddScoreBoardItem(player);
        }
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

    [ServerRpc (RequireOwnership = false)]
    public void SendPlayerServerRpc(string name, int wins)
    {
        Debug.Log("I am pinging");
        PlayerInfo player1 = new PlayerInfo();
        player1.SetName(name);
        player1.SetWins(wins);
        nameList.Add(player1);
        SendPlayerClientRpc(name, wins);
    }

    [ClientRpc]
    public void SendPlayerClientRpc(string name, int wins)
    {
        Debug.Log("I am ponging");
        PlayerInfo player1 = new PlayerInfo();
        player1.SetName(name);
        player1.SetWins(wins);
        nameList.Add(player1);
        SendPlayerServerRpc(name, wins);
    }

}

public class PlayerInfo
{
    private string name;
    private int wins = 0;

    public void SetName(string n)
    {
        name = n;
    }

    public void AddWin()
    {
        wins += 1;
    }

    public void SetWins(int w)
    {
        wins = w;
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
