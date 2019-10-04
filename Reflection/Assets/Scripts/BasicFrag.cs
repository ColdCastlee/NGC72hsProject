using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BasicFrag : AbstractActorHealth
{
    public int FragSize = 1;
    public UnityEvent OnCollectedFragrant;
    private Animator _animator;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        CheckDeath();
    }

    public void Init()
    {
        this.Hp = 1;
        _animator = GetComponent<Animator>();
        transform.Rotate(0,0,Random.Range(0,360));
        FragSize = Random.Range(1, 3);
        switch (FragSize)
        {
            case 1:
                this.transform.localScale = Vector3.one * 0.5f;
                break;
            case 2:
                this.transform.localScale = Vector3.one * 1.0f;
                break;
            case 3:
                this.transform.localScale = Vector3.one * 1.5f;
                break;
        }
    }

    public override void CheckDeath()
    {
        if (!IsDied())
        {
            return;
        }
        
        OnCollectedFragrant.Invoke();
        Destroy(this.gameObject);
    }
}
