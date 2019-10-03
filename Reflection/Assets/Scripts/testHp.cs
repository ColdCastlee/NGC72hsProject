using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testHp : MonoBehaviour
{
    private HP _hp;
    // Start is called before the first frame update
    void Start()
    {
        _hp = gameObject.GetComponentInChildren<HP>();
    }

    private void Update()
    {
       
 
    }
}
