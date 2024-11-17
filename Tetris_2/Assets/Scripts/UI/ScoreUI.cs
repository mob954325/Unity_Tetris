using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    GameManager manager;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();   
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        SetText(0);
        manager.OnScoreChange += SetText;        
    }

    public void SetText(int scoreValue)
    {
        scoreText.text = scoreValue.ToString();
    }
}