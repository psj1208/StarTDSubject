using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RectEffectObject : MonoBehaviour
{
    [SerializeField] protected int value;

    public virtual void Init(int val)
    {
        value = val;
    }
}
