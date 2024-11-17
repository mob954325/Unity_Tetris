using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TextMeshProUGUI countText;

    private float readyTime = 5f;
    private int score;

    /// <summary>
    /// 점수 접근 및 수정 프로퍼티
    /// </summary>
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChange?.Invoke(score);
        }
    }

    public Action<int> OnScoreChange;
    public Action OnStartGame;
    public Action OnEndGame;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        countText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Score = 0;
        countText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 시작 버튼용 함수
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(StartProcess());
    }

    private IEnumerator StartProcess()
    {
        float timeElapsed = 0.0f;

        countText.gameObject.SetActive(true);

        while (timeElapsed < readyTime)
        {
            timeElapsed += Time.deltaTime;

            countText.text = $"{(int)readyTime - (int)timeElapsed}";
            yield return null;
        }

        countText.gameObject.SetActive(false);
        OnStartGame?.Invoke();
    }
}