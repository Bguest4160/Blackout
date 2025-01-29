using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DummyBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _lastHealthEventTime = 0f;
    private static float _damageCooldown = 0.5f;
   
    public GameObject frontHealthSlider;
    public GameObject backHealthSlider;
    public GameObject backHealthSliderFill;
    public UnitBar DummyHealth;
    
    // Methods
    void Start() {
        DummyHealth = new UnitBar(1000, 1000, frontHealthSlider, backHealthSlider, backHealthSliderFill);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            DummyTakeDamage(200);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            PlayerHeal(100);
        }
        
        DummyHealth.ChipHealth();
    }

    void OnCollisionEnter(Collision collision) { 
        Debug.Log(collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other) {
        if (CooldownCheck(_damageCooldown)) {
            if (other.gameObject.CompareTag("IsDamageTrigger")) {
                DummyTakeDamage(100);
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

    private void DummyTakeDamage(int amount) {
        if (DummyHealth.Value > 0) {
            DummyHealth.Subtract(amount);
            _lastHealthEventTime = Time.time;

            if (DummyHealth.Value <= 0) {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().AddForce(Vector3.back * 20, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce(Vector3.up * 13, ForceMode.Impulse);
            }
        }
    }

    private void PlayerHeal(int amount) {
        if (DummyHealth.Value > 0 && DummyHealth.Value < DummyHealth.MaxValue) {
            DummyHealth.Add(amount);
            _lastHealthEventTime = Time.time;
        }
    }
}
