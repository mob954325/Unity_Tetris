using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    // 블록 생성
    // 블록 범위 체크
    // 한 줄 체크

    /// <summary>
    /// 플레이어 인풋 (플레이어)
    /// </summary>
    Player player;

    /// <summary>
    /// 스폰 위치 트랜스폼
    /// </summary>
    private Transform spawnPoint;

    /// <summary>
    /// 테트리스 블록들의 부모 트랜스폼
    /// </summary>
    private Transform tetrominoContainer;

    /// <summary>
    /// 테트리스 블록 프리팹
    /// </summary>
    public GameObject tetrominoPrefab;

    /// <summary>
    /// 보드 셀 배열 (테트리스 블록 오브젝트 확인용)
    /// </summary>
    private Cell[,] cells;

    /// <summary>
    /// 보드 넓이
    /// </summary>
    public float boardWidth;

    /// <summary>
    /// 보드 크기
    /// </summary>
    public float boardHeight;

    /// <summary>
    /// x 개수
    /// </summary>
    private int count_x;

    /// <summary>
    /// y 개수
    /// </summary>
    private int count_y;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;

        Init();
    }

    private void Update()
    {
        CheckAllBlockIsVaild();

        if(player.currentTetromino != null)
        {
            if(!player.currentTetromino.checkMoveAllow())
            {
                // 블록 새로 만들고
                // 해당 블록위치 셀에 저장
                int index = 0;
                foreach(var obj in player.currentTetromino.GetBlocks())
                {
                    Vector2Int grid = WorldToGrid(player.currentTetromino.transform.localPosition + obj.transform.localPosition);
                    Debug.Log(grid);
                    cells[grid.y, grid.x].SetBlockObject(obj);
                }
            }
        }
    }

    public void Init()
    {
        count_x = (int)(boardWidth / 0.25f);
        count_y = (int)(boardHeight / 0.25f);

        CreateCells();
    }

    /// <summary>
    /// 셀 생성 함수
    /// </summary>
    private void CreateCells()
    {
        cells = new Cell[count_y, count_x];

        for(int y = 0; y < count_y; y++)
        {
            for(int x = 0; x < count_x; x++)
            {
                cells[y, x] = new Cell(x, y);
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
        tetromino.gameObject.name = $"Tetromino_{type}";

        tetromino.Init(type);

        player.currentTetromino = tetromino; // 임시
    }

    /// <summary>
    /// 블록이 존재하는 위치에 있는지 확인하는 함수 (업데이트 함수)
    /// </summary>
    private void CheckVaildPosition()
    {
        if (player.currentTetromino == null)
            return;

        Tetromino curBlock = player.currentTetromino;
        // 컨테이너 오브젝트는 (0,0) 이므로 고려 안하고 작성
        if (curBlock.transform.localPosition.x < 0) // x가 0보다 작으면
        {
            curBlock.transform.localPosition = new Vector2(0, curBlock.transform.localPosition.y);
        }

        if(curBlock.transform.localPosition.x > boardWidth) // x가 보드 최대값 보다 높으면
        {
            curBlock.transform.localPosition = new Vector2(boardWidth - 0.25f, curBlock.transform.localPosition.y);
        }

        if(curBlock.transform.localPosition.y < 0) // y가 0보다 작으면
        {
            curBlock.transform.localPosition = new Vector2(curBlock.transform.localPosition.x, 0);
        }
    }

    /// <summary>
    /// 모든 블록이 조건 내에 존재할 수 있는지 확인하는 함수 (블록이 겹치지 않았는지 보드 내에 있는지 확인)
    /// </summary>
    /// <returns>만족하면 true 아니면 false</returns>
    private void CheckAllBlockIsVaild()
    {
        if (player.currentTetromino == null)
             return;

        Tetromino curBlock = player.currentTetromino;
        int condition = 0; // 해당 블록이 어떤 상태인지 체크하는 변수

        foreach(var obj in curBlock.GetBlocks())
        {
            Vector2 pos = obj.transform.localPosition + curBlock.transform.localPosition; // 한 블록의 월드상 위치
            Vector2Int grid = WorldToGrid(pos);
            //Debug.Log($"{obj.name} : {pos}");
            // 해당위치에 블록이 있는지 확인
            if (grid.x > 0 && grid.y > 0 && grid.x < count_x && grid.y < count_y)
            {
                if (!cells[grid.y, grid.x].CheckVaild())
                {
                    Debug.Log(curBlock.prevVector);
                    curBlock.transform.localPosition = curBlock.prevVector;
                }
            }

            // 벗어났는지 확인
            condition = 0; // 초기화

            if (pos.x < 0) // 001
            {
                condition += 1;
            }

            if(pos.x > boardWidth) // 010
            {
                condition += 2;
            }

            if(pos.y < 0) // 100
            {
                condition += 4;
            }

            if(condition != 0) // 블록이 특정 상황에 있음
            {
                SetBlockByCondition(curBlock, obj, condition); // 블록 위치 잡기
            }
        }
    }

    private void SetBlockByCondition(Tetromino curTetromino, GameObject curBlock, int condition)
    {
        int mask = 1;
        for (int i = 0; i < 3; i++) // 3개의 조건 확인
        {
            if ((condition & mask) == Mathf.Pow(2, i))
            {
                switch (i)
                {
                    case 0: // x가 0보다 작음
                        curTetromino.transform.localPosition = new Vector2(0, curTetromino.transform.localPosition.y);
                        break;
                    case 1: // x가 0보다 큼
                        curTetromino.transform.localPosition = new Vector2(boardWidth - 0.25f, curTetromino.transform.localPosition.y);
                        break;
                    case 2: // y가 0보다 작음
                        curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x, 0);
                        if (curBlock.transform.localPosition.y < 0)
                            curTetromino.transform.localPosition += Vector3.up * 0.25f;
                        break;
                }
            }
            mask <<= 1; // 다음 자리 수로 수 변경
        }
    }

    // 좌표 변환 ===================================================================================

    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int(Mathf.CeilToInt(world.x / 0.25f + 0.01f), Mathf.CeilToInt(world.y / 0.25f + 0.01f)); // 2D 프로젝트이여서 월드의 x y값 사용
    }

    public Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(grid.x * 0.25f, grid.y * 0.25f);
    }
}