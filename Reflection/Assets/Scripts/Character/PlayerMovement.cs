using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{

    private Vector2 _playerMoveDir;
    private Vector2 _playerMoveVelocity;

	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 2;

	Vector3 velocity;
	private float velocityXSmoothing;
	private float velocityYSmoothing;

	Controller2D controller;

	Vector2 directionalInput;

	void Start() {
		controller = GetComponent<Controller2D> ();
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

	private void Update()
	{
		//
	}


	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}
	

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		float targetVelocityY = directionalInput.y * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		velocity.y = Mathf.SmoothDamp (velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeGrounded);
		
		//TODO::Round the whole velocity.
		
	}
}
