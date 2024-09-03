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

    Player player;
    PlayerInputAction playerInputAction;

    /// <summary>
    /// 인풋 벡터
    /// </summary>
    private Vector2 inputVec = Vector2.zero;

    /// <summary>
    /// 인풋 권한이 있는지 확인 변수
    /// </summary>
    public bool allowInput = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Move.performed += OnBlockMove;
        playerInputAction.Player.Move.canceled += OnBlockMove;

        playerInputAction.Player.Drop.performed += OnDrop;
        playerInputAction.Player.Drop.canceled += OnDrop;

        playerInputAction.Player.Rotate.performed += OnRotate;

        playerInputAction.Player.SaveBlock.performed += OnSaveBlock;
    }

    private void OnDisable()
    {
        playerInputAction.Player.Rotate.performed -= OnRotate;

        playerInputAction.Player.Move.performed -= OnBlockMove;
        playerInputAction.Player.Move.canceled -= OnBlockMove;

        playerInputAction.Player.Drop.performed -= OnDrop;
        playerInputAction.Player.Drop.canceled -= OnDrop;

        playerInputAction.Disable();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        player.OnSpace?.Invoke();
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        player.OnKey_R?.Invoke();
    }

    private void OnBlockMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
        if(inputVec.x < -0.9f || inputVec.x > 0.9f || inputVec.y > 0.9f || inputVec.y < -0.9f) // 대각선 방지
        {
            player.GetPlayerTetromino().MoveObjet(inputVec);
        }
    }

    private void OnSaveBlock(InputAction.CallbackContext context)
    {
        player.OnKey_Q?.Invoke();
    }
}