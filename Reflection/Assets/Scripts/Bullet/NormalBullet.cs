using Boss;
using Character;
using UnityEngine;

namespace Bullet
{
    public class NormalBullet : AbstractBullet
    {
        private bool _isRefelcted = false;
        private bool _isInitialized = false;
        RaycastHit2D _hit;
        private LayerMask _layerMasks;
        private Vector2 _rayPoint;

        private float _collisionTime = 3; //最大反弹次数，未实装


        public override void Update()
        {
            base.Update();
            if (!_isInitialized)
            {
                Debug.LogError("Bullet Not Initialized. Check This Error.");
            }

            HandleRayCastingDetect();
        }

        private void HandleRayCastingDetect()
        {
            Debug.DrawLine(this.transform.position,
                this.transform.position + new Vector3(BulletMoveDir.x, BulletMoveDir.y, 0), Color.yellow);
            //镜子
            _hit = Physics2D.Raycast(transform.position, BulletMoveDir, 10.0f, MirrorLayerMask);
            if (_hit)
            {
                if (Mathf.Abs(_hit.distance) <= 0.02f)
                {
//                    Debug.Log("Hit");
                    var mirrorScript = _hit.transform.gameObject.GetComponent<Mirror>();
                    this.BulletMoveDir = mirrorScript.ReflectionDir.normalized;
                    mirrorScript.TakeDamage(1);
                    _isRefelcted = true;
                    BulletAtkDamage++;
                }

                //TODO::Add LayerMask Check Code.
            }

            //玩家
            _hit = Physics2D.Raycast(transform.position, BulletMoveDir, 10.0f, PlayerLayerMask);
            if (_hit)
            {
                if (Mathf.Abs(_hit.distance) <= 0.02f)
                {
//                    Debug.Log("Hit");
                    //要检查是否被反射过
                    if (!_isRefelcted)
                    {
                        var playerHealth = _hit.transform.gameObject.GetComponent<PlayerHealth>();
                        playerHealth.TakeDamage(this.BulletAtkDamage);
                        Die();
                    }
                }
            }

            //BOSS
            _hit = Physics2D.Raycast(transform.position, BulletMoveDir, 10.0f, BossLayerMask);
            if (_hit)
            {
                if (Mathf.Abs(_hit.distance) <= 0.02f)
                {
//                    Debug.Log("Hit");
                    if (_isRefelcted)
                    {
                        var bossHealth = _hit.transform.gameObject.GetComponent<BossHealth>();
                        bossHealth.TakeDamage(this.BulletAtkDamage);
                        Die();
                    }
                }
            }

            //墙壁
            _hit = Physics2D.Raycast(transform.position, BulletMoveDir, 10.0f, WallLayerMask);
            if (_hit)
            {
                if (Mathf.Abs(_hit.distance) <= 0.02f)
                {
//                    Debug.Log("Hit");
                    this.Die();
                }
            }

            //todo::还有僵尸
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
//            if (hitTarget.gameObject.layer == PlayerLayerMask)
//            {
//                Debug.Log("Hit Player");
//                //TODO::IMPLEMENT THIS
//                //不反弹掉血
//            }
//            else if (hitTarget.gameObject.layer == BossLayerMask)
//            {
//                Debug.Log("Hit Boss");
//                //TODO::IMPLEMENT THIS
//            }
//            else if (hitTarget.gameObject.layer == ZombieLayerMask)
//            {
//                Debug.Log("Hit Zombie.");
//                //TODO::IMPLEMENT THIS
//            }
//            else if (hitTarget.gameObject.layer == MirrorLayerMask)
//            {
//                //TODO::IMPLEMENT THIS
//                Debug.Log("Hit Glass.");
//                var _reflection = hitTarget.gameObject.GetComponent<Mirror>();
//
//                _collisionTime++;
//                //如果击中的是第三次,那么毁掉自己.
//            }
        }

        public override void Die()
        {
            Destroy(this.gameObject);
            //ONDIE
            //TODO::Implement this
        }
    }
}