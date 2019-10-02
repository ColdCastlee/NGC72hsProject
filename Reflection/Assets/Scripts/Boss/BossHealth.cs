using UnityEngine;
using UnityEngine.Events;

namespace Boss
{
    public class BossHealth : AbstractActorHealth
    {
        public int BossMaxHp;
        private Animator _bossAnimator;
        public UnityEvent BossOnHit;
        public UnityEvent BossOnDead;
    
        // Start is called before the first frame update
        void Start()
        {
            this.Hp = BossMaxHp;
            _bossAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckDeath();
        }

        public override void CheckDeath()
        {
            if (!IsDied()) return;
            //音效
            //粒子特效
            //死亡动画
            BossOnDead.Invoke();
            Destroy(this.gameObject, 2.0f);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            BossOnHit.Invoke();
            //粒子特效
            //音效
            //无敌帧
            //闪动特效
        }
    
        //TODO::加入动画状态机
    }
}
