using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeEnemy : Enemy
{
    HasHpBar hpUI = new HasHpBar();

    protected override void Start()
    {
        base.Start();
        hpUI.Init(transform, maxHp);
    }

    public override void GetDamage(float dam)
    {
        curHp = Mathf.Clamp(curHp - dam, 0, maxHp);
        Debug.Log($"{gameObject.name}이 공격받음! Dam : {dam}");
        hpUI.SetHp(curHp);
        UIManager.Instance.show<DamageText>((prefab) =>
        {
            prefab.SetDamage(dam, transform.position);
        });
        Death();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        hpUI.DestroyUI();
    }
}
