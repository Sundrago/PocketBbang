using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Manages the store fish data and update store UI.
/// </summary>
public class StoreFishManager : MonoBehaviour
{
    [Serializable]
    public class StoreFishData
    {
        public int itemCode;
        public Sprite Sprite;
        public int amount;
        public int price;
        public string name, descr;
    }
    
    [SerializeField] [TableList] private List<StoreFishData> StoreFishDatas;
    [SerializeField] private List<Store_fish_item> storeFishItems;
    [SerializeField] private Store_FishDetailPanel detailPanel;

    private void OnEnable()
    {
        detailPanel.gameObject.SetActive(false);
    }

    public void ItemClicked(int idx)
    {
        detailPanel.Init(StoreFishDatas[idx]);
    }

    public void PurchaseItem(StoreFishData data, int count)
    {
        var price = data.price * count;
        if (PlayerStatusManager.Instance.SubtractMoney(PlayerStatusManager.MoneyType.Diamond, data.price * count))
        {
            var idxs = new int[count];
            var amts = new int[count];

            for (var i = 0; i < count; i++)
            {
                idxs[i] = data.itemCode;
                amts[i] = data.amount;
            }

            RewardItemManager.Instance.Init(idxs, amts, "fishStore", "아이템을 구매했다!");
            detailPanel.CloseBtnClicked();
        }
        else
        {
            BalloonUIManager.Instance.ShowMsg("다이아몬드가 부족하다..");
        }
    }
    
#if UNITY_EDITOR
    [Button]
    private void ImportJSONData(string json)
    {
        var tmp = JsonConvert.DeserializeObject<Dictionary<int, StoreFishData>>(json);

        for (var i = 0; i < storeFishItems.Count; i++)
        {
            StoreFishDatas[i].name = tmp[i].name;
            StoreFishDatas[i].descr = tmp[i].descr;
            StoreFishDatas[i].itemCode = tmp[i].itemCode;
            StoreFishDatas[i].amount = tmp[i].amount;
            StoreFishDatas[i].price = tmp[i].price;
        }
    }

    [Button]
    private void InitItems()
    {
        for (var i = 0; i < storeFishItems.Count; i++)
        {
            var data = StoreFishDatas[i];
            storeFishItems[i].InitBtn(i, data.name, data.descr, data.price, data.Sprite);
        }
    }
#endif
}