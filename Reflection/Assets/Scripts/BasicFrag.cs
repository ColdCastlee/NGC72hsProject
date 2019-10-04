using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;


public class BasicFrag : AbstractActorHealth
{
    public float LiveTime = 6.0f;
    private float _livedTime = 0.0f;
    
    public int FragSize = 1;
    public UnityEvent OnCollectedFragrant;
    private Animator _animator;
    private float _generateTimer = 0.0f;
    public float FinishGenerateTime = 2.0f;
    private bool _finishedInitializing = false;

    public bool FinishedInitializing => _finishedInitializing;

    private float _finalSize;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        HandleSizeAndRotation();
        
        CheckDeath();
        
        CheckLiving();
    }

    private void HandleSizeAndRotation()
    {
        if (!_finishedInitializing)
        {
            _generateTimer += Time.deltaTime;
            if (_generateTimer <= FinishGenerateTime)
            {
                this.transform.Rotate(0,0,90*Time.deltaTime);
                this.transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one * _finalSize,
                    _generateTimer / FinishGenerateTime);
                //音效
            }
            else
            {
                _finishedInitializing = true;
            }
        }
    }

    public void Init()
    {
        this.Hp = 1;
        _animator = GetComponent<Animator>();
        transform.Rotate(0,0,Random.Range(0,360));
        transform.localScale = Vector3.one * 0.1f;
        FragSize = Random.Range(1, 3);
        switch (FragSize)
        {
            case 1:
                _finalSize = 0.5f;
                break;
            case 2:
                _finalSize = 0.8f;
                break;
            case 3:
                _finalSize = 1.2f;
                break;
        }

        this._finishedInitializing = false;
    }

    private void CheckLiving()
    {
        if (FinishedInitializing)
        {
            _livedTime += Time.deltaTime;
            if (_livedTime > LiveTime)
            {
                Die();
            }
        }
    }

    public override void CheckDeath()
    {
        if (!IsDied())
        {
            return;
        }
        
        Die();
    }

    private void Die()
    {
        OnCollectedFragrant.Invoke();
        Destroy(this.gameObject);
    }
}
