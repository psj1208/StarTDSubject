using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHpBar
{
    private float maxHp;
    private float curHp;
    private Transform targetTransform;
    private HpUI ui;

    public void Init(Transform target, float max)
    {
        targetTransform = target;
        maxHp = max;
        curHp = max;

        UIManager.Instance.show<HpUI>((hpUI) =>
        {
            ui = hpUI;
            ui.Init(targetTransform, maxHp);
        });
    }

    public void SetHp(float cur)
    {
        ui.SetHp(cur);
    }

    public void DestroyUI()
    {
        ui.DestroyUI();
    }
}
