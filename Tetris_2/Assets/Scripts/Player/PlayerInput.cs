using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions action;

    public Action<Vector2> OnMove;

    private void Awake()
    {
        action = new PlayerInputActions();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += OnMoveInput;
        //action.Player.Move.canceled += OnMove;
        action.Player.Drop.performed += OnDropInput;
        action.Player.Drop.canceled += OnDropInput;
        action.Player.Pause.performed += OnEscapeInput;
    }

    private void OnDisable()
    {
        action.Player.Pause.performed -= OnEscapeInput;
        //action.Player.Drop.canceled -= OnDrop;
        action.Player.Drop.performed -= OnDropInput;
        action.Player.Move.canceled -= OnMoveInput;
        action.Player.Move.performed -= OnMoveInput;
        action.Player.Disable();
    }

    private void OnEscapeInput(InputAction.CallbackContext context)
    {
        Debug.Log("esc");
    }

    private void OnDropInput(InputAction.CallbackContext context)
    {
        Debug.Log("drop");
    }

    private void OnMoveInput(InputAction.CallbackContext obj)
    {
        Vector2 moveVec = obj.ReadValue<Vector2>();

        if (moveVec != Vector2.one)
        {
            OnMove?.Invoke(moveVec);    
        }
    }

    private Vector2Int VectorToVectorInt(Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }
}