using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{
	public enum States
	{
		Roll,
		RollSleep
	}
	private StateMachine<States> fsm;
	
	private PlayerHealth _playerHealth;

	private float _dashTimer = .5f;
	
	private float _dashTime = .5f;
	
	private float _dashSleepTime = 0.2f;
	private float _dashSleepTimer = .0f;
	
	public bool _canDash = true;
	
	private Vector2 _dashDir;

	public bool _isDashing
	{
		get { return _dashTimer < _dashTime; }
	}

	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 2;
	public float dashSpeed = 2f;

	Vector3 velocity;
	private float velocityXSmoothing;
	private float velocityYSmoothing;

	Controller2D controller;

	Vector2 directionalInput;

	private void Awake()
	{
		fsm = StateMachine<States>.Initialize(this);
	}

	void Start() 
	{
		fsm.ChangeState(States.RollSleep);
		controller = GetComponent<Controller2D> ();
		_playerHealth = GetComponent<PlayerHealth>();
	}

	private void FixedUpdate()
	{
		CalculateVelocity ();
		
		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;	
		}

		if (controller.collisions.left || controller.collisions.right)
		{
			velocity.x = 0;
		}
	}
	
	public void BeginDashSleep()
	{
		_dashSleepTimer = 0.0f;
	}

	public void OnDashing()
	{
		this.velocity = _dashDir * dashSpeed;
		_dashTimer += Time.deltaTime;
	}

	public void OnDashEnd()
	{
		_dashTimer = _dashTime;
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void Dash(Vector2 dir)
	{
		if (_isDashing || !_canDash)
		{
			return;
		}
		
		_dashDir = dir;
		this.velocity = dir * moveSpeed;
		//Debug.Log("CHnage state to roll");
		fsm.ChangeState(States.Roll);
	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		float targetVelocityY = directionalInput.y * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		velocity.y = Mathf.SmoothDamp (velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeGrounded);
		
		//TODO::Round the whole velocity.
	}
	
	
	
	//State Machine
	void Roll_Enter()
	{
		_dashTimer = 0.0f;
	}

	void Roll_Update()
	{
		OnDashing();
		if (_dashTimer > _dashTime)
		{
			_dashTimer = _dashTime;
			fsm.ChangeState(States.RollSleep);
		}
	}

	void Roll_Exit()
	{
		OnDashEnd();
	}
	
	//ROLL SLEEP
	void RollSleep_Enter()
	{
		BeginDashSleep();
	}

	void RollSleep_Update()
	{
		if (_dashSleepTimer < _dashSleepTime)
		{
			//begin sleep
			_canDash = false;
			_dashSleepTimer += Time.deltaTime;
		}
		else
		{
			_canDash = true;
			_dashSleepTimer = _dashSleepTime;
		}
	}

	void RollSleep_Exit()
	{
		
	}
	
}
