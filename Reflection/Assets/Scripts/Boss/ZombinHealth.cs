using System.Collections;
using System.Collections.Generic;
using ReadyGamerOne.MemorySystem;
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
            
            this.gameObject.AddComponent<AudioMgr>();
            MemoryMgr.LoadAssetFromResourceDir<AudioClip>(typeof(AudioName),"Audio/",(name,clip)=>
            {
                if(AudioMgr.Instance.audioclips.ContainsKey(name)==false)
                    AudioMgr.Instance.audioclips.Add(name, clip);
            });
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
            AudioMgr.Instance.PlayEffect(AudioName._zombieDie);
            Debug.Log("炸了");
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

