using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Exceed : ScriptableObject
{
    public string effectName;
    public string description;

    public abstract void Apply(Unit unit);
}
