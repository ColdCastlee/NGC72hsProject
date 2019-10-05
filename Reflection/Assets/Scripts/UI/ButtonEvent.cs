using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {
    
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
 
    public void MouseClickRestart()
    {
        SceneManager.LoadScene(0);
    }
    
    public void MouseClickMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    public void MouseClickExit()
    {
        Application.Quit();
    }
    
    
}