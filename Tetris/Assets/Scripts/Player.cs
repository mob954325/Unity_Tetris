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
    /// R키 눌렀을 때 호출되는 델리게이트 (회전)
    /// </summary>
    public Action OnKey_R;

    /// <summary>
    /// Q키를 눌렀을 때 호출 되는 델리게이트 (블록 저장)
    /// </summary>
    public Action OnKey_Q;

    /// <summary>
    /// 현재 저장된 블록
    /// </summary>
    private ShapeType savedType = ShapeType.None;

    /// <summary>
    /// 블록 드랍 딜레이 타이머
    /// </summary>
    private float dropTimer = 0f;

    /// <summary>
    /// 블록 드랍 딜레이 값
    /// </summary>
    private float dropDelay = 0.5f;

    private void Update()
    {
        dropTimer += Time.deltaTime;
    }

    /// <summary>
    /// 플레이어 초기화 (게임 시작 전 초기화)
    /// </summary>
    public void Init()
    {
        OnSpace = DropTetromino;
        OnKey_R = RotateTetromino;
    }

    /// <summary>
    /// 블록 드랍 시 호출되는 함수
    /// </summary>
    private void DropTetromino()
    {
        if (dropTimer < dropDelay) return;

        dropTimer = 0f;
        currentTetromino.DropObject();
    }

    /// <summary>
    /// 블록 회전시 호출되는 함수
    /// </summary>
    private void RotateTetromino()
    {
        currentTetromino.RotateObject();
    }

    /// <summary>
    /// 현재 블록 타입 저장함수
    /// </summary>
    public void SaveCurrentBlock()
    {
        savedType = currentTetromino.Type;
    }

    /// <summary>
    /// 현재 저장된 타입을 반환하는 함수
    /// </summary>
    /// <returns>현재 타입 반환 (없으면 None타입 반환)</returns>
    public ShapeType GetSavedType()
    {
        return savedType;
    }

    /// <summary>
    /// 플레이어가 현재 조종하고 있는 블록을 반환하는 함수
    /// </summary>
    public Tetromino GetPlayerTetromino()
    {
        return currentTetromino;
    }
}