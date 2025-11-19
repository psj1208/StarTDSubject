using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyUnitUI : BaseUI
{
    [SerializeField] protected TextMeshProUGUI priceText;
    protected float buyPrice;
    protected PointerObject tileObj;

    [SerializeField] Button button;

    public virtual void Init(PointerObject obj, float price = 50)
    {
        transform.position = obj.transform.position;
        tileObj = obj;
        buyPrice = price;
        priceText.text = buyPrice.ToString();
    }

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void Update()
    {
        CheckOuterClickAndDestroy(button.gameObject);    
    }

    protected virtual void OnButtonClicked()
    {
        //여기에는 재화 까는 메서드를 넣으면 될 듯.

        Debug.Log("BuyButton 클릭");
        BuildManager.Instance.TryFirstBuild(tileObj);

        Destroy(gameObject);
    }
}
