//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputAction/TestInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TestInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestInputAction"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""91d464c5-8e06-4159-a029-b8868b2a4fcf"",
            ""actions"": [
                {
                    ""name"": ""Key1"",
                    ""type"": ""Button"",
                    ""id"": ""3b182581-f5da-411a-b8bf-f28b9a3e395b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key2"",
                    ""type"": ""Button"",
                    ""id"": ""793c8f13-1712-4090-ba26-ed83fa49b566"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key3"",
                    ""type"": ""Button"",
                    ""id"": ""e7afa466-954a-4673-9003-3460b6045200"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key4"",
                    ""type"": ""Button"",
                    ""id"": ""436b007a-6b22-4bd4-9b33-1e7f742ca944"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Key5"",
                    ""type"": ""Button"",
                    ""id"": ""4c526209-4c90-44b7-a3d1-304d03e4fc0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dc7292e5-4ade-4ca3-9f80-cc89121d072a"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Key1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39eb991e-3114-45c0-8cb8-ed927c9791a9"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Key2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1565a51-74d6-4efb-b842-6272d3e3b3be"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Key3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7a07c15-d0fa-44eb-a81a-6eccd8645bd6"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Key4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09edb1e4-cf36-4700-a92a-b82dcd643478"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Key5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Key1 = m_Test.FindAction("Key1", throwIfNotFound: true);
        m_Test_Key2 = m_Test.FindAction("Key2", throwIfNotFound: true);
        m_Test_Key3 = m_Test.FindAction("Key3", throwIfNotFound: true);
        m_Test_Key4 = m_Test.FindAction("Key4", throwIfNotFound: true);
        m_Test_Key5 = m_Test.FindAction("Key5", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Test
    private readonly InputActionMap m_Test;
    private List<ITestActions> m_TestActionsCallbackInterfaces = new List<ITestActions>();
    private readonly InputAction m_Test_Key1;
    private readonly InputAction m_Test_Key2;
    private readonly InputAction m_Test_Key3;
    private readonly InputAction m_Test_Key4;
    private readonly InputAction m_Test_Key5;
    public struct TestActions
    {
        private @TestInputAction m_Wrapper;
        public TestActions(@TestInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Key1 => m_Wrapper.m_Test_Key1;
        public InputAction @Key2 => m_Wrapper.m_Test_Key2;
        public InputAction @Key3 => m_Wrapper.m_Test_Key3;
        public InputAction @Key4 => m_Wrapper.m_Test_Key4;
        public InputAction @Key5 => m_Wrapper.m_Test_Key5;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void AddCallbacks(ITestActions instance)
        {
            if (instance == null || m_Wrapper.m_TestActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestActionsCallbackInterfaces.Add(instance);
            @Key1.started += instance.OnKey1;
            @Key1.performed += instance.OnKey1;
            @Key1.canceled += instance.OnKey1;
            @Key2.started += instance.OnKey2;
            @Key2.performed += instance.OnKey2;
            @Key2.canceled += instance.OnKey2;
            @Key3.started += instance.OnKey3;
            @Key3.performed += instance.OnKey3;
            @Key3.canceled += instance.OnKey3;
            @Key4.started += instance.OnKey4;
            @Key4.performed += instance.OnKey4;
            @Key4.canceled += instance.OnKey4;
            @Key5.started += instance.OnKey5;
            @Key5.performed += instance.OnKey5;
            @Key5.canceled += instance.OnKey5;
        }

        private void UnregisterCallbacks(ITestActions instance)
        {
            @Key1.started -= instance.OnKey1;
            @Key1.performed -= instance.OnKey1;
            @Key1.canceled -= instance.OnKey1;
            @Key2.started -= instance.OnKey2;
            @Key2.performed -= instance.OnKey2;
            @Key2.canceled -= instance.OnKey2;
            @Key3.started -= instance.OnKey3;
            @Key3.performed -= instance.OnKey3;
            @Key3.canceled -= instance.OnKey3;
            @Key4.started -= instance.OnKey4;
            @Key4.performed -= instance.OnKey4;
            @Key4.canceled -= instance.OnKey4;
            @Key5.started -= instance.OnKey5;
            @Key5.performed -= instance.OnKey5;
            @Key5.canceled -= instance.OnKey5;
        }

        public void RemoveCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestActions instance)
        {
            foreach (var item in m_Wrapper.m_TestActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestActions @Test => new TestActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface ITestActions
    {
        void OnKey1(InputAction.CallbackContext context);
        void OnKey2(InputAction.CallbackContext context);
        void OnKey3(InputAction.CallbackContext context);
        void OnKey4(InputAction.CallbackContext context);
        void OnKey5(InputAction.CallbackContext context);
    }
}
