using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using ReadyGamerOne.MemorySystem;
public class StartGame : MonoBehaviour
{

    public Button startButton;

    private void Awake()
    {
        var play = this.gameObject.AddComponent<AudioMgr>();
    }
    void Start()
    {
        MemoryMgr.LoadAssetFromResourceDir<AudioClip>(typeof(AudioName),"Audio/",(name,clip)=>
        {
            if(AudioMgr.Instance.audioclips.ContainsKey(name)==false)
                AudioMgr.Instance.audioclips.Add(name, clip);
        });
        
        
        AudioMgr.Instance.PlayBgm(AudioName._1);
        
        Global.GetInstance().loadName = "TestCharacterMovementScen";
        startButton.onClick.AddListener(() =>
        {
            
            Application.LoadLevel("loadingScene");
        });


    }
    
}
