using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpDownAni : MonoBehaviour
{
    Image sr;
    RectTransform rect;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float duration = 1f;
    Vector3 original;

    private void Awake()
    {
        sr = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        original = rect.anchoredPosition;
        sr.enabled = false;
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            sr.enabled = true;
            rect.anchoredPosition = original;

            rect.DOAnchorPosY(original.y + distance, duration)
            .SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            sr.enabled = false;
            rect.DOKill();
        }
    }
}
