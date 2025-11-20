using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.show<CoinUI>((prefab)=>
        {
            GameResourceManager.Instance.AddCoin(50);
        });
        GameManager.Instance.SetPath();
        GameManager.Instance.GameStart(0);
    }
}
