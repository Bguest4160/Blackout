using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static float cubeCooldown = 5f;
    public static float lastHealthEventTime = 0f;

    void Start()
    {
        
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

        // Debug.Log(CooldownCheck(cubeCooldown));
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (CooldownCheck(cubeCooldown)) {
            if (hit.gameObject.name == "Damage Cube") {
                PlayerTakeDamage(10);
                Debug.Log("Health: " + GameManager.gameManager._playerHealth.Health);
            }
        }
    }

    bool CooldownCheck(float cooldown) {
        if (Time.time - lastHealthEventTime >= cooldown || Time.time = 0f) {
            return true;
        }
        return false;
    }

    private void PlayerTakeDamage(int damage) {
        GameManager.gameManager._playerHealth.Damage(damage);
        lastHealthEventTime = Time.time;
        Debug.Log("lastHealthEventTime: " + PlayerBehavior.lastHealthEventTime);
    }

    private void PlayerHeal(int healing) {
        GameManager.gameManager._playerHealth.Heal(healing);
        lastHealthEventTime = Time.time;
        Debug.Log("lastHealthEventTime: " + PlayerBehavior.lastHealthEventTime);
    }
}
