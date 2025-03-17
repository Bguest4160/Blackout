using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectGrabable : NetworkBehaviour
{
    public float throwForce = 10f;
    public float damageForce = 75f;
    
    [Space(15)] 
    
    public Transform playerTransform;
    public Transform playerCamera;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    public NetworkVariable<string> state = new NetworkVariable<string>("static");
    public bool activateCollider;

    
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    public float GetDamageForce() {
        return damageForce;
    }

    public void SetPlayerCamera(Transform cameraTransform)
    {
        playerCamera = cameraTransform;
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        SetServerRpcState("held");
        objectRigidbody.constraints = RigidbodyConstraints.None;
    }

    public void Throw()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.AddForce(playerCamera.forward * throwForce, ForceMode.VelocityChange);
        if (state.Value == "held")
        {
            activateCollider = true;
        }
        SetServerRpcState("thrown");
    }

    [ServerRpc]
    public void SetServerRpcState(string state) {
        if (!IsServer) return;

        this.state.Value = state;
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

