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
            float value = TotalAtk;
            foreach (var s in skills)
                value = s.ProcessDamage(value);
            atkTarget.GetDamage(value);
            animator?.Play("Attack");
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
