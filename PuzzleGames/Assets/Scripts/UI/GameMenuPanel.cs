using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour
{
    TetrisBoard board; 

    private TextMeshProUGUI highScoreText;
    private Button startButton;

    private void Awake()
    {
        board = FindAnyObjectByType<TetrisBoard>();

        Transform child = transform.GetChild(1);
        highScoreText = child.GetComponent<TextMeshProUGUI>();        

        child = transform.GetChild(2);
        startButton = child.GetComponent<Button>();
        startButton.onClick.AddListener(() =>
        {
            board.StartTetris();
            this.gameObject.SetActive(false);
        });
    }

    public void SetHighScore(int score)
    {
        highScoreText.text = score.ToString();
    }
}