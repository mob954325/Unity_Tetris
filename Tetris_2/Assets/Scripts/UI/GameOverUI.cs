using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private GameManager manager;

    private Button RestartBtn;
    private ScoreUI resultUI;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        resultUI = child.GetComponent<ScoreUI>();

        child = transform.GetChild(1);
        RestartBtn = child.GetComponent<Button>();
        RestartBtn.onClick.AddListener(() => { SceneManager.LoadScene(0); }); // 게임 씬 다시 로드

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Start()
    {
        manager.OnEndGame += () =>
        {
            resultUI.SetText(manager.Score);

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        };
    }

}