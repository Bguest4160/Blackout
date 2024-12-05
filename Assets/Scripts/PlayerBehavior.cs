using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _cubeCooldown = 0.5f;
    private static float _lastHealthEventTime = 0f;
   
    public GameObject frontHealthSliderObject;
    public GameObject backHealthSliderObject;
    public GameObject backHealthSliderFillObject;
    public UnitBar PlayerHealth;
    
    // Methods
    void Start() {
        PlayerHealth = new UnitBar(1000, 1000, frontHealthSliderObject, backHealthSliderObject, backHealthSliderFillObject);
    }

    void Update()
    {
        if (Input.GetKeyDown("g")) {
            PlayerTakeDamage(200);
        }
        if (Input.GetKeyDown("h")) {
            PlayerHeal(100);
        }
        
        PlayerHealth.ChipHealth();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(_cubeCooldown)) {
                PlayerTakeDamage(50);
                Debug.Log("Health: " + PlayerHealth.Value);
            }
        }
    }

    bool CooldownCheck(float cooldown) {
        if (Time.time - cooldown <= 0 && _initCooldownUsed == false) {
            _initCooldownUsed = true;
            return true;
        }
        
        return (Time.time - _lastHealthEventTime >= cooldown);
    }

    private void PlayerTakeDamage(int amount) {
        PlayerHealth.Subtract(amount);
        _lastHealthEventTime = Time.time;
        // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
    }

    private void PlayerHeal(int amount) {
        PlayerHealth.Add(amount);
        _lastHealthEventTime = Time.time;
        // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
    }
}
