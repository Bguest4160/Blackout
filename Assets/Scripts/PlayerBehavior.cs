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
        // if (Input.GetKeyDown(KeyCode.Mouse0)) {
        //     StartCoroutine(PunchController());
        // }
        
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
    
    public void PunchController(string hand) {
        if (hand == "Left") {
            Debug.Log("Left punch triggered");
        }
        if (hand == "Right") {
            Debug.Log("Right punch triggered");
        }
    }

    // IEnumerator PunchController() {
    //     bool leftPunch = true;
    //     
    //     while (Input.GetKey(KeyCode.Mouse0)) {
    //         if (leftPunch) {
    //             yield return new WaitForSeconds(0.45f);
    //     
    //             leftFistCollider.enabled = true;
    //             leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
    //
    //             yield return new WaitForSeconds(0.4f);
    //
    //             leftFistCollider.enabled = false;
    //             leftHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
    //             leftPunch = false;
    //         }
    //         else {
    //             yield return new WaitForSeconds(0.45f);
    //     
    //             rightFistCollider.enabled = true;
    //             rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0.352941f);
    //
    //             yield return new WaitForSeconds(0.4f);
    //     
    //             rightFistCollider.enabled = false;
    //             rightHitboxRenderer.material.color = new Color(1f, 0.027450f, 0f, 0);
    //             leftPunch = true;
    //         }
    //     }
    // }

    bool CooldownCheck(float cooldown) {
        if (Time.time - cooldown <= 0 && _initCooldownUsed == false) {
            _initCooldownUsed = true;
            return true;
        }
        return (Time.time - _lastHealthEventTime >= cooldown);
    }

    void PlayerTakeDamage(int amount) {
        if (PlayerHealth.Value > 0) {
            PlayerHealth.Subtract(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }

    void PlayerHeal(int amount) {
        if (PlayerHealth.Value > 0 && PlayerHealth.Value < PlayerHealth.MaxValue) {
            PlayerHealth.Add(amount);
            _lastHealthEventTime = Time.time;
            // Debug.Log("lastHealthEventTime: " + _lastHealthEventTime);
        }
    }
}
