using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _cubeDamageCooldown = 0.2f;
    private static float _damageCooldown = 0.5f;
    private static float _lastHealthEventTime = 0f;
    private static bool _hitboxViewerActive = false;
   
    public GameObject frontHealthSlider;
    public GameObject backHealthSlider;
    public GameObject backHealthSliderFill;
    public UnitBar PlayerHealth;
    
    [Space(15)] 
    
    public GameObject rightFistHitbox;
    public GameObject leftFistHitbox;
    
    private Renderer rightHitboxRenderer;
    private Renderer leftHitboxRenderer;
    private Collider rightFistCollider;
    private Collider leftFistCollider;
    
    // Methods
    void Start() {
        PlayerHealth = new UnitBar(1000, 1000, frontHealthSlider, backHealthSlider, backHealthSliderFill);
        
        rightHitboxRenderer = rightFistHitbox.GetComponent<Renderer>();
        leftHitboxRenderer = leftFistHitbox.GetComponent<Renderer>();
        rightFistCollider= rightFistHitbox.GetComponent<Collider>();
        leftFistCollider = leftFistHitbox.GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) {
            Debug.Log("Health: " + PlayerHealth.Value);
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            PlayerTakeDamage(200);
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            PlayerHeal(100);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            _hitboxViewerActive = !_hitboxViewerActive;

            if (_hitboxViewerActive) {
                leftHitboxRenderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
                rightHitboxRenderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
            }
            else {
                leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
                rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
            }
        }
        
        PlayerHealth.ChipHealth();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(_cubeDamageCooldown)) {
                PlayerTakeDamage(20);
            }
        }
    }
    
    
    
    void OnTriggerEnter(Collider other) {
        if (CooldownCheck(_damageCooldown)) {
            if (other.gameObject.CompareTag("IsDamageTrigger")) {
                PlayerTakeDamage(100);
            }
        }
    }
    
    IEnumerator RightHandPunch() {
        rightFistCollider.enabled = true;
        if (_hitboxViewerActive) {
            rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
        }
        yield return new WaitForSeconds(0.2f);
        rightFistCollider.enabled = false;
        if (_hitboxViewerActive) {
            rightHitboxRenderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
        }
    }

    IEnumerator LeftHandPunch() {
        leftFistCollider.enabled = true;
        if (_hitboxViewerActive) {
            leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
        }
        yield return new WaitForSeconds(0.2f);
        leftFistCollider.enabled = false;
        if (_hitboxViewerActive) {
            leftHitboxRenderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
        }
    }

    bool CooldownCheck(float cooldown) {
        if (Time.time - cooldown <= 0 && _initCooldownUsed == false) {
            _initCooldownUsed = true;
            return true;
        }
        return (Time.time - _lastHealthEventTime >= cooldown);
    }

    public void PlayerTakeDamage(int amount) {
        if (PlayerHealth.Value > 0) {
            PlayerHealth.Subtract(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }

    public void PlayerHeal(int amount) {
        if (PlayerHealth.Value > 0 && PlayerHealth.Value < PlayerHealth.MaxValue) {
            PlayerHealth.Add(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }
}
