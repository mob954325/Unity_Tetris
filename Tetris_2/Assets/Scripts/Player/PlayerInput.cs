using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions action;
    Board board;

    private void Awake()
    {
        action = new PlayerInputActions();
        board = FindAnyObjectByType<Board>();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += OnMove;
        //action.Player.Move.canceled += OnMove;
        action.Player.Drop.performed += OnDrop;
        action.Player.Drop.canceled += OnDrop;
        action.Player.Pause.performed += OnEscape;
    }

    private void OnDisable()
    {
        action.Player.Pause.performed -= OnEscape;
        //action.Player.Drop.canceled -= OnDrop;
        action.Player.Drop.performed -= OnDrop;
        action.Player.Move.canceled -= OnMove;
        action.Player.Move.performed -= OnMove;
        action.Player.Disable();
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        Debug.Log("esc");
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        Debug.Log("drop");
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        Vector2 moveVec = obj.ReadValue<Vector2>();

        if (moveVec != Vector2.one)
        {
            board.MoveCurBlock(VectorToVectorInt(moveVec));
        }
    }

    private Vector2Int VectorToVectorInt(Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }
}