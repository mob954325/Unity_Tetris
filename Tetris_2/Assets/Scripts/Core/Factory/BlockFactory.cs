using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    [Header("Product")]
    public GameObject BlockPrefab;

    [Header("FactorySetting")]

    [SerializeField]
    private List<GameObject> productsList;
    private Queue<GameObject> readyQueue; 

    public int capacity = 4;

    private void Awake()
    {
        productsList = new List<GameObject>(capacity);
        readyQueue = new Queue<GameObject>(capacity);

        for(int i = 0; i < capacity; i++)
        {
            InitializeBlock(i);
        }
    }


    /// <summary>
    /// 블록 스폰 함수
    /// </summary>
    /// <param name="position">생성할 위치</param>
    /// <returns>스폰될 블록 오브젝트</returns>
    public GameObject SpawnBlock(Vector3 position)
    {
        GameObject product = null;

        if (readyQueue.Count <= 0)
        { 
            ExtendCapacity();
        }

        product = readyQueue.Dequeue();
        product.transform.position = position;
        product.GetComponent<Block>().BeforeDisable += () => { readyQueue.Enqueue(product); };

        product.SetActive(true);

        return product;
    }

    /// <summary>
    /// 블록 생성 및 초기화 함수, 리스트 및 레디 큐에 추가됨
    /// </summary>
    /// <param name="number">프로덕트 넘버링</param>
    /// <returns>생성된 오브젝트</returns>
    private GameObject InitializeBlock(int number)
    {
        GameObject obj = null;

        GameObject product = Instantiate(BlockPrefab);
        product.name = $"Block_{number}";
        productsList.Add(product);
        readyQueue.Enqueue(product);
        
        Block block = product.GetComponent<Block>();
        block.Initialize();
        
        obj = product;
        
        product.gameObject.SetActive(false);

        return obj;
    }

    private void ExtendCapacity()
    {
        int preCapacity = capacity;
        capacity *= 2;

        List<GameObject> list = new List<GameObject>(preCapacity);

        foreach(GameObject product in productsList)
        {
            list.Add(product);
        }

        productsList.Clear();
        readyQueue.Clear();

        productsList = new List<GameObject>(capacity);
        readyQueue = new Queue<GameObject>(capacity);

        for (int i = 0; i < capacity; i++)
        {
            if (i < preCapacity)
            {
                productsList.Add(list[i]);
            }
            else
            {
                InitializeBlock(i);
            }
        }
    }
}
