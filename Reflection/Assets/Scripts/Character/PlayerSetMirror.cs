using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSetMirror : MonoBehaviour
{
    //
    private int _hp = 3;
    private Vector2 _mouseDir;
    private bool _isSettingMirror = false;

    public GameObject MirrorImg;

    public GameObject Mirror;
    public UnityEvent OnBroken;
    //也有可能 需要杀死一定数量的怪物 才能进入下一个阶段

    private readonly TimeScaleManager _timeScaleManager = new TimeScaleManager();

    private void Start()
    {
        MirrorImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Slow");
            _isSettingMirror = true;
            //_timeScaleManager.DoSlowMotion();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isSettingMirror = false;
            Debug.Log("Reset");

            BuildMirror(_mouseDir);
        }

        HandleTimeScale();

        UpdateMouseDir();

        ShowWhereWillMirrorLand();
    }

    private void HandleTimeScale()
    {
        if (!_isSettingMirror)
        {
            _timeScaleManager.ResetTimeScale();

            return;
        }

        _timeScaleManager.DoSlowMotion();
    }

    private void UpdateMouseDir()
    {
        if (!_isSettingMirror)
        {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosOnScreen = Input.mousePosition;
        mousePosOnScreen.z = screenPos.z;
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
        _mouseDir = new Vector2((mousePosInWorld - this.transform.position).normalized.x,
            (mousePosInWorld - this.transform.position).normalized.y);
    }

    private void ShowWhereWillMirrorLand()
    {
        if (!_isSettingMirror)
        {
            if (MirrorImg.activeSelf)
            {
                MirrorImg.SetActive(false);
                
            }    
            return;
        }
        //TODO::半透明效果

        if (!MirrorImg.activeSelf)
        {
            MirrorImg.SetActive(true);
        }

        MirrorImg.transform.position = this.transform.position + new Vector3(_mouseDir.x, _mouseDir.y, 0) * 0.12f;
        MirrorImg.transform.localRotation = Quaternion.Euler(new Vector3(_mouseDir.x, _mouseDir.y, 0));
        MirrorImg.transform.right = new Vector3(_mouseDir.x, _mouseDir.y, 0);
    }

    private void BuildMirror(Vector2 dir)
    {
        //TODO::PARENT
        var mirror = Instantiate(Mirror, this.transform.position + new Vector3(dir.x, dir.y, 0) * 0.12f, Quaternion.identity);
        var mirrorScript = mirror.GetComponent<Mirror>();
        mirrorScript.Init(dir);
    }

    public void OnHit()
    {
        this._hp--;
        CheckHp();
    }

    private void CheckHp()
    {
        if (_hp <= 0)
        {
            OnBroken.Invoke();
            Destroy(this.gameObject,0.1f);
        }
    }
}