using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTileUI : BuyUnitUI
{
    public override void Init(PointerObject obj)
    {
        base.Init(obj);
        BuyPrice = Static.Replace_Tile_Price;
    }
    protected override void OnButtonClicked()
    {
        if (GameResourceManager.Instance.HaveEnoughResource(GameResType.Coin, buyPrice))
        {
            GameResourceManager.Instance.SpendResource(GameResType.Coin, buyPrice);
            BuildManager.Instance.ReplaceTile(tileObj.transform.position);
        }

        Destroy(gameObject);
    }
}
