using UnityEngine;

namespace Bullet
{
    public class NormalBullet : AbstractBullet
    {
        private bool _isInitialized = false;
        RaycastHit2D hit;
        private LayerMask _layerMasks;
        private Vector2 rayPoint;
        private float collisionTime;//最大反弹次数

        public override void Start()
        {
            base.Start();
            this.BulletMoveDir = new Vector2(.1f,-.1f);
            
        }

        public override void Update()
        {
            base.Update();
            if (!_isInitialized)
            {
                //Debug.LogError("Bullet Not Initialized. Check This Error.");
            }
            
            Move();
            
            //
            hit = Physics2D.Raycast(transform.position, BulletMoveDir,50.0f,MirrorLayerMask);
            if(hit)
            {
//                Debug.Log("Hit");
                var mirrorScript = hit.transform.gameObject.GetComponent<Mirror>();
                this.BulletMoveDir = mirrorScript.ReflectionDir;
                
                //TODO::Add LayerMask Check Code.
            }
        }

        public void Init(int bulletDamage, Vector2 moveDir, float maxVelocity)
        {
            this.BulletAtkDamage = bulletDamage;
            this.BulletMoveDir = moveDir;
            this.MaxMoveSpeedXy = maxVelocity;
            _isInitialized = true;
        }

        public override void Move()
        {
            this.transform.position += new Vector3(this.BulletMoveVelocity.x, this.BulletMoveVelocity.y, 0);
        }

        public override void CalculateBulletVelocity()
        {
            this.BulletMoveVelocity = this.MaxMoveSpeedXy * this.BulletMoveDir;
        }

        public override void OnTriggerEnter2D(Collider2D hitTarget)
        {
            if (hitTarget.gameObject.layer == PlayerLayerMask)
            {
                Debug.Log("Hit Player");
                //TODO::IMPLEMENT THIS
                //不反弹掉血
                
                
            }
            else if (hitTarget.gameObject.layer == BossLayerMask)
            {
                Debug.Log("Hit Boss");
                //TODO::IMPLEMENT THIS
            }else if (hitTarget.gameObject.layer == ZombieLayerMask)
            {
                Debug.Log("Hit Zombie.");
                //TODO::IMPLEMENT THIS
            }
            else if (hitTarget.gameObject.layer == MirrorLayerMask)
            {
                //TODO::IMPLEMENT THIS
                Debug.Log("Hit Glass.");
                var _reflection = hitTarget.gameObject.GetComponent<Mirror>();

                collisionTime++;
                
                //如果击中的是第三次,那么毁掉自己.
            }
        
        }
    }
}
