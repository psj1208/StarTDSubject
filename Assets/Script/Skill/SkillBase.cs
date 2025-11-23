using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected Unit unit;

    public virtual void Init(Unit value)
    {
        unit = value;
        unit.AddSkill(this);
    }

    public virtual float ProcessDamage(float value)
    {
        return value;
    }

    protected virtual void OnDestroy()
    {
        if (unit != null)
            unit.RemoveSkill(this);
    }
}
