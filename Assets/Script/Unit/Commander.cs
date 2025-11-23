using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : OneTarget
{
    [SerializeField] private float hp;
    [SerializeField] private float maxHp = 100;
    HasHpBar hpBar = new HasHpBar();

    protected override void Start()
    {
        base.Start();
        hp = maxHp;
        hpBar.Init(transform, maxHp);
    }

    public void GetDamage(int dam)
    {
        hp = Mathf.Clamp(hp - dam, 0, maxHp);
        hpBar.SetHp(hp);

        if (IsDeath())
            DeathAction();
    }

    private bool IsDeath()
    {
        return hp <= 0 ? true : false;
    }
    private void DeathAction()
    {
        Debug.Log("Commander Die!");
        GameManager.Instance.GameEnd(false);
    }
}
