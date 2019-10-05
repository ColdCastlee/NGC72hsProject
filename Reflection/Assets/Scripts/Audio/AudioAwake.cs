using System;
using System.Collections;
using System.Collections.Generic;
using ReadyGamerOne.MemorySystem;
using UnityEngine;

public class AudioAwake : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.AddComponent<AudioMgr>();
    }
    private void Start()
    {
        MemoryMgr.LoadAssetFromResourceDir<AudioClip>(typeof(AudioName),"Audio/",(name,clip)=>
        {
            if(AudioMgr.Instance.audioclips.ContainsKey(name)==false)
                AudioMgr.Instance.audioclips.Add(name, clip);
        });
        AudioMgr.Instance.PlayBgm(AudioName._stage1_1);
    }
}
