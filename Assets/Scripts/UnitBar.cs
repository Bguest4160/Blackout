using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UnitBar
{
   // Fields
   private int _currentValue;
   private int _currentMaxValue;
   
   public float chipSpeed = 1.5f;
   private float _lerpTimer;
   private float _delayTimer;
   
   private Slider _bar;
   private Slider _backBar;
   private Image _backBarImage;

   // Properties
   public int Value {
       get {
           return _currentValue;
       }
       set {
           _currentValue = value;
       }
   }

   public int MaxValue {
       get {
           return _currentMaxValue;
       }
       set {
           _currentMaxValue = value;
       }
   }

   // Constructors
    public UnitBar(int value, int maxValue, GameObject barParent) {
        _currentValue = value;
        _currentMaxValue = maxValue;
        
        _bar = barParent.GetComponent<Slider>();
        
        _bar.value = value;
        _bar.maxValue = maxValue;
    }
    
    public UnitBar(int value, int maxValue, GameObject barParent, float chipSpeed) {
        _currentValue = value;
        _currentMaxValue = maxValue;
        
        _bar = barParent.GetComponent<Slider>();
        
        _bar.value = value;
        _bar.maxValue = maxValue;

        this.chipSpeed = chipSpeed;
    }

    public UnitBar(int value, int maxValue, GameObject barParent, GameObject backBarParent, GameObject backBarFillParent) {
        _currentValue = value;
        _currentMaxValue = maxValue;
        
        _bar = barParent.GetComponent<Slider>();
        _backBar = backBarParent.GetComponent<Slider>();
        _backBarImage = backBarFillParent.GetComponent<Image>();
        
        _bar.value = value;
        _bar.maxValue = maxValue;
        _backBar.value = value;
        _backBar.value = maxValue;
    }

    // Methods
    
    public void Subtract(int amount) {
        _delayTimer = 0f;
        _lerpTimer = 0f;
        
        _currentValue = Mathf.Clamp(_currentValue - amount, 0, _currentMaxValue);

        if (_backBar != null) {
            _bar.value = _currentValue;
            _backBarImage.color = new Color(1f, 0.517647f, 0.517647f);
        }
    }

    public void Add(int amount) {
        _delayTimer = 0f;
        _lerpTimer = 0f;
        
        _currentValue = Mathf.Clamp(_currentValue + amount, 0, _currentMaxValue);
            
        if (_backBar != null) {
            _backBar.value = _currentValue;
            _backBarImage.color = new Color(0.541176f, 0.835294f, 0.541176f);
        }
    }

    public void ChipHealth() {
        _delayTimer += Time.deltaTime;
        
        if (_backBar != null) {               // if true health chipping is enabled, do allat
            if (_backBar.value > _currentValue && _delayTimer > 0.3f) {
                _lerpTimer += Time.deltaTime;
                float percentComplete = _lerpTimer / chipSpeed;
                _backBar.value = Mathf.Lerp(_backBar.value, _currentValue, percentComplete);
            }
            else if (_bar.value < _currentValue && _delayTimer > 0.3f) {
                _lerpTimer += Time.deltaTime;
                float percentComplete = _lerpTimer / chipSpeed;
                _bar.value = Mathf.Lerp(_bar.value, _currentValue, percentComplete);
            }
        }
        
        else if (_bar.value != _currentValue) {  // otherwise, simply tween health to whatever it needs to be
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / chipSpeed;
            _bar.value = Mathf.Lerp(_bar.value, _currentValue, percentComplete);
        }
    }
}