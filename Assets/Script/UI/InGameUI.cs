using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : BaseUI
{
    [SerializeField] private CoinUI coinUI;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private InteractUI interactUI;

    protected override void Start()
    {
        base.Start();
    }
}
