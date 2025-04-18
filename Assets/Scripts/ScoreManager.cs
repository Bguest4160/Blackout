using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScoreManager : MonoBehaviour
{
    ArrayList masterlist = new ArrayList();
    ArrayList activelist = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(string name)
    {
        masterlist.Add(name);
        Debug.Log(name);
    }
}
