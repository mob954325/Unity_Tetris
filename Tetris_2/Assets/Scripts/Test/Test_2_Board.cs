#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_2_Board : TestBase
{
    public GameObject boardObj;

    public int horzonCount = -1;
    public int vertivalCount = -1;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        GameObject parentObj = new GameObject("Board");

        // 가로
        for(int i = 0; i < horzonCount + 2; i++)
        {
            GameObject obj = Instantiate(boardObj, parentObj.transform);
            obj.transform.localPosition = Vector3.right * i * FixedValues.BlockGap;
        }

        // 세로
        for(int i = 1; i < vertivalCount + 1; i++)
        {
            GameObject rightObj = Instantiate(boardObj, parentObj.transform); 
            GameObject leftObj = Instantiate(boardObj, parentObj.transform); 

            rightObj.transform.localPosition = Vector3.up * i * FixedValues.BlockGap;
            leftObj.transform.localPosition = rightObj.transform.position + Vector3.right * (horzonCount + 1) * FixedValues.BlockGap;
        }
    }
}
#endif