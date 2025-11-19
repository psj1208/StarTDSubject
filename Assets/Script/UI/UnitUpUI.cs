using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpUI : BuyUnitUI
{
    protected override void OnButtonClicked()
    {
        BuildManager.Instance.UnitUpAction(tileObj);

        Destroy(this.gameObject);
    }
}
