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

    private void OnCollisionEnter(Collision collision)
    {
        camPivot.Translate(0f, 0f, -2);
    }

    private void OnCollisionExit(Collision collision)
    {
        camPivot = camHolder;
    }
}
