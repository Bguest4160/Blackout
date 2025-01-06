using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupThrow : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTranform;
    private ObjectGrabable objectGrabable;

    void Start()
    {
        GameManager.activateCollider = false;
    }



    private void Update()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            if (objectGrabable == null)
            {
                //try to grab
                float pickUpDistance = 3f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    
                    if (raycastHit.transform.TryGetComponent(out objectGrabable))
                    {
                        
                        objectGrabable.Grab(objectGrabPointTranform);
                       
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            objectGrabable.Throw();
            objectGrabable.Throw();
            objectGrabable = null;
            GameManager.activateCollider = true;
            
        }
    }

 
}
public class GameManager : MonoBehaviour
{
    public static bool activateCollider;
}
