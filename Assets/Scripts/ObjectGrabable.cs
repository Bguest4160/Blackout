using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabable : MonoBehaviour
{
    public float throwForce = 10f;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    public Transform playerTransform;
    public Transform playerCamera;
    public bool held = false;
    public bool activateCollider;
   
    
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }   


    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        held = true;
    }

    public void Throw()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.AddForce(playerCamera.forward * throwForce, ForceMode.VelocityChange);
        if (held == true)
        {
            activateCollider = true;
        }
        held = false;
    }


    private void FixedUpdate()
    {


        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 15f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }
        else
        {
            objectRigidbody.AddForce(Physics.gravity * (3 / 2), ForceMode.Acceleration);
        } 
      
    }

    public bool GetactivateCollier()
    {
        return activateCollider;
    }

    public void SetActivateCollider(bool o)
    {
        activateCollider = o;
    }
}

