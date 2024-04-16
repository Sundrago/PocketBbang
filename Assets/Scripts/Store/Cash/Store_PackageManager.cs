using System;
using System.Collections.Generic;
using MyUtility;
using Sirenix.OdinInspector;
using UnityEngine;

public class Store_PackageManager : MonoBehaviour
{
    [SerializeField] private StorePackageUI[] packageUis;
    [SerializeField] private StorePackageDetailPanel detailPanel;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private HardWorkManager hardWorkManager;

    public StorePackageBoughtData boughtData;

    private void OnEnable()
    {
        detailPanel.gameObject.SetActive(false);
    }

    [Button]
    private void InitPackageUIs()
    {
        for (var i = 0; i < packageUis.Length; i++) packageUis[i].Init(JsonData.Instance.StorePackageDatas[i]);
    }

    public void OpenDetailInfo(int idx)
    {
        if (idx == 2 && hardWorkManager.IsHardworkActive())
        {
            hardWorkManager.OpenPanel();
            return;
        }

        detailPanel.Init(idx);
    }

    public void RequestPurchaseItem(int idx)
    {
        if (CheckIfSoldOut(idx))
        {
            if (idx <= 1)
                BalloonUIManager.Instance.ShowMsg("이미 구매했다!");
            else
                BalloonUIManager.Instance.ShowMsg("일주일에 한 번만 구매할 수 있어요.\n(일요일 0시에 초기화 됩니다.)");
            return;
        }

        switch (idx)
        {
            case 0:
                IAPManager.Instance.BuyProduct(IAPManager.Product.itemBundle);
                loadingBar.SetActive(true);
                break;
            case 1:
                IAPManager.Instance.BuyProduct(IAPManager.Product.maxheartplus);
                PlayerHealthManager.Instance.AddHeartByAmt(10);
                loadingBar.SetActive(true);
                break;
            case 2:
                hardWorkManager.UpdateData();
                if (hardWorkManager.IsHardworkActive()) return;

                IAPManager.Instance.BuyProduct(IAPManager.Product.hardworking);
                loadingBar.SetActive(true);
                break;
            default:
                if (PlayerHealthManager.Instance.SubtractMoney(PlayerHealthManager.MoneyType.Diamond,
                        JsonData.Instance.StorePackageDatas[idx].needAmt))
                    ItemPurchased(idx);
                else
                    BalloonUIManager.Instance.ShowMsg("다이아몬드가 부족하다..");
                break;
        }
    }

    public void ItemPurchased(int idx)
    {
        loadingBar.SetActive(false);

        if (boughtData == null) LoadBoughData();
        boughtData.boughtCount[idx] += 1;
        boughtData.boughtDate[idx] = Converter.DateTimeToString(DateTime.Now);
        SaveBoughtData();
        UpdateSoldOut();

        switch (idx)
        {
            case 1:
                PlayerHealthManager.Instance.InitMaxHeart();
                break;
            case 2:
                hardWorkManager.StartNewPlan();
                hardWorkManager.OpenPanel();
                break;
            default:
                RewardItemManager.Instance.Init(JsonData.Instance.StorePackageDatas[idx].itemCode,
                    JsonData.Instance.StorePackageDatas[idx].itemAmt, "Product.itemBundle", "아이템을 구매했다!");
                break;
        }
    }

    public void UpdateSoldOut()
    {
        LoadBoughData();
        for (var i = 0; i < packageUis.Length; i++) packageUis[i].SetSoldOut(CheckIfSoldOut(i));
    }

    private bool CheckIfSoldOut(int idx)
    {
        if (idx == 0)
            return PlayerPrefs.GetInt("product_itemBundle", 0) == 1;
        if (idx == 1)
            return PlayerPrefs.GetInt("product_maxheartplus", 0) == 1;
        if (idx == 2)
            return hardWorkManager.IsHardworkActive();

        if (boughtData == null) LoadBoughData();
        var boughtDateStr = boughtData.boughtDate[idx];
        if (string.IsNullOrEmpty(boughtDateStr)) return false;
        var boughtDate = Converter.StringToDateTime(boughtDateStr);
        return Converter.GetWeekOfYear(DateTime.Now) == Converter.GetWeekOfYear(boughtDate);
    }

    public void LoadBoughData()
    {
        if (PlayerPrefs.HasKey("StorePackageBoughtData"))
        {
            boughtData = JsonUtility.FromJson<StorePackageBoughtData>(PlayerPrefs.GetString("StorePackageBoughtData"));
            if (boughtData.boughtCount.Count < packageUis.Length)
            {
                var count = packageUis.Length - boughtData.boughtCount.Count;

                for (var i = 0; i < count; i++)
                {
                    boughtData.boughtCount.Add(0);
                    boughtData.boughtDate.Add("");
                }

                PlayerPrefs.SetString("StorePackageBoughtData", JsonUtility.ToJson(boughtData));
            }
        }
        else
        {
            boughtData = new StorePackageBoughtData();
            boughtData.boughtCount = new List<int>();
            boughtData.boughtDate = new List<string>();

            for (var i = 0; i < packageUis.Length; i++)
            {
                boughtData.boughtCount.Add(0);
                boughtData.boughtDate.Add("");
            }

            PlayerPrefs.SetString("StorePackageBoughtData", JsonUtility.ToJson(boughtData));
        }
    }

    private void SaveBoughtData()
    {
        PlayerPrefs.SetString("StorePackageBoughtData", JsonUtility.ToJson(boughtData));
        PlayerPrefs.Save();
    }

    [Serializable]
    public class StorePackageBoughtData
    {
        public List<int> boughtCount;
        public List<string> boughtDate;
    }
}