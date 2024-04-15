using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreRewardItemUI : MonoBehaviour
{
    [SerializeField] private Image rewardItem;
    [SerializeField] private TextMeshProUGUI amount;

    private int itemCode;
    
    [Button]
    public void InitItem(int idx, int amt)
    {
        itemCode = idx;
        if (idx == -1)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        rewardItem.sprite = ItemDataManager.Instacne.GetItemData(idx).sprite;
        amount.text = amt + "개";
    }

    public void BtnClicked()
    {
        ItemInfoUI.Instance.OpenPanel(itemCode);
    }
}
