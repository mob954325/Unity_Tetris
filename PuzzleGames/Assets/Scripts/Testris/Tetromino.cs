using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType
{
    None,
    O,
    I,
    L,
    T,
    S
}

public class Tetromino : MonoBehaviour
{
    public ShapeType type = ShapeType.None;

    private ShapeType Type
    {
        get => type;
        set
        {
            if (type != value)
            {
                type = value;
                MakeShape();
            }
        }
    }

    /// <summary>
    /// 테트리스 블록 배열
    /// </summary>
    private GameObject[] blocks;

    /// <summary>
    /// 경계선 체크하는 기준 오브젝트
    /// </summary>
    public GameObject checkObject;

    /// <summary>
    /// 중심 값 벡터
    /// </summary>
    private Vector2 centerVector = new Vector2(0.125f, 0.125f);

    /// <summary>
    /// 떨어지는 값 크기 (0.25 == 한 칸)
    /// </summary>
    private const float DropScale = 0.25f;

    /// <summary>
    /// 보드 넓이
    /// </summary>
    private float boardWidth;

    /// <summary>
    /// 보드 높이
    /// </summary>
    private float boardHeight;

    /// <summary>
    /// 다음 칸으로 떨어지는데 걸리는 시간 타이머 (시간이 되면 초기화 후 위치변경)
    /// </summary>
    private float dropTimer = 0f;

    /// <summary>
    /// 움직일 수 있는지 체크하는 변수 (움직일 수 있으면 true 아니면 false)
    /// </summary>
    public bool allowMove = false;

    private void Awake()
    {
        blocks = new GameObject[4];

        for(int i = 0; i < blocks.Length; i++)
        {
            Transform child = transform.GetChild(i);
            blocks[i] = child.gameObject;
        }

        checkObject = blocks[0]; // 센터 오브젝트 임의 지정
    }

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (allowMove && IsVaildPosition() && dropTimer > 1f)
        {
            dropTimer = 0f;
            transform.Translate(Vector2.down * DropScale);
        }
    }

    /// <summary>
    /// 테트리스 블록 초기화 함수
    /// </summary>
    /// <param name="type">블록 타입</param>
    /// <param name="width">보드 넓이</param>
    /// <param name="height">보드 높이</param>
    public void Init(ShapeType type, float width, float height)
    {
        Type = type;
        boardHeight = height;
        boardWidth = width;

        allowMove = true;
    }
    
    /// <summary>
    /// 블록 만드는 함수
    /// </summary>
    private void MakeShape()
    {
        switch(Type) 
        { 
            case ShapeType.O:
                break;
            case ShapeType.I:
                for(int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].transform.localPosition = centerVector - (Vector2.up * 0.25f) + (Vector2.up * 0.25f) * i;

                    if(blocks[i].transform.localPosition.y < checkObject.transform.localPosition.y)
                    {
                        checkObject = blocks[i];
                    }
                }
                break;
            case ShapeType.L: 
                break;
            case ShapeType.T:
                break;
            case ShapeType.S:
                break;
        }
    }

    /// <summary>
    /// 블록이 존재할 수 있는 공간인지 확인하는 함수 (카메라 안 = 존재가능, 그 외 = false)
    /// </summary>
    /// <returns></returns>
    private bool IsVaildPosition()
    {
        Vector2 currnetPosition = transform.localPosition + (checkObject.transform.localPosition * 2);
        return (currnetPosition.x < boardWidth
                && currnetPosition.x > 0 
                && currnetPosition.y < boardHeight + 2f
                && currnetPosition.y > 0);
    }

    /// <summary>
    /// 움직임 권한 설정 함수 (true : 움직임 가능, false : 움직이지 않음)
    /// </summary>
    /// <param name="value">true 권한 부여, false 권한 제거</param>
    public void SetMoveAllow(bool value)
    {
        allowMove = value;
    }

    /// <summary>
    /// 오브젝트 움직이는 함수
    /// </summary>
    /// <param name="inputVec">인풋 값</param>
    public void MoveObjet(Vector2 inputVec)
    {
        //if (!IsVaildPosition()) return;

        transform.Translate(inputVec * 0.25f);
    }

    /// <summary>
    /// 오브젝트 회전 함수
    /// </summary>
    public void RotateObject()
    {

    }
}
