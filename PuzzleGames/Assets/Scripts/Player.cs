using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 현재 플레이어가 조종하는 테트리스 블록
    /// </summary>
    public Tetromino currentTetromino;

    /// <summary>
    /// 스페이스를 눌렀을 때 호출되는 델리게이트
    /// </summary>
    public Action OnSpace;

    /// <summary>
    /// R키 눌렀을 때 호출되는 델리게이트
    /// </summary>
    public Action OnKey_R;

    public void Init()
    {
        OnSpace = DropTetromino;
        OnKey_R = RotateTetromino;
    }

    /// <summary>
    /// 
    /// </summary>
    private void DropTetromino()
    {
        // 블록 드랍
        currentTetromino.DropObject(currentTetromino.transform.localPosition * Vector2.right); // 위치 임시 설정
    }

    private void RotateTetromino()
    {
        currentTetromino.RotateObject();
    }

    /// <summary>
    /// 플레이어가 현재 조종하고 있는 블록을 반환하는 함수
    /// </summary>
    public Tetromino GetPlayerTetromino()
    {
        return currentTetromino;
    }
}