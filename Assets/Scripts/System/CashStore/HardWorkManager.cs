using System;
using DG.Tweening;
using MyUtility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HardWorkManager : MonoBehaviour
{
    public enum HardworkingStatus
    {
        Received,
        Ready,
        NotReceived
    }

    private const int RewardAmt = 80;

    [Header("Managers and Controllers")] 
    [SerializeField] private HardWorkingData hardWorkingData;

    [SerializeField] private Transform panel;
    [SerializeField] private HardworkingItemUI[] itemUis;
    [SerializeField] private TextMeshProUGUI btnText, doolgiText, descrText;
    [SerializeField] private Image btnImage, bg;
    [SerializeField] private RectTransform doolgiImg, doolgiBalloon;
    [SerializeField] private GameObject gotoStoreBtn;

    public bool hasReward;
    public int diff;
    private DateTime startDate;


    [Button]
    public void UpdateData(int addDays = 0)
    {
        LoadData();
        if (!hardWorkingData.isActive) return;

        startDate = Converter.StringToDateTime(hardWorkingData.startDate);
        diff = (DateTime.Now.AddDays(addDays).Date - startDate).Days;

        if (diff <= 6 && diff >= 0)
            if (hardWorkingData.statuses[diff + 1] == HardworkingStatus.NotReceived)
            {
                hardWorkingData.statuses[diff + 1] = HardworkingStatus.Ready;
                SaveData();
                OpenPanel();
            }

        hasReward = false;
        for (var i = 0; i < itemUis.Length; i++)
        {
            itemUis[i].Init(i);
            itemUis[i].SetUIStatus(hardWorkingData.statuses[i]);
            if (hardWorkingData.statuses[i] == HardworkingStatus.Ready) hasReward = true;
        }

        if (diff >= 6 && hasReward == false)
        {
            hardWorkingData.isActive = false;
            SaveData();
            OpenPanel();
        }
    }

    public bool IsHardworkActive()
    {
        UpdateData();
        return hardWorkingData.isActive;
    }

    private void UpdateUI()
    {
        LoadData();

        hasReward = false;
        for (var i = 0; i < itemUis.Length; i++)
        {
            itemUis[i].Init(i);
            itemUis[i].SetUIStatus(hardWorkingData.statuses[i]);
            if (hardWorkingData.statuses[i] == HardworkingStatus.Ready) hasReward = true;
        }

        startDate = Converter.StringToDateTime(hardWorkingData.startDate);

        if (hardWorkingData.isActive)
            descrText.text = "보상은 매일 밤 0시에 업데이트됩니다!\n성실함의 보상 시작일 : " + startDate.Date.ToString("yyyy-M-d");
        else descrText.text = "성실함의 보상이 만료되었어요!\n성실함의 보상 시작일 : " + startDate.Date.ToString("yyyy-M-d");

        if (hasReward)
        {
            btnImage.color = new Color(1, 0.8f, 0.4f);
            btnText.text = "보상받기";
        }
        else
        {
            btnImage.color = new Color(1, 0.8f, 0.7f);
            btnText.text = "닫기";
        }

        gotoStoreBtn.SetActive(!hardWorkingData.isActive);
    }

    public void GetRewardBtnClicked()
    {
        if (!hasReward)
        {
            ClosePanel();
            return;
        }

        var readyStatusesCount = CountReadyStatuses();

        var idxs = InitializeArrayWithValue(readyStatusesCount, 1003);
        var amts = InitializeArrayWithValue(readyStatusesCount, RewardAmt);

        RewardItemManager.Instance.Init(idxs, amts, "hardworking", "성실함의 보상을 받았다!");
        SetAllReadyStatusesToReceived();

        SaveData();
        UpdateData();
        UpdateUI();
    }

    private int CountReadyStatuses()
    {
        var counter = 0;
        foreach (var status in hardWorkingData.statuses)
            if (status == HardworkingStatus.Ready)
                ++counter;

        return counter;
    }

    private int[] InitializeArrayWithValue(int length, int value)
    {
        var array = new int[length];
        for (var i = 0; i < length; i++) array[i] = value;

        return array;
    }

    private void SetAllReadyStatusesToReceived()
    {
        for (var i = 0; i < hardWorkingData.statuses.Length; i++)
            if (hardWorkingData.statuses[i] == HardworkingStatus.Ready)
                hardWorkingData.statuses[i] = HardworkingStatus.Received;
    }

    [Button]
    public void StartNewPlan()
    {
        hardWorkingData = new HardWorkingData();
        hardWorkingData.isActive = true;
        hardWorkingData.startDate = Converter.DateTimeToString(DateTime.Now.Date);
        hardWorkingData.statuses = new HardworkingStatus[8]
        {
            HardworkingStatus.Ready,
            HardworkingStatus.Ready,
            HardworkingStatus.NotReceived,
            HardworkingStatus.NotReceived,
            HardworkingStatus.NotReceived,
            HardworkingStatus.NotReceived,
            HardworkingStatus.NotReceived,
            HardworkingStatus.NotReceived
        };
        SaveData();
        UpdateUI();
    }

    [Button]
    private void SaveData()
    {
        if (hardWorkingData == null) LoadData();
        PlayerPrefs.SetString("HardWorkingData", JsonUtility.ToJson(hardWorkingData));
        PlayerPrefs.Save();
    }

    [Button]
    private void LoadData()
    {
        if (PlayerPrefs.HasKey("HardWorkingData"))
        {
            hardWorkingData = JsonUtility.FromJson<HardWorkingData>(PlayerPrefs.GetString("HardWorkingData"));
        }
        else
        {
            hardWorkingData = new HardWorkingData();
            hardWorkingData.isActive = false;
            hardWorkingData.startDate = Converter.DateTimeToString(DateTime.MinValue);
            hardWorkingData.statuses = new HardworkingStatus[8]
            {
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived,
                HardworkingStatus.NotReceived
            };
            SaveData();
        }

        if (hardWorkingData.isActive) startDate = Converter.StringToDateTime(hardWorkingData.startDate);
    }

    [Button]
    public void OpenPanel()
    {
        if (DOTween.IsTweening(panel)) return;

        UpdateData();

        if (!hardWorkingData.isActive && diff >= 7)
        {
            StoreManager.Instance.OpenStoreAt("hardWork");
            ClosePanel();
            return;
        }

        UpdateUI();
        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.localPosition = new Vector3(0, 50, 0);
        panel.transform.eulerAngles = Vector3.zero;

        panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.8f, 0.2f);

        ShowDoolgi();
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        if (!gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;

        if (hasReward) GetRewardBtnClicked();

        bg.DOFade(0f, 0.3f);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic)
            .OnComplete(() => { gameObject.SetActive(false); });
    }

    [Button]
    private void ShowDoolgi()
    {
        if (doolgiImg.gameObject.activeSelf) return;

        DOTween.Kill(doolgiImg);
        doolgiImg.gameObject.SetActive(true);
        doolgiImg.anchoredPosition = new Vector2(-250, -200);
        doolgiImg.DOAnchorPosY(200, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            doolgiBalloon.DOPunchScale(Vector3.one * 0.1f, 1f);
        });
        doolgiImg.DOScale(Vector3.one, 2.5f).OnComplete(() => { HideDoolgi(); });

        if (diff == 0)
        {
            doolgiText.text = "가보자구구!";
            return;
        }

        var rnd = Random.Range(0, 4);
        switch (rnd)
        {
            case 0:
                doolgiText.text = "대박이라구구!";
                break;
            case 1:
                doolgiText.text = "대단하구구!";
                break;
            case 2:
                doolgiText.text = "멋지다구구!";
                break;
            case 3:
                doolgiText.text = "최고다구구!";
                break;
        }
    }

    [Button]
    private void HideDoolgi()
    {
        doolgiImg.DOAnchorPosY(-200, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            doolgiImg.gameObject.SetActive(false);
        });
    }

    [Serializable]
    private class HardWorkingData
    {
        public bool isActive;
        public string startDate;
        public HardworkingStatus[] statuses;
    }
}