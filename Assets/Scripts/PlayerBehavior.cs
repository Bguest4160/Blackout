using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _cubeDamageCooldown = 0.2f;
    private static float _lastHealthEventTime = 0f;
    private static bool _hitboxViewerActive = false;
    private static bool _blocking = false;

    public static Color Transparent = new Color(0.8f, 0.8f, 0.8f, 0);
    public static Color PassiveHitbox = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
    public static Color ActiveHitbox = new Color(1f, 0.027450f, 0f, 0.352941f);
   
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
                leftHitboxRenderer.material.color = PassiveHitbox;
                rightHitboxRenderer.material.color = PassiveHitbox;
            }
            else {
                leftHitboxRenderer.material.color = Transparent;
                rightHitboxRenderer.material.color = Transparent;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            Invoke("BlockDown", 0.5f);
        }
        
        PlayerHealth.ChipHealth();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            PlayerTakeDamage((int)projectile.GetDamageForce());
        }
        
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(_cubeDamageCooldown)) {
                PlayerTakeDamage(20);
            }
        }
    }
    
    IEnumerator RightHandPunch() {
        rightFistCollider.enabled = true;
        if (_hitboxViewerActive) {
            rightHitboxRenderer.material.color = ActiveHitbox;
        }
        yield return new WaitForSeconds(0.2f);
        rightFistCollider.enabled = false;
        if (_hitboxViewerActive) {
            rightHitboxRenderer.material.color = PassiveHitbox;
        }
    }

    IEnumerator LeftHandPunch() {
        leftFistCollider.enabled = true;
        if (_hitboxViewerActive) {
            leftHitboxRenderer.material.color = ActiveHitbox;
        }
        yield return new WaitForSeconds(0.2f);
        leftFistCollider.enabled = false;
        if (_hitboxViewerActive) {
            leftHitboxRenderer.material.color = PassiveHitbox;
        }
    }

    void BlockUp() {
        foreach (GameObject hitbox in GameObject.FindGameObjectsWithTag("IsBlockingHitbox")) {
            hitbox.GetComponent<Collider>().enabled = true;
            _blocking = true;
            if (_hitboxViewerActive) {
                hitbox.GetComponent<Renderer>().material.color = PassiveHitbox;
            }
        }
    }

    void BlockDown() {
        foreach (GameObject hitbox in GameObject.FindGameObjectsWithTag("IsBlockingHitbox")) {
            hitbox.GetComponent<Collider>().enabled = false;
            _blocking = false;
            if (_hitboxViewerActive) {
                hitbox.GetComponent<Renderer>().material.color = Transparent;
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
