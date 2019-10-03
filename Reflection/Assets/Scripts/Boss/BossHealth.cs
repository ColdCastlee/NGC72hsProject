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
        public Slider _hpSilider;
        // Start is called before the first frame update
        void Start()
        {
            this.Hp = BossMaxHp;
            _bossAnimator = GetComponent<Animator>();
            _hpSilider = gameObject.GetComponentInChildren<Slider>();
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

            _hpSilider.value = this.Hp;
            
            BossOnHit.Invoke();
            //粒子特效
            //音效
            //无敌帧
            //闪动特效
        }
    
        //TODO::加入动画状态机
    }
}
