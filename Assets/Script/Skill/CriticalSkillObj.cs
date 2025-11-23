using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/CriticalSkill")]
public class CriticalSkillObj : Exceed
{
    public override void Apply(Unit unit)
    {
        unit.AddComponent<CriticalSkill>().Init(unit);
    }

}
