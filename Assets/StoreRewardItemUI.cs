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

    [Button]
    private void InitItem(int idx, int amt)

    {
        rewardItem.sprite = ItemDataManager.Instacne.GetItemData(idx).sprite;
        amount.text = amt.ToString();
    }
}
