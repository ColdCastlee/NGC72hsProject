﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    PlayerMovement player;

    void Start () {
        player = GetComponent<PlayerMovement> ();
    }

    private void FixedUpdate()
    {
        Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
        player.SetDirectionalInput (directionalInput);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //翻滚
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (dir == Vector2.zero)
            {
                return;
            }
            player.Dash(dir);
        }
    }

}
