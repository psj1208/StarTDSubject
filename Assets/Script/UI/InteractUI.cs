using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : BaseUI
{

    [SerializeField] private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private GameObject reinforceUI;
    [SerializeField] private float probeBuyPrice = 30;
    [SerializeField] Image prizeSr;
    [SerializeField] private float prizeCoolTime = 60f;
    [SerializeField] UpDownAni upAni;
    float curTime = 0;

    protected override void Start()
    {
        base.Start();
        reinforceUI.SetActive(false);
        buttons[0].GetComponent<Button>().onClick.AddListener(OnPrizeButtonClicekd);
        buttons[1].GetComponent<Button>().onClick.AddListener(OnProbeButtonClicked);
        buttons[1].AddComponent<HoverUI>().Init(GameResType.Coin, probeBuyPrice);
        buttons[2].GetComponent<Button>().onClick.AddListener(OnReinforceButtonClicked);
        curTime = 0;
        prizeSr.fillAmount = 0;
        upAni.SetActive(true);
    }

    IEnumerator PrizeCoolTime()
    {
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            prizeSr.fillAmount = Mathf.Clamp01(curTime / prizeCoolTime);
            yield return null;
        }
        curTime = 0;
        upAni.SetActive(true);
    }

    private void OnPrizeButtonClicekd()
    {
        if (curTime <= 0)
        {
            Debug.Log("´­¸²!");
            upAni.SetActive(false);
            curTime = prizeCoolTime;
            GameManager.Instance.SpawnPrizeMonster();
            StartCoroutine(PrizeCoolTime());
        }
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