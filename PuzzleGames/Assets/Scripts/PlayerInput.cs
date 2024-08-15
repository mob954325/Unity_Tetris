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

    /// <summary>
    /// 인풋 벡터
    /// </summary>
    private Vector2 inputVec = Vector2.zero;

    public bool allowInput = false;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Move.started += OnBlockMoveStart;
        playerInputAction.Player.Move.performed += OnBlockMove;
        playerInputAction.Player.Move.canceled += OnBlockMove;

        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;
    }


    private void OnDisable()
    {
        playerInputAction.Player.Move.started -= OnBlockMoveStart;
        playerInputAction.Player.Move.performed -= OnBlockMove;
        playerInputAction.Player.Move.canceled -= OnBlockMove;

        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;

        playerInputAction.Disable();
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
    }

    private void OnBlockMoveEnd(InputAction.CallbackContext context)
    {
    }

    /// <summary>
    /// 인풋 벡터 반환 함수
    /// </summary>
    public Vector2 GetInputVec()
    {
        return inputVec;
    }
}