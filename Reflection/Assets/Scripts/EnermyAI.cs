using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using  Pathfinding;
using Random = System.Random;

public class EnermyAI : MonoBehaviour
{

    public Transform target;
    
    public float speed = 2000f;

    public float nextWaypointDistance = 3f;

    private Path _path;

    private int currentWaypoint = 0;

    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;
    public Transform Boss;
    private LineRenderer _line;
    private float stopTime;
    private float curTime;
    private bool reachingEndOfPath = false;//判断是否到终点
    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.Find("Player").transform;
        Boss = GameObject.Find("Boss").transform;
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath",0f,.5f);
        _line = this.gameObject.GetComponent<LineRenderer>();
        _line.startWidth = 0.05f;
        _line.endWidth = 0.05f;
        _line.material = new Material(Shader.Find("Default"));
        _line.positionCount = 2;
        curTime = 0;
    }

    void UpdatePath()
    {
        if(_seeker.IsDone())
            _seeker.StartPath(_rigidbody2D.position, target.position, OnPathComplete);
    }
    

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;//如果没有问题就把当前路径设置为新路径
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if(_path== null)
            return;
        if (currentWaypoint >= _path.vectorPath.Count)
        {
            reachingEndOfPath = true;
            return;
        }
        else
        {
            reachingEndOfPath = false;
        }
        
        stopTime = UnityEngine.Random.Range(3f, 5f);
        Vector2 direction = ((Vector2) _path.vectorPath[currentWaypoint] - _rigidbody2D.position).normalized;
        
        Vector2 force = direction * speed * Time.deltaTime;
        
        _rigidbody2D.AddForce(force);

        
        direction = _rigidbody2D.velocity;
        
        //得到垂直的向量.
        var _pointToBoss = Boss.position - this.transform.position;
        var _pointToBossVec2 = new Vector2(_pointToBoss.x,_pointToBoss.y);
        Vector2 _verticalPos;
       
        if (direction.x != 0)
        {
            _verticalPos = new Vector2(-direction.y/direction.x,1).normalized;
        }
        else
            _verticalPos = new Vector2(1,0);

        if (direction.y == 0)
             _verticalPos.y = 1;
        if (Vector2.Dot(_pointToBossVec2, _verticalPos) >= 0)
        {
            
        }
        else
        {
            _verticalPos = -_verticalPos;
        }
        
        
        var tempPosition = new Vector2(this.transform.position.x,this.transform.position.y);
        float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        _line.SetPosition(0,tempPosition);
        _line.SetPosition(1, tempPosition+_verticalPos);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("受到了10点伤害");
            //TODO::小僵尸对玩家进行伤害。
        }

       
    }
}
