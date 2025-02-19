using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start() {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }
    
    void OnCollisionEnter(Collision other) {
        Debug.Log("Block hitbox entered");
        
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            _playerBehavior.blockingModifier = 0.5f;
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox")) {
            _playerBehavior.blockingModifier = 0.5f;
        }
    }

    void OnCollisionExit(Collision other) {
        Debug.Log("Block hitbox exited");
        
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            _playerBehavior.blockingModifier = 0.1f;
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox")) {
            _playerBehavior.blockingModifier = 0.1f;
        }
    }
}
