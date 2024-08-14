#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetris_01_CreateBlock : TestBase
{
    public ShapeType type;
    public TetrisBoard board;

    private void Start()
    {
        board = FindAnyObjectByType<TetrisBoard>();
    }

    public override void OnKey1(InputAction.CallbackContext context)
    {
        board.CreateTetromino(type);
    }
}
#endif