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
    public bool holding = false;

    void Start()
    {
        
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
                        holding = true;
                       
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            objectGrabable.Throw();
            objectGrabable.Throw();
            objectGrabable = null;
            holding = false;

            
        }
    }

    public bool GetHolding()
    {
        return holding;
    }

    public void SetHolding(bool o)
    {
        holding = o;
    }

 
}
