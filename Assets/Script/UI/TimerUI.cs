using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI timeText;

    protected override void Start()
    {
        base.Start();
        timeText.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.Instance.timeChangeAction += ChangeTimeText;
    }

    private void OnDisable()
    {
        GameManager.Instance.timeChangeAction -= ChangeTimeText;
    }

    private void ChangeTimeText(int sec)
    {
        timeText.enabled = true;

        timeText.text = sec.ToString();

        timeText.transform.DOKill();
        timeText.transform.localScale = Vector3.one;

        timeText.transform
            .DOScale(1.2f, 0.15f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                timeText.transform.DOScale(1f, 0.15f).SetEase(Ease.InOutQuad);
            });

        if (sec == 0)
            AfterDisable(1);
    }

    private void AfterDisable(float time)
    {
        StartCoroutine(DisableRoutine(time));
    }

    IEnumerator DisableRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        timeText.enabled = false;   
    }
}
