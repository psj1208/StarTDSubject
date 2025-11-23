using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/AroundDamage")]
public class AroundDamage : Exceed
{
    public override void Apply(Unit unit)
    {
        unit.AddComponent<AroundSkill>();
    }
}
