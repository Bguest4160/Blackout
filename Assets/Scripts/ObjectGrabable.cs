using Unity.Netcode;
using UnityEngine;

public class ObjectGrabable : NetworkBehaviour {
    public float throwForce = 10f;
    public float damageForce = 75f;

    [Space(15)] public Transform playerTransform;
    public Transform playerCamera;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private NetworkVariable<byte> state = new NetworkVariable<byte>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); // 0 = static, 1 = held, 2 = thrown
    private NetworkVariable<Vector3> lerpPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool activateCollider;
    private float lerpSpeed = 15f;


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
    public void RequestOwnershipServerRpc(ulong newClientId) {
        NetworkObject.ChangeOwnership(newClientId);
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
    
    [ServerRpc(RequireOwnership = false)]
    private void SetLerpPositionServerRpc(Vector3 objectGrabPointTransformPosition) {
        lerpPosition.Value = Vector3.Lerp(transform.position, objectGrabPointTransformPosition, Time.deltaTime * lerpSpeed);
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
        objectRigidbody.useGravity = false;
        objectRigidbody.constraints = RigidbodyConstraints.None;
        objectRigidbody.isKinematic = false;
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
        if (objectGrabPointTransform != null && state.Value < 2) {
            SetLerpPositionServerRpc(objectGrabPointTransform.position);
        }
        if (IsServer && lerpPosition.Value != Vector3.zero && state.Value < 2) {
            objectRigidbody.MovePosition(lerpPosition.Value);
            Debug.Log("SERVER || " + objectRigidbody.name + " lerped to " + objectRigidbody.position + "; client objectgrabpointtransform: " + lerpPosition.Value);
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
}