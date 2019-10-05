using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Boss
{
    public class BossHealth : AbstractActorHealth
    {
        public int BossMaxHp;
        private Animator _bossAnimator;
        public UnityEvent BossOnHit;
        public UnityEvent BossOnDead;

        public float HealthPercentage => (float) Hp / (float) BossMaxHp;

        // Start is called before the first frame update
        void Start()
        {
            this.Hp = 25;
            _bossAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckDeath();
            
            var percent = (float)this.Hp / (float)this.BossMaxHp;
            UIManager.Instance.ChangeBossHpPercentage(percent);
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

            var percent = 1.0f - (float)this.Hp / this.BossMaxHp;
            UIManager.Instance.ChangeBossHpPercentage(percent);
            
            BossOnHit.Invoke();
            //粒子特效
            //音效
            //无敌帧
            //闪动特效
        }
    
        //TODO::加入动画状态机
    }
}
