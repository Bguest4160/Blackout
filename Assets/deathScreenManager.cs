using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDeathScreen()
    {
        gameObject.SetActive(true);
    }

}
