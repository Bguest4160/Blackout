using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHandler : MonoBehaviour {
    private PlayerBehavior _playerBehavior;

    void Start()
    {
        _playerBehavior = GetComponentInParent<PlayerBehavior>();
    }

    // DAMAGE SOURCES
    // Remember to update BlockHandler version for any changes, cooldowns can be modified in PlayerBehavior
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.TryGetComponent(out ObjectGrabable projectile) && _playerBehavior.CooldownCheck(_playerBehavior.ProjectileDamageCooldown)) {
            if (projectile.state.Value == "thrown") {
                _playerBehavior.PlayerTakeDamage((int)projectile.GetDamageForce());
            }
        }
        else if (other.gameObject.CompareTag("IsMeleeHitbox") && _playerBehavior.CooldownCheck(_playerBehavior.MeleeDamageCooldown)) {
            _playerBehavior.PlayerTakeDamage(100);
        }
        else if (other.gameObject.name == "Damage Cube" && _playerBehavior.CooldownCheck(_playerBehavior.CubeDamageCooldown)) {
            _playerBehavior.PlayerTakeDamage(100);
        }
    }
}
