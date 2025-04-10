using Unity.Netcode;
using UnityEngine;

public class ObjectGrabable : NetworkBehaviour
{
    public float throwForce = 10f;
    public float damageForce = 75f;
    
    [Space(15)] 
    
    public Transform playerTransform;
    public Transform playerCamera;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private byte state = 0; // 0 = static, 1 = held, 2 = thrown
    public bool activateCollider;

    
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    public float GetDamageForce() {
        return damageForce;
    }

    public byte GetState() {
        return state;
    }

    public void SetPlayerCamera(Transform cameraTransform)
    {
        playerCamera = cameraTransform;
    }
    
    public void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        state = 1;
        objectRigidbody.constraints = RigidbodyConstraints.None;
    }

    public void Throw()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.AddForce(playerCamera.forward * throwForce, ForceMode.VelocityChange);
        if (state == 1)
        {
            activateCollider = true;
            soundManager.PlaySound(SoundType.GRUNT2);
            Debug.Log("thrown");
        }
        state = 2;
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

