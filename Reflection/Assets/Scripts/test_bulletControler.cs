using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullet;
//这是一阶段的无差别弹幕
public class test_bulletControler : MonoBehaviour
{
    public GameObject Bullet;
    private float ClockTime = 0;
    public float bullet1T = 3;//第一种子弹的出现时间
    public float bullet2T = 5;//第二种子弹的出现时间
    public float bulletAllT = 9;//第三种子弹的出现时间
    public float AtackCD = 0.5f;
    public float AtackTime = 0;
    public bool isStarShoot = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }
    public void start(bool isStart)
    {
        isStarShoot = isStart;
    }
    public void shoot()
    {
        if (isStarShoot == true)
        {
            ClockTime += Time.deltaTime;
            AtackTime += Time.deltaTime;


            if (ClockTime <= bullet1T && AtackTime >= AtackCD)
            {


                var obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(0, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, 0), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(0, -1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, 0), 2.0f);

                AtackTime = 0;

            }
            else if (ClockTime >= bullet1T && ClockTime < bullet2T && AtackTime >= AtackCD)
            {
                var obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, -1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, -1), 2.0f);


                AtackTime = 0;

            }
            else if (ClockTime >= bullet2T && ClockTime < bulletAllT && AtackTime >= AtackCD)
            {
                var obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(0, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, 0), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(0, -1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, 0), 2.0f);
                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(1, -1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, 1), 2.0f);

                obj = Instantiate(Bullet, transform.position, Quaternion.identity).GetComponent<NormalBullet>();
                obj.Init(1, new Vector2(-1, -1), 2.0f);

                AtackTime = 0;

            }
            if (ClockTime >= bulletAllT)
            {
                ClockTime = 0;
            }


        }
    }
}
