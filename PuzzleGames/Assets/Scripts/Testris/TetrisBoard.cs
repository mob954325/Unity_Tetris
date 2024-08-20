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
        if(player.currentTetromino != null)
        {
            if(!player.currentTetromino.checkMoveAllow()) // 해당 블록위치 셀에 저장
            {
                // 블록 새로 만들고
                foreach(var obj in player.currentTetromino.GetBlocks())
                {
                    Vector2Int grid = WorldToGrid(player.currentTetromino.transform.localPosition + obj.transform.localPosition);
                    Debug.Log($"{obj.name} {grid.x} , {grid.y}");
                    cells[grid.y, grid.x].SetBlockObject(obj);
                }

                // 한 줄 체크
                CheckHorizontal();
            }
        }

        CheckAllBlockIsVaild();
    }
    
    /// <summary>
    /// 초기화 함수
    /// </summary>
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
        tetromino.transform.localPosition = spawnPoint.transform.localPosition; //
        tetromino.gameObject.name = $"Tetromino_{type}";

        tetromino.Init(type);

        player.currentTetromino = tetromino; // 임시
        player.Init();
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

            Debug.Log($"{obj.gameObject.name} : {pos} {grid}");
            // 해당위치에 블록이 있는지 확인
            if (grid.x >= 0 && grid.y >= 0 && grid.x < count_x && grid.y < count_y + 5f) //
            {
                if (!cells[grid.y, grid.x].CheckVaild())
                {
                    curBlock.transform.localPosition = curBlock.prevVector;
                    curBlock.SetMoveAllow(false);
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

    /// <summary>
    /// 블록 위치 조정하는 함수 (CheckAllBlockIsVaild 함수용)
    /// </summary>
    /// <param name="curTetromino">테트리스 블록</param>
    /// <param name="curBlock">현재 체크하고 있는 테트리스 블록의 자식 블록</param>
    /// <param name="condition">현재 블록 상태</param>
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
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x < 0)
                            curTetromino.transform.localPosition += Vector3.right * 0.25f;
                        break;
                    case 1: // x가 boardWidth 보다 큼
                        curTetromino.transform.localPosition = new Vector2(boardWidth - 0.25f, curTetromino.transform.localPosition.y);
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x > boardWidth - 0.25f)
                            curTetromino.transform.localPosition += Vector3.left * 0.25f;
                            break;
                    case 2: // y가 0보다 작음
                        curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x, 0);
                        if (curTetromino.transform.localPosition.y + curBlock.transform.localPosition.y < 0)
                            curTetromino.transform.localPosition += Vector3.up * 0.25f;
                        break;
                }
            }
            mask <<= 1; // 다음 자리 수로 수 변경
        }
    }

    /// <summary>
    /// 가로줄 체크 함수
    /// </summary>
    private void CheckHorizontal()
    {
        // 한줄 체크
        for(int y = 0; y < count_y; y++)
        {
            int count = 0; // y줄의 x블록 개수
            for(int x = 0; x < count_x; x++)
            {
                if (!cells[y,x].CheckVaild())
                {
                    count++;
                }
            }

            // 해당 줄에 모든 블록이 존재하면 제거 (1줄 제거)
            if (count >= count_x)
            {
                for(int i = 0; i < count_x; i++)
                {
                    cells[y, i].RemoveBlockObject();
                    DownOneBlock(i, y);
                }
            }
        }
    }

    /// <summary>
    /// 블록 한 칸 내리기
    /// </summary>
    private void DownOneBlock(int gridX, int gridY)
    {
        int checkHeight = gridY + 1;
        while(checkHeight < count_y)
        {
            if (!cells[checkHeight, gridX].CheckVaild()) // 해당 위치에 블록이 없으면 종료
            {
                GameObject upperObject = cells[checkHeight, gridX].GetBlockObject();
                upperObject.transform.localPosition = new Vector2(upperObject.transform.localPosition.x, upperObject.transform.localPosition.y - 0.25f); // 위치 내림
                cells[checkHeight - 1, gridX].SetBlockObject(upperObject);  // 셀 재설정
                cells[checkHeight, gridX].RemoveBlockObject(true);          // 기존에 있던 셀 정보 제거
            }

            checkHeight++;
        }
    }

    // 좌표 변환 ===================================================================================

    public Vector2Int WorldToGrid(Vector2 world)
    {
        return new Vector2Int(Mathf.CeilToInt(world.x / 0.25f + 0.01f) - 1, Mathf.CeilToInt(world.y / 0.25f + 0.01f) - 1); // 2D 프로젝트이여서 월드의 x y값 사용
    }

    public Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(grid.x * 0.25f, grid.y * 0.25f);
    }
}