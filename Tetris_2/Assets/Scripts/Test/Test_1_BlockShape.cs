#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_1_BlockShape : Test_0_Factory
{
    private Block[] curBlock;
    public BlockShape shape;

    private int colorTypeCount = -1;
    public int shapeTypeCount = -1;

    private bool isBlockExist = false;

    protected override void Start()
    {
        base.Start();
        curBlock = new Block[4];
        shapeTypeCount = Enum.GetValues(typeof(BlockShape)).Length;
        colorTypeCount = Enum.GetValues(typeof(ColorType)).Length;
    }

    private void LateUpdate()
    {
        if (!isBlockExist)
            return;

        foreach(var block in curBlock)
        {
            // 한 개라도 비활성화되면
            if (!block.AvailableDrop)
            {
                for(int i = 0; i < curBlock.Length; i++)
                {
                    // 모든 블록 y축 움직임 정지
                    curBlock[i].AvailableDrop = false;
                    isBlockExist = false;
                }
            }
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        MakeShape(shape);
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

        switch (shape)
        {
            case BlockShape.O:
                Make_O(curBlock, spawnPoint.position);
                break;
            case BlockShape.L:
                Make_L(curBlock, spawnPoint.position);
                break;
            case BlockShape.J:
                Make_J(curBlock, spawnPoint.position);
                break;
            case BlockShape.S:
                Make_S(curBlock, spawnPoint.position);
                break;
            case BlockShape.Z:
                Make_Z(curBlock, spawnPoint.position);
                break;
            case BlockShape.T:
                Make_T(curBlock, spawnPoint.position);
                break;
            case BlockShape.I:
                Make_I(curBlock, spawnPoint.position);
                break;
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
        objs[0].transform.position = CenterVec;
        objs[1].transform.position = CenterVec + Vector3.right * FixedValues.BlockGap;
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
}
#endif