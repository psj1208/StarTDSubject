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
    private Dictionary<string,MonoBehaviour> resourceManageAbout = new Dictionary<string,MonoBehaviour>();
    private Dictionary<int,int> reinforceValue = new Dictionary<int,int>();
    public event Action<int,int> ReinforecUpAction;

    public void AddDictionary<T>(T input) where T : MonoBehaviour
    {
        resourceManageAbout.Add(typeof(T).Name, input);
    }

    public T Get<T>() where T : MonoBehaviour
    {
        string key = typeof(T).Name;
        return resourceManageAbout[key] as T;
    }

    public void AddReinforce(int key)
    {
        if (!reinforceValue.ContainsKey(key))
            reinforceValue.Add(key, 0);
        reinforceValue[key]++;
        ReinforecUpAction?.Invoke(key, reinforceValue[key]);
    }

    public int GetReinForceLevel(int level)
    {
        if (reinforceValue.ContainsKey(level))
            return reinforceValue[level];
        return 0;
    }

    #region 자원 추가, 소비 관련
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
