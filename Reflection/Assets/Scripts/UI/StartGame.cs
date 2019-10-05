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
    private Fade fade;
    private void Awake()
    {
       this.gameObject.AddComponent<AudioMgr>();
       fade = this.gameObject.GetComponent<Fade>();
    }
    void Start()
    {
        MemoryMgr.LoadAssetFromResourceDir<AudioClip>(typeof(AudioName),"Audio/",(name,clip)=>
        {
            if(AudioMgr.Instance.audioclips.ContainsKey(name)==false)
                AudioMgr.Instance.audioclips.Add(name, clip);
        });
        
        AudioMgr.Instance.PlayBgm(AudioName._start);
        
        Global.GetInstance().loadName = "TestCharacterMovementScen";
        startButton.onClick.AddListener(() =>
        {
            fade.m_Sprite.gameObject.SetActive(true);
            fade.setIsActive();
        });
    }
    
}
