using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceText : BaseUI
{
    [SerializeField] Sprite coin;
    [SerializeField] Sprite mineral;
    [SerializeField] Image sr;

    [SerializeField] TextMeshProUGUI text;

    public void Init(Vector3 pos, GameResType type, float val)
    {
        transform.position = pos;
        sr.sprite = type == GameResType.Coin ? coin : mineral;
        text.text = val.ToString();
    }
}
