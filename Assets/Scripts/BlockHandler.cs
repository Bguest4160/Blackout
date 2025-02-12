using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start() {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }
    
    void OnCollisionEnter(Collision other) {
        Debug.Log("Block hitbox touched. dont touch in my block hitbox pls");
        
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            _playerBehavior.PlayerTakeDamage((int)(projectile.GetDamageForce() * 0.5));
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox")) {
            _playerBehavior.PlayerTakeDamage(50);
        }
    }
    
    // need to send some message (probably just create and change a variable in PlayerBehavior) to signify when an active block hitbox has been hit and reduce damage based on that.
    // also consider the block hitboxes don't have rigidbodies so it will likely hit the player as well, triggering both BlockHandler and PlayerBehavior's OnCollisionEnter methods
}
