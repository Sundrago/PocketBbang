using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AssetKits.ParticleImage;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class ScrumbEventManager : SerializedMonoBehaviour
{
    public static ScrumbEventManager Instacne;
    [SerializeField] private GameObject OpenFx, foodIcon;
    [SerializeField] private RectTransform GooGooBar, Bird, FailFx;
    [SerializeField] private Image Qblock, QblockGlow, failQblock, btnImage;
    [SerializeField] private Transform failText;
    [SerializeField] private Button btn;
    [SerializeField] private Text btn_text;
    [SerializeField] private RectTransform scrumb_prefab;
    [SerializeField] private Transform bidulgi;
    [SerializeField] private List<GameObject> hideObj;
    [SerializeField] private List<bool> hideObjActive;

    // [SerializeField] private Slider guaranteed15_bar, guaranteed35_bar;
    // [SerializeField] private Text guaranteed15_txt, guaranteed35_txt;
    // [SerializeField] private Button guaranteed15_btn, guaranteed35_btn;
    // [SerializeField] private ParticleImage guaranteed15_fx, guaranteed35_fx;
    private void Awake()
    {
        Instacne = this;
        btn_text.text = "비둘기에게 밥주기";
        foodIcon.SetActive(true);
        gameObject.SetActive(false);
    }

    private List<int> idxs, amounts;

    [Button]
    public void DoScrumbEvent()
    {
        StartCoroutine(DoScrumbEventAsync());
    }

    private IEnumerator DoScrumbEventAsync()
    {
        PlayerPrefs.SetInt("ScrumbEventCount", PlayerPrefs.GetInt("ScrumbEventCount", 0) + 1);
        // AddGuaranteed();
        yield return new WaitForSeconds(0.35f);
        bidulgi.localScale = Vector3.one;
        bidulgi.DOPunchScale(Vector3.one * 0.1f, 0.5f);
        

        float scrumbRnd = Random.value;
        if (PlayerPrefs.GetInt("ScrumbEventCount", 0) < 6)
        {
            scrumbRnd = Random.Range(0.3f, 1.1f);
        } else if (PlayerPrefs.GetInt("ScrumbEventCount", 0) < 10)
        {
            scrumbRnd = Random.Range(0.2f, 1.05f);
        } else if (PlayerPrefs.GetInt("ScrumbEventCount", 0) < 20)
        {
            scrumbRnd = Random.Range(0.1f, 1.00f);
        }
        
        int count = 0;
        
        if (scrumbRnd < 0.7f)
        {
            AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.scrumb_fail);
            btn.interactable = true;
            BalloonControl.Instance.ShowMsg("아무런 일도 일어나지 않았다.");
            yield break;
        } else if (scrumbRnd < 0.96)
        {
            count = 1;
        }
        else
        {
            count = 5;
        }

        GetItems(count);

        if (idxs.Count == 0)
        {
            btn.interactable = true;
            AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.scrumb_fail);
            yield break;
        }
        
        if (idxs.Count == 1)
        {
            BalloonControl.Instance.ShowMsg("비둘기가 선물을 물고왔다!!");
            DoFailFx();
        }
        else
        {
            BalloonControl.Instance.ShowMsg("비둘기가 대박 선물을 갖고왔다!!!");
            DoOpenFx();
        }
    }

    private void GetItems(int count)
    {
        idxs = new List<int>();
        amounts = new List<int>();
        
        for (int i = 0; i < count; i++)
        {
            int rnd = Random.Range(0, 1000000);
            int probability = 0;
            foreach (var data in JsonData.Instance.ScrumbEventDatas)
            {
                probability += data.percentage;
                if (rnd < probability)
                {
                    if (ItemDataManager.Instacne.ItemDatas.ContainsKey(data.itemIdx))
                    {
                        idxs.Add(data.itemIdx);
                        amounts.Add(Random.Range(data.rewardItemAmt_min, data.rewardItemAmt_max+1));
                        break;
                    }
                }
            }
        }
    }
    //
    // [Button]
    // private void AddGuaranteed()
    // {
    //     PlayerPrefs.SetInt("ScrumbGuaranteed35", PlayerPrefs.GetInt("ScrumbGuaranteed35", 0) + 1);
    //     PlayerPrefs.SetInt("ScrumbGuaranteed15", PlayerPrefs.GetInt("ScrumbGuaranteed15", 0) + 1);
    //     UpdateGuaranteedUI();
    // }

    // [Button]
    // private void UpdateGuaranteedUI()
    // {
    //     int amt10 = PlayerPrefs.GetInt("ScrumbGuaranteed15", 0);
    //     int amt25 = PlayerPrefs.GetInt("ScrumbGuaranteed35", 0);
    //     // guaranteed15_btn.interactable = amt10 >= 15;
    //     // guaranteed35_btn.interactable = amt25 >= 35;
    //     // guaranteed15_bar.DOValue(amt10 / 15f, 0.25f).SetEase(Ease.OutBack);
    //     // guaranteed35_bar.DOValue(amt25 / 35f, 0.25f).SetEase(Ease.OutBack);
    //     // guaranteed15_txt.text = amt10 + "/15";
    //     // guaranteed35_txt.text = amt25 + "/35";
    //     //
    //     // guaranteed15_fx.rateOverTime = amt10 >= 15 ? 30 : amt10 / 15f * 10f;
    //     // guaranteed35_fx.rateOverTime = amt25 >= 35 ? 30 : amt25 / 35f * 10f;
    // }
    //
    // public void Btn10Clicked(){
    //     if(PlayerPrefs.GetInt("ScrumbGuaranteed15") < 15) return;
    //     if(btnStatus != BtnStatus.throwScrumb) return;
    //     PlayerPrefs.SetInt("ScrumbGuaranteed15", PlayerPrefs.GetInt("ScrumbGuaranteed15", 0) - 15);
    //     GetItems(1);
    //     DoFailFx();
    //     UpdateGuaranteedUI();
    // }
    //
    // public void Btn25Clicked()
    // {
    //     if(PlayerPrefs.GetInt("ScrumbGuaranteed35", 0) < 35) return;
    //     if(btnStatus != BtnStatus.throwScrumb) return;
    //     PlayerPrefs.SetInt("ScrumbGuaranteed35", PlayerPrefs.GetInt("ScrumbGuaranteed35", 0) - 35);
    //     GetItems(5);
    //     DoOpenFx();
    //     UpdateGuaranteedUI();
    // }
    //
    [Button]
    public void DoOpenFx()
    {
        AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.scrumb_big);
        
        DOTween.Kill(Bird);
        DOTween.Kill(GooGooBar);
        DOTween.Kill(QblockGlow);
        Qblock.color = Color.white;
        Qblock.gameObject.SetActive(true);
        
        Bird.anchoredPosition = new Vector2(0, -2000);
        Bird.DOAnchorPosY(700, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f)
            .OnComplete(() =>
            {
                foodIcon.SetActive(false);
                btn_text.text = "비둘기 대박 선물 확인하기";
                btnImage.color = Color.yellow;;
                btnStatus = BtnStatus.getReward;
                btn.interactable = true;
            });
        
        GooGooBar.anchoredPosition = new Vector2(0, -2000);
        GooGooBar.DOAnchorPosY(0, 1f).SetEase(Ease.OutElastic);
        
        OpenFx.gameObject.SetActive(true);
        
        QblockGlow.color = Color.white;
        QblockGlow.DOFade(0, 0.5f).SetDelay(0.5f);
    }

    [Button]
    public void OpenFxItemBtnClicked()
    {
        RewardItemManager.Instance.Init(MyUtility.Converter.List2Array(idxs), MyUtility.Converter.List2Array(amounts), "scrumb", "비둘기에게 대박보상을 받았다!");

        Qblock.DOFade(0, 0.45f);
        Qblock.transform.DOPunchScale(Vector3.one * 1.2f, 0.5f, 5)
            .OnComplete(() =>
            {
                btnImage.color = Color.white;
                foodIcon.SetActive(true);
                btn_text.text = "비둘기에게 밥주기";
                Qblock.gameObject.SetActive(false);
                HideOpenFx();
            });
    }

    [Button]
    public void HideOpenFx()
    {
        if(!OpenFx.gameObject.activeInHierarchy) return;
        
        DOTween.Kill(Bird);
        DOTween.Kill(GooGooBar);
        
        Bird.DOAnchorPosY(2700, 0.2f).SetEase(Ease.InExpo).SetDelay(0.1f);;
        GooGooBar.DOAnchorPosY(2000, 0.35f).SetEase(Ease.InExpo)
            .OnComplete(()=>
            {
                btnStatus = BtnStatus.throwScrumb;
                btn.interactable = true;
                OpenFx.SetActive(false);
            });
    }

    [Button]
    public void DoFailFx()
    {
        AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.scrumb_small);
        DOTween.Kill(FailFx.transform);
        DOTween.Kill(failText);

        failQblock.gameObject.SetActive(true
            );
        FailFx.anchoredPosition = new Vector2(0, -1000);
        FailFx.DOAnchorPos(new Vector2(125, 830), 0.35f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            foodIcon.SetActive(false);
            btn_text.text = "비둘기 선물 확인하기";
            btnImage.color = Color.yellow;;
            btnStatus = BtnStatus.getFailReward;
            btn.interactable = true;
        });
        failText.localScale = Vector3.one;
        failText.DOScale(Vector3.one * 1.1f, 0.7f).SetEase(Ease.OutElastic).SetDelay(0.3f);
        
        FailFx.gameObject.SetActive(true);
    }

    [Button]
    public void GetFailReward()
    {
        RewardItemManager.Instance.Init(MyUtility.Converter.List2Array(idxs), MyUtility.Converter.List2Array(amounts), "scrumb","비둘기에게 보상을 받았다");
        failQblock.gameObject.SetActive(false);
        HideFailFx();
        btnImage.color = Color.white;
        foodIcon.SetActive(true);
        btn_text.text = "비둘기에게 밥주기";
    }
    
    [Button]
    public void HideFailFx()
    {
        if(!FailFx.gameObject.activeInHierarchy) return;

        DOTween.Kill(FailFx.transform);
        FailFx.DOAnchorPos(new Vector2(300, -1000), 0.3f).SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                btnStatus = BtnStatus.throwScrumb;
                btn.interactable = true;
                FailFx.gameObject.SetActive(false);
            });
    }
    
    enum BtnStatus
    {
        throwScrumb, getReward, getFailReward
    }

    private BtnStatus btnStatus;
    public void OnBtnClicked()
    {
        if (btnStatus == BtnStatus.throwScrumb)
        {
            if (Heart_Control.Instance.SubtractMoney(Heart_Control.MoneyType.Scrumb, 1))
            {
                // DoFailFx();
                // DoOpenFx();
                btn.interactable = false;
                DOTween.Kill(btn.gameObject.transform);
                btn.gameObject.transform.localScale = Vector3.one;
                btn.gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
                
                RectTransform scrumb = Instantiate(scrumb_prefab, bidulgi);
                Vector2 targetPos = new Vector2(Random.Range(-150, 150f), Random.Range(-100, 100));
                scrumb.DOAnchorPos(new Vector2(targetPos.x / 2f, 300), 0.3f).SetEase(Ease.OutExpo);
                scrumb.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.3f);
                scrumb.DORotate(new Vector3(0, 0, Random.Range(0, 360)), 0.75f);
                scrumb.DOScale(Vector3.zero, 1.5f).OnComplete(() =>
                {
                    Destroy(scrumb.gameObject);
                });
                scrumb.gameObject.SetActive(true);
                DoScrumbEvent();
            }
            else
            {
                BalloonControl.Instance.ShowMsg("빵조각이 없다..");
            }
        } else if (btnStatus == BtnStatus.getFailReward)
        {
            GetFailReward();
            btn.interactable = false;
        } else if (btnStatus == BtnStatus.getReward)
        {
            OpenFxItemBtnClicked();
            btn.interactable = false;
        }
    }

    public void QbtnClicked()
    {
        if (btn.interactable == true) OnBtnClicked();
    }

    public void OpenPanel()
    {
        if(DOTween.IsTweening(gameObject.transform)) return;
        if(gameObject.activeSelf) return;

        // UpdateGuaranteedUI();

        hideObjActive = new List<bool>();
        foreach (var obj in hideObj)
        {
            hideObjActive.Add(obj.activeSelf);
            obj.SetActive(false);
        }
        gameObject.transform.localPosition = new Vector3(0, -3000, 0);
        gameObject.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo);
        gameObject.SetActive(true);
        
        AudioControl.Instance.PlayMusic(10);
    }

    public void ClosePanel()
    {
        if(DOTween.IsTweening(gameObject.transform)) return;
        if(!gameObject.activeSelf) return;
        
        foreach (var obj in hideObj)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i < hideObj.Count; i++)
        {
            hideObj[i].SetActive(hideObjActive[i]);
        }
        
        gameObject.transform.DOLocalMoveY(3000, 0.25f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        AudioControl.Instance.PlayMusic(5);
    }
}
