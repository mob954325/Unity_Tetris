using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 간단한 싱글톤 생성
    private GameMenuPanel gameMenuPanel;

    /// <summary>
    /// 최고 점수
    /// </summary>
    private int highestScore = -1;

    /// <summary>
    /// 최고 점수 접근 및 수정용 프로퍼티
    /// </summary>
    public int HighestScore
    {
        get => highestScore;
        set
        {
            highestScore = value;
        }
    }

    private string scorePath = Application.dataPath + "/Score/score.txt";

    /// <summary>
    /// 게임이 시작되었는지 확인하는 변수
    /// </summary>
    public bool isGameStart = false;

    /// <summary>
    /// 게임이 시작 될 때 호출되는 델리게이트
    /// </summary>
    public Action OnGameStart;

    /// <summary>
    /// 게임이 종료 될 때 호출되는 델리게이트
    /// </summary>
    public Action OnGameEnd;

    /// <summary>
    /// 최고 점수를 얻을 때 호출되는 델리게이트
    /// </summary>
    public Action<int> OnHighScoreGet;

    private void Awake()
    {
        Instance = this;
        gameMenuPanel = FindAnyObjectByType<GameMenuPanel>();
    }

    private void Start()
    {
        ReadHighScore();
    }

    // 파일 텍스트 코드 =========================================================================================

    /// <summary>
    /// 텍스트 파일의 점수 쓰기
    /// </summary>
    public void WriteHighScore(int score)
    {
        if (score < HighestScore) // 최고 점수가 아니면 무시
            return;

        if(!File.Exists(scorePath))
        {
            using(StreamWriter sw = new StreamWriter(scorePath))
            {
                sw.WriteLine("0");
            }
        }
        else
        {
            StreamWriter writer = new StreamWriter(scorePath, false); //Write some text to the score.txt file
            writer.Write($"{score}");
            writer.Close();
        }
    }

    /// <summary>
    /// 텍스트 파일의 점수 읽기
    /// </summary>
    private void ReadHighScore()
    {
        if (!File.Exists(scorePath))
        {
            using (StreamReader sr = new StreamReader(scorePath))
            {
                Debug.Log("File Does not exist");
            }
        }

        //Read the text from directly from the score.txt file
        StreamReader reader = new StreamReader(scorePath);
        HighestScore = Int32.Parse(reader.ReadToEnd());
        gameMenuPanel.SetHighScore(HighestScore);

        reader.Close();
    }
}
