using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBar
{
   // Fields
   private int _currentValue;
   private int _currentMaxValue;
   public float chipSpeed = 10f;
   private float _lerpTimer;
   
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
        _currentValue = Mathf.Clamp(_currentValue - amount, 0, _currentMaxValue);
        _bar.value = _currentValue;
    }

    public void Add(int amount) {
        if (_currentValue > 0) {
            _currentValue = Mathf.Clamp(_currentValue + amount, 0, _currentMaxValue);
            _bar.value = _currentValue;
            // _backBarImage.color = Color.green;
            //
            // _lerpTimer += Time.deltaTime;
            // float percentComplete = _lerpTimer / chipSpeed;
            // _backBar.value = Mathf.Lerp(_backBar.value, _currentValue, percentComplete);
        }
    }

    public void ChipHealth() {
        if (_backBar.value > _bar.value) {
            Debug.Log("Back health tweening...");
            _backBarImage.color = Color.yellow;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / chipSpeed;
            _backBar.value = Mathf.Lerp(_backBar.value, _currentValue, percentComplete);
        }
        else {
            Debug.Log("Back health tween complete!");
        }
    }
}