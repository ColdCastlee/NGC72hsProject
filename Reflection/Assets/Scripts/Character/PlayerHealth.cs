using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerHealth : AbstractActorHealth
    {
	    private SpriteRenderer _spriteRenderer;

	    private PlayerMovement _playerMovement;
	    
        public int PlayerMaxHp;
        private Animator _playerAnimator;
        public UnityEvent PlayerOnHit;
        public UnityEvent PlayerOnDead;

        private float OnHitInvincibleTime = 1.0f;
        private float _onHitInvincibleTimer = 0.0f;

	    private float _dashInvincibleTimer = 0.2f;
	    private float _dashInvincibleTime = 0.2f;
	    
	    private bool _invincible = false;
	    private bool _playerFlickering = false;

        
        private float OnHitSlowTime = 1.0f;
        private float _onHitSlowTimer = 0.0f;
    
        // Start is called before the first frame update
        void Start()
        {
	        _playerMovement = GetComponent<PlayerMovement>();
	        _spriteRenderer = GetComponent<SpriteRenderer>();
            this.Hp = PlayerMaxHp<=0?1:PlayerMaxHp;
            _playerAnimator = GetComponent<Animator>();
	        
        }

        // Update is called once per frame
        void Update()
        {
	        if (Input.GetKeyDown(KeyCode.Space))
	        {
		        
	        }
	        
            CheckDeath();
	        
            UpdateUI();
	        
	        IfInvincible();
        }
        
        public void IfInvincible()
        {
            //无敌时间到了
            if (_onHitInvincibleTimer >= OnHitInvincibleTime)
            {
                _onHitInvincibleTimer = OnHitInvincibleTime;
	            _spriteRenderer.enabled = true;
	            _playerFlickering = false;
	            _invincible = false;
            }
            else
            {
	
				if (_playerFlickering)
				{
					float remainder = _onHitInvincibleTimer % 0.2f;
					_spriteRenderer.enabled = remainder > 0.1f; 		        
				}
				
				_onHitInvincibleTimer += Time.deltaTime;
				_invincible = true;            
            }

	        if (_playerMovement._isDashing)
	        {
		        _invincible = true;
	        }
	        
        }

        public void UpdateUI()
        {
            UIManager.Instance.UpdatePlayerHp(this.Hp);
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

	    public void InvincibleForSeconds()
	    {
		    
	    }

        public override void TakeDamage(int damage)
        {
//	        Debug.Log(_invincible);
//	        Debug.Log(_onHitInvincibleTimer);
	        if (!_invincible)
	        {
				base.TakeDamage(damage);
				PlayerOnHit.Invoke();
				global::TimeScaleManager.Instance.DoSlowMotionForSeconds(OnHitSlowTime);
		        OnHitInvincibleTime = 1.0f;
				_onHitInvincibleTimer = 0.0f;
		        _playerFlickering = true;
		        //粒子特效
		        //音效
		        //无敌帧		        
	        }
        }
    
        //TODO::加入动画状态机
    
	    
	    private void OnTriggerEnter2D(Collider2D other)
	    {
		    if (other.CompareTag("Boss"))    
		    {
			    Debug.Log("BOSS HIT ME");
			    this.TakeDamage(1);
		    }else if (other.CompareTag("Zombie"))
		    {
			    this.TakeDamage(1);
		    }
	    }
    
    }
}
