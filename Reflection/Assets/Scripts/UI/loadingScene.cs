using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingScene : MonoBehaviour{
  
    public Slider mProgress;
    private AsyncOperation async;
    void Start () {  
        StartCoroutine(loadScene());  
    }  
      
    IEnumerator loadScene()  
    {  
        async = Application.LoadLevelAsync(Global.GetInstance().loadName);  
        yield return async;  
    }  
    
    void Update () {
        mProgress.value = async.progress;
    }
}
