using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetMirror : MonoBehaviour
{
    //
    private bool _isSettingMirror = false;

    private readonly TimeScaleManager _timeScaleManager = new TimeScaleManager();
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Slow");
            _isSettingMirror = true;
            //_timeScaleManager.DoSlowMotion();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isSettingMirror = false;
            Debug.Log("Reset");
        }

        HandleTimeScale();
    }

    private void HandleTimeScale()
    {
        if (!_isSettingMirror)
        {
            _timeScaleManager.ResetTimeScale();
            
            return;
        }

        _timeScaleManager.DoSlowMotion();
    }
}
