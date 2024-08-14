#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    TestInputAction inputActions;

    public virtual void Awake()
    {
        inputActions = new TestInputAction();
    }

    public virtual void OnEnable()
    {
        inputActions.Enable();
        inputActions.Test.Key1.performed += OnKey1;
        inputActions.Test.Key2.performed += OnKey2;
        inputActions.Test.Key3.performed += OnKey3;
        inputActions.Test.Key4.performed += OnKey4;
        inputActions.Test.Key5.performed += OnKey5;
    }

    public virtual void OnDisable()
    {
        inputActions.Test.Key5.performed += OnKey5;
        inputActions.Test.Key4.performed += OnKey4;
        inputActions.Test.Key3.performed += OnKey3;
        inputActions.Test.Key2.performed += OnKey2;
        inputActions.Test.Key1.performed += OnKey1;
        inputActions.Disable();
    }

    public virtual void OnKey1(InputAction.CallbackContext context)
    {
        // 사용할 함수 작성
    }
    public virtual void OnKey2(InputAction.CallbackContext context)
    {
        // 사용할 함수 작성
    }
    public virtual void OnKey3(InputAction.CallbackContext context)
    {
        // 사용할 함수 작성
    }
    public virtual void OnKey4(InputAction.CallbackContext context)
    {
        // 사용할 함수 작성
    }
    public virtual void OnKey5(InputAction.CallbackContext context)
    {
        // 사용할 함수 작성
    }
}
#endif