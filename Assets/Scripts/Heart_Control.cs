using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Heart_Control : MonoBehaviour
{
    [SerializeField] NotificationCtrl notice;
    [SerializeField] BalloonControl balloon;
    [SerializeField] Animator bus;
    [SerializeField] private FishManager fishManager;
    
    [SerializeField] HeartIcon[] heartImg = new HeartIcon[8];
    [SerializeField] GameObject timerHolder;
    [SerializeField] GameObject btn1, btn2, bbangEatCount_ui, chocoCount_ui;
    [SerializeField] GameObject moneyAmount_ui;
    [SerializeField] Text timer;
    [SerializeField] Text busText;
    
    [SerializeField] private Text coinCount_ui, diamondCount_ui, scrumbCount_ui, fish0Count_ui, fish1Count_ui, fish2Count_ui;
    [SerializeField] private GameObject scrumbObj, diamondObj, fishObj;
    
    const string format = "yyyy/MM/dd HH:mm:ss";

    System.DateTime oldTime, arriveTime;
    System.TimeSpan timeGap, leftTime, remainTime;
    System.IFormatProvider provider;

    private int heartCount = 0;
    public int currentBbangCount;
    public bool received, went;

    int bbang_choco, bbang_mat, bbang_strawberry, bbang_hot;

    private int coinCount, scrumbCount, diamondCount, fish0Count, fish1Count, fish2Count;
    public static Heart_Control Instance;
    private int maxHeartCount = 6;

    private bool isGoldFishActive;
    
    private void Awake()
    {
        Instance = this;
    }

    public void InitMaxHeart()
    {
        if (PlayerPrefs.GetInt("product_maxheartplus", 0) == 1)
        {
            maxHeartCount = 8;
        }
        else maxHeartCount = 6;

        for (int i = 0; i < heartImg.Length; i++)
        {
            if(i<maxHeartCount) heartImg[i].SetEnabled(true);
            else heartImg[i].SetEnabled(false);
        }
    }
    void Start()
    {
        InitMaxHeart();

#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("Money", "totaBalance", GetBalance());
#endif
        
        if (!PlayerPrefs.HasKey("heartCount"))
        {
            PlayerPrefs.SetInt("heartCount", 5);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("oldTime"))
        {
            PlayerPrefs.SetString("oldTime", System.DateTime.Now.ToString(format));
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("received"))
        {
            PlayerPrefs.SetString("oldTime", System.DateTime.Now.ToString(format));
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("went"))
        {
            PlayerPrefs.SetInt("went", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("received"))
        {
            PlayerPrefs.SetInt("received", 1);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("arriveTime"))
        {
            PlayerPrefs.SetString("arriveTime", System.DateTime.Now.ToString(format));
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetInt("money", 10000);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("totalMoney"))
        {
            PlayerPrefs.SetInt("totalMoney", 10000);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("storeDate"))
        {
            PlayerPrefs.SetString("storeDate", System.DateTime.Now.Date.ToString());
            PlayerPrefs.SetInt("storeDateCount", 0);
            PlayerPrefs.Save();
        }

        heartCount = PlayerPrefs.GetInt("heartCount");
        UpdateHeartUI(heartCount);

        oldTime = System.DateTime.ParseExact(PlayerPrefs.GetString("oldTime"), format, provider);
        arriveTime = System.DateTime.ParseExact(PlayerPrefs.GetString("arriveTime"), format, provider);

        if (PlayerPrefs.GetInt("received") == 0)
        {
            received = false;
        } else
        {
            received = true;
        }
        if (PlayerPrefs.GetInt("went") == 0)
        {
            went = false;
        }
        else
        {
            went = true;
        }
        
        coinCount = PlayerPrefs.GetInt("money", 0);
        scrumbCount = PlayerPrefs.GetInt("scrumbCount", 0);
        diamondCount = PlayerPrefs.GetInt("diamondCount", 0);
        fish0Count = PlayerPrefs.GetInt("fish0Count", 0);
        fish1Count = PlayerPrefs.GetInt("fish1Count", 0);
        fish2Count = PlayerPrefs.GetInt("fish2Count", 0);
        UpdateUI();
        
        // UpdateMoney(0);
    }

    public bool IsHeartFull()
    {
        return heartCount >= maxHeartCount;
    }

    public bool ConsumeSingleHeart()
    {
        if (heartCount <= 0) return false;
        SetHeart(-1);
        return true;
    }

    public bool IsHeartEmpty()
    {
        return heartCount < 1;
    }
    
    public void Bus_launch()
    {
        went = true;
        PlayerPrefs.SetInt("went", 1);
        arriveTime = System.DateTime.Now;
        arriveTime = arriveTime.AddMinutes(30);
        print(arriveTime);
        PlayerPrefs.SetString("arriveTime", arriveTime.ToString(format));
        PlayerPrefs.Save();
        bus.SetTrigger("launch");
    }

    public void Bus_arrive()
    {
        went = false;
        received = false;
        PlayerPrefs.SetInt("went", 0);
        PlayerPrefs.SetInt("received", 0);
        PlayerPrefs.Save();
        bus.SetTrigger("arrive");
        gameObject.GetComponent<Main_control>().BusBtnUpdate();
    }

    public void Bus_Receive()
    {
        int rnd;
        if (PlayerPrefs.GetInt("myRealTotalCard") >= 250) rnd = Random.Range(1, 2);
        else if (PlayerPrefs.GetInt("myRealTotalCard") >= 100) rnd = Random.Range(1, 3);
        else rnd = Random.Range(1, 4);

        // gameObject.GetComponent<Main_control>().AddBBangType(rnd, "빵셔틀","choco");
        // gameObject.GetComponent<Main_control>().balloon.ShowMsg("셔틀이 초코롤빵 " + rnd + "개를 가져왔습니다!");

        int[] idx = new int[rnd];
        int[] amt = new int[rnd];
        
        for(int i = 0; i<rnd; i++)
        {
            idx[i] = 2000;
            amt[i] = 1;
        }

        RewardItemManager.Instance.Init(idx, amt, "shuttle","셔틀이 빵 " + rnd + "개를 가져왔다!");
        
        received = true;
        PlayerPrefs.SetInt("received", 1);
        PlayerPrefs.Save();
        bus.SetTrigger("ready");
    }

    public void Bus_SetAnim()
    {
        print("amim SET : " + went);
        if (went) bus.SetTrigger("launched");
        else if (!went & !received) bus.SetTrigger("arrive");
        else if (!went & received) bus.SetTrigger("ready");
    }

    public void UpdateMoney(int money)
    {
        print("UpdateMoney : " + money);
        // PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + money);

        string moneyString = string.Format("{0:#,###0}", money);
        string totlaMoneyString = string.Format("{0:#,###0}", PlayerPrefs.GetInt("money"));
        
        if(money > 0)
        {
            PlayerPrefs.SetInt("totalMoney", PlayerPrefs.GetInt("totalMoney") + money);
            moneyAmount_ui.GetComponent<Text>().color = Color.red;
            moneyAmount_ui.GetComponent<Text>().text = "+ " + moneyString + "냥";
        } else if(money < 0)
        {
            moneyAmount_ui.GetComponent<Text>().color = Color.blue;
            moneyAmount_ui.GetComponent<Text>().text = moneyString + "냥";
        } 
        moneyAmount_ui.GetComponent<Animator>().SetTrigger("show");
        PlayerPrefs.Save();

        if (money >= 0) AddMoney(MoneyType.Coin, money);
        else SubtractMoney(MoneyType.Coin, -money);
    }

    public int GetBalance()
    {
        return PlayerPrefs.GetInt("money");
    }

    public void StoreDateCheck()
    {
        if(PlayerPrefs.GetString("storeDate") != System.DateTime.Now.Date.ToString())
        {
            PlayerPrefs.SetString("storeDate", System.DateTime.Now.Date.ToString());
            PlayerPrefs.SetInt("storeDateCount", 0);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if(Time.frameCount % 30 == 0)
        {
            notice.UpdateCUTime();
            UpdateHeartCount();
            StoreDateCheck();
            //UpdateBbangInfo();

            if (heartCount < maxHeartCount & timeGap.TotalMinutes < 5)
            {
                timerHolder.transform.position = new Vector3(heartImg[heartCount].transform.position.x, timerHolder.transform.position.y, 1);
                if (!timerHolder.activeSelf) timerHolder.SetActive(true);
                leftTime = new System.TimeSpan(0, 5, 0);
                leftTime = leftTime - timeGap;
                timer.text = leftTime.ToString(@"mm\:ss");
            } else
            {
                if (timerHolder.activeSelf) timerHolder.SetActive(false);
            }

            if (went)
            {
                remainTime = arriveTime - System.DateTime.Now;
                busText.text = remainTime.ToString(@"mm\:ss") + " 후에 도착합니다.";
                if (remainTime.TotalSeconds < 1)
                {
                    Bus_arrive();
                }
            }

            if(!went & !received)
            {
                btn1.GetComponent<Image>().color = Color.yellow;
                btn2.GetComponent<Image>().color = Color.yellow;
            } else {
                btn1.GetComponent<Image>().color = Color.white;
                btn2.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void SetFishAnim(bool _isGoldFishActive)
    {
        isGoldFishActive = _isGoldFishActive;
        
        if (isGoldFishActive)
        {
            StartCoroutine(DoFishHeartAnim());
        }
        else
        {
            for (int i = 0; i < maxHeartCount; i++)
            {
                heartImg[i].EndFishAnim();
            }
        }
    }

    private IEnumerator DoFishHeartAnim()
    {
        for (int i = 0; i < maxHeartCount; i++)
        {
            heartImg[i].DoFishAnim();
            yield return new WaitForSeconds(0.08f);
        }
    }

    void UpdateHeartCount()
    {
        if(isGoldFishActive) return;
        if (heartCount == maxHeartCount) return;
        timeGap = System.DateTime.Now - oldTime;
        while (heartCount < maxHeartCount & timeGap.TotalMinutes >= 5)
        {
            SetHeart(1);
            oldTime = oldTime.AddMinutes(5);
            timeGap = System.DateTime.Now - oldTime;
        }
    }

    public void UpdateHeartUI(int count = -1)
    {
        if(count <0 && isGoldFishActive) return;
        if (count == -1) count = heartCount;
        if(count > maxHeartCount)
        {
            print("heart count error");
            return;
        }

        for(int i = 0; i < count; i++)
        {
            heartImg[i].Show();
        }
        for (int i = count; i < maxHeartCount; i++)
        {
            heartImg[i].Hide();
        }
        PlayerPrefs.SetInt("heartCount", heartCount);
        PlayerPrefs.Save();
    }

    public void SetHeart(int i)
    {
        if(isGoldFishActive) return;
        if (i == -1)
        {
            if(heartCount > 0)
            {
                heartImg[heartCount - 1].Hide();
                if(heartCount == maxHeartCount)
                {
                    oldTime = System.DateTime.Now;
                }
                heartCount -= 1;
            }
        }
        if (i == 1)
        {
            if (heartCount < maxHeartCount)
            {
                heartImg[heartCount].Show();
                heartCount += 1;
            }
        }

        PlayerPrefs.SetString("oldTime", oldTime.ToString(format));
        PlayerPrefs.SetInt("heartCount", heartCount);
        PlayerPrefs.Save();
    }

    public void HeartFull()
    {
        heartCount += 3;
        if (heartCount > maxHeartCount) heartCount = maxHeartCount;
        UpdateHeartUI(heartCount);
    }

    public void AddHeartByAmt(int amount)
    {
        heartCount += amount;
        if (heartCount > maxHeartCount) heartCount = maxHeartCount;
        UpdateHeartUI(heartCount);
    }
    
    public void UpdateBbangInfo()
    {
        currentBbangCount = PlayerPrefs.GetInt("currentBbangCount");
        bbang_mat = PlayerPrefs.GetInt("bbang_mat");
        bbang_choco = PlayerPrefs.GetInt("bbang_choco");
        bbang_strawberry = PlayerPrefs.GetInt("bbang_strawberry");
        bbang_hot = PlayerPrefs.GetInt("bbang_hot");

        bbangEatCount_ui.GetComponent<Text>().text = currentBbangCount + "개";
        chocoCount_ui.GetComponent<Text>().text = "초코 " + bbang_choco + "개\n"
            + "맛동산 " + bbang_mat + "개\n"
            + "딸기 " + bbang_strawberry + "개\n"
            + "핫소스 " + bbang_hot + "개\n"
            + "메이플 " + PlayerPrefs.GetInt("bbang_maple") + "개\n"
            + "푸린 " + PlayerPrefs.GetInt("bbang_purin") + "개\n"
            + "바캉스 " + PlayerPrefs.GetInt("bbang_vacance") + "병\n"
            + "야쿠르트 " + PlayerPrefs.GetInt("bbang_yogurt") + "병\n"
            + "빙그 " + PlayerPrefs.GetInt("bbang_bingle") + "개\n"
            + "Noti info : " + PlayerPrefs.GetString("CUArriveTime") + " | status : " + PlayerPrefs.GetInt("notificationSent");
    }

    public void bbangCountBtnClicked()
    {
        if (currentBbangCount <= 5)
        {
            balloon.ShowMsg("빵을 열심히 모아야겠다.");
        }
        else if (currentBbangCount <= 10)
        {
            balloon.ShowMsg("남은 빵들을 어떻게 하는 게 좋을까?");
        }
        else if (currentBbangCount <= 50)
        {
            balloon.ShowMsg("먹다 남은 빵이 점점 쌓이고 있다.");
        }
        else if (currentBbangCount < 100)
        {
            balloon.ShowMsg("빵이 많이 모였다.");
        }
        else if (currentBbangCount < 100)
        {
            balloon.ShowMsg("빵을 단군에 팔 수는 없을까?");
        }
        else
        {
            int rnd = Random.Range(0, 6);
            switch (rnd)
            {
                case 0:
                    balloon.ShowMsg("빵을 좀 어떻게 하는 게 좋겠어..");
                    break;
                case 1:
                    balloon.ShowMsg("이게 사람이 사는 집일까..");
                    break;
                case 2:
                    balloon.ShowMsg("남은 빵들을 단군마켓에 팔아볼까..?");
                    break;
                case 3:
                    balloon.ShowMsg("빵이 인간적으로 너무 많다.");
                    break;
                case 4:
                    balloon.ShowMsg("단군마켓에 팔아야겠어..");
                    break;
                case 5:
                    balloon.ShowMsg("빵때문에 렉이 걸리는 것 같아..");
                    break;
            }
        }
    }
    
    public enum MoneyType { Coin, Scrumb, Diamond, Fish0, Fish1, Fish2 }

    public int GetAmount(MoneyType type)
    {
        switch (type)
        {
            case MoneyType.Coin:
                return coinCount;
                break;
            case MoneyType.Scrumb :
                return scrumbCount;
                break;
            case MoneyType.Diamond:
                return diamondCount;
                break;
            case MoneyType.Fish0:
                return fish0Count;
                break;
            case MoneyType.Fish1:
                return fish1Count;
                break;
            case MoneyType.Fish2:
                return fish2Count;
                break;
        }

        return -1;
    }
    
    public void UpdateUI()
    {
        coinCount_ui.text = string.Format("{0:#,###0}", coinCount);
        diamondCount_ui.text = string.Format("{0:#,###0}", diamondCount);
        scrumbCount_ui.text = string.Format("{0:#,###0}", scrumbCount);
        fish0Count_ui.text = fish0Count.ToString();
        fish1Count_ui.text = fish1Count.ToString();
        fish2Count_ui.text = fish2Count.ToString();
        
        PlayerPrefs.SetInt("money", coinCount);
        PlayerPrefs.SetInt("diamondCount", diamondCount);
        PlayerPrefs.SetInt("scrumbCount", scrumbCount);
        PlayerPrefs.SetInt("fish0Count", fish0Count);
        PlayerPrefs.SetInt("fish1Count", fish1Count);
        PlayerPrefs.SetInt("fish2Count", fish2Count);
        PlayerPrefs.Save();
        
        /*
         * PlayerPrefs.SetInt("totalScrubCount", PlayerPrefs.GetInt("totalScrubCount",0) + amount);
           PlayerPrefs.SetInt("totalDiamondCount", PlayerPrefs.GetInt("totalDiamondCount",0) + amount);
         */
        scrumbObj.SetActive(PlayerPrefs.GetInt("totalScrubCount",0)>0 || scrumbCount > 0);
        diamondObj.SetActive(PlayerPrefs.GetInt("totalDiamondCount",0)>0 || diamondCount > 0);
        fishObj.SetActive(PlayerPrefs.GetInt("totalFishCount",0)>0 ||fish0Count>0 || fish1Count>0 || fish2Count>0 || isGoldFishActive);
        
        fishManager.UpdateFishStatus();
    }

    public void AddMoney(MoneyType _type, int amount)
    {
        // AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.coinInJar);
        int startValue, endValue;
        switch (_type)
        {
            case MoneyType.Coin:
                startValue = coinCount;
                coinCount += amount;
                PlayerPrefs.SetInt("money", coinCount);

                endValue = startValue + amount;
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    coinCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                break;
            case MoneyType.Scrumb:
                startValue = scrumbCount;
                scrumbCount += amount;
                PlayerPrefs.SetInt("scrumbCount", scrumbCount);
                PlayerPrefs.SetInt("totalScrubCount", PlayerPrefs.GetInt("totalScrubCount",0) + amount);

                endValue = startValue + amount;
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    scrumbCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                break;
            case MoneyType.Diamond:
                startValue = diamondCount;
                diamondCount += amount;
                PlayerPrefs.SetInt("diamondCount", diamondCount);
                PlayerPrefs.SetInt("totalDiamondCount", PlayerPrefs.GetInt("totalDiamondCount",0) + amount);

                endValue = startValue + amount;
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    diamondCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                break;
            
            case MoneyType.Fish0:
                fish0Count += amount;
                PlayerPrefs.SetInt("fish0Count", fish0Count);
                PlayerPrefs.SetInt("totalFishCount", PlayerPrefs.GetInt("totalFishCount",0) + amount);
                break;
            case MoneyType.Fish1:
                fish1Count += amount;
                PlayerPrefs.SetInt("fish1Count", fish1Count);
                PlayerPrefs.SetInt("totalFishCount", PlayerPrefs.GetInt("totalFishCount",0) + amount);
                break;
            case MoneyType.Fish2:
                fish2Count += amount;
                PlayerPrefs.SetInt("fish2Count", fish2Count);
                PlayerPrefs.SetInt("totalFishCount", PlayerPrefs.GetInt("totalFishCount",0) + amount);
                break;
            default:
                break;
        }
        UpdateUI();
    }

    public bool HasEnoughMoney(MoneyType _type, int amount)
    {
        switch (_type)
        {
            case MoneyType.Coin:
                return (coinCount >= amount);
                break;
            case MoneyType.Scrumb:
                return (scrumbCount >= amount);
                break;
            case MoneyType.Diamond:
                return (diamondCount >= amount);
                break;
            case MoneyType.Fish0:
                return (fish0Count >= amount);
                break;
            case MoneyType.Fish1:
                return (fish1Count >= amount);
                break;
            case MoneyType.Fish2:
                return (fish2Count >= amount);
                break;
            default:
                return false;
                break;
        }
    }

    public bool SubtractMoney(MoneyType _type, int amount)
    {
        if (!HasEnoughMoney(_type, amount)) return false;
        
        int startValue, endValue;
        switch (_type)
        {
            case MoneyType.Coin:
                startValue = coinCount;
                coinCount -= amount;
                PlayerPrefs.SetInt("money", coinCount);
                endValue = startValue - amount;
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    coinCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                
                break;
            case MoneyType.Scrumb:
                startValue = scrumbCount;
                scrumbCount -= amount;
                PlayerPrefs.SetInt("scrumbCount", scrumbCount);
                endValue = startValue - amount;
                
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    scrumbCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                break;
            case MoneyType.Diamond:
                startValue = diamondCount;
                diamondCount -= amount;
                PlayerPrefs.SetInt("diamondCount", diamondCount);
                endValue = startValue - amount;
                
                DOVirtual.Int(startValue, endValue, 0.5f, value => {
                    diamondCount_ui.text = string.Format("{0:#,###0}", Mathf.Round(value));
                });
                break;
            case MoneyType.Fish0:
                fish0Count -= amount;
                PlayerPrefs.SetInt("fish0Count", fish0Count);
                break;
            case MoneyType.Fish1:
                fish1Count -= amount;
                PlayerPrefs.SetInt("fish1Count", fish1Count);
                break;
            case MoneyType.Fish2:
                fish2Count -= amount;
                PlayerPrefs.SetInt("fish2Count", fish2Count);
                break;
            default:
                startValue = 0;
                break;
        }
        UpdateUI();
        return true;
    }
}
