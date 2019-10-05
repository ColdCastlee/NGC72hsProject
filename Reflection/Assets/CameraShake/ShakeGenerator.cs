using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeGenerator : MonoBehaviour
{
    public static ShakeGenerator Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ShakeGenerator>();
            }
            return _instance;
        }
    }

    private static ShakeGenerator _instance;

    public void ShakeSmall()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(.5f,.5f,0));
    }

    public void ShakeMedium()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(1.0f,1.0f,0));
    }

    public void ShakeHuge()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(1.5f,1.5f,0));
    }

    public void ShakeTiny()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(.2f,.2f,0));
    }
    
}
