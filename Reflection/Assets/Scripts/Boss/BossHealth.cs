using System.Collections;
using ReadyGamerOne.MemorySystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Boss
{
    public class BossHealth : AbstractActorHealth
    {
        private SpriteRenderer _spriteRenderer;
        
        public int BossMaxHp;
        private Animator _bossAnimator;
        public UnityEvent BossOnHit;
        public UnityEvent BossOnDead;

        public float HealthPercentage => (float) Hp / (float) BossMaxHp;

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            this.Hp = BossMaxHp;
            this.Hp = 51;
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
            Destroy(this.gameObject, 10.0f);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            var percent = 1.0f - (float)this.Hp / this.BossMaxHp;
            UIManager.Instance.ChangeBossHpPercentage(percent);
            StartCoroutine(OnHitFlashEffect());
            switch (damage)
            {
                case 1:
                    ShakeGenerator.Instance.ShakeTiny();
                    break;
                case 2:
                    ShakeGenerator.Instance.ShakeSmall();
                    break;
                case 4:
                    ShakeGenerator.Instance.ShakeMedium();
                    break;
                default:
                    ShakeGenerator.Instance.ShakeMedium();
                    break;
            }
            BossOnHit.Invoke();
            AudioMgr.Instance.PlayEffect(AudioName._takeDamage3);
            //粒子特效
            //音效
            //无敌帧
            //闪动特效
        }

        IEnumerator OnHitFlashEffect()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = Color.white;
        }
    
        //TODO::加入动画状态机
    }
}
