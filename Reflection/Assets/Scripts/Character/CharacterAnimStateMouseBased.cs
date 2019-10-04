using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using MonsterLove.StateMachine;

public class CharacterAnimStateMouseBased : MonoBehaviour
{
    private PlayerMovement _playerMovementScript;
    private PlayerHealth _playerHealthScript;
    private PlayerSetMirror _setMirrorScript;
    private PlayerInput _playerInputScript;
    private Animator _playerAnimator;

    [SerializeField] private bool _hasInput;

    private Vector2 _rollVector = Vector2.zero;
    
    public enum States
    {
        Idle,
        Move,
        Roll,
        TakeDamage,
        Die
    }

    public enum Dirctions
    {
        Up,
        Right,
        Down,
        UpRight,
        DownRight
    }

    [SerializeField]
    private bool _isFacingUp = false;
    [SerializeField]
    private bool _isFacingRight = true;
    
    private Dirctions _mouseDirAbs = Dirctions.Right;
    
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
        CheckMouseDir();
        CheckAnimDir();    
    }

    private void CheckMouseDir()
    {
        var mouseDir = _setMirrorScript.MouseDir;
        //check flip
        if (mouseDir.x > 0)
        {
            if (!_isFacingRight)
            {
                _isFacingRight = true;
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            if (_isFacingRight)
            {
                _isFacingRight = false;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        
        //check angle and dir
        var angle = Vector2.Angle(Vector2.right, mouseDir);
        if (angle <= 22.5f)
        {
            _mouseDirAbs = Dirctions.Right;
            //正右
        }else if (angle <= 67.5f)
        {
            if (mouseDir.y < 0)
            {
                _mouseDirAbs = Dirctions.DownRight;
                //右下
            }
            else
            {
                _mouseDirAbs = Dirctions.UpRight;
            }
            //右上和右下
        }
        else if (angle <= 112.5f)
        {
            if (mouseDir.y < 0)
            {
                _mouseDirAbs = Dirctions.Down;
            }
            else
            {
                _mouseDirAbs = Dirctions.Up;
            }
            //正上和正下
        }else if (angle <= 157.5f)
        {
            if (mouseDir.y < 0)
            {
                _mouseDirAbs = Dirctions.DownRight;
                //右下
            }
            else
            {
                _mouseDirAbs = Dirctions.UpRight;
            }
        }
        else
        {
            _mouseDirAbs = Dirctions.Right;
            //正左
        }
    }

    private void CheckAnimDir()
    {
        if (_playerInputScript.PlayerMoveInput != Vector2.zero)
        {
            _hasInput = true;
        }
        else
        {
            _hasInput = false;
        }
    }
    
    //IDLE
    void Idle_Enter()
    {
        CheckAnimDir();

        CheckIdleAnimation();
    }
    void Idle_Update()
    {
        CheckIdleAnimation();
        
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
        switch (_mouseDirAbs)
        {
            case Dirctions.Up:
                _playerAnimator.Play("UpMove");
                break;
            case Dirctions.Right:
                _playerAnimator.Play("RightMove");
                break;
            case Dirctions.Down:
                _playerAnimator.Play("DownMove");
                break;
            case Dirctions.UpRight:
                _playerAnimator.Play("UpRightMove");
                break;
            case Dirctions.DownRight:
                _playerAnimator.Play("DownRightMove");
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
        switch (_mouseDirAbs)
        {
            case Dirctions.Up:
                _playerAnimator.Play("UpIdle");
                break;
            case Dirctions.Right:
                _playerAnimator.Play("RightIdle");
                break;
            case Dirctions.Down:
                _playerAnimator.Play("DownIdle");
                break;
            case Dirctions.UpRight:
                _playerAnimator.Play("UpRightIdle");
                break;
            case Dirctions.DownRight:
                _playerAnimator.Play("DownRightIdle");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
