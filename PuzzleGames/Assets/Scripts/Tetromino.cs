using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float dropTimer = 0f;

    private void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (dropTimer > 1f)
        {
            dropTimer = 0f;
            transform.Translate(Vector2.down * 0.25f);
        }
    }
}
