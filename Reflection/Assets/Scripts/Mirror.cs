using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mirror : AbstractActorHealth
{
    private Vector2 _reflectionDir = Vector2.right;
    public UnityEvent OnMirrorBroken;
    [SerializeField]
    private LineRenderer _line;

    public Vector2 Debug_ReflectionDir;
    
    public Vector2 ReflectionDir => _reflectionDir;

    private void Start()
    {
        _line = GetComponentInChildren<LineRenderer>();
        this.Hp = 3;
    }

    public void Init(Vector2 reflectionDir)
    {
        this._reflectionDir = reflectionDir;
    }

    public void ShowReflectionDir()
    {
        _line.positionCount = 2;
        _line.SetPosition(0,Vector3.zero);
        _line.SetPosition(1,new Vector3(_reflectionDir.x, _reflectionDir.y, 0) * 0.2f);
        _line.startWidth = _line.endWidth = 0.02f;
        _line.startColor = _line.endColor = Color.cyan;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Mirror Health: " + this.Hp);
        }
        ShowReflectionDir();
        Debug_ReflectionDir = _reflectionDir;
        
        CheckDeath();
    }

    public override void CheckDeath()
    {
        if (!IsDied())
        {
            return;
        }   
        
        Debug.Log("Glass Died");
        
        OnMirrorBroken.Invoke();
        Destroy(this.gameObject);
    }

}
