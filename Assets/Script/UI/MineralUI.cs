using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineralUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI text;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        GameResourceManager.Instance.OnMineralChangedWithValue += DetectChangeValue;
        GameResourceManager.Instance.OnMineralChanged += ChangeText;
    }

    private void ChangeText(float value)
    {
        text.text = value.ToString();
    }

    private void DetectChangeValue(float value)
    {
        UIManager.Instance.show<FloatingText>((prefab) =>
        {
            Vector3 pos = rect.TransformPoint(new Vector3(0, rect.rect.height * (1 - rect.pivot.y), 0));
            prefab.Init(value, pos);
        });
    }

    private void OnDisable()
    {
        GameResourceManager.Instance.OnMineralChangedWithValue -= DetectChangeValue;
        GameResourceManager.Instance.OnMineralChanged -= ChangeText;
    }
}
