using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    // 블록 생성
    // 블록 범위 체크
    // 한 줄 체크

    /// <summary>
    /// 스폰 위치 트랜스폼
    /// </summary>
    private Transform spawnPoint;

    private Transform tetrominoContainer;

    /// <summary>
    /// 테트리스 블록 프리팹
    /// </summary>
    public GameObject tetrominoPrefab;

    /// <summary>
    /// 가장 최근에 스폰된 테트리스 블록
    /// </summary>
    public Tetromino currentTetromino;

    /// <summary>
    /// 보드 넓이
    /// </summary>
    public float boardWidth;

    /// <summary>
    /// 보드 크기
    /// </summary>
    public float boardHeight;

    /// <summary>
    /// 보드 셀 배열 (테트리스 블록 오브젝트 확인용)
    /// </summary>
    private Cell[] cells;

    private void Awake()
    {
        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (currentTetromino != null)
        {
            if(!IsVaildPosition())
            {
                currentTetromino.SetMoveAllow(false);
                // 새로 생성
            }
        }
    }

    public void Init()
    {
        CreateCells();
    }

    /// <summary>
    /// 셀 생성 함수
    /// </summary>
    private void CreateCells()
    {
        int count_x = (int)(boardWidth / 0.25f);
        int count_y = (int)(boardHeight / 0.25f);

        cells = new Cell[count_x * count_y];

        for(int y = 0; y < count_y; y++)
        {
            for(int x = 0; x < count_x; x++)
            {
                cells[y * count_x + x] = new Cell(x, y);
            }
        }
    }

    /// <summary>
    /// 테트리스 블록 생성 함수
    /// </summary>
    public void CreateTetromino(ShapeType type)
    {
        Tetromino tetromino = Instantiate(tetrominoPrefab).GetComponent<Tetromino>();
        tetromino.transform.parent = tetrominoContainer;
        tetromino.transform.localPosition = spawnPoint.transform.localPosition;

        tetromino.Init(type, boardWidth, boardHeight);

        currentTetromino = tetromino;
    }

    /// <summary>
    /// 현재 떨어지는 블록들이 게임 보드 안에 있는지 체크하는 함수
    /// </summary>
    /// <returns>안에 있으면 true 아니면 false</returns>
    private bool IsVaildPosition()
    {
        bool result = true;
        foreach(var obj in currentTetromino.GetBlocks())
        {
            Vector2Int grid = WorldToGrid(obj.transform.parent.localPosition + obj.transform.localPosition);

            Debug.Log($"{obj.gameObject.name} : {grid}");
            if(grid.x < 1 || grid.x > boardWidth / 0.25f || grid.y < 1 || grid.y > boardHeight / 0.25f + 5)
            {
                result = false;
            }
        }

        Debug.Log(result);
        return result;
    }

    // 좌표 변환 ===================================================================================
    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int((int)(world.x / 0.25f), (int)(world.y / 0.25f)); // 2D 프로젝트이여서 월드의 x y값 사용
    }
}