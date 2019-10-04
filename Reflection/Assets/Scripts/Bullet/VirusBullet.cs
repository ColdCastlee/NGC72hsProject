using System.Collections;
using System.Collections.Generic;
using Bullet;
using Character;
using UnityEngine;

public class VirusBullet : NormalBullet
{
    private GameObject _player;

    public override void Update()
    {
        base.Update();
        
        CalculateDirToPlayer();
    }
    
    public void Init(int bulletDamage, GameObject player, float maxVelocity, float liveTime)
    {
        _player = player;
        this.BulletAtkDamage = bulletDamage;
        CalculateBulletVelocity();
        this.MaxMoveSpeedXy = maxVelocity;
        this.MaxLiveTime = liveTime;
    }

    private void CalculateDirToPlayer()
    {
        var dir = (_player.transform.position - transform.position).normalized;
        this.BulletMoveDir = dir;
    }

    public override void Move()
    {
        transform.Translate(BulletDeltaMovement);
    }

    public override void CalculateBulletVelocity()
    {
        this.BulletDeltaMovement = this.MaxMoveSpeedXy * BulletMoveDir * Time.deltaTime;
    }

    public override void OnTriggerEnter2D(Collider2D hitTarget)
    {
        //Debug.Log("Bullet Hitted: "+ hitTarget.name + "Layer: " + hitTarget.gameObject.layer);
        //玩家
        if (hitTarget.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("HIT PLAYER");
            var playerHealth = hitTarget.transform.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(this.BulletAtkDamage);
            Die();
        }
    }

    public override void Die()
    {
        Destroy(this.gameObject);
        //ONDIE
        //TODO::Implement this
    }
    
}