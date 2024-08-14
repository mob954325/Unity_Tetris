using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    // 테트리스 조작
    // asd = 블록 조작 (꾹 누르면 계속 이동)
    // space(doubleTap) = 블록 드랍

    PlayerInputAction playerInputAction;
    public Tetromino CurrentTetromino;

    /// <summary>
    /// 인풋 벡터
    /// </summary>
    Vector2 inputVec = Vector2.zero;

    public bool allowInput = false;
    private bool isMove = false;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Move.started += OnBlockMoveStart;
        playerInputAction.Player.Move.performed += OnBlockMove;
        playerInputAction.Player.Move.canceled += OnBlockMoveEnd;

        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;
    }


    private void OnDisable()
    {
        playerInputAction.Player.Move.started -= OnBlockMoveStart;
        playerInputAction.Player.Move.performed -= OnBlockMove;
        playerInputAction.Player.Move.canceled -= OnBlockMoveEnd;

        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;

        playerInputAction.Disable();
    }

    private void FixedUpdate()
    {
        if(isMove)
        {
            if (CurrentTetromino != null
                && inputVec.x > 0.9f || inputVec.x < -0.9f || inputVec.y < -0.9f)  // 대각선 움직임 방지
            {
                CurrentTetromino.MoveObjet(inputVec);
            }
        }
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
    }

    private void OnBlockMoveStart(InputAction.CallbackContext context)
    {

    }

    private void OnBlockMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
        isMove = context.performed;
    }

    private void OnBlockMoveEnd(InputAction.CallbackContext context)
    {
        isMove = false;
    }
}