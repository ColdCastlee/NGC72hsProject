using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager
{
    public float slowDownFactor = 0.02f;
    public float slowDownTime;

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    
    
}
