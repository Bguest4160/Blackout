using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBumper : MonoBehaviour
{
    public Transform camPivot;
    public Transform camHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == 3)
        {
            camPivot.Translate(0f, 0f, -.05f);
            Debug.Log(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            camPivot.Translate(0f, 0f, 0.5f);
            Debug.Log("exited");
        }
        
    }
}
