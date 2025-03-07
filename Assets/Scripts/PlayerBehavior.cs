using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerBehavior : NetworkBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    public float CubeDamageCooldown = 0.8f;
    public float ProjectileDamageCooldown = 0.1f;
    
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
    
    public Renderer rightHitboxRenderer;
    public Renderer leftHitboxRenderer;
    public Collider rightFistCollider;
    public Collider leftFistCollider;
    
    // Methods
    void Start() {
        PlayerHealth = new UnitBar(1000, 1000, frontHealthSlider, backHealthSlider, backHealthSliderFill);
        
        // rightHitboxRenderer = rightFistHitbox.GetComponent<Renderer>();
        // leftHitboxRenderer = leftFistHitbox.GetComponent<Renderer>();
        // rightFistCollider= rightFistHitbox.GetComponent<Collider>();
        // leftFistCollider = leftFistHitbox.GetComponent<Collider>();
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
            }
            else {
                leftHitboxRenderer.material.color = Transparent;
                rightHitboxRenderer.material.color = Transparent;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            Invoke("BlockDown", 0.5f);
        }
        
        // PlayerHealth.ChipHealth();
    }

    // DAMAGE SOURCES
    // Remember to update BlockHandler version for any changes
    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Debug.Log(hit.gameObject.name);
        if (hit.gameObject.TryGetComponent(out ObjectGrabable projectile) && CooldownCheck(ProjectileDamageCooldown)) {
            PlayerTakeDamage((int)projectile.GetDamageForce());
        }
        else if (hit.gameObject.CompareTag("IsMeleeHitbox")) {
            PlayerTakeDamage(100);
        }
        else if (hit.gameObject.name == "Damage Cube" && CooldownCheck(CubeDamageCooldown)) { 
            PlayerTakeDamage(100);
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
            if (_hitboxViewerActive) {
                hitbox.GetComponent<Renderer>().material.color = PassiveHitbox;
            }
        }
    }

    void BlockDown() {
        foreach (GameObject hitbox in GameObject.FindGameObjectsWithTag("IsBlockingHitbox")) {
            hitbox.GetComponent<Collider>().enabled = false;
            if (_hitboxViewerActive) {
                hitbox.GetComponent<Renderer>().material.color = Transparent;
            }
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
