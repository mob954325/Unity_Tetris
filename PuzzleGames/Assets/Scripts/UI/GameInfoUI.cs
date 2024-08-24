using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoUI : MonoBehaviour
{
    TetrisBoard tetris;

    /// <summary>
    /// 스코어 UI
    /// </summary>
    ScoreUI scoreUI;

    /// <summary>
    /// 레벨 UI
    /// </summary>
    LevelUI levelUI;

    /// <summary>
    /// 저장된 블록 출력하는 UI
    /// </summary>
    SavedUI savedUI;

    private void Awake()
    {
        tetris = FindAnyObjectByType<TetrisBoard>();

        scoreUI = GetComponentInChildren<ScoreUI>();
        levelUI = GetComponentInChildren<LevelUI>();
        savedUI = GetComponentInChildren<SavedUI>();

        tetris.OnScoreChange = scoreUI.SetScoreText;
        tetris.OnLevelChange = levelUI.SetLevelText;
        tetris.OnShapeTypeChange = savedUI.SetBlockUI;
    }
}