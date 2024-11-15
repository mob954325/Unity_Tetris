#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_0_Factory : TestBase
{
    public BlockFactory factory;

    protected Transform spawnPoint;

    protected virtual void Start()
    {
        if(transform.GetChild(0) == null)
        {
            Transform child = Instantiate(transform);
            child.SetParent(transform);
            spawnPoint = child;
        }
        else
        {
            spawnPoint = transform.GetChild(0);
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        factory.SpawnBlock(spawnPoint.position);
    }
}
#endif