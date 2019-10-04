using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BossMovement : MonoBehaviour
{
    
    private Vector2 _bossMoveDir = Vector2.right;
    private Vector3 _velocity;
    private Controller2D controller;

    public float moveSpeed = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D> ();
    }

    public void ResetVelocity(float speed)
    {
        this.moveSpeed = speed;
    }

    public void Move(Vector2 dir)
    {
        _bossMoveDir = dir;
        
        CalculateVelocity ();
		
        controller.Move (_velocity * Time.deltaTime, Vector2.zero);

        if (controller.collisions.above || controller.collisions.below) {
            _velocity.y = 0;	
        }

        if (controller.collisions.left || controller.collisions.right)
        {
            _velocity.x = 0;
        }
    }
    
    void CalculateVelocity()
    {
        
        Vector2 targetDirNormalized = _bossMoveDir.normalized;
        
        _velocity = new Vector3(targetDirNormalized.x, targetDirNormalized.y, 0 ) * moveSpeed; 
		
    }
}
