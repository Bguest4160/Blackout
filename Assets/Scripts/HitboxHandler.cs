using UnityEngine;
using Unity.Netcode;

public class HitboxHandler : NetworkBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start()
    {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }

    // DAMAGE SOURCES
    // Remember to update BlockHandler version for any changes, cooldowns can be modified in PlayerBehavior
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile) && _playerBehavior.CooldownCheck(_playerBehavior.ProjectileDamageCooldown)) {
            // Debug.Log(projectile.GetState());
            if (projectile.GetState() == 2) {
                _playerBehavior.PlayerTakeDamage((int)projectile.GetDamageForce());
            }
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox") && _playerBehavior.CooldownCheck(_playerBehavior.MeleeDamageCooldown)) {
            _playerBehavior.PlayerTakeDamage(100);
        }
    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.name == "Damage Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeCooldown)) {
            _playerBehavior.PlayerTakeDamage(100);
        }
        else if (other.gameObject.name == "Heal Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeCooldown)) {
            _playerBehavior.PlayerHeal(1000);
        }
    }
}
