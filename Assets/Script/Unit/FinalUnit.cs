using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalUnit : OneTarget
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.finalUnit = this;
    }
}
