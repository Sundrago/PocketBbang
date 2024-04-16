using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Store_DiamondManager : MonoBehaviour
{
    [SerializeField] private List<Store_Item> storeItems;
    [SerializeField] private Store_DiamondDetailInfo diamondDetailInfo;
    [SerializeField] private StoreDiamondWatchAdsPanel watchAdsPanel;
    [SerializeField] private GameObject loadingBar;

    private StoreDiamondBoughtData boughtData;

    private void OnEnable()
    {
        diamondDetailInfo.gameObject.SetActive(false);
        watchAdsPanel.gameObject.SetActive(false);
        watchAdsPanel.UpateUI();
    }

    [Button]
    private void InitDiamondStore()
    {
        var datas = JsonData.Instance.StoreDiamondDatas;

        for (var i = 1; i < 5; i++) storeItems[i].Init(datas[i].amount, datas[i].price);
        LoadDiamondBoughtData();
    }

    public void BuyBtnClicked(int idx)
    {
        if (boughtData == null) LoadDiamondBoughtData();
        if (idx == 0) watchAdsPanel.OpenPanel();
        else
            diamondDetailInfo.Init(idx, JsonData.Instance.StoreDiamondDatas[idx].amount,
                JsonData.Instance.StoreDiamondDatas[idx].price, boughtData.boughtCount[idx] == 0);
    }

    public void RequestPurchase(int idx)
    {
        loadingBar.SetActive(true);
        switch (idx)
        {
            case 1:
                IAPManager.Instance.BuyProduct(IAPManager.Product.diamonds0);
                break;
            case 2:
                IAPManager.Instance.BuyProduct(IAPManager.Product.diamonds1);
                break;
            case 3:
                IAPManager.Instance.BuyProduct(IAPManager.Product.diamonds2);
                break;
            case 4:
                IAPManager.Instance.BuyProduct(IAPManager.Product.diamonds3);
                break;
            case 5:
                IAPManager.Instance.BuyProduct(IAPManager.Product.diamonds4);
                break;
        }
    }

    public void DiamondPurchased(int idx)
    {
        int[] idxs, amts;
        storeItems[idx].transform.DOPunchScale(Vector3.one * 0.2f, 1f);

        if (boughtData == null) LoadDiamondBoughtData();
        if (boughtData.boughtCount[idx] == 0)
        {
            idxs = new int[2] { 1003, 1003 };
            amts = new int[2]
                { JsonData.Instance.StoreDiamondDatas[idx].amount, JsonData.Instance.StoreDiamondDatas[idx].amount };
        }
        else
        {
            idxs = new int[1] { 1003 };
            amts = new int[1] { JsonData.Instance.StoreDiamondDatas[idx].amount };
        }

        RewardItemManager.Instance.Init(idxs, amts, "storeDiamond", "보석을 구매했다!");
        boughtData.boughtCount[idx] += 1;
        SaveDiamondBoughtData();
        loadingBar.SetActive(false);
    }

    public void LoadDiamondBoughtData()
    {
        if (PlayerPrefs.HasKey("StoreDiamondBoughtData"))
        {
            boughtData = JsonUtility.FromJson<StoreDiamondBoughtData>(PlayerPrefs.GetString("StoreDiamondBoughtData"));
        }
        else
        {
            boughtData = new StoreDiamondBoughtData();
            boughtData.boughtCount = new int[6] { 0, 0, 0, 0, 0, 0 };
            PlayerPrefs.SetString("StoreDiamondBoughtData", JsonUtility.ToJson(boughtData));
        }

        for (var i = 1; i < 6; i++) storeItems[i].SetFirstBuy(boughtData.boughtCount[i] == 0);
    }


    private void SaveDiamondBoughtData()
    {
        PlayerPrefs.SetString("StoreDiamondBoughtData", JsonUtility.ToJson(boughtData));
        PlayerPrefs.Save();
        for (var i = 1; i < 6; i++) storeItems[i].SetFirstBuy(boughtData.boughtCount[i] == 0);
    }

    private class StoreDiamondBoughtData
    {
        public int[] boughtCount = new int[6];
    }
}