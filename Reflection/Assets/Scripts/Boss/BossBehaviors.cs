using System.Collections;
using System.Collections.Generic;
using Boss;
using Bullet;
using UnityEngine;

public class BossBehaviors : MonoBehaviour
{
    public GameObject Player;

    public GameObject Bullet;

    //基本变量
    private Vector2 _targetDir;

    //基础射击
    public bool BasicAtkActivated = false;
    public float ShootInterval = 0.5f;
    public float ShootVelocity = 0.15f;
    public int BulletDamage = 1;
    private float _shootClock = 1.0f;
    //圆圈射击

    //小僵尸
    public float generateInterval = 5f;
    private float curTime = 0;
    private GenerateZombin _zombin;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        _zombin = gameObject.GetComponentInChildren<GenerateZombin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetDir();
        
        //基础射击状态判断
        BasicAtk();
        
        //圆圈射击状态判断
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

    private void BasicAtk()
    {
        if (!BasicAtkActivated)
        {
            return;
        }

        _shootClock += Time.deltaTime;
        if (_shootClock >= ShootInterval)
        {
            //TODO::PARENT
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<NormalBullet>().Init(BulletDamage, _targetDir, ShootVelocity);
            //bullet.transform.right = _targetDir;
            _shootClock = 0.0f;
        }
    }
}

