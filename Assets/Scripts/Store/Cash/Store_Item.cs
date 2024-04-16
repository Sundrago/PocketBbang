using MyUtility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Store_Item : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title_ui, amt_ui, bonus_ui, price_ui;
    [SerializeField] private GameObject firstBuy_ui;

    [Button]
    public void Init(int amt, int price)
    {
        if (price == 0) return;
        title_ui.text = "다이아 " + amt + "개";
        amt_ui.text = amt.ToString();
        bonus_ui.text = "보너스 +" + amt;
        price_ui.text = "\u20a9" + Converter.IntToCommaSeparatedString(price);
    }

    public void SetFirstBuy(bool isFirstBuy)
    {
        firstBuy_ui.SetActive(isFirstBuy);
    }
}