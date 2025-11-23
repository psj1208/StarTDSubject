using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    [SerializeField] protected SkillSelectUI selectUI;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI DescriptionText;
    [SerializeField] Exceed skillObject;
    Unit targetUnit;
    RectTransform rect;
    Button button;

    public void Init(Exceed obj, Unit unit, SkillSelectUI ui)
    {
        rect = GetComponent<RectTransform>();
        selectUI = ui;
        targetUnit = unit;
        skillObject = obj;
        nameText.text = skillObject.effectName;
        DescriptionText.text = skillObject.description;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnButtonClicked()
    {
        skillObject.Apply(targetUnit);
        selectUI.SelectOver();
    }
}
