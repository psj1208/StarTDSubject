using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.show<CoinUI>((prefab)=>
        {
            GameResourceManager.Instance.AddResource(GameResType.Coin, 50);
        });
        UIManager.Instance.show<TimerUI>();
        UIManager.Instance.show<MineralUI>();
        UIManager.Instance.show<InteractUI>();
        GameManager.Instance.SetPath();
        GameManager.Instance.MakeStage(0);
        GameManager.Instance.winAction += () => UIManager.Instance.show<GameWinUI>();
        GameManager.Instance.loseAction += () => UIManager.Instance.show<GameLoseUI>();
        GameManager.Instance.GetWaitingTime();
    }

    private void OnDestroy()
    {
        GameManager.Instance.winAction -= () => UIManager.Instance.show<GameWinUI>();
        GameManager.Instance.loseAction -= () => UIManager.Instance.show<GameLoseUI>();
    }
}
