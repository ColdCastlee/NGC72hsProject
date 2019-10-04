using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScene : MonoBehaviour{
  
    public Slider mProgress;
    private AsyncOperation async;
    private float timer = 0;
    private float target;
    void Start () {
        StartCoroutine(loadScene());  
    }  
    
    
    IEnumerator loadScene()  
    {  
        async = Application.LoadLevelAsync(Global.GetInstance().loadName);  
        yield return new WaitForEndOfFrame();   //等待帧结束
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(1);   //异步加载场景API，返回异步参数
        asyncOperation.allowSceneActivation = false;   //设置不允许加载完成后自动跳转界面
        while (!asyncOperation.isDone)       //是否加载完成
        { 
            target = asyncOperation.progress;          //  加载进度
            mProgress.value = Mathf.Lerp(mProgress.value, target, 0.1f);					//fill均匀增加
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;               //计时器
            if (timer > 4f)
            {
                asyncOperation.allowSceneActivation = true;           //四秒后进入场景
            }
                

        }
    }
    

}
