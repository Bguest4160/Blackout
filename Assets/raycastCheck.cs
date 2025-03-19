using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastCheck : MonoBehaviour
{
    
    public Transform playerCamera;
    public Transform camHolder3;
    float distance = .4f;
    public Transform headPivot;
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LayerMask layermask = LayerMask.GetMask("ground");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance, layermask))
        {
            MoveCamBack();
        }

        if (Physics.Raycast(camHolder3.position, camHolder3.TransformDirection(Vector3.up), distance, layermask))
        {
            MoveCamUp();
        }

        if(!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance, layermask) && !Physics.Raycast(camHolder3.position, camHolder3.TransformDirection(Vector3.up), distance, layermask))
        {
            playerCamera.position = transform.position;
        }
    }

    void MoveCamBack()
    {
        LayerMask layermask = LayerMask.GetMask("ground");
        if (Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), .2f, layermask))
        {
            playerCamera.Translate(0f, 0f, -.1f);
            Debug.Log("raycast hit");
        }
    }

    void MoveCamUp()
    {
        LayerMask layermask = LayerMask.GetMask("ground");
        if (Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.up), .2f, layermask))
        {
            playerCamera.Translate(0f, -0.1f, 0f);
            //headPivot.Rotate(Vector3.up, 2f);
            Debug.Log("top head seen");
        }
    }
}
