using System;
using DG.Tweening;
using MyUtility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FishManager : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject fishGroup, timerGroup, emptyItem;
    [SerializeField] private GameObject[] fishGroupItem, fishBtns;
    [SerializeField] private Text timerTxt;
    
    private DateTime goldFishEndTime;
    private bool isGoldFishActive;

    private void Awake()
    {
        var goldFishEndTimeString = PlayerPrefs.GetString("goldFishEndTime",
            Converter.DateTimeToString(DateTime.Now.AddDays(-1)));
        goldFishEndTime = Converter.StringToDateTime(goldFishEndTimeString);

        UpdateGoldFishActive();
        CLosePanel();
    }

    private void Update()
    {
        if (Time.frameCount % 30 != 0) return;
        if (!isGoldFishActive) return;

        var span = goldFishEndTime - DateTime.Now;
        if (span.TotalSeconds <= 0)
        {
            UpdateGoldFishActive();
            PlayerHealthManager.Instance.UpdateUI();
            return;
        }

        timerTxt.text = span.ToString(@"mm\:ss");
    }

    private void SetDisplayGroup()
    {
        fishGroup.SetActive(!isGoldFishActive);
        timerGroup.SetActive(isGoldFishActive);
    }

    [Button]
    public void OpenPanel()
    {
        if (panel.gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;
        panel.gameObject.SetActive(true);
        panel.DOAnchorPosY(-250, 0.4f).SetEase(Ease.OutBack);
        SetDisplayGroup();
    }

    [Button]
    public void CLosePanel()
    {
        if (!panel.gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;
        panel.DOAnchorPosY(250, 0.4f).SetEase(Ease.InOutExpo).OnComplete(() => { panel.gameObject.SetActive(false); });
    }

    [Button]
    public void UpdateFishStatus()
    {
        var fish0Count = PlayerPrefs.GetInt("fish0Count", 0);
        var fish1Count = PlayerPrefs.GetInt("fish1Count", 0);
        var fish2Count = PlayerPrefs.GetInt("fish2Count", 0);

        gameObject.SetActive(fish0Count + fish1Count + fish2Count != 0 || isGoldFishActive ||
                             PlayerPrefs.GetInt("totalFishCount", 0) > 0);

        SetActive(0, fish0Count != 0);
        SetActive(1, fish1Count != 0);
        SetActive(2, fish2Count != 0);

        void SetActive(int idx, bool isActive)
        {
            fishGroupItem[idx].SetActive(isActive);
            fishBtns[idx].SetActive(isActive);
        }

        emptyItem.SetActive(fish0Count + fish1Count + fish2Count == 0);
    }

    public void FishToggleClicked()
    {
        if (emptyItem.activeSelf) StoreManager.Instance.OpenStoreAt("fish");
        if (panel.gameObject.activeSelf) CLosePanel();
        else OpenPanel();
    }

    public void FishBtnClicked(int idx)
    {
        switch (idx)
        {
            case 0:
                if (PlayerHealthManager.Instance.GetAmount(PlayerHealthManager.MoneyType.Fish0) <= 0)
                {
                    BalloonUIManager.Instance.ShowMsg("황금 잉어빵이 없다.. 먹고싶다..");
                    break;
                }

                PopupTextManager.Instance.ShowYesNoPopup("황금 잉어빵을 먹을까요?\n(10분 동안 체력 무한 효과)", () =>
                {
                    if (isGoldFishActive)
                    {
                        PopupTextManager.Instance.ShowOKPopup("황금 잉어빵 효과가 아직 남아있다!");
                    }
                    else
                    {
                        PlayerHealthManager.Instance.SubtractMoney(PlayerHealthManager.MoneyType.Fish0, 1);
                        PlayerHealthManager.Instance.AddHeartByAmt(100);
                        SetFishTimer();
                        UpdateGoldFishActive();
                    }
                });
                break;
            case 1:
                if (PlayerHealthManager.Instance.GetAmount(PlayerHealthManager.MoneyType.Fish1) <= 0)
                {
                    BalloonUIManager.Instance.ShowMsg("슈크림 잉어빵이 없다.. 상점에 가볼까..?");
                    break;
                }

                PopupTextManager.Instance.ShowYesNoPopup("슈크림 잉어빵을 먹을까요?\n(하트를 가득 회복합니다)", () =>
                {
                    if (PlayerHealthManager.Instance.IsHeartFull())
                    {
                        BalloonUIManager.Instance.ShowMsg("이미 힘이 넘쳐난다!");
                    }
                    else
                    {
                        PlayerHealthManager.Instance.SubtractMoney(PlayerHealthManager.MoneyType.Fish1, 1);
                        PlayerHealthManager.Instance.AddHeartByAmt(100);
                    }
                });
                break;
            case 2:
                if (PlayerHealthManager.Instance.GetAmount(PlayerHealthManager.MoneyType.Fish2) <= 0)
                {
                    BalloonUIManager.Instance.ShowMsg("팥 잉어빵이 없다.. 비둘기가 줄 수도 있을까..?");
                    break;
                }

                PopupTextManager.Instance.ShowYesNoPopup("팥 잉어빵을 먹을까요?\n(하트를 두 칸 회복합니다)", () =>
                {
                    if (PlayerHealthManager.Instance.IsHeartFull())
                    {
                        BalloonUIManager.Instance.ShowMsg("이미 힘이 넘쳐난다!");
                    }
                    else
                    {
                        PlayerHealthManager.Instance.SubtractMoney(PlayerHealthManager.MoneyType.Fish2, 1);
                        PlayerHealthManager.Instance.AddHeartByAmt(2);
                    }
                });
                break;
        }

        fishBtns[idx].transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
    }

    [Button]
    private void UpdateGoldFishActive()
    {
        isGoldFishActive = (goldFishEndTime - DateTime.Now).TotalSeconds > 0;
        PlayerHealthManager.Instance.SetFishAnim(isGoldFishActive);
        SetDisplayGroup();
    }

    private void SetFishTimer()
    {
        goldFishEndTime = DateTime.Now.AddMinutes(10);
        PlayerPrefs.SetString("goldFishEndTime", Converter.DateTimeToString(goldFishEndTime));
        PlayerPrefs.Save();
        UpdateGoldFishActive();
        CLosePanel();
    }
}