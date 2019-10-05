
using Character;
using UnityEngine;
using  Pathfinding;

public class EnermyAI : MonoBehaviour
{

    private Transform target;
    
    public float speed = 2000f;

    public float nextWaypointDistance = 3f;

    private Path _path;

    private int currentWaypoint = 0;
    
    
    
    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;
    private Transform Boss;
    private LineRenderer _line;
    private float stopTime;
    private float curTime;
    private bool reachingEndOfPath = false;//判断是否到终点
    
    //反射方向
    public Vector2 direction;//这个是运动的方向
    public Vector2 newVec;
    private float radian;

    private Transform zombin;
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
        _line.positionCount = 2;
        _line.startColor = _line.endColor = Color.white;
        
        radian = UnityEngine.Random.Range(-60f, 60f);
        curTime = 0;

        zombin = gameObject.GetComponentInChildren<Transform>();

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

    private void Update()
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
        
        Vector2 direction = ((Vector2) _path.vectorPath[currentWaypoint] - _rigidbody2D.position).normalized;
        
        Vector2 force = direction * speed * Time.deltaTime;
        
        _rigidbody2D.AddForce(force);
        
        direction = _rigidbody2D.velocity;
        
        //        //得到垂直的向量.
    //        var _pointToBoss = Boss.position - this.transform.position;
    //        var _pointToBossVec2 = new Vector2(_pointToBoss.x,_pointToBoss.y);
    //        Vector2 _verticalPos;
    //       
    //        if (direction.x != 0)
    //        {
    //            _verticalPos = new Vector2(-direction.y/direction.x,1).normalized;
    //        }
    //        else
    //            _verticalPos = new Vector2(1,0);
    //
    //        if (direction.y == 0)
    //             _verticalPos.y = 1;
    //        if (Vector2.Dot(_pointToBossVec2, _verticalPos) >= 0)
    //        {
    //            
    //        }
    //        else
    //        {
    //            _verticalPos = -_verticalPos;
    //        }
        
        float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
         var tempPosition = new Vector3(this.transform.position.x,this.transform.position.y,0);
         Vector3  targetPos=(this.transform.position- target.transform.position).normalized;

         targetPos.z = 0;
         Vector3 result =  Quaternion.AngleAxis(radian , Vector3.forward)* targetPos;
//         Debug.Log(targetPos+""+radian+""+result);

        newVec = result;
        _line.SetPosition(0,tempPosition);
        _line.SetPosition(1, tempPosition+result*0.3f);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            var playerHealth = other.transform.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1);
        }
    }
}
