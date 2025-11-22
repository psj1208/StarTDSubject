using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReinForcePanel : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<float> pricePerButton = new List<float>();

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int num = i;
            buttons[i].AddComponent<HoverUI>().Init(GameResType.Mineral, pricePerButton[num]);

            buttons[i].onClick.AddListener(() =>
            {
                if (num >= pricePerButton.Count)
                    return;
                if (GameResourceManager.Instance.HaveEnoughResource(GameResType.Mineral, pricePerButton[num]))
                {
                    GameResourceManager.Instance.SpendResource(GameResType.Mineral, pricePerButton[num]);
                    GameResourceManager.Instance.AddReinforce(num);
                    int level = GameResourceManager.Instance.GetReinForceLevel(num);
                    levelTexts[num].text = $"Lv.{level}";
                }
            });
        }
    }
}
