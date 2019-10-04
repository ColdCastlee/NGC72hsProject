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
        int displayProgress = 0;
        int toProgress = 0;
        async.allowSceneActivation = false;
        while(async.progress < 0.9f) {
            toProgress = (int)async.progress * 100;
            while(displayProgress < toProgress) {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }
 
        toProgress = 100;
        while(displayProgress < toProgress){
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;

    }

    void SetLoadingPercentage(int displayProgress)
    {
        mProgress.value = displayProgress;
    }
}
