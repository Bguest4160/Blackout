using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    public Transform objectGrabPoint;
    public Transform grabPointHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask layerMask = LayerMask.NameToLayer("ground");
        if (Physics.Raycast(transform.position, transform.forward, 2f, layerMask))
        {
            objectGrabPoint.Translate(0, 0, -.2f);
        }

        else
        {
            objectGrabPoint = grabPointHolder;
        }
    }
}
