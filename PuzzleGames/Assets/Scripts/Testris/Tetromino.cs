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
    /// 블록 그리드 위치 저장 배열 (4x4)
    /// </summary>
    private Vector2[] gridPositions;

    /// <summary>
    /// 테트리스 블록 배열
    /// </summary>
    private GameObject[] blocks;

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

        gridPositions = new Vector2[16];
        for(int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            { 
                gridPositions[x + 4 * y] = new Vector2(-0.25f + x * 0.25f, -0.25f + y * 0.25f) + centerVector;
            }
        }
    }

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (allowMove && dropTimer > 0.25f)
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
        switch (Type) 
        { 
            case ShapeType.O: // 5 6 9 10
                SetBlockPosition(5, 6, 9, 10);
                break;
            case ShapeType.I: // 1 5 9 13
                SetBlockPosition(1, 5, 9, 13);
                break;
            case ShapeType.L: // 5 6 9 13
                SetBlockPosition(5, 6, 9, 13);
                break;
            case ShapeType.T: // 4 5 6 9
                SetBlockPosition(4, 5, 6, 9);
                break;
            case ShapeType.S: // 4 5 9 10
                SetBlockPosition(4, 5, 9, 10);
                break;
        }
    }

    private void SetBlockPosition(int first, int second, int third, int fourth)
    {
        int[] ints = { first, second, third, fourth };
        int index = 0;

        foreach (var obj in blocks)
        {
            obj.transform.localPosition = gridPositions[ints[index]];
            index++;
        }
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

        transform.Translate(inputVec * 0.25f);
    }

    /// <summary>
    /// 테트리스 블록을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetBlocks()
    {
        return blocks;
    }

    /// <summary>
    /// 오브젝트 회전 함수
    /// </summary>
    public void RotateObject()
    {

    }
}