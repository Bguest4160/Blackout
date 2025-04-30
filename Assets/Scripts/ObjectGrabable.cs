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
        if (state.Value == 1) {
            objectRigidbody.useGravity = false;
            objectRigidbody.constraints = RigidbodyConstraints.None;
        }
        else {
            objectRigidbody.useGravity = true;
        }
    }

    public void SetPlayerCamera(Transform cameraTransform) {
        playerCamera = cameraTransform;
    }

    public void SetPlayerTransform(Transform transform) {
        playerTransform = transform;
    }

    public void Grab(Transform objectGrabPointTransform) {
        this.objectGrabPointTransform = objectGrabPointTransform;
        SetStateServerRpc(1);
    }

    public void Throw() {
        objectGrabPointTransform = null;
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
            // objectRigidbody.AddForce(Physics.gravity * (3 / 2), ForceMode.Acceleration);
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
        Debug.Log("SERVER || " + objectRigidbody.name + " lerped to " + objectRigidbody.position + "; client objectgrabpointtransform: " + objectGrabPointTransformPosition);
        // rabLerpClientRpc(objectGrabPointTransformPosition);
    }

    // [ClientRpc]
    // private void GrabLerpClientRpc(Vector3 objectGrabPointTransformPosition) {
    //     float lerpSpeed = 15f;
    //     Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransformPosition, Time.deltaTime * lerpSpeed);
    //     objectRigidbody.MovePosition(newPosition);
    //     Debug.Log("CLIENT || " + objectRigidbody.name + " lerped to " + objectRigidbody.position + "; client objectgrabpointtransform: " + objectGrabPointTransformPosition);
    // }

    /* objectgrabpointtransform (lerp destination) synced succesfully always also object state syncing and everything else in client debug is fine dont mess with that.
    for some reason the lerp function just does not want to cooperate. might be an issue with the lerp resetting itself OR the object's original position (opposite of destination) (transform.position in this method) not being set correctly.
    */
}