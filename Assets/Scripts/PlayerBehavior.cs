using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    private static bool initCooldownUsed = false;
    private static float cubeCooldown = 0.5f;
    private static float lastHealthEventTime = 0f;

    [SerializeField] Healthbar _healthbar;

    void Start() {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("g")) {
            PlayerTakeDamage(50);
            Debug.Log("Health: " + GameManager.gameManager._playerHealth.Health);
        }
        if (Input.GetKeyDown("h")) {
            PlayerHeal(30);
            Debug.Log("Health: " + GameManager.gameManager._playerHealth.Health);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(cubeCooldown)) {
                PlayerTakeDamage(50);
                Debug.Log("Health: " + GameManager.gameManager._playerHealth.Health);
            }
        }
    }

    bool CooldownCheck(float cooldown) {
        if (Time.time - cooldown <= 0 && initCooldownUsed == false) {
            initCooldownUsed = true;
            return true;
        }
        
        return (Time.time - lastHealthEventTime >= cooldown);
    }

    private void PlayerTakeDamage(int damage) {
        GameManager.gameManager._playerHealth.Damage(damage);
        lastHealthEventTime = Time.time;
        Debug.Log("lastHealthEventTime: " + PlayerBehavior.lastHealthEventTime);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    private void PlayerHeal(int healing) {
        GameManager.gameManager._playerHealth.Heal(healing);
        lastHealthEventTime = Time.time;
        Debug.Log("lastHealthEventTime: " + PlayerBehavior.lastHealthEventTime);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }
}
