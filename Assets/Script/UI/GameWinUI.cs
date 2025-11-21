using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : BaseUI
{
    [SerializeField] private float WaitTime = .5f;
    [SerializeField] private RectTransform panelRect;
    private RectMask2D rectMask;
    [SerializeField] private float duration = 1f;
    Vector4 pad;

    protected override void Start()
    {
        base.Start();
        rectMask = panelRect.GetComponent<RectMask2D>();
        pad = rectMask.padding;
        float val = panelRect.sizeDelta.y / 2;
        pad = new Vector4(0, val, 0, val);

        PlayMaskAnimation();
    }

    private void PlayMaskAnimation()
    {
        Vector4 pad = rectMask.padding;

        float targetTop = 0f;
        float targetBottom = 0f;

        DOTween.To(() => pad, x =>
        {
            pad = x;
            rectMask.padding = pad;
        },
        new Vector4(pad.x, targetTop, pad.z, targetBottom),
        duration)
        .SetDelay(WaitTime)
        .SetEase(Ease.OutCubic);
    }
}
