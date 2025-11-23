using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceedUnitUI : BuyUnitUI
{
    public override void Init(PointerObject obj)
    {
        base.Init(obj);
        BuyPrice = Static.exceed_Price;
    }

    protected override void OnButtonClicked()
    {
        if (GameResourceManager.Instance.HaveEnoughResource(GameResType.Coin, buyPrice))
        {
            GameResourceManager.Instance.SpendResource(GameResType.Coin, buyPrice);
            BuildManager.Instance.ExceedUnit(tileObj);
        }

        Destroy(this.gameObject);
    }
}
