using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public enum FollowType
{
    Top,
    Bottom
}
public class FollowWithRectMask : MonoBehaviour
{
    [SerializeField] private RectTransform followRect;
    [SerializeField] private FollowType type;
    private RectTransform rect;
    private RectMask2D rectMask;
    float standard;
    Vector4 pad;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rectMask = followRect.GetComponent<RectMask2D>();
        standard = followRect.sizeDelta.y / 2;
    }

    private void Update()
    {
        pad = rectMask.padding;
        Vector2 pos = rect.anchoredPosition;

        if (type == FollowType.Top)
        {
            pos.y = standard - pad.y;
        }
        else if(type == FollowType.Bottom)
        {
            pos.y = pad.w - standard;
        }

        rect.anchoredPosition = pos;
    }
}
