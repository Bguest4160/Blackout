using UnityEngine;
using Unity.Netcode;

public class BlockHandler : NetworkBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start() {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }
    
    // DAMAGE SOURCES
    // Should be the same as PlayerBehavior's but halved
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile) && _playerBehavior.CooldownCheck(_playerBehavior.ProjectileDamageCooldown)) {
            _playerBehavior.PlayerTakeDamage((int)(projectile.GetDamageForce() * 0.5));
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox")) {
            _playerBehavior.PlayerTakeDamage(50);
        }
        else if (other.gameObject.name == "Damage Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeDamageCooldown)) { 
            _playerBehavior.PlayerTakeDamage(50);
        }
    }
}
