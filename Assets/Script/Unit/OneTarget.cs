using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OneTarget : Unit
{
    protected override void Attack()
    {
        if (curTime >= attackTerm && (hits.Count > 0))
        {
            curTime = 0;
            hits[0].GetComponent<Enemy>().GetDamage(atk);
        }
    }
}
