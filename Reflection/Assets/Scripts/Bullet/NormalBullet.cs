using UnityEngine;

namespace Bullet
{
    public class NormalBullet : AbstractBullet
    {
        private bool _isInitialized = false;
        public override void Update()
        {
            base.Update();
            if (!_isInitialized)
            {
                Debug.LogError("Bullet Not Initialized. Check This Error.");
            }
            //
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
        
        }
    }
}
