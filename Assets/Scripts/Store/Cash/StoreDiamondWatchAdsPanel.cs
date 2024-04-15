using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StoreDiamondWatchAdsPanel : MonoBehaviour
{
    private int[] rewardAmt = new[] { 2, 3, 4, 5, 5 };
    
    [SerializeField] private TextMeshProUGUI amt_ui, item_amt, item_title;
    [SerializeField] private Text descr_ui, btn_ui;
    [SerializeField] private Button whatchAdBtn, itemBtn, itemAdBtn;
    [SerializeField] private Image bg;
    [SerializeField] private Transform panel;
    
    private StoreDiamondAdsData diamondAdsData = null;
    private int amt = 0;
    
    [Button]
    public void UpateUI()
    {
        if(diamondAdsData == null) LoadData();
        int count = diamondAdsData.count;
        
        if (count >= rewardAmt.Length)
        {
            btn_ui.text = "광고보기 (5/5)";
            whatchAdBtn.interactable = false;
            itemBtn.interactable = false;
            itemAdBtn.interactable = false;
            return;
        }
        whatchAdBtn.interactable = true;
        itemBtn.interactable = true;
        itemAdBtn.interactable = true;
        amt = rewardAmt[count];

        amt_ui.text = amt + "개";
        item_title.text = "다이아 " + amt + "개";
        item_amt.text = amt.ToString();
        btn_ui.text = "광고보기 (" + (count+1) + "/5)";
        descr_ui.text = "광고를 보고 다이아몬드\n" + amt + "개를 받을까요?";
    }
    
    public void OpenPanel()
    {
        if(DOTween.IsTweening(panel)) return;

        UpateUI();
        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.localPosition = new Vector3(0, 50, 0);
        panel.transform.eulerAngles = Vector3.zero;
        
        panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.6f, 0.2f);
        
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        if(!gameObject.activeSelf) return;
        if(DOTween.IsTweening(panel)) return;

        bg.DOFade(0f, 0.3f);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
    
    public void ConfirmBtnClicked()
    {
        Ad_Control.Instance.PlayAds(Ad_Control.AdsType.diamondStore);
        // WatchedAd();
    }

    public void WatchedAd()
    {
        ClosePanel();
        RewardItemManager.Instance.Init(new int[]{1003}, new []{amt}, "DiamondAd", "광고를 보고 보상을 받았다!");
        diamondAdsData.count += 1;
        SaveData();
        UpateUI();
    }


    private void LoadData()
    {
        if (PlayerPrefs.HasKey("StoreDiamondAdsData"))
        {
            diamondAdsData = JsonUtility.FromJson<StoreDiamondAdsData>(PlayerPrefs.GetString("StoreDiamondAdsData"));
            DateTime date = MyUtility.Converter.StringToDateTime(diamondAdsData.date);
            if (date.DayOfYear != DateTime.Now.DayOfYear)
            {
                diamondAdsData.date = MyUtility.Converter.DateTimeToString(DateTime.Now);
                diamondAdsData.count = 0;
                SaveData();
            }
        }
        else
        {
            diamondAdsData = new StoreDiamondAdsData();
            diamondAdsData.date = MyUtility.Converter.DateTimeToString(DateTime.Now.AddDays(-1));
            diamondAdsData.count = 0;
            PlayerPrefs.SetString("StoreDiamondAdsData", JsonUtility.ToJson(diamondAdsData));
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("StoreDiamondAdsData", JsonUtility.ToJson(diamondAdsData));
        PlayerPrefs.Save();
    }

    private class StoreDiamondAdsData
    {
        public string date;
        public int count;
    }
}
