using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _cubeCooldown = 0.2f;
    private static float _lastHealthEventTime = 0f;
    private static bool _punchHitboxActive = false;
   
    public GameObject frontHealthSlider;
    public GameObject backHealthSlider;
    public GameObject backHealthSliderFill;
    public UnitBar PlayerHealth;
    
    [Space(15)] 
    
    public GameObject rightFistHitbox;
    public GameObject leftFistHitbox;
    private Animator playerAnimator;
    
    private Renderer rightHitboxRenderer;
    private Renderer leftHitboxRenderer;
    private Collider rightFistCollider;
    private Collider leftFistCollider;
    
    // Methods
    void Start() {
        PlayerHealth = new UnitBar(1000, 1000, frontHealthSlider, backHealthSlider, backHealthSliderFill);
        
        playerAnimator = GetComponent<Animator>();
        
        rightHitboxRenderer = rightFistHitbox.GetComponent<Renderer>();
        leftHitboxRenderer = leftFistHitbox.GetComponent<Renderer>();
        rightFistCollider= rightFistHitbox.GetComponent<Collider>();
        leftFistCollider = leftFistHitbox.GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            PlayerTakeDamage(200);
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            PlayerHeal(100);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            _punchHitboxActive = true;
            Invoke("HitBoxController", 3f);
        }
        else {
            _punchHitboxActive = false;
            HitBoxController();
        }
        
        PlayerHealth.ChipHealth();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.name == "Damage Cube") {
            if (CooldownCheck(_cubeCooldown)) {
                PlayerTakeDamage(20);
                Debug.Log("Health: " + PlayerHealth.Value);
            }
        }
    }

    void HitBoxController() {
        if (_punchHitboxActive) {
            rightFistCollider.enabled = true;
            leftFistCollider.enabled = true;
            rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
            leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
        }
        else {
            rightFistCollider.enabled = false;
            leftFistCollider.enabled = false;
            rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
            leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
        }
    }

    bool CooldownCheck(float cooldown) {
        if (Time.time - cooldown <= 0 && _initCooldownUsed == false) {
            _initCooldownUsed = true;
            return true;
        }
        
        return (Time.time - _lastHealthEventTime >= cooldown);
    }

    bool IsAnimationPlaying(string animationName) {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void PlayerTakeDamage(int amount) {
        if (PlayerHealth.Value > 0) {
            PlayerHealth.Subtract(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }

    private void PlayerHeal(int amount) {
        if (PlayerHealth.Value > 0 && PlayerHealth.Value < PlayerHealth.MaxValue) {
            PlayerHealth.Add(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }
}
