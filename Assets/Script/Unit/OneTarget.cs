using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OneTarget : Unit
{
    [SerializeField] Enemy atkTarget;
    protected override void Update()
    {
        curTime += Time.deltaTime;

        SearchTarget();
        Attack();
    }
    protected override void Attack()
    {
        if (atkTarget != null && curTime >= attackTerm)
        {
            curTime = 0;
            atkTarget.GetDamage(atk);
        }
    }

    protected virtual void SearchTarget()
    {
        atkTarget = null;

        if (hits.Count <= 0)
            return;
        foreach (var h in hits)
        {
            if (h == null) continue;

            if (!h.IsDead)
            {
                atkTarget = h;
                return;
            }
        }
    }
}
