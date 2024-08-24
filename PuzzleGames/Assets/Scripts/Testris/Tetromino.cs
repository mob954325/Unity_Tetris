using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ShapeType
{
    None,
    O,
    I,
    L,
    T,
    S,
    Z
}

public class Tetromino : MonoBehaviour
{
    /// <summary>
    /// 회전 모양 저장용 딕셔너리
    /// </summary>
    Dictionary<ShapeType, int[,]> RotateShape = new Dictionary<ShapeType, int[,]>();

    /// <summary>
    /// 회전 인덱스
    /// </summary>
    int rotateIndex = 0;

    /// <summary>
    /// 현재 Tetromino의 블록 타입
    /// </summary>
    private ShapeType type = ShapeType.None;

    /// <summary>
    /// 블록 타입 변경 및 접근용 프로퍼티
    /// </summary>
    public ShapeType Type
    {
        get => type;
        set
        {
            type = value;
            if (type != ShapeType.None)
            {
                MakeShape();
            }
        }
    }
    /// <summary>
    /// 테트리스 블록 배열
    /// </summary>
    private GameObject[] blocks;

    /// <summary>
    /// 블록 모양 생성용 그리드 위치 저장 배열 (4x4)
    /// 12 13 14 15
    /// 8 9 10 11
    /// 4 5 6 7
    /// 0 1 2 3
    /// </summary>
    private Vector2[] gridPositions;

    /// <summary>
    /// 중심 값 벡터
    /// </summary>
    private Vector2 centerVector = new Vector2(0.125f, 0.125f);

    /// <summary>
    /// 이전 위치 벡터
    /// </summary>
    public Vector2 prevVector = Vector2.zero;

    /// <summary>
    /// 가장 낮은 Y 위치 (블록 드랍용)
    /// </summary>
    public Vector2 lowestYVector = Vector2.zero;

    /// <summary>
    /// 떨어지는 값 크기 (0.25 == 한 칸), 블록 오브젝트의 크기
    /// </summary>
    private const float DropScale = 0.25f;

    /// <summary>
    /// 다음 칸으로 떨어지는데 걸리는 시간 타이머 (시간이 되면 초기화 후 위치변경)
    /// </summary>
    private float dropTimer = 0f;

    /// <summary>
    /// 떨어지는데 걸리는 시간 (default = 0.5f)
    /// </summary>
    private float dropDelay = 0.5f;

    /// <summary>
    /// 블록이 멈춰있을 때 활성화되는 타이머 (일정 시간이 지나면 블록 멈춤)
    /// </summary>
    private float stopTimer = 1f;

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

    private void Update()
    {
        dropTimer += Time.deltaTime;

        if((Vector3)prevVector != transform.localPosition)
        {
            stopTimer = 1f; // 1초 대기
        }

        if (allowMove && dropTimer > dropDelay)
        {
            dropTimer = 0f;
            prevVector = transform.localPosition;
            transform.Translate(Vector2.down * DropScale, Space.World);
        }

        CheckIsStop();
    }

    /// <summary>
    /// 블록이 멈췄는지 확인하는 함수 (물리업데이트 함수)
    /// </summary>
    private void CheckIsStop()
    {
        stopTimer -= Time.fixedDeltaTime;

        if(stopTimer < 0f)
        {
            allowMove = false;
        }
    }

    /// <summary>
    /// 테트리스 블록 초기화 함수
    /// </summary>
    /// <param name="type">블록 타입</param>
    /// <param name="width">보드 넓이</param>
    /// <param name="height">보드 높이</param>
    public void Init(ShapeType type, float dropDelay = 0.5f)
    {
        Type = type;
        allowMove = true;
        this.dropDelay = dropDelay;

        SetRandomColor();
        SetRotateShape();
    }

    /// <summary>
    /// 회전 모양 설정 함수
    /// </summary>
    private void SetRotateShape()
    {
        RotateShape.Add(ShapeType.O, new int[,]
        {
            { 5, 6, 9, 10},
        });

        RotateShape.Add(ShapeType.I, new int[,]
        {
            { 1, 5, 9, 13  },
            { 4, 5, 6, 7   },
            { 2, 6, 10, 14 },
            { 4, 5, 6, 7   }
        });

        RotateShape.Add(ShapeType.L, new int[,]
        {
            { 5, 6, 9, 13  },
            { 8, 9, 10, 14 },
            { 5, 9, 12, 13 },
            { 4, 8, 9, 10  }
        });

        RotateShape.Add(ShapeType.T, new int[,]
        {
            { 4, 5, 6, 9 },
            { 1, 4, 5, 9 },
            { 1, 4, 5, 6 },
            { 1, 5, 6, 9 }
        });

        RotateShape.Add(ShapeType.S, new int[,]
        {
            { 4, 5, 9, 10 },
            { 2, 6, 5, 9  },
            { 4, 5, 9, 10 },
            { 1, 4, 5, 8  }
        });

        RotateShape.Add(ShapeType.Z, new int[,]
        {
            { 5, 6, 8, 9  },
            { 0, 4, 5, 9  },
            { 5, 6, 8, 9  },
            { 1, 5, 6, 10 }
        });
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
            case ShapeType.Z: // 5 6 8 9
                SetBlockPosition(5, 6, 8, 9);
                break;
        }
    }

    /// <summary>
    /// 블록 위치를 설정하는 함수 (gridPositions 배열 사용)
    /// </summary>
    /// <param name="first">첫번째 위치</param>
    /// <param name="second">두번째 위치</param>
    /// <param name="third">세번째 위치</param>
    /// <param name="fourth">네번째 위치</param>
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
    /// allowMove 반환 함수 (움직임 권한 반환 함수)
    /// </summary>
    public bool checkMoveAllow()
    {
        return allowMove;
    }

    /// <summary>
    /// 블록 색상 랜덤 설정 함수
    /// </summary>
    private void SetRandomColor()
    {
        float rand1 = Random.value;
        float rand2 = Random.value;
        float rand3 = Random.value;
        foreach(var obj in blocks)
        {
            Material material = obj.GetComponent<SpriteRenderer>().material;
            material.color = new Color(rand1, rand2, rand3);
        }
    }

    /// <summary>
    /// 오브젝트 움직이는 함수
    /// </summary>
    /// <param name="inputVec">인풋 값</param>
    public void MoveObjet(Vector2 inputVec)
    {
        // 블록이 위로 올라가는 것 방지
        if(inputVec.y > 0)
        {
            inputVec.y = 0;
        }

        // 블록 움직이기 (한칸)
        prevVector = transform.localPosition;
        transform.Translate(inputVec * 0.25f, Space.World);
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
        if (!allowMove)
            return;

        if (Type == ShapeType.O) // O 모양은 회전 안함
            return;


        rotateIndex++;           // 블록 회전 인덱스 추가
        rotateIndex %= 4;

        RotateShape.TryGetValue(Type, out int[,] result);
        SetBlockPosition(result[rotateIndex, 0], result[rotateIndex, 1], result[rotateIndex, 2], result[rotateIndex, 3]);
    }

    /// <summary>
    /// 오브젝트 드랍 함수
    /// </summary>
    /// </param>
    public void DropObject()
    {
        prevVector = new Vector2(lowestYVector.x, lowestYVector.y + 0.25f);
        transform.localPosition = lowestYVector;
    }

    /// <summary>
    /// 가장 낮은 Y값을 가진 벡터를 저장하는 함수 (블록 드랍용)
    /// </summary>
    /// <param name="newLowestYVector"></param>
    public void SetLowestYVector(Vector2 newLowestYVector)
    {
        lowestYVector = newLowestYVector;
    }
}