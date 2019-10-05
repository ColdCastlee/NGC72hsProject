using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    PlayerMovement player;
    private CharacterAnimStateMouseBased _playerStateMachine;

    private Vector2 _playerMoveInput;

    public Vector2 PlayerMoveInput
    {
        get { return _playerMoveInput; }
    }

    void Start ()
    {
        _playerStateMachine = GetComponent<CharacterAnimStateMouseBased>();
        player = GetComponent<PlayerMovement> ();
    }

    private void FixedUpdate()
    {
        _playerMoveInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
        player.SetDirectionalInput (_playerMoveInput);

        if (Input.GetKeyDown(KeyCode.Space) && player._canDash)
        {
            //翻滚
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (dir == Vector2.zero)
            {
                return;
            }
            _playerStateMachine.BeginRoll(dir);
            player.Dash(dir);
        }
    }

}
