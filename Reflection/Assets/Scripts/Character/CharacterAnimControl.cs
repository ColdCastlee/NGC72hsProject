using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Character;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEditor;

public class CharacterAnimControl : MonoBehaviour
{
    private PlayerSetMirror _setMirrorScript;
    private PlayerMovement _playerMovementScript;
    private PlayerInput _playerInputScript;
    private PlayerHealth _playerHealthScript;

    private Animator _playerAnimator;

    [SerializeField]
    private bool _noVertInput = true;
    [SerializeField]
    private bool _noHoriInput = true;
    [SerializeField]
    private bool _isFacingUp;
    [SerializeField]
    private bool _isFacingRight = true;

    private Vector2 _rollVector = Vector2.zero;
    
    public enum States
    {
        Idle,
        Move,
        Roll,
        TakeDamage,
        Die
    }
    private StateMachine<States> fsm;

    private void Awake()
    {
        fsm = StateMachine<States>.Initialize(this);
    }

    public void BeginRoll(Vector2 dir)
    {
        _rollVector = dir;
        fsm.ChangeState(States.Roll);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _setMirrorScript = GetComponent<PlayerSetMirror>();
        _playerMovementScript = GetComponent<PlayerMovement>();
        _playerInputScript = GetComponent<PlayerInput>();
        _playerHealthScript = GetComponent<PlayerHealth>();
        fsm.ChangeState(States.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnimDir();    
    }
    
    

    private void CheckAnimDir()
    {
        if (Math.Abs(_playerInputScript.PlayerMoveInput.x) < 0.001f && !_playerMovementScript._isDashing)
        {
            _noHoriInput = true;
        }
        if (Math.Abs(_playerInputScript.PlayerMoveInput.y) < 0.001f && !_playerMovementScript._isDashing)
        {
            _noVertInput = true;
        }
        
        //翻滚不算，判断是否看向右边
        if (_playerInputScript.PlayerMoveInput.x > 0 && !_playerMovementScript._isDashing)
        {
            _noHoriInput = false;
            //向右输入
            if (!_isFacingRight)
            {
                _isFacingRight = true;
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else if(_playerInputScript.PlayerMoveInput.x < 0 && !_playerMovementScript._isDashing)
        {
            _noHoriInput = false;
            if (_isFacingRight)
            {
                _isFacingRight = false;
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        
        //翻滚另算，判断是否看向上边
        if (_playerInputScript.PlayerMoveInput.y > 0 && !_playerMovementScript._isDashing)
        {
            _noVertInput = false;
            //向上输入
            if (!_isFacingUp)
            {
                _isFacingUp = true;
            }
        }
        else if(_playerInputScript.PlayerMoveInput.y < 0 && !_playerMovementScript._isDashing)
        {
            _noVertInput = false;
            if (_isFacingUp)
            {
                _isFacingUp = false;
            }
        }
        
        //Debug.Log("RIGHT: " + _isFacingRight + " UP: " + _isFacingUp + " Moving:" + _noHoriInput);
    }
    
    //IDLE
    void Idle_Enter()
    {
        CheckAnimDir();

        CheckIdleAnimation();
    }
    void Idle_Update()
    {
        if (_playerInputScript.PlayerMoveInput != Vector2.zero)
        {
            fsm.ChangeState(States.Move);
        }
    }
    void Idle_Exit()
    {

    }
    //MOVE
    void Move_Enter()
    {
        CheckAnimDir();
    }
    void Move_Update()
    {
        CheckAnimDir();
        CheckMoveAnimation();
        if (_playerInputScript.PlayerMoveInput == Vector2.zero)
        {
            fsm.ChangeState(States.Idle);
        }
    }
    void Move_Exit()
    {
        CheckAnimDir();
    }
    //ROLL
    void Roll_Enter()
    {
        CheckAnimDir();
        CheckRollAnimation();
    }
    void Roll_Update()
    {
        
        if (!_playerMovementScript._isDashing)
        {
            fsm.ChangeState(States.Idle);
        }
    }
    void Roll_Exit()
    {
        CheckAnimDir();
    }
    //TAKE DAMAGE
    void TakeDamage_Enter()
    {
        _playerAnimator.Play("TakeDamage");
    }
    void TakeDamage_Update()
    {
        
    }
    void TakeDamage_Exit()
    {
        
    }
    //DIE
    void Die_Enter()
    {
        _playerAnimator.Play("Die");
    }
    void Die_Update()
    {
        
    }
    void Die_Exit()
    {
        
    }
    
    
    //CHECK ANIMATIONS
    private void CheckMoveAnimation()
    {
        if (!_noHoriInput)
        {
            if (_noVertInput)
            {
                //正右
                _playerAnimator.Play("RightMove");
            }
            else if (_isFacingUp)
            {
                //右上
                _playerAnimator.Play("UpRightMove");
            }
            else
            {
                //右下
                _playerAnimator.Play("DownRightMove");
            }
        }
        else
        {
            if (_isFacingUp)
            {
                //正上
                _playerAnimator.Play("UpMove");
            }
            else
            {
                //正下
                _playerAnimator.Play("DownMove");
            }
        }
    }

    private void CheckRollAnimation()
    {
        //x = 0
        if (Math.Abs(_rollVector.x) < 0.001f)
        {
            //X为0，y向上
            if (_rollVector.y > 0)
            {
                _playerAnimator.Play("UpRoll");
            }
            //X为0，y向下
            else
            {
                _playerAnimator.Play("DownRoll");
            }
        }else if (_rollVector.x > 0.001f)
        {
            _isFacingRight = true;
            this.GetComponent<SpriteRenderer>().flipX = false;
            //X为1，y为0
            if (Math.Abs(_rollVector.y) < 0.001f)
            {
                _playerAnimator.Play("RightRoll");
            }
            //X=1, y=1
            else if(_rollVector.y > 0.001f)
            {
                _isFacingUp = true;
                _playerAnimator.Play("UpRightRoll");
            }
            else //x=1 y=-1
            {
                _isFacingUp = false;
                _playerAnimator.Play("DownRightRoll");
            }
        }
        else // x < 0
        {
            _isFacingRight = false;
            this.GetComponent<SpriteRenderer>().flipX = true;
            //X为-1，y为0
            if (Math.Abs(_rollVector.y) < 0.001f)
            {
                _playerAnimator.Play("RightRoll");
            }
            //X=-1, y=1
            else if(_rollVector.y > 0.001f)
            {
                _playerAnimator.Play("UpRightRoll");
            }
            else//x = -1 , y= -1
            {
                _playerAnimator.Play("DownRightRoll");
            }
        }
    }

    private void CheckIdleAnimation()
    {
        _playerAnimator.SetTrigger("Idle");
//        //默认正右
//        _playerAnimator.Play("RightIdle");
//        
//        if (!_noHoriInput)
//        {
//            if (_noVertInput)
//            {
//                //正右
//                _playerAnimator.Play("RightIdle");
//            }
//            else if (_isFacingUp)
//            {
//                //右上
//                _playerAnimator.Play("UpRightIdle");
//            }
//            else
//            {
//                //右下
//                _playerAnimator.Play("DownRightIdle");
//            }
//        }
//        else
//        {
//            if (_isFacingUp)
//            {
//                //正上
//                _playerAnimator.Play("UpIdle");
//            }
//            else
//            {
//                //正下
//                _playerAnimator.Play("DownIdle");
//
// }
//        }
    }
}
