using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum GameResType
{
    Coin,
    Mineral
}
public class GameResourceManager : Singleton<GameResourceManager>
{
    protected override bool dontDestroy => false;

    public event Action<float> OnCoinChanged;
    public event Action<float> OnCoinChangedWithValue;
    public event Action<float> OnMineralChanged;
    public event Action<float> OnMineralChangedWithValue;
    [SerializeField] private float coin;
    [SerializeField] private float mineral;

    #region 코인 관련
    public void AddResource(GameResType type, float num)
    {
        if (type == GameResType.Coin)
        {
            coin += num;
            OnCoinChangedWithValue?.Invoke(num);
            OnCoinChanged?.Invoke(coin);
        }
        else if (type == GameResType.Mineral)
        {
            mineral += num;
            OnMineralChangedWithValue?.Invoke(num);
            OnMineralChanged?.Invoke(mineral);
        }
    }

    public bool HaveEnoughResource(GameResType type, float num)
    {
        if (type == GameResType.Coin)
            return coin >= num ? true : false;
        else if (type == GameResType.Mineral)
            return mineral >= num ? true : false;
        return false;
    }

    public void SpendResource(GameResType type, float num, Action OnSpend = null)
    {
        if (type == GameResType.Coin)
        {
            coin = Mathf.Clamp(coin - num, 0, coin);
            OnCoinChangedWithValue?.Invoke(-num);
            OnCoinChanged?.Invoke(coin);
        }
        else if (type == GameResType.Mineral)
        {
            mineral = Mathf.Clamp(mineral - num, 0, mineral);
            OnMineralChangedWithValue?.Invoke(-num);
            OnMineralChanged?.Invoke(mineral);
        }
        OnSpend?.Invoke();
    }
    #endregion
}
