using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour
{
    TextMeshProUGUI endGameScoreText;
    Button restartButton;

    private int endGameScore = -1;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        endGameScoreText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        child = transform.GetChild(1);
        restartButton = child.GetComponent<Button>();
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

    }

    private void Start()
    {
        GameManager.Instance.OnGameEnd += ActivePanel;
        gameObject.SetActive(false);
    }

    private void ActivePanel()
    {
        this.gameObject.SetActive(true);
        TetrisBoard board = FindAnyObjectByType<TetrisBoard>();

        endGameScore = board.TetrisScore;
        endGameScoreText.text = endGameScore.ToString();

        GameManager.Instance.WriteHighScore(endGameScore);
    }
}
