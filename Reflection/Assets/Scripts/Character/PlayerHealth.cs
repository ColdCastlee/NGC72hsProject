using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class PlayerHealth : AbstractActorHealth
    {
        public int PlayerMaxHp;
        private Animator _playerAnimator;
        public UnityEvent PlayerOnHit;
        public UnityEvent PlayerOnDead;
    
        // Start is called before the first frame update
        void Start()
        {
            this.Hp = PlayerMaxHp<=0?1:PlayerMaxHp;
            _playerAnimator = GetComponent<Animator>();
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
            //人物消失
            PlayerOnDead.Invoke();
            Destroy(this.gameObject, 2.0f);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            PlayerOnHit.Invoke();
            //粒子特效
            //音效
            //无敌帧
        }
    
        //TODO::加入动画状态机
    
    
    }
}
