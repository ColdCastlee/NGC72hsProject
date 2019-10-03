using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeScaleManager : MonoBehaviour
{
    private static TimeScaleManager _instance;

    public static TimeScaleManager Instance
    {
        get{
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeScaleManager>();
            }
            return _instance;
        }
    }
    
    
    [FormerlySerializedAs("slowDownFactor")] public float _slowDownFactor = 0.2f;
    private float _slowDownTimer = 0.0f;
    private bool _refillTimeScale = false;
    private float _slowDownTime = 1.0f;
    

    public void DoSlowMotion()
    {
        Time.timeScale = _slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void ResetTimeScale()
    {
        if (_refillTimeScale == false)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;            
        }
    }

    public void DoSlowMotionForSeconds(float time)
    {
        DoSlowMotion();
        _slowDownTime = time;
        _refillTimeScale = true;
    }

    public void RefillTimeScale()
    {
        if (!_refillTimeScale)
        {
            return;
        }
//
        Time.timeScale += (1f / _slowDownTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp01(Time.timeScale);

        if (Time.timeScale >= 1.0f)
        {
            _refillTimeScale = false;
        }
    }

    private void Update()
    {
        RefillTimeScale();
    }
}
