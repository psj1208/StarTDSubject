using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalSkill : SkillBase
{
    private float criticalChance = 0.5f;
    private float criticalMultiplier = 2f;

    public override float ProcessDamage(float value)
    {
        if (Random.value <= criticalChance)
            return value * criticalMultiplier;

        return value;
    }
}
