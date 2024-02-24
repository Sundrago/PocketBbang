using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Store_PackageManager : MonoBehaviour
{
    [SerializeField] private StorePackageUI[] packageUis;
    [SerializeField] private StorePackageDetailPanel detailPanel;
    [SerializeField] private GameObject loadingBar;

    private StorePackageBoughtData boughtData = null;
    
    [Button]
    private void InitPackageUIs()
    {
        for (int i = 0; i < packageUis.Length; i++)
        {
            packageUis[i].Init(JsonData.Instance.StorePackageDatas[i]);
        }
    }

    public void OpenDetailInfo(int idx)
    {
        detailPanel.Init(idx);
    }
    
    public void RequestPurchaseItem(int idx)
    {
        BalloonControl.Instance.ShowMsg("buy " + idx);
        switch (idx)
        {
            case 0:
                if (PlayerPrefs.GetInt("product_itemBundle", 0) == 1)
                {
                    BalloonControl.Instance.ShowMsg("이미 구매했다!");
                    return;
                }
                IAPManager.Instance.BuyProduct(IAPManager.Product.itemBundle);
                loadingBar.SetActive(true);
                break;
            case 1:
                if (PlayerPrefs.GetInt("product_maxheartplus", 0) == 1)
                {
                    BalloonControl.Instance.ShowMsg("이미 구매했다!");
                    return;
                }
                IAPManager.Instance.BuyProduct(IAPManager.Product.maxheartplus);
                loadingBar.SetActive(true);
                break;
            case 2:
                IAPManager.Instance.BuyProduct(IAPManager.Product.hardworking);
                loadingBar.SetActive(true);
                break;
        }
    }

    public void ItemPurchased(int idx)
    {
        if (boughtData == null) LoadBoughData();
        boughtData.boughtCount[idx] += 1;
        boughtData.boughtDate[idx] = MyUtility.Converter.DateTimeToString(DateTime.Now);
        SaveBoughtData();
        
        switch (idx)
        {
            case 0:
                RewardItemManager.Instance.Init(JsonData.Instance.StorePackageDatas[1].itemCode, JsonData.Instance.StorePackageDatas[1].itemAmt, "Product.itemBundle", "아이템을 구매했다!");
                break;
            case 1:
                Heart_Control.Instance.InitMaxHeart();
                break;
            case 2:
                break;
        }
    }
    
    public void UpdateSoldOut()
    {
        LoadBoughData();
        
        packageUis[0].SetSoldOut(false);
        packageUis[1].SetSoldOut(false);
        
        for (int i = 2; i < packageUis.Length; i++)
        {
            
        }
    }
    
    public void LoadBoughData()
    {
        if (PlayerPrefs.HasKey("StorePackageBoughtData"))
        {
            boughtData = JsonUtility.FromJson<StorePackageBoughtData>(PlayerPrefs.GetString("StorePackageBoughtData"));
            if (boughtData.boughtCount.Count < packageUis.Length)
            {
                int count = packageUis.Length - boughtData.boughtCount.Count;

                for (int i = 0; i < count; i++)
                {
                    boughtData.boughtCount.Add(0);
                    boughtData.boughtDate.Add("");
                }
                
                PlayerPrefs.SetString("StoreDiamondBoughtData", JsonUtility.ToJson(boughtData));
            }
        }
        else
        {
            boughtData = new StorePackageBoughtData();
            boughtData.boughtCount = new List<int>();
            boughtData.boughtDate = new List<string>();

            for (int i = 0; i < packageUis.Length; i++)
            {
                boughtData.boughtCount.Add(0);
                boughtData.boughtDate.Add("");
            }
            
            PlayerPrefs.SetString("StoreDiamondBoughtData", JsonUtility.ToJson(boughtData));
        }
    }
    

    private void SaveBoughtData()
    {
        PlayerPrefs.SetString("StoreDiamondBoughtData", JsonUtility.ToJson(boughtData));
        PlayerPrefs.Save();
    }
    
    class StorePackageBoughtData
    {
        public List<int> boughtCount;
        public List<string> boughtDate;
    }
}
