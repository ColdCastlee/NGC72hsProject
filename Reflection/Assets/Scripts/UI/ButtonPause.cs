using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : MonoBehaviour
{
    private bool isPause = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPause == false)
        {
            TimeScaleManager.Instance.Pause();
            Debug.Log(Time.timeScale);
            Debug.Log("您按下了esc键"); 
            isPause = true;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) && isPause == true)
        {
            TimeScaleManager.Instance.Resume();
            Time.timeScale = 1;
            isPause = false;
        }
    }
}
