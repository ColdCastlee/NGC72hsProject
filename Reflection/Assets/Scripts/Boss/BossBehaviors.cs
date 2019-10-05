using System;
using System.Collections;
using System.Collections.Generic;
using Boss;
using Bullet;
using UnityEngine;
using MonsterLove.StateMachine;
using ReadyGamerOne.MemorySystem;
using UnityEditor;
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
        Move,
        ZombieAtk,
        Die
    }

    private Animator _bossAnimator;

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
    private float RandomMoveOnceTime = 5.0f;
    private float _moveTimer = 0.0f;
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
    private AudioMgr BgmPlayer;
    
    
    bool isPlayedState1_2 = false;
    bool isPlayedState2_2 = false;
    
    
    private void Awake()
    {
        fsm = StateMachine<States>.Initialize(this);
        
        BgmPlayer = this.gameObject.AddComponent<AudioMgr>();
        BgmPlayer.EffectVolume = 0.5f;
        MemoryMgr.LoadAssetFromResourceDir<AudioClip>(typeof(AudioName),"Audio/",(name,clip)=>
        {
            if(AudioMgr.Instance.audioclips.ContainsKey(name)==false)
                AudioMgr.Instance.audioclips.Add(name, clip);
        });
        
    }

    //小僵尸
    public float generateInterval = 5f;
    private float curTime = 0;
    private GenerateZombin _zombin;

    // Start is called before the first frame update
    void Start()
    {
        _bossAnimator = GetComponent<Animator>();
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

        if (_bossHealth.IsDied())
        {
            fsm.ChangeState(States.Die);
        }
    }

    private void MoveToPlayer()
    {
        
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
           BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
        
            BgmPlayer.PlayEffect(AudioName._bossBullet);
            theta = offsetAngle + i * degreeInterval;
            float dirX = Mathf.Cos(Mathf.Deg2Rad * theta);
            float dirY = Mathf.Sin(Mathf.Deg2Rad * theta);
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>()
                .Init(CircularBulletDamage, new Vector2(dirX, dirY), CircularShootVelocity);
        }
    }

    IEnumerator GenerateCircularWaveOnce(float angleFrom, float angleTo, float angleInterval, float fireInterval,
        float offset = 0.0f)
    {
        for (float angle = angleFrom + offset; angle <= angleTo + offset; angle += angleInterval)
        {
            BgmPlayer.PlayEffect(AudioName._bossBullet);
           // AudioMgr.Instance.PlayEffect(AudioName._bossBullet);
            float dirX = Mathf.Cos(Mathf.Deg2Rad * angle);
            float dirY = Mathf.Sin(Mathf.Deg2Rad * angle);
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>()
                .Init(CircularBulletDamage, new Vector2(dirX, dirY), CircularShootVelocity);
            yield return new WaitForSeconds(fireInterval);
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
            BgmPlayer.PlayEffect(AudioName._bossBullet);
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

            BgmPlayer.PlayEffect(AudioName._bossBullet);
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
        _bossAnimator.Play("FirstStateInit");
     
        BgmPlayer.PlayBgm(AudioName._stage1_1);

        Debug.Log("Hello Young Man...");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Let's Begin Our Dance...");
        //动画 镜头 角色移动 镜子出现
        yield return new WaitForSeconds(4.0f);
        fsm.ChangeState(States.BasicAtk);
    }

    //Level Two Init
    IEnumerator LevelTwoInit_Enter()
    {
        _bossAnimator.Play("SecondStateInit");
        BgmPlayer.PlayBgm(AudioName._stage2_1);
        AudioMgr.Instance.PlayEffect(AudioName._BossBomb);
        Debug.Log("Hello, I am...Your Master.");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("So Can you please die for me?");
        //动画 镜头 角色移动 镜子出现
        yield return new WaitForSeconds(9.0f);
        fsm.ChangeState(States.BasicAtk);
    }


    //Basic Atk
    void BasicAtk_Enter()
    {
        if (!_secondStateMode)
        {
            BasicShootInterval = 0.6f;
            BasicShootVelocity = 4.0f;
        }
        else
        {
            BasicShootInterval = 0.3f;
            BasicShootVelocity = 4.5f;
        }
    }

    void BasicAtk_Update()
    {
        
        _stateTimer += Time.deltaTime;
        
        BasicAtk();
        
        if (_secondStateMode)
        {
            _bossAnimator.Play("SecondStateBasic");
        }
        else
        {
            _bossAnimator.Play("BasicAtkUpdate");
        }
        
        if (_stateTimer > _changeStateTime)
        {
            _stateTimer = 0.0f;
            fsm.ChangeState(States.CircularAtk);
        }
        
    }

    void BasicAtk_Exit()
    {
        lastState = States.BasicAtk;
    }

    //Circular Atk
    void CircularAtk_Enter()
    {
        if (!_secondStateMode)
        {
            CircularShootInterval = 0.8f;
            CircularShootVelocity = 4.0f;
        }
        else
        {
            CircularShootInterval = 0.4f;
            CircularShootVelocity = 4.5f;
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
            _bossAnimator.Play("SecondStateCircular");
            BasicAtk();
        }
        else
        {
            _bossAnimator.Play("CircularAtkUpdate");
        }

        if (_stateTimer > 2 * _overAllCircularTime)
        {
            _stateTimer = 0.0f;
            fsm.ChangeState(States.SpecialAtk);
        }
    }

    void CircularAtk_Exit()
    {
        lastState = States.CircularAtk;
    }

    //Crazy Atk
    void CrazyAtk_Enter()
    {
        StartCoroutine(GenerateCircularWaveOnce(0.0f, 360.0f, 10.0f, 0.05f,Random.Range(0,360)));
        
        if (!_secondStateMode)
        {
            _crazyShootInterval = 0.6f;
            CrazyShootVelocity = 4.0f;
           // AudioMgr.Instance.CloseEffect(AudioName._stage1_1);

           if (!isPlayedState1_2)
           {
               BgmPlayer.PlayBgm(AudioName._stage1_2);
               isPlayedState1_2 = true;
           }
            
            _crazyShootInterval = 0.5f;
            CrazyShootVelocity = 1.5f;
        }
        else
        {
          
            if (!isPlayedState2_2)
            {
                BgmPlayer.PlayBgm(AudioName._stage2_2);
                isPlayedState2_2 = true;
            }
         
            _crazyShootInterval = 0.4f;
            CrazyShootVelocity = 4.5f;
        }
    }

    void CrazyAtk_Update()
    {
        _stateTimer += Time.deltaTime;
        
        
        
        if (_stateTimer % _overallCrazyAtkTime < _crazyFirstTime)
        {
            CrazyAtkFirstMode();
            //first Mode
        }
        else if (_stateTimer % _overallCrazyAtkTime < _crazySecondTime)
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
            CircularThirdMode();
            BasicAtk();
        }

        if (_stateTimer > _overallCrazyAtkTime)
        {
            StartCoroutine(GenerateCircularWaveOnce(0.0f, 360.0f, 10.0f, 0.05f,Random.Range(0,360)));
            _stateTimer = 0.0f;
            fsm.ChangeState(States.BasicAtk);
        }
    }

    void CrazyAtk_Exit()
    {
        if (_secondStateMode)
        {
            StartCoroutine(GenerateCircularWaveOnce(0.0f, 360.0f, 10.0f, 0.05f,Random.Range(0,360)));
        }
        lastState = States.CrazyAtk;
    }

    //Special Atk
    void SpecialAtk_Enter()
    {
        if (!_secondStateMode)
        {
            VirusShootInterval = 0.4f;
            VirusShootVelocity = 3.5f;
        }
        else
        {
            VirusShootInterval = 0.2f;
            VirusShootVelocity = 4.0f;
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
            _stateTimer = 0.0f;
            fsm.ChangeState(States.BasicAtk);
        }

        if (_secondStateMode)
        {
            _bossAnimator.Play("SecondStateVirus");
        }
        else
        {
            _bossAnimator.Play("VirusAtkUpdate");
        }
    }

    void SpecialAtk_Exit()
    {
        lastState = States.SpecialAtk;
    }

    //MOVE State
    void Move_Enter()
    {
        
    }

    void Move_Update()
    {
        _bossMovement.Move(_targetDir);
        if (_secondStateMode)
        {
            _bossAnimator.Play("Boss2Move");
        }
        else
        {
            _bossAnimator.Play("Move");            
        }
        
        _moveTimer += Time.deltaTime;
        if (_secondStateMode)
        {
            BasicAtk();
            BasicAtk();
        }
        
        if (_moveTimer > MoveInterval)
        {
            fsm.ChangeState(lastState);
        }
    }

    void Move_Exit()
    {
        lastState = States.Move;
        _moveTimer = 0.0f;
    }

    void Die_Update()
    {
        this._bossAnimator.Play("Die");
    }

    #endregion
}