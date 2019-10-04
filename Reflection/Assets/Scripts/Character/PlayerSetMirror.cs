using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerSetMirror : MonoBehaviour
{
    
    //立盾
    public bool _settingShield;
    private float maxShieldTime = 2.0f;
    private float shieldRecoverTime = 10.0f;
    private float _curShieldValue = 1.0f;
    public GameObject ShieldImg;
    
    //
    private int _collectedFragrants = 0;

    private float _maxSlowEffectTime = 2.0f;
    private float _slowEffectTimePassed = 0.0f;
    
    private Vector2 _mouseDir;
    private bool _isSettingMirror = false;

    public GameObject MirrorImg;

    public GameObject Mirror;

    public Vector2 MouseDir
    {
        get { return _mouseDir; }
    }

    //也有可能 需要杀死一定数量的怪物 才能进入下一个阶段


    private void Start()
    {
        ShieldImg.SetActive(false);
        MirrorImg.SetActive(false);
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdatePlayerFrag(_collectedFragrants);
        UIManager.Instance.ChangeStaminaPercentage(1.0f - _slowEffectTimePassed/_maxSlowEffectTime);
    }

    
    // Update is called once per frame
    void Update()
    {

        UpdateUI();
        
        
        if (Input.GetMouseButtonDown(0) && _collectedFragrants >= 2)
        {
            Debug.Log("Slow");
            _isSettingMirror = true;
        }else if(Input.GetMouseButtonDown(0) && _collectedFragrants <= 2)
        {
            //播放音效
            Debug.Log("Not Enough Ammo.");
        }

        if (Input.GetMouseButtonUp(0)&& _collectedFragrants >= 2 && _isSettingMirror)
        {
            SetDownMirror();
        }


        //RIGHT CLICK - SHIELD
        if (Input.GetMouseButtonDown(1))
        {
            if (_curShieldValue > 0.25f)
            {
                _settingShield = true;
            }
            else
            {
                _settingShield = false;
                if (_curShieldValue <= 0)
                {
                    _curShieldValue = 0;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _settingShield = false;
        }

        HandleShieldValue();
        
        HandleTimeScale();

        UpdateMouseDir();

        ShowWhereWillMirrorLand();
        
        ShowShield();
    }

    private void HandleShieldValue()
    {
        if (_settingShield)
        {
            _curShieldValue -= Time.deltaTime * 1 / maxShieldTime;
            //TODO::INTRODUCE A VARIABLE - 消耗的能量
            if (_curShieldValue <= 0.25f)
            {
                _settingShield = false;
            }
        }
        else
        {
            _curShieldValue += Time.deltaTime * 1 / shieldRecoverTime;
        }
    }

    public void ShieldDefendOnce()
    {
        _curShieldValue -= 0.25f;
    }

    private void ShowShield()
    {
        //正在举盾
        if (_settingShield)
        {
            if (!ShieldImg.activeSelf)
            {
                ShieldImg.SetActive(true);
            }
            
            ShieldImg.transform.localRotation = Quaternion.Euler(new Vector3(_mouseDir.x, _mouseDir.y, 0));
            ShieldImg.transform.right = new Vector3(_mouseDir.x, _mouseDir.y, 0);
            
            //Debug.Log(_mouseDir);
            
        }
        else
        {
            if (ShieldImg.activeSelf)
            {
                ShieldImg.SetActive(false);
            }
        }
    }
    
    private void SetDownMirror()
    {
        _isSettingMirror = false;
        _slowEffectTimePassed = 0.0f;
        Debug.Log("Reset");
        BuildMirror(_mouseDir);
    }

    private void HandleTimeScale()
    {
        if (!_isSettingMirror)
        {
            global::TimeScaleManager.Instance.ResetTimeScale();
            
            return;
        }
        global::TimeScaleManager.Instance.DoSlowMotion();
        _slowEffectTimePassed += Time.unscaledDeltaTime;
        if (_slowEffectTimePassed > _maxSlowEffectTime)
        {
            SetDownMirror();
            _slowEffectTimePassed = 0.0f;
        }
        
        
    }

    private void UpdateMouseDir()
    {
        if (_isSettingMirror || _settingShield)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePosOnScreen = Input.mousePosition;
            mousePosOnScreen.z = screenPos.z;
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            _mouseDir = new Vector2((mousePosInWorld - this.transform.position).normalized.x,
                (mousePosInWorld - this.transform.position).normalized.y);
        }
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
        this._collectedFragrants -= 2;
        var mirror = Instantiate(Mirror, this.transform.position + new Vector3(dir.x, dir.y, 0) * 0.12f, Quaternion.identity);
        var mirrorScript = mirror.GetComponent<Mirror>();
        mirrorScript.Init(dir);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MirrorFragrant"))
        {
            //Debug.Log("CollectedSomething?");
            var mirrorFragScript = other.GetComponent<BasicFrag>();
            mirrorFragScript.TakeDamage(1);
            this._collectedFragrants += mirrorFragScript.FragSize;
            
        }
    }
}

