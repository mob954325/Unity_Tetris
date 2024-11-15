#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_5_Spin : TestBase
{
    public Board board;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        board.RotateCurBlock();
    }
}
#endif