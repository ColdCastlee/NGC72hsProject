using Boss;
using Character;
using UnityEngine;

namespace Bullet
{
    public class NormalBullet : AbstractBullet
    {
        private bool _canBeReflected = true;
        private bool _isRefelcted = false;
        private bool _isInitialized = false;

        public override void Update()
        {
            base.Update();
            if (!_isInitialized)
            {
                //Debug.LogError("Normal Bullet Not Initialized. Check This Error.");
            }
        }

        public void Init(int bulletDamage, Vector2 moveDir, float maxVelocity)
        {
            this.BulletAtkDamage = bulletDamage;
            this.BulletMoveDir = moveDir.normalized;
            //Debug.Log(BulletMoveDir);
            this.MaxMoveSpeedXy = maxVelocity;
            _isInitialized = true;
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
                var playerSetMirrorScript = hitTarget.GetComponent<PlayerSetMirror>();
                if (playerSetMirrorScript._settingShield)
                {
                    //TODO::整合被反弹函数，防止以后更改位置过多
                    playerSetMirrorScript.ShieldDefendOnce();
                    
                    this.transform.position = playerSetMirrorScript.transform.position;
                    _isRefelcted = true;
                    this.MaxMoveSpeedXy *= 2;
                    BulletAtkDamage++;
                    this.BulletMoveDir = playerSetMirrorScript.MouseDir;
                }
                //要检查是否被反射过或者正在翻滚
                if (!_isRefelcted && !hitTarget.gameObject.GetComponent<PlayerMovement>()._isDashing)
                {
                    var playerHealth = hitTarget.transform.gameObject.GetComponent<PlayerHealth>();
                    playerHealth.TakeDamage(this.BulletAtkDamage);
                    Die();
                }
                //不反弹掉血
            }
            //BOSS
            else if (hitTarget.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
//                Debug.Log("Hit Boss");
                if (_isRefelcted)
                {
                    var bossHealth = hitTarget.transform.gameObject.GetComponent<BossHealth>();
                    bossHealth.TakeDamage(this.BulletAtkDamage);
                    Die();
                }
            }
            //ZOMBIE
            else if (hitTarget.gameObject.layer == LayerMask.NameToLayer("Zombie"))
            {
                //Debug.Log("Hit Zombie.");
                //TODO::IMPLEMENT THIS
                if (_isRefelcted)
                {
                    var _zombinHealth = hitTarget.transform.gameObject.GetComponent<ZombinHealth>();
                    this.transform.position = hitTarget.transform.position;
                    this.MaxMoveSpeedXy *= 2;
                    BulletAtkDamage++;
                    var _zombin = hitTarget.gameObject.GetComponent<EnermyAI>();
                    this.BulletMoveDir = _zombin.newVec.normalized;
                    _zombinHealth.TakeDamage(this.BulletAtkDamage);
                    Debug.Log(_zombinHealth.Hp);
                }
            }
            //MIRROR
            else if (hitTarget.gameObject.layer == LayerMask.NameToLayer("Mirror"))
            {
                Debug.Log("Hit Glass.");

                //Debug.Log("Hit");
                var mirrorScript = hitTarget.transform.gameObject.GetComponent<Mirror>();
                
                this.BulletMoveDir = mirrorScript.ReflectionDir.normalized;
                this.transform.position = mirrorScript.transform.position;
                mirrorScript.TakeDamage(1);
                _isRefelcted = true;
                this.MaxMoveSpeedXy *= 2;
                BulletAtkDamage++;
            }else if (hitTarget.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                this.Die();
            }
        }

        public override void Die()
        {
            Destroy(this.gameObject);
            //ONDIE
            //TODO::Implement this
        }
    }
}