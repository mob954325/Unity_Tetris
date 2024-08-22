using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private GameObject currnetBlockObject;

    private int x;
    private int y;
    private bool isVaild = true;

    /// <summary>
    /// Cell 초기화 
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 해당 위치 오브젝트를 저장하는 함수
    /// </summary>
    /// <param name="obj">해당 위치에 있는 오브젝트</param>
    public void SetBlockObject(GameObject obj)
    {
        currnetBlockObject = obj;
        isVaild = false;
    }

    /// <summary>
    /// 해당 셀의 정보를 제거하는 함수 (블록 제거)
    /// </summary>
    /// <param name="activeSelf">블록 오브젝트 활성화 여부 (default : false)</param>
    public void RemoveBlockObject(bool activeSelf = false)
    {
        currnetBlockObject.SetActive(activeSelf);
        currnetBlockObject = null;
        isVaild = true;
    }

    public GameObject GetBlockObject()
    {
        return currnetBlockObject;
    }

    /// <summary>
    /// SetVaild 변경 함수
    /// </summary>
    public void SetVaild(bool value)
    {
        isVaild = value;
    }

    /// <summary>
    /// 해당 위치가 유효한 Cell인지 확인하는 함수 
    /// </summary>
    /// <returns>유효하면 true 아니면 false</returns>
    public bool CheckVaild()
    {
        return isVaild;
    }
}