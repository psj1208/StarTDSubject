using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineralUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        GameResourceManager.Instance.OnMineralChanged += ChangeText;
    }

    private void ChangeText(float value)
    {
        text.text = value.ToString();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameResourceManager.Instance.OnMineralChanged -= ChangeText;
    }
}
