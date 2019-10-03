using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(() => { Application.LoadLevel(0); }); 
    }

}
