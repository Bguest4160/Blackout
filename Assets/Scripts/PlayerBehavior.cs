     using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;
using UnityEngine.iOS;


public class PlayerBehavior : NetworkBehaviour {
    // Fields
    private static bool _initCooldownUsed = false; 
    public float CubeCooldown = 0.8f;
    public float ProjectileDamageCooldown = 0.1f;
    public float MeleeDamageCooldown = 0.5f;
    
    private static float _lastHealthEventTime = 0f;
    private static bool _hitboxViewerActive = false;

    public static Color Transparent = new Color(0.8f, 0.8f, 0.8f, 0);
    public static Color PassiveHitbox = new Color(0.8f, 0.8f, 0.8f, 0.352941f);
    public static Color ActiveHitbox = new Color(1f, 0.027450f, 0f, 0.352941f);
    
    [Space(15)]
    
    public GameObject frontHealthSlider;
    public GameObject backHealthSlider;
    public GameObject backHealthSliderFill;
    public UnitBar PlayerHealth;
    
    [Space(15)] 
    
    public GameObject rightFistHitbox;
    public GameObject leftFistHitbox;
    public GameObject blockingHitbox;
    public GameObject playerHitbox;
    
    private Renderer rightHitboxRenderer;
    private Renderer leftHitboxRenderer;
    private Collider rightFistCollider;
    private Collider leftFistCollider;
    private Collider blockingCollider;
    private Canvas UICanvas;
    
    // Methods
    
    void Start() {
        PlayerHealth = new UnitBar(1000, 1000, frontHealthSlider, backHealthSlider, backHealthSliderFill);
        
        rightHitboxRenderer = rightFistHitbox.GetComponent<Renderer>();
        leftHitboxRenderer = leftFistHitbox.GetComponent<Renderer>();
        rightFistCollider= rightFistHitbox.GetComponent<Collider>();
        leftFistCollider = leftFistHitbox.GetComponent<Collider>();
        blockingCollider = blockingHitbox.GetComponent<Collider>();
        UICanvas = GetComponentInChildren<Canvas>();

        if (!IsOwner) {
            UICanvas.enabled = false;
        }
    }

    void Update() {
        if (!IsOwner) return;
        if (!IsLocalPlayer) return;
        
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
                playerHitbox.GetComponent<Renderer>().material.color = PassiveHitbox;
            }
            else {
                leftHitboxRenderer.material.color = Transparent;
                rightHitboxRenderer.material.color = Transparent;
                playerHitbox.GetComponent<Renderer>().material.color = Transparent;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            blockingCollider.enabled = true;
            if (_hitboxViewerActive) {
                blockingHitbox.GetComponent<Renderer>().material.color = PassiveHitbox;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            blockingCollider.enabled = false;
            blockingHitbox.GetComponent<Renderer>().material.color = Transparent;
        }
        
        PlayerHealth.ChipHealth();

        if (PlayerHealth.Value <= 0){
            Debug.Log("You are dead");
            this.gameObject.SetActive(false);
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

    public bool CooldownCheck(float cooldown) {
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
            Debug.Log("Damaged " + amount);
        }
    }

    public void PlayerHeal(int amount) {
        if (PlayerHealth.Value > 0 && PlayerHealth.Value < PlayerHealth.MaxValue) {
            PlayerHealth.Add(amount);
            _lastHealthEventTime = Time.time;
            Debug.Log("Healed " + amount);
        }
    }
}
