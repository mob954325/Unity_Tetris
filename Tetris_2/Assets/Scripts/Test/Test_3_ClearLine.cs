#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_3_ClearLine : TestBase
{
    public Board board;

    private void Start()
    {
        board = FindAnyObjectByType<Board>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        board.Test_ClearLine();
    }
}
#endif