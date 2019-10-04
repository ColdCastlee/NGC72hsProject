using System;
using System.Collections;
using System.Collections.Generic;
using Boss;
using Bullet;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class BossBehaviors : MonoBehaviour
{
    //普通弹幕射击4s，每0.5s射击一次
    //普通弹幕射击完成后进行无差别弹幕射击，2套。每套9秒
    //2套完成后进行3s普通弹幕射击，然后进行病毒攻击
    public enum States
    {
        LevelOneInit,
        BasicAtk,
        CircularAtk,
        CrazyAtk,
        LevelTwoInit,
        SpecialAtk,
        Move
    }

    private StateMachine<States> fsm;

    public GameObject Player;
    public GameObject Bullet;
    public GameObject VirusBullet;

    private BossMovement _bossMovement;
    private BossHealth _bossHealth;


    //基本变量
    private Vector2 _targetDir;
    private float _stateTimer = 0.0f;
    private float _changeStateTime = 4.0f;
    private States lastState;
    private bool _secondStateMode = false;
    private bool _crazyMode = false;
    
    
    //基础射击
    [Header("基础射击")] [FormerlySerializedAs("ShootInterval")]
    public float BasicShootInterval = 0.5f;

    [FormerlySerializedAs("ShootVelocity")]
    public float BasicShootVelocity = 0.15f;

    [FormerlySerializedAs("BulletDamage")] public int BasicBulletDamage = 1;

    private float _basicShootClock = 1.0f;

    //圆圈射击
    [Header("无差别角度射击")] public float CircularShootInterval = 1.0f;
    public float CircularShootVelocity = 0.4f;
    public int CircularBulletDamage = 1;
    private float _circularShootClock = 0.0f;
    private float _overAllCircularTime = 9.0f;
    private float _firstAtkModeTime = 3.0f;
    private float _secondAtkModeTime = 5.0f;

    //病毒攻击
    [Header("病毒射击")] public float VirusShootInterval = 1.0f;
    public float VirusShootVelocity = 0.4f;
    public int VirusShootDamage = 2;
    public float VirusBulletLiveTime = 5.0f;
    private float _virusShootClock = 0.0f;
    private float _normalAtkTime = 3.0f;
    private float _virusAtkTime = 3.0f;

    //移动
    private float _b4MoveCountClock = 0.0f;
    public float HowManyTimeMoveOnce = 5.0f;
    public float MoveInterval = 2.0f;
    public float MoveVelocity = 0.3f;
    
    //Crazy Attack
    private float _overallCrazyAtkTime = 10.0f;
    public float CrazyShootVelocity = 2.0f;
    public int CrazyShootDamage = 1;
    private float _crazyShootInterval = 0.5f;
    private float _crazyFirstTime = 4.0f;
    private float _crazySecondTime = 7.0f;
    private float _crazyAtkTimer = 0.0f;
    
    
    private void Awake()
    {
        fsm = StateMachine<States>.Initialize(this);
    }

    //小僵尸
    public float generateInterval = 5f;
    private float curTime = 0;
    private GenerateZombin _zombin;
    
    // Start is called before the first frame update
    void Start()
    {
        _bossHealth = GetComponent<BossHealth>();
        _bossMovement = GetComponent<BossMovement>();
        Player = GameObject.Find("Player");
        fsm.ChangeState(States.LevelOneInit);
        _zombin = gameObject.GetComponentInChildren<GenerateZombin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetDir();

        if (fsm.State != States.Move)
        {
            _b4MoveCountClock += Time.deltaTime;            
        }
        if (_b4MoveCountClock > HowManyTimeMoveOnce)
        {
            _b4MoveCountClock = 0.0f;
            fsm.ChangeState(States.Move);
        }

        //二阶段开始
        if (_bossHealth.HealthPercentage <= 0.5f && !_secondStateMode)
        {
            _secondStateMode = true;
            _crazyMode = false;
            fsm.ChangeState(States.LevelTwoInit);
        }
        //一阶段狂化
        if (_bossHealth.HealthPercentage <= 0.75f && !_secondStateMode && !_crazyMode)
        {
            _crazyMode = true;
            fsm.ChangeState(States.CrazyAtk);
        }
        //二阶段狂化
        if (_bossHealth.HealthPercentage <= 0.25f && _secondStateMode && !_crazyMode)
        {
            _crazyMode = true;
            fsm.ChangeState(States.CrazyAtk);
        }
        //生成小僵尸
        curTime += Time.deltaTime;

        generateZombin();

    }

    private void generateZombin()
    {
        if (curTime >= generateInterval)
        {
            _zombin.generateZombin();
            curTime = 0;
        }
    }
    private void UpdateTargetDir()
    {
        Debug.DrawLine(this.transform.position, Player.transform.position, Color.black);
        _targetDir = (Player.transform.position - this.transform.position).normalized;
    }

    //
    private void BasicAtk()
    {
        _basicShootClock += Time.deltaTime;

        if (_basicShootClock >= BasicShootInterval)
        {
            //TODO::PARENT
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>().Init(BasicBulletDamage, _targetDir, BasicShootVelocity);
            //bullet.transform.right = _targetDir;
            _basicShootClock = 0.0f;
        }
    }

    //90°无差别
    private void CircularFirstMode()
    {
        _circularShootClock += Time.deltaTime;
        if (_circularShootClock >= CircularShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(90, 0);
            //bullet.transform.right = _targetDir;
            _circularShootClock = 0.0f;
        }
    }

    //90°倾斜无差别
    private void CircularSecondMode()
    {
        _circularShootClock += Time.deltaTime;
        if (_circularShootClock >= CircularShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(90, 45);
            //bullet.transform.right = _targetDir;
            _circularShootClock = 0.0f;
        }
    }

    //45°无差别
    private void CircularThirdMode()
    {
        _circularShootClock += Time.deltaTime;
        if (_circularShootClock >= CircularShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(45, 0);
            //bullet.transform.right = _targetDir;
            _circularShootClock = 0.0f;
        }
    }

    //30°无差别
    private void CrazyAtkFirstMode()
    {
        _crazyAtkTimer += Time.deltaTime;
        int randomAngle = Random.Range(0, 10);
        if (_crazyAtkTimer >= _crazyShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(30, 0 + randomAngle);
            //bullet.transform.right = _targetDir;
            _crazyAtkTimer = 0.0f;
        }
    }
    
    //30°无差别 15°偏角
    private void CrazyAtkSecondMode()
    {
        _crazyAtkTimer += Time.deltaTime;
        int randomAngle = Random.Range(0, 10);
        if (_crazyAtkTimer >= _crazyShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(30, 0 + randomAngle + 15);
            //bullet.transform.right = _targetDir;
            _crazyAtkTimer = 0.0f;
        }
    }
    
    //全方位角 10°无差别
    private void CrazyAtkThirdMode()
    {
        _crazyAtkTimer += Time.deltaTime;
        int randomAngle = Random.Range(0, 5);
        if (_crazyAtkTimer >= _crazyShootInterval)
        {
            //TODO::PARENT
            GenerateDegreeWaveOnce(10, 0 + randomAngle);
            //bullet.transform.right = _targetDir;
            _crazyAtkTimer = 0.0f;
        }
    }
    

    //基本无差别函数
    private void GenerateDegreeWaveOnce(float degreeInterval, float offsetAngle)
    {
        //从offset Angle出发，每隔degreeInterval便射出一个子弹，直到360°
        int bulletNum = Mathf.FloorToInt(360.0f / degreeInterval);
        float theta = offsetAngle;
        for (int i = 0; i < bulletNum; i++)
        {
            theta = offsetAngle + i * degreeInterval;
            float dirX = Mathf.Cos(Mathf.Deg2Rad * theta);
            float dirY = Mathf.Sin(Mathf.Deg2Rad * theta);
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>()
                .Init(CircularBulletDamage, new Vector2(dirX, dirY), CircularShootVelocity);
        }
    }

    //
    private void GenerateCavityWaveOnce(Vector2 dir)
    {
        //根据dir得出角度，从0度出发，所以是：
        float angle = Vector2.Angle(Vector2.right, dir);
        //从offset Angle出发，每隔degreeInterval便射出一个子弹，直到360°
        int bulletNum = Mathf.FloorToInt(360.0f / 10.0f);
        float theta = 0;
        for (int i = 0; i < bulletNum; i++)
        {
            theta = i * 10.0f;
            if (theta - angle < 10.0f && theta - angle >= 0.0f)
            {
                continue;
            }

            float dirX = Mathf.Cos(Mathf.Deg2Rad * theta);
            float dirY = Mathf.Sin(Mathf.Deg2Rad * theta);
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>().Init(BasicBulletDamage, new Vector2(dirX, dirY), BasicShootVelocity);
        }
    }

    //Special Atk
    private void VirusAtk()
    {
        _virusShootClock += Time.deltaTime;
        if (_virusShootClock > VirusShootInterval)
        {
            var bullet = Instantiate(VirusBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<VirusBullet>().Init(VirusShootDamage, Player, VirusShootVelocity, VirusBulletLiveTime);
            //bullet.transform.right = _targetDir;
            _virusShootClock = 0.0f;
        }
    }

    
    private void UpdateVariables(bool isSecondState)
    {
        if (isSecondState)
        {
            
        }
        else
        {
            
        }
    }

    
    //TODO:: USE IENUMERATOR TO GENERATE FEW WAVES OF BULLET

    #region StateMachine

    //Level One Init
    IEnumerator LevelOneInit_Enter()
    {
        Debug.Log("Hello Young Man...");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Let's Begin Our Dance...");
        //动画 镜头 角色移动 镜子出现
        yield return new WaitForSeconds(3.0f);
        fsm.ChangeState(States.BasicAtk);
    }
    //Level Two Init
    IEnumerator LevelTwoInit_Enter()
    {
        Debug.Log("Hello, I am...Your Master.");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("So Can you please die for me?");
        //动画 镜头 角色移动 镜子出现
        yield return new WaitForSeconds(3.0f);
        fsm.ChangeState(States.BasicAtk);
    }
    
    
    //Basic Atk
    void BasicAtk_Enter()
    {
        if (!_secondStateMode)
        {
            BasicShootInterval = 0.5f;
            BasicShootVelocity = 1.5f;
        }
        else
        {
            BasicShootInterval = 0.4f;
            BasicShootVelocity = 2.0f;
        }

    }

    void BasicAtk_Update()
    {
        _stateTimer += Time.deltaTime;
        BasicAtk();
        if (_stateTimer > _changeStateTime)
        {
            _stateTimer = 0.0f;
            fsm.ChangeState(States.CircularAtk);
        }
    }

    void BasicAtk_Exit()
    {
        _stateTimer = 0.0f;
        lastState = States.BasicAtk;
    }

    //Circular Atk
    void CircularAtk_Enter()
    {
        if (!_secondStateMode)
        {
            CircularShootInterval = 0.5f;
            CircularShootVelocity = 1.5f;
        }
        else
        {
            CircularShootInterval = 0.4f;
            CircularShootVelocity = 2.0f;
        }
    }

    void CircularAtk_Update()
    {
        _stateTimer += Time.deltaTime;

        if (_stateTimer % _overAllCircularTime < _firstAtkModeTime)
        {
            CircularFirstMode();
            //first Mode
        }
        else if (_stateTimer % _overAllCircularTime < _secondAtkModeTime)
        {
            CircularSecondMode();
            //second Mode
        }
        else
        {
            CircularThirdMode();
            //Third Mode
        }

        if (_secondStateMode)
        {
            BasicAtk();
        }

        if (_stateTimer > 2 * _overAllCircularTime)
        {
            fsm.ChangeState(States.SpecialAtk);
        }
    }

    void CircularAtk_Exit()
    {
        lastState = States.CircularAtk;
        _stateTimer = 0.0f;
    }

    //Crazy Atk
    void CrazyAtk_Enter()
    {
        if (!_secondStateMode)
        {
            _crazyShootInterval = 0.5f;
            CrazyShootVelocity = 1.5f;
        }
        else
        {
            _crazyShootInterval = 0.4f;
            CrazyShootVelocity = 2.0f;
        }
    }

    void CrazyAtk_Update()
    {
        _stateTimer += Time.deltaTime;

        if (_stateTimer % _overallCrazyAtkTime < _firstAtkModeTime)
        {
            CrazyAtkFirstMode();
            //first Mode
        }
        else if (_stateTimer % _overallCrazyAtkTime < _secondAtkModeTime)
        {
            CrazyAtkSecondMode();
            //second Mode
        }
        else
        {
            CrazyAtkThirdMode();
            //Third Mode
        }

        if (_secondStateMode)
        {
            BasicAtk();
        }

        if (_stateTimer > _overallCrazyAtkTime)
        {
            fsm.ChangeState(States.BasicAtk);
        }
    }

    void CrazyAtk_Exit()
    {
        _stateTimer = 0.0f;
        lastState = States.CrazyAtk;
    }

    //Special Atk
    void SpecialAtk_Enter()
    {
        if (!_secondStateMode)
        {
            VirusShootInterval = 0.5f;
            VirusShootVelocity = 1.5f;
        }
        else
        {
            VirusShootInterval = 0.4f;
            VirusShootVelocity = 2.0f;
        }
    }

    void SpecialAtk_Update()
    {
        _stateTimer += Time.deltaTime;

        if (_stateTimer < _normalAtkTime)
        {
            BasicAtk();
        }
        else if (_stateTimer < _normalAtkTime + _virusAtkTime)
        {
            VirusAtk();
        }
        else
        {
            fsm.ChangeState(States.BasicAtk);
        }
    }

    void SpecialAtk_Exit()
    {
        _stateTimer = 0.0f;
        lastState = States.SpecialAtk;
    }

    //MOVE State
    void Move_Enter()
    {
        
    }
    
    void Move_Update()
    {
        _bossMovement.Move(_targetDir);
        _stateTimer += Time.deltaTime;
        if (_stateTimer > MoveInterval)
        {
            fsm.ChangeState(lastState);   
        }
    }

    void Move_Exit()
    {
        lastState = States.Move;
        _stateTimer = 0.0f;
    }
    
    #endregion
}