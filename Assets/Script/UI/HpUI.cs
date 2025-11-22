using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : BaseUI
{
    [SerializeField] Transform parentInWorld;
    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    public float CurHp { set { curHp = value; text.text = curHp.ToString(); } }
    [SerializeField] Image hpbar;
    [SerializeField] TextMeshProUGUI text;
    RectTransform rect;
    RectTransform mainRect;
    Camera cam;

    public void Init(Transform trans, float max)
    {
        parentInWorld = trans;
        maxHp = max;
        CurHp = maxHp;
        hpbar.fillAmount = 1f;
        rect = GetComponent<RectTransform>();
        mainRect = UIManager.Instance.MainCanvas.GetComponent<RectTransform>();
        cam = Camera.main;
    }

    public void SetHp(float cur)
    {
        hpbar.fillAmount = curHp / maxHp;
        CurHp = cur;
    }

    private void Update()
    {
        rect.anchoredPosition = Util.WorldToCanvasInCameraSpace(parentInWorld.position, mainRect, cam) + new Vector2(0, 50f);
    }
}
