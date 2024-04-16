using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DangunManager : MonoBehaviour
{
    private const string format = "yyyy/MM/dd HH:mm:ss";

    private const int interval = 3600;
    [SerializeField] private GameManager main;
    [SerializeField] private PhoneMessageController Msg;
    [SerializeField] private DangunCharacterManager DangunCha;
    [SerializeField] private CollectionPanelManager collectionPanel;
    [SerializeField] private GameObject resetbtn_ui;
    [SerializeField] private ADManager ad;

    [SerializeField] private BalloonUIManager balloon;
    [SerializeField] private BbangBoyContoller bbangBoy;
    [SerializeField] private GameObject MsgBox;
    [SerializeField] private Text msgboxText_ui;

    [SerializeField] private CollectionManager collection;
    [SerializeField] private GameObject[] soldOuts = new GameObject[7];
    [SerializeField] private Text[] soldOutstext_ui = new Text[7];
    [SerializeField] private Text[] sellbuyTitle = new Text[2];
    [SerializeField] private Text[] sellbuyPrice = new Text[2];
    [SerializeField] private Image[] sellbuyImage = new Image[2];

    private readonly bool[] available = new bool[7];

    private string check, nowTime;
    private DateTime oldTime, newTime;
    private IFormatProvider provider;

    private TimeSpan timeGap;

    private void Start()
    {
        print(nowTime = DateTime.Now.ToString(format));
        MsgBox.SetActive(false);

        if (!PlayerPrefs.HasKey("dangun6"))
        {
            PlayerPrefs.SetString("dangun0", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun1", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun2", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun3", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun4", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun5", "2022/06/15 04:48:15");
            PlayerPrefs.SetString("dangun6", "2022/06/15 04:48:15");
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (Time.frameCount % 30 == 0)
            if (NeedToUpdate())
                for (var i = 0; i < available.Length; i++)
                    if (!available[i])
                    {
                        soldOutstext_ui[i].text = GetRemainTime(i) + " 뒤에 돌아올게요!";

                        available[i] = CheckDateAvailability(i);
                        if (available[i])
                        {
                            soldOuts[i].SetActive(false);
                            UpdateState();
                        }
                    }
    }

    //Open Dangun Panel
    public void OpenDangun()
    {
        gameObject.SetActive(true);
        UpdateState();
    }

    //Update Availability State
    public void UpdateState()
    {
        for (var i = 0; i < available.Length; i++)
        {
            available[i] = CheckDateAvailability(i);
            soldOuts[i].SetActive(!available[i]);
        }

        if (NeedToUpdate())
            resetbtn_ui.GetComponent<Animator>().SetTrigger("play");
        else
            resetbtn_ui.GetComponent<Animator>().SetTrigger("stop");

        if (available[5])
            if (PlayerPrefs.GetInt("sellUpdated" + 0) == 0)
                ResetSellCard(0);

        if (available[6])
            if (PlayerPrefs.GetInt("sellUpdated" + 1) == 0)
                ResetBuyCard(1);

        PrintSellbuyCard();
    }

    //Retrun Availability in boolean
    public bool CheckDateAvailability(int idx)
    {
        //oldTime = System.DateTime.ParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider);
        if (!DateTime.TryParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider, DateTimeStyles.None,
                out oldTime))
        {
            PlayerPrefs.SetString("dangun" + idx, "2022/06/15 04:48:15");
            if (!DateTime.TryParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider, DateTimeStyles.None,
                    out oldTime)) balloon.ShowMsg("데이터 오류 : dangun" + idx);
        }

        timeGap = DateTime.Now - oldTime;

        if (timeGap.TotalSeconds >= interval) return true;
        return false;
    }

    //Return Remaining time in string
    public string GetRemainTime(int idx)
    {
        //oldTime = System.DateTime.ParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider);
        if (!DateTime.TryParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider, DateTimeStyles.None,
                out oldTime))
        {
            PlayerPrefs.SetString("dangun" + idx, "2022/06/15 04:48:15");
            if (!DateTime.TryParseExact(PlayerPrefs.GetString("dangun" + idx), format, provider, DateTimeStyles.None,
                    out oldTime)) balloon.ShowMsg("데이터 오류 : dangun" + idx);
        }

        timeGap = oldTime.AddSeconds(interval) - DateTime.Now;

        var outputString = timeGap.Minutes + "분 " + timeGap.Seconds + "초";
        return outputString;
    }

    //return if any obj need to wait
    public bool NeedToUpdate()
    {
        for (var i = 0; i < available.Length; i++)
            if (available[i] == false)
                return true;
        return false;
    }

    //set pref data when bought
    public void SetDateData(int idx)
    {
        nowTime = DateTime.Now.ToString(format);
        PlayerPrefs.SetString("dangun" + idx, nowTime);
        PlayerPrefs.Save();
    }

    public void CloseDangun()
    {
        gameObject.SetActive(false);
    }

    public void DangunBtnClicked(int idx)
    {
        print("DangunBtnClicked : " + idx);
        if (available[idx] == false)
        {
            balloon.ShowMsg("지금은 거래완료! 잠시 뒤에 다시 거래하자.");
            return;
        }

        switch (idx)
        {
            case 0:
                Msg.SetMsg("뜯은 빵 80개를 \n 안 뜯은 빵 1개와 교환합니다.", 2, "BbangBoyCallBack");
                break;
            case 1:
                Msg.SetMsg("맛동산을 박스당 2천원에 판매합니다.\n(최대 10개 거래 가능)", 2, "SellMatCallBack");
                break;
            case 2:
                Msg.SetMsg("2만원을 내고 핫소스빵을 구매합니다.\n(최대 3개 거래 가능)", 2, "BuyHotCallBack");
                break;
            case 3:
                Msg.SetMsg("같은 A급 스티커 4장을 바캉스 한 병과 교환합니다.\n(최대 3개 거래 가능)", 2, "BuyVacanceCallBack");
                break;
            case 4:
                Msg.SetMsg("같은 B급 스티커 4장을 야쿠르트 한 병과 교환합니다.\n(최대 3개 거래 가능)", 2, "BuyYogurtCallBack");
                break;
            case 5:
                Msg.SetMsg(collection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].rank + "급 스티커\n" +
                           "[" + collection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "]을(를)" +
                           "\n" + PlayerPrefs.GetInt("sellPrice" + 0) + "원에 구매합니다.\n" +
                           "\n(나의 보유 슈량 : " + collection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].count + "개)", 2,
                    "BuyCardCallBack");
                break;
            case 6:
                Msg.SetMsg(collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].rank + "급 스티커\n" +
                           "[" + collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + "]을(를)" +
                           "\n" + PlayerPrefs.GetInt("sellPrice" + 1) + "원에 판매합니다.\n" +
                           "\n(나의 보유 슈량 : " + collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)", 2,
                    "SellCardCallBack");
                break;
            default:
                balloon.ShowMsg("조금만 기다려 주세요! \n 열심히 개발하고 있어요!");
                break;
        }
    }

    public void BuyCardCallBack()
    {
        print(PlayerPrefs.GetInt("money"));
        if (PlayerPrefs.GetInt("money") < PlayerPrefs.GetInt("sellPrice" + 0))
        {
            balloon.ShowMsg("돈이 부족해..");
        }
        else
        {
            main.BbangBoyAction("CallBuyBoy");
            CloseDangun();
        }
    }

    public void SellCardCallBack()
    {
        print(collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count);

        if (collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count <= 0)
        {
            balloon.ShowMsg(collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + " 스티커가 없다..");
        }
        else
        {
            main.BbangBoyAction("CallSellBoy");
            CloseDangun();
        }
    }


    public void BbangBoyCallBack()
    {
        //80개 수량 확인
        if (PlayerPrefs.GetInt("currentBbangCount") >= 80)
        {
            main.BbangBoyAction("CallBbangBoy");
            nowTime = DateTime.Now.ToString(format);
            SetDateData(0);
            CloseDangun();
        }
        else
        {
            balloon.ShowMsg("빵을 더 많이 모아야겠다..");
        }
    }


    //PlayerPrefs.GetInt("Matdongsan")
    public void SellMatCallBack()
    {
        //맛동산
        if (PlayerPrefs.GetInt("bbang_mat") > 0)
        {
            main.BbangBoyAction("CallMatBoy");
            CloseDangun();
        }
        else
        {
            balloon.ShowMsg("맛동산이 부족하다..");
        }
    }

    public void BuyHotCallBack()
    {
        //돈 확인
        if (PlayerPrefs.GetInt("money") >= 20000)
        {
            main.BbangBoyAction("CallReSellBoy");
            CloseDangun();
        }
        else
        {
            balloon.ShowMsg("돈이 부족하다..");
        }
    }

    public void BuyVacanceCallBack()
    {
        if (collectionPanel.UpdateAndReturnSelectionAvailability(5, 'A'))
            main.BbangBoyAction("CallVacanceBoy");
        else
            balloon.ShowMsg("다섯 장 이상 모은 A급 스티커가 없다..");
    }

    //BuyYogurtCallBack
    public void BuyYogurtCallBack()
    {
        if (collectionPanel.UpdateAndReturnSelectionAvailability(5, 'B'))
            main.BbangBoyAction("CallYogurtBoy");
        else
            balloon.ShowMsg("다섯 장 이상 모은 B급 스티커가 없다..");
    }

    public void ReSetDateTimeData()
    {
        balloon.ShowMsg("좋아! 다시 거래해볼까?");
        PlayerPrefs.SetString("dangun0", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun1", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun2", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun3", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun4", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun5", "2022/06/15 04:48:15");
        PlayerPrefs.SetString("dangun6", "2022/06/15 04:48:15");
        ResetSellCard(0);
        ResetBuyCard(1);
        PrintSellbuyCard();
        PlayerPrefs.Save();
    }

    public void ResetBtnClicked()
    {
        Msg.SetMsg("광고를 보고\n단군마켓을 다시 로드할까요?\n\n단군거래를 다시 할 수 있고\n스티커가 새로고침됩니다.", 2, "WatchAdsToResetDangun");
    }

    public void WatchAdsToResetDangun()
    {
        ad.PlayAds(ADManager.AdsType.dangun);
    }

    public void PrintSellbuyCard()
    {
        collection.Start();
        //ResetSellCard(0);
        sellbuyTitle[0].text = collection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + " 스티커 팔아요!";
        sellbuyImage[0].sprite = collection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].image;
        sellbuyPrice[0].text = "판매가격 : " + PlayerPrefs.GetInt("sellPrice" + 0) + "냥";

        //ResetBuyCard(1);
        sellbuyTitle[1].text = collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + " 스티커 사요!";
        sellbuyImage[1].sprite = collection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].image;
        sellbuyPrice[1].text = "보상 : " + PlayerPrefs.GetInt("sellPrice" + 1) + "냥";
    }

    public void ResetSellCard(int idx)
    {
        int sellIdx, sellPrice;

        collection.Start();
        sellIdx = Random.Range(0, collection.myCard.Count);

        //print(sellIdx);
        if (collection.myCard[sellIdx].rank == 'S')
        {
            ResetSellCard(idx);
            return;
        }

        if (collection.myCard[sellIdx].rank == 'A')
        {
            sellPrice = Random.Range(20, 41) * 1000;
        }
        else if (collection.myCard[sellIdx].rank == 'B')
        {
            sellPrice = Random.Range(10, 21) * 1000;
        }
        else if (collection.myCard[sellIdx].rank == 'M')
        {
            sellPrice = Random.Range(15, 30) * 1000;
        }
        else
        {
            print("collection.myCard[sellIdx].rank ERROR : " + collection.myCard[sellIdx].rank);
            sellPrice = Random.Range(1, 10) * 1000;
        }

        PlayerPrefs.SetInt("sellIdx" + idx, sellIdx);
        PlayerPrefs.SetInt("sellPrice" + idx, sellPrice);
        PlayerPrefs.SetInt("sellUpdated" + idx, 1);
    }

    public void ResetBuyCard(int idx)
    {
        int sellIdx, sellPrice;

        collection.Start();
        sellIdx = Random.Range(0, collection.myCard.Count);

        if (collection.myCard[sellIdx].rank == 'S')
        {
            sellPrice = Random.Range(1, 4) * 10000;
        }
        else if (collection.myCard[sellIdx].rank == 'A')
        {
            sellPrice = Random.Range(3, 6) * 1000;
        }
        else if (collection.myCard[sellIdx].rank == 'B')
        {
            sellPrice = Random.Range(1, 4) * 1000;
        }
        else
        {
            print("collection.myCard[sellIdx].rank ERROR : " + collection.myCard[sellIdx].rank);
            sellPrice = Random.Range(1, 10) * 99999;
        }

        PlayerPrefs.SetInt("sellIdx" + idx, sellIdx);
        PlayerPrefs.SetInt("sellPrice" + idx, sellPrice);
        PlayerPrefs.SetInt("sellUpdated" + idx, 1);
        PlayerPrefs.Save();
    }
}