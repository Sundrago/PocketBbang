using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hideObj;
    [SerializeField] private List<bool> hideObjActive;
    
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private GameObject soldOut_product_maxheartplus, soldOut_product_itemBundle;
    [SerializeField] private List<Transform> items;
    [SerializeField] private Store_TabUI storeTabUI;
    [SerializeField] private HardWorkManager hardWorkManager;
    
    public Store_DiamondManager DiamondManager;
    public Store_PackageManager PackageManager;
    public Store_FishManager FishManager;
    
    public static StoreManager Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void OpenStoreAt(string id)
    {
        switch (id)
        {
            case "fish":
                OpenStore(1);
                break;
            case "diamond":
                OpenStore(2);
                break;
            case "scrumb":
                if(Random.value < 0.5f) BalloonControl.Instance.ShowMsg("비둘기한테 주면 좋아할 것 같다..");
                else BalloonControl.Instance.ShowMsg("비둘기 공원에 가볼까..?");
                // OpenStore(1);
                break;
            case "money":
                break;
            case "maxHeart":
                OpenStore(0);
                PackageManager.OpenDetailInfo(1);
                break;
            case "hardWork":
                hardWorkManager.ClosePanel();
                OpenStore(0);
                PackageManager.OpenDetailInfo(2);
                break;
        }
        //BalloonControl.Instance.ShowMsg("Opne store at " + id);
    }
    
    public void OpenStore(int idx = 0)
    {
        hideObjActive = new List<bool>();
        foreach (var obj in hideObj)
        {
            hideObjActive.Add(obj.activeSelf);
            obj.SetActive(false);
        }
        storeTabUI.BtnClicked(idx);
        gameObject.SetActive(true);
        DiamondManager.LoadDiamondBoughtData();
        PackageManager.UpdateSoldOut();
    }

    public void CloseStore()
    {
        for (int i = 0; i < hideObj.Count; i++)
        {
            hideObj[i].SetActive(hideObjActive[i]);
        }
        gameObject.SetActive(false);
    }

    public void RequestRestorePurchase()
    {
        loadingBar.SetActive(true);
        IAPManager.Instance.RestorePurchases();
    }

    public void RestorePurchaseRetrieved()
    {
        loadingBar.SetActive(false);
        PackageManager.UpdateSoldOut();
    }


    // public void BuyProduct(string id)
    // {
    //     switch (id)
    //     {
    //         case "itemBundle":
    //             if (PlayerPrefs.GetInt("product_itemBundle", 0) == 1)
    //             {
    //                 BalloonControl.Instance.ShowMsg("이미 구매했다!");
    //                 return;
    //             }
    //             IAPManager.Instance.BuyProduct(IAPManager.Product.itemBundle);
    //             loadingBar.SetActive(true);
    //             break;
    //         case "maxheartplus":
    //             if (PlayerPrefs.GetInt("product_maxheartplus", 0) == 1)
    //             {
    //                 BalloonControl.Instance.ShowMsg("이미 구매했다!");
    //                 return;
    //             }
    //             IAPManager.Instance.BuyProduct(IAPManager.Product.maxheartplus);
    //             loadingBar.SetActive(true);
    //             break;
    //     }
    // }

    public void HideLoadingBar()
    {
        loadingBar.SetActive(false);
    }
}
