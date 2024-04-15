using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class Store_FishManager : MonoBehaviour
{
    [SerializeField, TableList] private List<StoreFishData> StoreFishDatas;
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
        int price = data.price * count;
        if (Heart_Control.Instance.SubtractMoney(Heart_Control.MoneyType.Diamond, data.price*count))
        {
            int[] idxs = new int[count];
            int[] amts = new int[count];

            for (int i = 0; i < count; i++)
            {
                idxs[i] = data.itemCode;
                amts[i] = data.amount;
            }
            RewardItemManager.Instance.Init(idxs, amts, "fishStore", "아이템을 구매했다!");
            detailPanel.CloseBtnClicked();
        }
        else
        {
            BalloonControl.Instance.ShowMsg("다이아몬드가 부족하다..");
        }
    }

    #if UNITY_EDITOR
    [Button]
    private void ImportJSONData(string json)
    {
        Dictionary<int, StoreFishData> tmp = JsonConvert.DeserializeObject<Dictionary<int, StoreFishData>>(json);

        for (int i = 0; i < storeFishItems.Count; i++)
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
        for (int i = 0; i < storeFishItems.Count; i++)
        {
            StoreFishData data = StoreFishDatas[i];
            storeFishItems[i].InitBtn(i, data.name, data.descr, data.price, data.Sprite);
        }
    }
    #endif

    [Serializable] 
    public class StoreFishData
    {
        public int itemCode;
        public Sprite Sprite;
        public int amount;
        public int price;
        public string name, descr;
    }
}
