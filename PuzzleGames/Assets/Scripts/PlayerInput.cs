using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputAction playerInputAction;
    public Tetromino CurrentTetromino;

    public bool allowInput = false;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
        playerInputAction.Player.Move.performed += OnMove;
        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;
    }

    private void OnDisable()
    {
        playerInputAction.Player.Move.performed -= OnMove;
        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;
        playerInputAction.Disable();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("움직임");
        Vector2 inputVec = context.ReadValue<Vector2>();    

        if(CurrentTetromino != null 
            && inputVec.x > 0.9f || inputVec.x < -0.9f || inputVec.y < -0.9f)  // 대각선 움직임 방지
        {
            CurrentTetromino.MoveObjet(inputVec);
        }
    }
}