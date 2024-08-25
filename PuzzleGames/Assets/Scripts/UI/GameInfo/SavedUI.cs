using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedUI : MonoBehaviour
{
    public Sprite[] BlockSprites;

    private Image blockUI;

    private void Awake()
    {
        blockUI = GetComponent<Image>();
    }

    public void SetBlockUI(ShapeType type)
    {
        blockUI.sprite = BlockSprites[(int)type];
    }
}