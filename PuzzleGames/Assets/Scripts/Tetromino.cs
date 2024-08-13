using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    /// <summary>
    /// 떨어지는 값 크기 (0.25 == 한 칸)
    /// </summary>
    private const float DropScale = 0.25f;

    /// <summary>
    /// 다음 칸으로 떨어지는데 걸리는 시간 타이머 (시간이 되면 초기화 후 위치변경)
    /// </summary>
    private float dropTimer = 0f;

    /// <summary>
    /// 움직일 수 있는지 체크하는 변수 (움직일 수 있으면 true 아니면 false)
    /// </summary>
    public bool allowMove = false;

    private void Awake()
    {
        allowMove = !allowMove;
    }

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (allowMove && IsVaildPosition() && dropTimer > 1f)
        {
            dropTimer = 0f;
            transform.Translate(Vector2.down * DropScale);
        }
    }

    /// <summary>
    /// 블록이 존재할 수 있는 공간인지 확인하는 함수 (카메라 안 = 존재가능, 그 외 = false)
    /// </summary>
    /// <returns></returns>
    private bool IsVaildPosition()
    {
        Vector2 currentPos = Camera.main.WorldToScreenPoint(transform.position);

        return (currentPos.x < Screen.width && currentPos.x > 0 && currentPos.y < Screen.height && currentPos.y > 0);
    }

    public void MoveObjet(Vector2 inputVec)
    {
        if (!IsVaildPosition()) return;

        transform.Translate(inputVec * 0.25f);
    }
}
