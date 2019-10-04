using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Boss
{
    public class ZombinHealth : AbstractActorHealth
    {
        public int ZombinHp = 1;
        private Animator _zombinAnimator;
        public UnityEvent ZombinOnHit;
        public UnityEvent ZombinOnDead;
        
        
        // Start is called before the first frame update
        void Start()
        {
            this.Hp = 1;
            _zombinAnimator = GetComponent<Animator>();
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
            ZombinOnDead.Invoke();
            Destroy(this.gameObject);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            ZombinOnHit.Invoke();
            //粒子特效
            //音效
            //无敌帧
        }
    
        //TODO::加入动画状态机
    }
}

