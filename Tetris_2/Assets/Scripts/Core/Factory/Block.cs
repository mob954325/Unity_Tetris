using System;
using UnityEngine;

public class Block : MonoBehaviour, IProduct
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] blockSprite;

    [SerializeField] private Vector2Int gridPosition;

    /// <summary>
    /// 블록 그리드 좌표 접근용 프로퍼티
    /// </summary>
    public Vector2Int GridPosition { get => gridPosition; }

    private string productName = "Block";

    public string ProductName { get; set; }

    private float timer = 0f;
    private float maxTime = 1f;

    public bool availableDrop = true; //

    /// <summary>
    /// availableDrop 변수 접근 및 수정 프로퍼티
    /// </summary>
    public bool AvailableDrop { get => availableDrop; set => availableDrop = value; }

    public Action BeforeDisable { get; set; }

    private void FixedUpdate()
    {
        if (timer > maxTime)
        {
            if(AvailableDrop)
            {
                transform.position -= Vector3.up * FixedValues.BlockGap;
            }

            timer = 0f;
        }

        gridPosition = WorldToGrid();
        timer += Time.fixedDeltaTime;
    }

    private void OnDisable()
    {
        timer = 0f; // 타이머 초기화 후 비활성화(재생성될 때 내려가는 속도 달라지는 거 방지)

        BeforeDisable?.Invoke();
        BeforeDisable = null;
    }

    public void Initialize()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.position = Vector3.zero;
        AvailableDrop = true;
    }

    public void SetSpawnPosition(Vector3 positionVec)
    {
        gameObject.transform.position = positionVec;
    }

    public void SetColor(int colorTypeNum)
    {
        spriteRenderer.sprite = blockSprite[colorTypeNum];
        spriteRenderer.color = Color.white;
    }

    public void SetGrid(Vector2Int grid)
    {
        gridPosition = grid;
    }

    public void Move(Vector2 moveVec)
    {
        transform.position = new Vector2(transform.position.x + moveVec.x, transform.position.y + moveVec.y);
    }

    public void Move(int x, int y)
    {
        transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
    }

    private Vector2Int WorldToGrid()
    {
        return new Vector2Int((int)(transform.position.x / FixedValues.BlockGap), (int)(transform.position.y / FixedValues.BlockGap));
    }
}