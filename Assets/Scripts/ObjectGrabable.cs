using Unity.Netcode;
using UnityEngine;

public class ObjectGrabable : NetworkBehaviour {
    public float throwForce = 10f;
    public float damageForce = 75f;

    [Space(15)] public Transform playerTransform;
    public Transform playerCamera;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private NetworkVariable<byte> state =
        new NetworkVariable<byte>(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner); // 0 = static, 1 = held, 2 = thrown
    public bool activateCollider;


    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public float GetDamageForce() {
        return damageForce;
    }

    public byte GetState() {
        return state.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateServerRpc(byte stateValue) {
        state.Value = stateValue;
    }

    public void SetPlayerCamera(Transform cameraTransform) {
        playerCamera = cameraTransform;
        Debug.Log("player camera set; " + cameraTransform.name);
    }

    public void SetPlayerTransform(Transform transform) {
        playerTransform = transform;
        Debug.Log("player transform set; " + transform.name);
    }

    public void Grab(Transform objectGrabPointTransform) {
        this.objectGrabPointTransform = objectGrabPointTransform;
        Debug.Log("object grab point transform set; " + objectGrabPointTransform.name);
        objectRigidbody.useGravity = false;
        SetStateServerRpc(1);
        objectRigidbody.constraints = RigidbodyConstraints.None;
        Debug.Log("rigidbody properties and object state set");
    }

    public void Throw() {
        objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.AddForce(playerCamera.forward * throwForce, ForceMode.VelocityChange);
        if (state.Value == 1) {
            activateCollider = true;
            soundManager.PlaySound(SoundType.GRUNT2);
            Debug.Log("thrown");
        }
        SetStateServerRpc(2);
    }

    private void FixedUpdate() {

        if (objectGrabPointTransform != null) {
            GrabLerpServerRpc(objectGrabPointTransform.position);
        }
        else {
            objectRigidbody.AddForce(Physics.gravity * (3 / 2), ForceMode.Acceleration);
        }

    }

    public bool GetActivateCollider() {
        return activateCollider;
    }

    public void SetActivateCollider(bool o) {
        activateCollider = o;
    }

    [ServerRpc(RequireOwnership = false)]
    private void GrabLerpServerRpc(Vector3 objectGrabPointTransformPosition) {
        float lerpSpeed = 15f;
        Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransformPosition, Time.deltaTime * lerpSpeed);
        objectRigidbody.MovePosition(newPosition);
        Debug.Log(objectRigidbody.name + " lerped to " + objectRigidbody.position + "; client objectgrabpointtransform: " +
                  objectGrabPointTransformPosition);
    }

    /* objectgrabpointtransform (lerp destination) synced succesfully always also object state syncing and everything else in client debug is fine dont mess with that.
    for some reason the lerp function just does not want to cooperate. might be an issue with the lerp resetting itself OR the object's original position (opposite of destination) (transform.position in this method) not being set correctly.
    */
}