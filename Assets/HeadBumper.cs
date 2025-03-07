using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBumper : MonoBehaviour
{
    public Transform camPivot;
    public Transform camHolder;
    public Transform PlayerCamera;
    public Transform camHolder2;
    public Transform colliderThing;
    public Transform sphere;
    public Transform sphereHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.layer == 3)
        {
            PlayerCamera.Translate(0f, 0f, -1f*Time.deltaTime);
            sphere.Translate(0f, 0f, -1f * Time.deltaTime);
            Debug.Log(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            PlayerCamera.position = camHolder2.position;
            sphere.position = sphereHolder.position;
            Debug.Log("exited");
        }
        
    }
}
