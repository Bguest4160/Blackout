using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DummyBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _cubeCooldown = 0.2f;
    private static float _lastHealthEventTime = 0f;
   
    public GameObject frontHealthSliderObject;
    public GameObject backHealthSliderObject;
    public GameObject backHealthSliderFillObject;
    public UnitBar DummyHealth;
    
    // Methods
    void Start() {
        DummyHealth = new UnitBar(1000, 1000, frontHealthSliderObject, backHealthSliderObject, backHealthSliderFillObject);
    }

    void Update()
    {
        if (Input.GetKeyDown("t")) {
            PlayerTakeDamage(200);
        }
        if (Input.GetKeyDown("y")) {
            PlayerHeal(100);
        }
        
        DummyHealth.ChipHealth();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(_cubeCooldown)) {
                PlayerTakeDamage(20);
                Debug.Log("Health: " + DummyHealth.Value);
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
        if (DummyHealth.Value > 0) {
            DummyHealth.Subtract(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }

    private void PlayerHeal(int amount) {
        if (DummyHealth.Value > 0 && DummyHealth.Value < DummyHealth.MaxValue) {
            DummyHealth.Add(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }
}
