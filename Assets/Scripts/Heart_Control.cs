using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart_Control : MonoBehaviour
{
    public int heartCount = 0;
    public GameObject[] heartImg = new GameObject[6];
    System.DateTime oldTime, arriveTime;
    System.TimeSpan timeGap, leftTime, remainTime;
    const string format = "yyyy/MM/dd HH:mm:ss";
    System.IFormatProvider provider;

    public GameObject timerHolder;
    public Text timer;

    public bool received, went;

    public Animator bus;
    public Text busText;

    public GameObject btn1, btn2, bbangEatCount_ui, chocoCount_ui, money_ui;

    public int currentBbangCount;
    public BalloonControl balloon;

    public GameObject moneyAmount_ui;

    int bbang_choco, bbang_mat, bbang_strawberry, bbang_hot;

    public NotificationCtrl notice;

    // Start is called before the first frame update
    void Start()
    {
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

        /*
        print(PlayerPrefs.GetString("arriveTime"));
        print(PlayerPrefs.GetInt("received"));
        print(PlayerPrefs.GetInt("went"));
        */

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

        UpdateMoney(0);
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
        if (PlayerPrefs.GetInt("myRealTotalCard") >= 200) rnd = Random.Range(1, 2);
        else if (PlayerPrefs.GetInt("myRealTotalCard") >= 100) rnd = Random.Range(1, 3);
        else rnd = Random.Range(1, 4);

        gameObject.GetComponent<Main_control>().AddBBang(rnd, "choco");
        gameObject.GetComponent<Main_control>().balloon.ShowMsg("셔틀이 초코롤빵 " + rnd + "개를 가져왔습니다!");
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
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + money);

        if(money > 0)
        {
            PlayerPrefs.SetInt("totalMoney", PlayerPrefs.GetInt("totalMoney") + money);
            moneyAmount_ui.GetComponent<Text>().color = Color.red;
            moneyAmount_ui.GetComponent<Text>().text = "+ " + money + "원";
        } else if(money < 0)
        {
            moneyAmount_ui.GetComponent<Text>().color = Color.blue;
            moneyAmount_ui.GetComponent<Text>().text = money + "원";
        } else
        {
            money_ui.GetComponent<Text>().text = PlayerPrefs.GetInt("money") + "";
            return;
        }
        moneyAmount_ui.GetComponent<Animator>().SetTrigger("show");
        money_ui.GetComponent<Text>().text = PlayerPrefs.GetInt("money") + "";

        PlayerPrefs.Save();
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

            if (heartCount < 6 & timeGap.TotalMinutes < 5)
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

    void UpdateHeartCount()
    {
        if (heartCount == 6) return;
        timeGap = System.DateTime.Now - oldTime;
        while (heartCount < 6 & timeGap.TotalMinutes >= 5)
        {
            SetHeart(1);
            oldTime = oldTime.AddMinutes(5);
            timeGap = System.DateTime.Now - oldTime;
        }
    }

    // Update is called once per frame
    void UpdateHeartUI(int count)
    {
        if(count > 6)
        {
            print("heart count error");
            return;
        }

        for(int i = 0; i < count; i++)
        {
            heartImg[i].GetComponent<Animator>().SetTrigger("show");
        }
        for (int i = count; i < heartImg.Length; i++)
        {
            heartImg[i].GetComponent<Animator>().SetTrigger("hide");
        }
    }

    public void SetHeart(int i)
    {
        if (i == -1)
        {
            if(heartCount > 0)
            {
                heartImg[heartCount - 1].GetComponent<Animator>().SetTrigger("disappear");
                if(heartCount == 6)
                {
                    oldTime = System.DateTime.Now;
                }
                heartCount -= 1;
            }
        }
        if (i == 1)
        {
            if (heartCount < 6)
            {
                heartImg[heartCount].GetComponent<Animator>().SetTrigger("appear");
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
        if (heartCount > 6) heartCount = 6;
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
            balloon.ShowMsg("빵을 어떻게 하는게 좋을 것 같다.");
        }
        else if (currentBbangCount < 150)
        {
            balloon.ShowMsg("이게 사람 사는 집인가..");
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
                    balloon.ShowMsg("집이 빵으로 가득 찼잖아...?!");
                    break;
                case 5:
                    balloon.ShowMsg("빵때문에 렉이 걸리는 것 같아..");
                    break;
            }
        }
    }
}
