using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {
    
	
    void Update () 
    {
        if (m_down && Time.time - _time > 2f)//按下2秒属于长按
        {
            m_down = false;
            Debug.Log("长按中");
        }
    }
    public void MouseEnter()
    {
        Debug.Log("鼠标进入");
        var text = this.gameObject.GetComponentInChildren<Text>();
        text.color = Color.white;

    }
    public void MouseExit()
    {
        Debug.Log("鼠标滑出");
        var text = this.gameObject.GetComponentInChildren<Text>();
        text.color = new Color(144f/255f,144f/255f,144f/255f);
    }
    public void MouseDown()
    {
        Debug.Log("按下");
        //MouseDownLong(true);//用于长按
    }
    public void MouseUp()
    {
        Debug.Log("抬起");
        //MouseDownLong(false);//用于长按终止
    }
    public void MouseClick()
    {
        Debug.Log("点击");
    }
    float _time;
    bool m_down = false;
    void MouseDownLong(bool _isdown)
    {
        m_down = _isdown;
        if (_isdown)
        {
            Debug.Log("开始长按");
            _time = Time.time;
        }
        else if (_time != 0)
        {
            _time = 0;
            Debug.Log("停止长按");
        }
    }
}