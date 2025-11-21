using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : RectEffectObject
{
    private void OnDestroy()
    {
        GameResourceManager.Instance.AddResource(GameResType.Coin, value);
    }
}
