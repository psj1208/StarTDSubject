using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameResourceManager : Singleton<GameResourceManager>
{
    protected override bool dontDestroy => false;

    public event Action<float> OnCoinChanged;
    [SerializeField] private float coin;

    public void AddCoin(float num)
    {
        coin += num;
        OnCoinChanged?.Invoke(coin);
    }

    public bool HaveEnoughCoin(float num)
    {
        return coin >= num ? true : false;
    }

    public void SpendCoin(float num, Action OnSpend = null)
    {
        coin = Mathf.Clamp(coin - num, 0, coin);
        OnCoinChanged?.Invoke(coin);
        OnSpend?.Invoke();
    }
}
