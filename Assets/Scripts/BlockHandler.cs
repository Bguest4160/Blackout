using UnityEngine;
using Unity.Netcode;

public class BlockHandler : NetworkBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start() {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }
    
    // DAMAGE SOURCES
    // Should be the same as HitboxHandler's but halved
    // Currently overridden to be 0 damage taken when blocking
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile) && _playerBehavior.CooldownCheck(_playerBehavior.ProjectileDamageCooldown)) {
            if (projectile.GetState() == 2) {
                _playerBehavior.PlayerTakeDamage((int)(projectile.GetDamageForce() * 0));
            }
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox") && _playerBehavior.CooldownCheck(_playerBehavior.MeleeDamageCooldown)) {
            _playerBehavior.PlayerTakeDamage(0);
        }
    }
    
    void OnCollisionStay(Collision other) {
        if (other.gameObject.name == "Damage Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeCooldown)) {
            _playerBehavior.PlayerTakeDamage(0);
        }
        else if (other.gameObject.name == "Heal Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeCooldown)) {
            _playerBehavior.PlayerHeal(1000);
        }
    }
}
