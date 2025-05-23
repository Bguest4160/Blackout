using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ObjectGrabable : NetworkBehaviour {
    public float throwForce = 10f;
    public float damageForce = 75f;

    [Space(15)] public Transform playerTransform;
    public Transform playerCamera;
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private NetworkVariable<byte> state = new NetworkVariable<byte>(); // 0 = static, 1 = held, 2 = thrown
    public bool activateCollider;
    private float lerpSpeed = 15f;
    private ClientNetworkTransform CNT;


    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        CNT = GetComponent<ClientNetworkTransform>();
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
        objectRigidbody.isKinematic = false;
        if (state.Value == 1) {
            objectRigidbody.useGravity = false;
            objectRigidbody.constraints = RigidbodyConstraints.None;
        }
        else {
            objectRigidbody.useGravity = true;
        }
        Debug.Log("state change successful");
    }

    public void SetPlayerCamera(Transform cameraTransform) {
        playerCamera = cameraTransform;
    }

    public void SetPlayerTransform(Transform transform) {
        playerTransform = transform;
    }

    public void Grab(Transform objectGrabPointTransform) {
        SetStateServerRpc(1);
        this.objectGrabPointTransform = objectGrabPointTransform;
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
        if (state.Value == 1 && Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("own held object?: " + IsOwner + "; object state: " + state.Value + "; gravity: " + objectRigidbody.useGravity + "; constraints: " + objectRigidbody.constraints + "; isKinematic: " + objectRigidbody.isKinematic);
            Debug.Log(CNT.IsServerAuthoritative());
        }
        if (IsOwner) {
            if (objectGrabPointTransform != null && state.Value < 2) {
                Vector3 lerpPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * lerpSpeed);
                objectRigidbody.MovePosition(lerpPosition);
                Debug.DrawLine(transform.position, lerpPosition, color: Color.red);
                Debug.DrawLine(lerpPosition, objectGrabPointTransform.position, color: Color.green);
            }
            else {
                objectRigidbody.AddForce(Physics.gravity * (3 / 2), ForceMode.Acceleration);
            }
        }
    }

    public bool GetActivateCollider() {
        return activateCollider;
    }

    public void SetActivateCollider(bool o) {
        activateCollider = o;
    }
}