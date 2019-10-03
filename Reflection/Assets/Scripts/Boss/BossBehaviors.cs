using System.Collections;
using System.Collections.Generic;
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

    //

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetDir();
        
        //基础射击状态判断
        BasicAtk();
        
        //圆圈射击状态判断
        
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

