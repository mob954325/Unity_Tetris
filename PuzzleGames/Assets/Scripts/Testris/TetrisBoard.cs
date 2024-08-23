using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisBoard : MonoBehaviour
{
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

    /// <summary>
    /// 테트리스 현재 스코어
    /// </summary>
    public int tetrisScore;

    /// <summary>
    /// 테트리스 스코어 접근 및 수정 프로퍼티
    /// </summary>
    public int TetrisScore
    {
        get => tetrisScore;
        set
        {
            tetrisScore = value;
            OnScoreChange?.Invoke(tetrisScore);
        }
    }

    /// <summary>
    /// 테트리스 현재 레벨
    /// </summary>
    public int tetrisLevel;

    /// <summary>
    /// 테트리스 레벨 접근 및 수정 프로퍼티
    /// </summary>
    public int TetrisLevel
    {
        get => tetrisLevel;
        set
        {
            tetrisLevel = value;
            OnLevelChange?.Invoke(tetrisLevel);
        }
    }

    /// <summary>
    /// 레벨 변경 타이머
    /// </summary>
    private float levelTimer = 0f;
    
    /// <summary>
    /// 레벨 변경 시간 ( 타이머가 해당 시간에 도달하면 레벨 증가)
    /// </summary>
    private const float levelMaxTime = 60f;

    /// <summary>
    /// 스코어 변경 시 호출되는 델리게이트
    /// </summary>
    public Action<int> OnScoreChange;

    /// <summary>
    /// 레벨 변경 시 호출되는 델리게이트
    /// </summary>
    public Action<int> OnLevelChange;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        Transform child = transform.GetChild(0);

        boardWidth = child.GetChild(0).transform.localScale.x;
        boardHeight = child.GetChild(0).transform.localScale.y;
        boardHeight += 0.25f * 5; // 5칸 추가 (패배 확인용)

        child = transform.GetChild(1);
        tetrominoContainer = child.gameObject.transform;

        child = transform.GetChild(2);
        spawnPoint = child.gameObject.transform;
    }

    private void Start()
    {
        Init();        
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.isGameStart)
            return;

        levelTimer += Time.fixedDeltaTime;

        if (levelTimer > levelMaxTime)
        {
            levelTimer = 0f;
            TetrisLevel++;
        }

        if(player.currentTetromino != null)
        {
            if(!player.currentTetromino.checkMoveAllow()) // 움직임이 멈추면 해당 위치 블록 저장
            {
                // 블록 저장
                foreach(var obj in player.currentTetromino.GetBlocks())
                {
                    Vector2Int grid = WorldToGrid(player.currentTetromino.transform.localPosition + obj.transform.localPosition);
                    cells[grid.y, grid.x].SetBlockObject(obj); // 블록 저장

                    if (grid.y >= count_y - 5) // 보드 범위에 벗어났으면 
                    {
                        // 게임 오버
                        GameManager.Instance.isGameStart = false;
                    }
                }

                int enumMaxLength = Enum.GetNames(typeof(ShapeType)).Length;
                int rand = Random.Range(1, enumMaxLength);  // 랜덤 블록 값

                CreateTetromino((ShapeType)rand);   // 새로운 블록 생성
            }
        }

        CheckHorizontal();      // 한 줄이 만들어졌는지 체크
        CheckAllBlockIsVaild(); // 현재 블록 위치 체크
        CheckBottom();
    }
    
    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init()
    {
        count_x = (int)(boardWidth / 0.25f);
        count_y = (int)(boardHeight / 0.25f);

        GameManager.Instance.isGameStart = true; // 임시
        TetrisLevel = 0;
        TetrisScore = 0;

        CreateCells();
        CreateTetromino(ShapeType.I);   // 새로운 블록 생성
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

        tetromino.Init(type, 0.5f - (0.1f * TetrisLevel));

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

            // 해당위치에 블록이 있는지 확인
            if (grid.x >= 0 && grid.y >= 0 && grid.x < count_x && grid.y < count_y - 5) // 보드 범위 내 
            {
                if (!cells[grid.y, grid.x].CheckVaild()) // 해당 위치에 블록이 존재하면
                {
                    Vector2 prev = curBlock.prevVector;
                    curBlock.transform.localPosition = curBlock.prevVector; // 이전 위치로 위치 변경 후
                    curBlock.prevVector = prev;
                    curBlock.SetMoveAllow(false);                           // 움직임 권한 제거
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
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x < 0)
                        {
                            float gap = curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x - 0.125f; // 경계선에서 넘어버린 크기 값
                            curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x - gap, curTetromino.transform.localPosition.y); // 차이 만큼 다시 안쪽으로 옮기기
                        }
                        break;
                    case 1: // x가 boardWidth 보다 큼
                        if (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x > boardWidth - 0.25f)
                        {
                            float gap = (boardWidth - 0.25f) - (curTetromino.transform.localPosition.x + curBlock.transform.localPosition.x - 0.125f);
                            curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x + gap, curTetromino.transform.localPosition.y);
                        }
                        break;
                    case 2: // y가 0보다 작음
                        if (curTetromino.transform.localPosition.y + curBlock.transform.localPosition.y < 0)
                        {
                            float gap = curTetromino.transform.localPosition.y + curBlock.transform.localPosition.y - 0.125f;
                            curTetromino.transform.localPosition = new Vector2(curTetromino.transform.localPosition.x, curTetromino.transform.localPosition.y - gap);
                        }
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
        int streakCount = 0;

        // 한줄 체크
        for(int y = 0; y < count_y; y++)
        {
            int xBlockCount = 0; // y줄의 x블록 개수

            for(int x = 0; x < count_x; x++)
            {
                if (!cells[y,x].CheckVaild())
                {
                    xBlockCount++;
                }
            }

            // 해당 줄에 모든 블록이 존재하면 제거 (1줄 제거)
            if (xBlockCount >= count_x)
            {
                for(int i = 0; i < count_x; i++)
                {
                    cells[y, i].RemoveBlockObject();
                    DownOneBlock(i, y);
                }
                streakCount++;
            }
        }

        TetrisScore += 1000 * (streakCount * 2);
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

    /// <summary>
    /// 밑 블록 체크 ( 블록 드랍 체크용 )
    /// </summary>
    private void CheckBottom()
    {
        Tetromino curBlock = player.currentTetromino;
        int lowestBlockY = count_y;
        Vector2 resultBlock = Vector2.zero;
        Vector2Int resultGrid = new Vector2Int(-1, -1); // 목표 위치 그리드 값

        foreach (var block in curBlock.GetBlocks())
        {
            // 가장 낮은 블록 찾기
            Vector2 world = curBlock.transform.localPosition + block.transform.localPosition;
            Vector2Int grid = WorldToGrid(world);

            // 높이 체크 
            for(int y = 0; y < count_y; y++) // 현재 x위치의 모든 y값 셀 체크
            {
                if (!cells[y, grid.x].CheckVaild()) continue;

                if (resultGrid.y < 0) // 처음 체크하고 가장 첫 줄이 비어있으면
                {
                    resultBlock = world;
                    resultGrid = new Vector2Int(grid.x, 0);
                }

                // 두번째 줄부터 밑에 블록이 있고 기존에 저장되있는 위치보다 높으면
                if (y > 0 && y > resultGrid.y && !cells[y - 1, grid.x].CheckVaild()) // 가장 높은 위치의 블록이면 변수 저장
                {
                    resultBlock = world;
                    resultGrid = new Vector2Int(grid.x, y);
                    break;
                }
                else if(y == resultGrid.y) continue; // 다른 블록이 같은 위치면 다음 블록 체크
            }
        }

        // 체크한 그리드와 curblock의 y축 차이가 존재하면 해당 값 많큼 올리기
        Vector2 resultVec = Vector2.zero; // 결과
        Vector2 gap = new Vector2(resultBlock.x - 0.125f, resultBlock.y - 0.125f) - (Vector2)curBlock.transform.localPosition; // 목표 블록과 현재 블록의 차이값

        Debug.Log($"result {gap}");
        if(gap.y != 0)
        {
            Vector2 worldVec = GridToWorld(resultGrid);
            resultVec = new Vector2(worldVec.x, worldVec.y + 0.25f); // 한 칸만큼 값 추가 (부모 포지션과 블록 포지션의 차이가 많아봤자 1칸만 차이나기 때문)
        }
        else
        {
            resultVec = GridToWorld(resultGrid);
        }

        Debug.Log($"result {resultVec}");

        curBlock.SetLowestYVector(resultVec - gap);
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