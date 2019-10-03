using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Slider _hpSlider;
    private bool isDie = false;
    void Start()
    {
        _hpSlider = gameObject.GetComponent<Slider>();
        Debug.Log(_hpSlider.maxValue);
    }

    public void TakeDemage(int _damage)
    {
        if (!isDie)
        {
            _hpSlider.value -= _damage;
            if (_hpSlider.value <= 0)
            {
                _hpSlider.value = 0;
                isDie = true;
                //todo：：die
                Debug.Log(_hpSlider.value);
            }
            
        }
    }
    
}
