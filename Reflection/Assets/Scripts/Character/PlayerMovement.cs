using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{

	private PlayerHealth _playerHealth;
	
	private float _dashTimer = .2f;
	private float _dashTime = .25f;
	private float _dashSleepTime = .1f;
	private float _dashSleepTimer = .1f;
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

	void Start() {
		controller = GetComponent<Controller2D> ();
		_playerHealth = GetComponent<PlayerHealth>();
	}

	private void FixedUpdate()
	{
		CalculateVelocity ();

		if (_canDash)
		{
			if (_dashTimer < _dashTime)
			{
				//Debug.Log("Dashing.");
				OnDashing();
			}
			else
			{	//not dashing
				OnDashEnd();
			}			
		}

		if (!_canDash)
		{
			if (_dashSleepTimer < _dashSleepTime)
			{
				//begin sleep
				_canDash = false;
				_dashSleepTimer += Time.deltaTime;
			}
			else
			{
				//end sleep, can dash.
				//Debug.Log("CAN DASH.");
				_canDash = true;
				_dashSleepTimer = _dashSleepTime;
			}			
		}
		
		controller.Move (velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;	
		}

		if (controller.collisions.left || controller.collisions.right)
		{
			velocity.x = 0;
		}
	}

	private void Update()
	{
		//
	}

	public void BeginDashSleep()
	{
		_canDash = false;
	}

	public void OnDashing()
	{
		this.velocity = _dashDir * dashSpeed;
		_dashTimer += Time.deltaTime;
	}

	public void OnDashEnd()
	{
		_dashTimer = _dashTime;

		BeginDashSleep();
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
		_dashTimer = 0.0f;
		
	}
	

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		float targetVelocityY = directionalInput.y * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		velocity.y = Mathf.SmoothDamp (velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeGrounded);
		
		//TODO::Round the whole velocity.
	}
}
