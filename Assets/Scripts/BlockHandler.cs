using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            PlayerBehavior.PlayerTakeDamage((int)(projectile.GetDamageForce() * 0.5));
            
            // in this context i am treating playerbehavior as a static class which it is kinda because of monobehavior
            // but also isn't technically so it gets mad so i need to figure out some way to call PlayerTakeDamage directly from the source of the specific gameobject (player) it's assigned to or make it an object class or somethng??? bruh i dont even know rn
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox")) {
            PlayerBehavior.PlayerTakeDamage(50);
        }
    }
}
