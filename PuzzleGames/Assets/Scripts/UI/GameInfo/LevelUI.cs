using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    TextMeshProUGUI levelText;

    private void Awake()
    {
        levelText = GetComponent<TextMeshProUGUI>();
    }

    public void SetLevelText(int currentLv)
    {
        levelText.text = $"Lv.{currentLv}";
    }
}