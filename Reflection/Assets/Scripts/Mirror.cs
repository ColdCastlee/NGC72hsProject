using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private Vector2 _reflectionDir = Vector2.right;
    [SerializeField]
    private LineRenderer _line;
    
    public Vector2 ReflectionDir => _reflectionDir;

    private void Start()
    {
        _line = GetComponentInChildren<LineRenderer>();
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
        ShowReflectionDir();
    }
}
