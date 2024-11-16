using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Board : MonoBehaviour
{
    private PlayerInput input;
    private Transform spawnPoint;
    
    private Block[,] blockGridInBoard;
    private int horizontalCount = 10;
    private int verticalCount = 30;

    private BlockFactory factory;
    private Block[] curBlock;
    private Block centerBlock;
    private BlockShape curShape;
    private Vector2Int[] blockCoordinates =
    {   // 회전 위치 좌표 배열 (시계방향)
        ///(-1, 1),  (0, 1), (1, 1),
        ///(-1, 0),  Center  (1, 0),
        ///(-1, -1), (0,-1), (1, -1)
        new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), 
        new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0,-1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 0)
    };
    /// <summary>
    /// 블록 칸 번호 저장변수 (curblock 배열의 0 2 3번째의 위치숫자)
    /// </summary>
    private int[] curBlockSlotNum;

    private int colorTypeCount = -1;
    public int shapeTypeCount = -1;

    private bool isBlockExist = false;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        input.OnMove += MoveCurBlock;

        factory = FindAnyObjectByType<BlockFactory>();
        spawnPoint = transform.GetChild(0);

        curBlock = new Block[4];
        curBlockSlotNum = new int[3];
        shapeTypeCount = Enum.GetValues(typeof(BlockShape)).Length;
        colorTypeCount = Enum.GetValues(typeof(ColorType)).Length;

        blockGridInBoard = new Block[horizontalCount + 1, verticalCount];
        for(int i = 0; i < horizontalCount + 1; i++)
        {
            for(int j = 0; j < verticalCount; j++)
            {
                blockGridInBoard[i, j] = null;
            }
        }

        SpawnBlock();
    }


    float timer = 0f;
    private void LateUpdate()
    {
        CheckGameOver();

        CheckBlockOverlap();

        // 밑에 블록이나 보드가 있으면 타이머 작동
        if (IsDownGridExist())
        {
            timer += Time.deltaTime;
        }
        else // 닿은게 없으면 타이머 초기화
        {
            timer = 0f;
        }

        // 대기 시간이 초과되면 블록 생성
        if (timer > 1f) 
        {
            BlockSpawnLoop();
            timer = 0f;
        }

        CheckLineClear();
    }

    private void BlockSpawnLoop()
    {
        foreach (var block in curBlock)
        {
            // 한 개라도 비활성화되면
            if (!block.AvailableDrop)
            {
                for(int i = 0; i < curBlock.Length; i++)
                {
                    int block_x = curBlock[i].GridPosition.x;
                    int block_y = curBlock[i].GridPosition.y;
                    blockGridInBoard[block_x, block_y] = curBlock[i]; // 블록 저장
                    Debug.Log(blockGridInBoard[block_x, block_y].AvailableDrop);
                }

                SpawnBlock();
                break;
            }
        }
    }

    /// <summary>
    /// 블록이 게임 보드 밖에 나갔는지 확인하는 함수 (게임 진행정지)
    /// </summary>
    private void CheckGameOver()
    {
        for(int i = 0; i < horizontalCount; i++)
        {
            if (blockGridInBoard[i, 20] != null) 
            {
                // y값 20에 있으면 블록 생상 중단 (어차피 그 이상은 20을 거쳐야하기 때문)
                isBlockExist = false;
            }
        }
    }
    
    /// <summary>
    /// 현재 움직이는 블록 밑 그리드 확인 함수 ( 블록 겹치기 방지 )
    /// </summary>
    private bool IsDownGridExist()
    {
        bool result = false;
        int availableGridCount = 0;

        foreach (var block in curBlock)
        {
            int block_x = block.GridPosition.x;
            int block_y = block.GridPosition.y;

            if (block_y == 0) // 지면과 만나면
            {
                // 모두 정지
                foreach (var item in curBlock)
                {
                    item.AvailableDrop = false;
                    result = true;
                }

                break;
            }
            else if (block_x > 0 && block_y > 0 && block_x < horizontalCount && block_y < verticalCount) // 보드 범위 내 체크
            {
                // y좌표 1칸 밑에 블록이 있으면 정지
                if (blockGridInBoard[block_x, block_y - 1] != null)
                {
                    // 모두 정지
                    foreach (var item in curBlock)
                    {
                        item.AvailableDrop = false;
                        result = true;
                    }

                    break;
                }
                else
                {
                    availableGridCount++;
                }
            }
        }

        if(availableGridCount == 4)
        {
            foreach (var item in curBlock)
            {
                item.AvailableDrop = true;
                result = false;
            }
        }

        return result;
    }

    private void CheckBlockOverlap()
    {
        if (curBlock == null)
            return;

        for (int i = 0; i < curBlock.Length; i++)
        {
            if (i == 1) continue;

            if (blockGridInBoard[curBlock[i].GridPosition.x, curBlock[i].GridPosition.y])
            {
                //Debug.Log("겹침");
                if (curBlockSlotNum[i > 0 ? i - 1 : i] == 7)
                {
                    // 왼쪽이 겹쳤는가
                    //Debug.Log("left");
                    foreach (var block in curBlock)
                    {
                        block.Move(1, 0);
                    }
                }
                else if (curBlockSlotNum[i > 0 ? i - 1 : i] == 3)
                {
                    // 오른쪽이 겹쳤는가
                    //Debug.Log("right");
                    foreach (var block in curBlock)
                    {
                        block.Move(-1, 0);
                    }

                }
                else
                {
                    // 위 상황에서 왼쪽이나 오른쪽으로 갈 공간이 부족한가
                    //Debug.Log("adsf");
                }
            }
        }
    }

    private void CheckLineClear()
    {
        GameObject[] blockObjs = new GameObject[horizontalCount];
        for(int y = 0; y < verticalCount; y++)
        {
            int count = 0;
            for(int x = 0; x < horizontalCount; x++)
            {
                if(blockGridInBoard[x + 1, y] != null)
                {
                    count++;
                    blockObjs[x] = blockGridInBoard[x + 1, y].gameObject;
                }
            }
            
            if(count == horizontalCount)
            {
                // 개수가 가로칸만큼 있으면 배열에 있는 모든 블록 비활성화
                for (int x = 0; x < horizontalCount; x++)
                {
                    blockObjs[x].SetActive(false);
                    blockObjs[x] = null;

                    blockGridInBoard[x + 1, y] = null;
                }

                // 모든 줄 내리기
                for (int upper = y + 1; upper < verticalCount; upper++)
                {
                    // Todo : 제거 후 위 블록 한 줄씩 내리고 다시 저장하기
                    //Block[] existBlockGrids = new Block[10];

                    for(int x = 1; x < horizontalCount + 1; x++)
                    {
                        // 배열 한 칸씩 내리기
                        // 실제 모델 한 칸씩 내리기
                        if (blockGridInBoard[x, upper] != null)
                        {
                            Block tempBlock = blockGridInBoard[x, upper];

                            blockGridInBoard[x, upper].Move(0, -1);
                            blockGridInBoard[x, upper] = null;
                            blockGridInBoard[x, upper - 1] = tempBlock;
                        }
                    }
                }
            }
        }
    }

    private void SpawnBlock()
    {
        isBlockExist = false;
        int rand = UnityEngine.Random.Range(0, shapeTypeCount);
        //int rand = (int)BlockShape.Z;
        MakeShape((BlockShape)rand);
        curShape = (BlockShape)rand;
        isBlockExist = true;
    }

    private void MakeShape(BlockShape shape)
    {
        int randColor = UnityEngine.Random.Range(0, colorTypeCount);

        for (int i = 0; i < curBlock.Length; i++)
        {
            GameObject obj = factory.SpawnBlock(Vector3.zero);
            curBlock[i] = obj.GetComponent<Block>();
            curBlock[i].SetColor(randColor);
        }

        // 모양대로 블록 생성
        switch (shape)
        {
            case BlockShape.O:
                Make_O(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 7;
                curBlockSlotNum[1] = 0;
                curBlockSlotNum[2] = 1;
                break;
            case BlockShape.L:
                Make_L(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 7;
                curBlockSlotNum[1] = 3;
                curBlockSlotNum[2] = 0;
                break;
            case BlockShape.J:
                Make_J(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 7;
                curBlockSlotNum[1] = 3;
                curBlockSlotNum[2] = 2;
                break;
            case BlockShape.S:
                Make_S(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 7;
                curBlockSlotNum[1] = 1;
                curBlockSlotNum[2] = 2;
                break;
            case BlockShape.Z:
                Make_Z(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 3;
                curBlockSlotNum[1] = 1;
                curBlockSlotNum[2] = 0;
                break;
            case BlockShape.T:
                Make_T(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 7;
                curBlockSlotNum[1] = 3;
                curBlockSlotNum[2] = 1;
                break;
            case BlockShape.I:
                Make_I(curBlock, spawnPoint.position);

                curBlockSlotNum[0] = 5;
                curBlockSlotNum[1] = 1;
                break;
        }

        centerBlock = curBlock[1]; // 회전을 위한 중심블록 설정
    }

    public void MoveCurBlock(Vector2 moveVec)
    {
        int checkCount = 0; // 이동가능한 블록 개수

        foreach(var block in curBlock)
        {
            block.SetGrid(new Vector2Int((int)block.transform.position.x, (int)block.transform.position.y)); // 정확한 위치 확인을 위한 그리드 위치값 갱신

            Vector2 nextPosition = block.GridPosition + moveVec;

            if(nextPosition.x > 0 && nextPosition.y >= 0 && nextPosition.x < horizontalCount + 1 && nextPosition.y < verticalCount)
            {
                Debug.Log(nextPosition);
                // 다음 위치에 블록이 존재하지 않으면
                if(blockGridInBoard[(int)nextPosition.x, (int)nextPosition.y] == null)
                {
                    checkCount++;
                }
            }
        }

        // 모든 블록이 이동 가능하면
        if(checkCount == 4)
        {
            foreach(var block in curBlock)
            {
                block.Move(moveVec);
            }
        }
    }

    public void RotateCurBlock()
    {
        // 블록 뒤섞여있을 때 빈칸 찾기
        // 블록 쌓이고 회전 시켜보기

        if(curShape == BlockShape.I)
        {
            // 0번째와 2번째만 이동시킨 후 
            for (int i = 0; i < curBlockSlotNum.Length - 1; i++)
            {
                int preNum = curBlockSlotNum[i] % 8;
                curBlockSlotNum[i] = (curBlockSlotNum[i] + 2) % 8;

                int nextX = blockCoordinates[curBlockSlotNum[i]].x - blockCoordinates[preNum].x;
                int nextY = blockCoordinates[curBlockSlotNum[i]].y - blockCoordinates[preNum].y;

                curBlock[i > 0 ? i + 1 : i].Move(nextX, nextY);

                // 3번째 블록을 2번째 블록 방향에 +1 위치로 옮기기
                if(i == 1)
                {
                    switch(curBlockSlotNum[1])
                    {
                        case 1:
                            curBlock[3].Move(nextX * 2, nextY * 2);
                            break;
                        case 3:
                            curBlock[3].Move(nextX * 2, nextY * 2);
                            break;
                        case 5:
                            curBlock[3].Move(nextX * 2, nextY * 2);
                            break;
                        case 7:
                            curBlock[3].Move(nextX * 2, nextY * 2);
                            break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < curBlockSlotNum.Length; i++)
            {
                int preNum = curBlockSlotNum[i] % 8;
                curBlockSlotNum[i] = (curBlockSlotNum[i] + 2) % 8;

                int nextX = blockCoordinates[curBlockSlotNum[i]].x - blockCoordinates[preNum].x;
                int nextY = blockCoordinates[curBlockSlotNum[i]].y - blockCoordinates[preNum].y;

                curBlock[i > 0 ? i + 1 : i].Move(nextX, nextY);
            }
        }

        // 보드를 통과 했는지 확인
        foreach(var checkBlock in curBlock)
        {
            // 보드 범위 확인
            if (checkBlock.transform.position.x < 1)
            {
                // 왼쪽 벽을 통과 했으면
                foreach (var block in curBlock)
                {
                    if (curShape == BlockShape.I && curBlockSlotNum[1] == 7) 
                    {
                        // I 모양은 특정 위치일 때 2칸옮기기
                        block.transform.position += Vector3.right * 2;
                    }
                    else
                    {
                        // 오른쪽으로 한 칸 옮기기
                        block.transform.position += Vector3.right;
                    }
                }
                break;
            }
            else if(checkBlock.transform.position.x > horizontalCount)
            {
                // 오른쪽 벽을 통과 했으면
                foreach (var block in curBlock)
                {                    
                    if (curShape == BlockShape.I && curBlockSlotNum[1] == 3) 
                    {
                        // 오른쪽으로 2칸 넘어올 때 2칸 왼쪽으로 옮기기
                        block.transform.position += Vector3.left * 2;
                    }
                    else
                    {
                        // 왼쪽으로 한 칸 옮기기
                        block.transform.position += Vector3.left;
                    }
                }
                break;
            }
        }
    }

    // 모양 함수 ======================================================================

    private void Make_O(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[2].transform.position = CenterVec + Vector3.up * FixedValues.BlockGap;
        objs[3].transform.position = CenterVec + Vector3.one * FixedValues.BlockGap;
    }

    private void Make_L(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[2].transform.position = objs[1].transform.position + Vector3.right * FixedValues.BlockGap;
        objs[3].transform.position = CenterVec + Vector3.up * FixedValues.BlockGap;
    }

    private void Make_J(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[2].transform.position = objs[1].transform.position + Vector3.right * FixedValues.BlockGap;
        objs[3].transform.position = objs[2].transform.position + Vector3.up * FixedValues.BlockGap;
    }

    private void Make_S(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[2].transform.position = objs[1].transform.position + Vector3.up * FixedValues.BlockGap;
        objs[3].transform.position = objs[2].transform.position + Vector3.right * FixedValues.BlockGap;
    }

    private void Make_Z(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[1].transform.position = CenterVec;
        objs[2].transform.position = CenterVec + Vector3.up * FixedValues.BlockGap;
        objs[3].transform.position = objs[2].transform.position + Vector3.left * FixedValues.BlockGap;
    }

    private void Make_T(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
        objs[2].transform.position = objs[1].transform.position + Vector3.right * FixedValues.BlockGap;
        objs[3].transform.position = objs[1].transform.position + Vector3.up * FixedValues.BlockGap;
    }

    private void Make_I(Block[] objs, Vector3 CenterVec)
    {
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.up * FixedValues.BlockGap;
        objs[2].transform.position = objs[1].transform.position + Vector3.up * FixedValues.BlockGap;
        objs[3].transform.position = objs[2].transform.position + Vector3.up * FixedValues.BlockGap;
    }

#if UNITY_EDITOR
    public void Test_ClearLine()
    {
        foreach(var block in curBlock)
        {
            block.AvailableDrop = false;
            block.gameObject.SetActive(false);
        }

        Block[] testBlocks = new Block[horizontalCount];

        // 1줄 생성 후
        for (int i = 0; i < horizontalCount; i++)
        {
            Vector2 horizonSpawnPoint = new Vector2(i + 1, 0);
            GameObject obj = factory.SpawnBlock(horizonSpawnPoint);
            testBlocks[i] = obj.GetComponent<Block>();
        }

        // 모든 블록 정지
        for (int i = 0; i < horizontalCount; i++)
        {
            blockGridInBoard[i + 1, 0] = testBlocks[i];
            testBlocks[i].availableDrop = false;
        }

        // 줄확인
        CheckLineClear();
    }
#endif
}