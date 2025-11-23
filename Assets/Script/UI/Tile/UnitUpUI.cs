using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitUpUI : BuyUnitUI
{
    public override void Init(PointerObject obj)
    {
        base.Init(obj);
        BuyPrice = Static.unit_Update_Price;
    }

    protected override void OnButtonClicked()
    {
        if (GameResourceManager.Instance.HaveEnoughResource(GameResType.Coin, buyPrice))
        {
            GameResourceManager.Instance.SpendResource(GameResType.Coin, buyPrice);
            BuildManager.Instance.UnitUpAction(tileObj);
        }

        Destroy(this.gameObject);
    }
}
