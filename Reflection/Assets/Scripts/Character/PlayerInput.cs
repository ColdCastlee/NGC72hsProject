using System.Collections;
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
        Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        player.SetDirectionalInput (directionalInput);
    }

}
