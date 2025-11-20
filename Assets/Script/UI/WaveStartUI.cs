using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartUI : BaseUI
{
    [SerializeField] private float firstMoveSpeed = 1f;
    [SerializeField] private float secondMoveSpeed = .5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();

        float startPosX = rect.anchoredPosition.x;
        float distance = Mathf.Abs(rect.anchoredPosition.x);

        Sequence seq = DOTween.Sequence();

        seq.Append(rect.DOAnchorPosX(startPosX + distance, firstMoveSpeed))
            .AppendInterval(1.0f)
            .Append(rect.DOAnchorPosX(startPosX + distance * 2, secondMoveSpeed))
            .OnComplete(() => Destroy(gameObject));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.WaveStart();
    }
}
