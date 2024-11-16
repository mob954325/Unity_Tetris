using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions action;

    private Vector2 moveVec;
    private IEnumerator OnMovePressCouroutine;
    private bool isPressedMove = false;

    public Action<Vector2> OnMove;
    public Action OnSpin;

    private void Awake()
    {
        action = new PlayerInputActions();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.started += OnMoveInputStart;
        action.Player.Move.performed += OnMoveInput;
        action.Player.Move.canceled += OnMoveInput;
        action.Player.Drop.performed += OnDropInput;
        action.Player.Drop.canceled += OnDropInput;
        action.Player.Pause.performed += OnEscapeInput;        
        action.Player.Spin.performed += OnSpinInput;
    }

    private void OnDisable()
    {
        action.Player.Pause.performed -= OnEscapeInput;
        //action.Player.Drop.canceled -= OnDrop;
        action.Player.Drop.performed -= OnDropInput;
        action.Player.Move.canceled -= OnMoveInput;
        action.Player.Move.performed -= OnMoveInput;
        action.Player.Move.started -= OnMoveInputStart;
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

    private void OnSpinInput(InputAction.CallbackContext context)
    {
        OnSpin?.Invoke();
    }


    private void OnMoveInputStart(InputAction.CallbackContext obj)
    {
        moveVec = obj.ReadValue<Vector2>();

        if (moveVec != Vector2.one)
        {
            OnMove?.Invoke(moveVec);
        }
    }

    private void OnMoveInput(InputAction.CallbackContext obj)
    {

        if(obj.performed)
        {
            OnMovePressCouroutine = OnMovePress(moveVec, obj.performed);
            StartCoroutine(OnMovePressCouroutine);
        }
        else
        {
            StopAllCoroutines();
        }
    }
    private IEnumerator OnMovePress(Vector2 moveVec, bool isPress)
    {
        yield return new WaitForSeconds(0.5f);

        while(isPress)
        {
            OnMove?.Invoke(moveVec);
            yield return null;
        }
    }

    private Vector2Int VectorToVectorInt(Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }
}