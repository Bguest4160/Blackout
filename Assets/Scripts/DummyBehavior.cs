using UnityEngine;
using Unity.Netcode;

public class DummyBehavior : NetworkBehaviour {
    // Fields
    private static bool _initCooldownUsed = false;
    private static float _lastHealthEventTime = 0f;
   
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
            DummyHeal(100);
        }
        
        DummyHealth.ChipHealth();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent(out ObjectGrabable projectile)) {
            if (projectile.GetState() == 2) {
                DummyTakeDamage((int)projectile.GetDamageForce());
            }
        }
        else if (collision.gameObject.CompareTag("IsMeleeHitbox")) {
            DummyTakeDamage(100);
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

    private void DummyHeal(int amount) {
        if (DummyHealth.Value > 0 && DummyHealth.Value < DummyHealth.MaxValue) {
            DummyHealth.Add(amount);
            _lastHealthEventTime = Time.time;
        }
    }
}
