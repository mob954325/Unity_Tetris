using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 간단한 싱글톤 생성

    public bool isGameStart = false;

    private void Awake()
    {
        Instance = this;
    }
}
