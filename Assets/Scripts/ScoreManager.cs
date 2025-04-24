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

    ArrayList nameList = new ArrayList();
    ArrayList scoreList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        foreach(int x in scoreList)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(string name)
    {
        Debug.Log(name);
        nameList.Add(name);
        
    }

    [ServerRpc]
    public void SendNameServerRpc(string name)
    {
        AddPlayer(name);
    }

}
