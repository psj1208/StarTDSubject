using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : BaseUI
{

    [SerializeField] private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private GameObject reinforceUI;
    [SerializeField] private float probeBuyPrice = 30;

    protected override void Start()
    {
        base.Start();
        reinforceUI.SetActive(false);
        buttons[1].GetComponent<Button>().onClick.AddListener(OnProbeButtonClicked);
        buttons[1].AddComponent<HoverUI>().Init(GameResType.Coin, probeBuyPrice);
        buttons[2].GetComponent<Button>().onClick.AddListener(OnReinforceButtonClicked);
    }

    private void OnProbeButtonClicked()
    {
        if (GameResourceManager.Instance.HaveEnoughResource(GameResType.Coin, probeBuyPrice))
        {
            GameResourceManager.Instance.SpendResource(GameResType.Coin, probeBuyPrice);
            GameResourceManager.Instance.Get<NexusInfo>().CreateProbe();
        }
    }

    private void OnReinforceButtonClicked()
    {
        reinforceUI.SetActive(!reinforceUI.activeSelf);
    }
}