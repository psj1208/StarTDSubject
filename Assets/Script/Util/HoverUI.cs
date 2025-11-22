using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameResType type;
    private float num;
    private GameObject hoverPanel;

    public void Init(GameResType type,float val)
    {
        this.type = type;
        num = val;     
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverPanel != null)
            return;

        UIManager.Instance.show<PriceText>((prefab) =>
        {
            hoverPanel = prefab.gameObject;
            prefab.Init(transform.position, type, num);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverPanel != null)
            Destroy(hoverPanel);
    }
}
