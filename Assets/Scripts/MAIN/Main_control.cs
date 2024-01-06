using System;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;
using Random = UnityEngine.Random;

public class Main_control : MonoBehaviour
{
    public GameObject lower_bar, phone_ui, store_panel;

    public GameObject front_panel,
        main_panel,
        frontCha,
        prompter,
        storePanelHolder,
        bbangCount_ui,
        bbang_panel,
        bbang_holder,
        collection,
        bbangCountPanel;

    public BalloonControl balloon;

    public GameObject newStore;

    public string storeOutAction;
    public AudioControl myAudio;

    public Bbang_showroom showroom;
    public GameObject watchAdPanel, busPanel, creditPanel, settingPanel, dangunPanel;
    public Text lauchText;
    public GameObject launchBtn;

    public GameObject debugPanel, chaSelectPanel, goToStoreBtn, phoneMsgBox;
    public AlbaControl alba;
    public PhoneMsgCtrl Msg;
    public GameObject debugData, parkBtn;
    public Text goBtnText_ui;
    public SettingPanelControl setAudio;
    public GameObject debugPanlShowBtn;

    public string currentLocation;
    public bool dangunIng;

    public bool gointToPark;

    public DangunChaCtrl dangunCha;

    //public LeaderboardManager leaderboard;
    public AchievementCtrl achievement;
    public Collection_Panel_Control collectionPanel;
    public NotificationCtrl notice;
    public bool bbobgiMode;

    public RankCtrl rank;
    public DdukCtrl dduk;
    private bool albaMode;

    private bool callBbangboy;
    private int callIdx;

    private int debugCount;
    private Heart_Control heartControl;
    private bool hidePhoneTmp;

    private bool matdongsanBuy;

    private int myStoreType, bbangCount;

    private int originalMusic = 1;
    private PrompterControl pmtComtrol;

    private int score;
    private string tanghuruState;

    [SerializeField] private DoMiCoinManager domiCoin;
    [SerializeField] private DomiTradePanel domiTradePanel;
    [SerializeField] private TanghuruGameManager tanghuruGameManager;
    
    public static Main_control Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        setAudio.Start();
        Msg.Start();

        heartControl = gameObject.GetComponent<Heart_Control>();
        currentLocation = "home";
        //DataUpdate totalAlbaCustomerCount
        if (!PlayerPrefs.HasKey("albaCustomerCount"))
        {
            PlayerPrefs.SetInt("albaCustomerCount", 0);
            PlayerPrefs.SetInt("albaTime", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("ChaName") | !PlayerPrefs.HasKey("ChaIdx"))
        {
            if (!PlayerPrefs.HasKey("ChaName")) PlayerPrefs.SetString("ChaName", "훈이");
            if (!PlayerPrefs.HasKey("ChaIdx")) PlayerPrefs.SetInt("ChaName", 0);
            PlayerPrefs.Save();
            chaSelectPanel.GetComponent<ChaSelectControl>().OpenChaSelect();
        }

        //DataUpdate MatDongSan
        if (!PlayerPrefs.HasKey("Matdongsan"))
        {
            PlayerPrefs.SetInt("Matdongsan", Mathf.RoundToInt(PlayerPrefs.GetInt("storeCount") / 6.5f));
            PlayerPrefs.Save();
        }

        //DataUpdate ShuttleCount
        if (!PlayerPrefs.HasKey("ShuttleCount"))
        {
            PlayerPrefs.SetInt("ShuttleCount", Mathf.RoundToInt(PlayerPrefs.GetInt("totalBbangCount") / 11f));
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("new_choco")) PlayerPrefs.SetInt("new_choco", PlayerPrefs.GetInt("bbang"));

        if (!PlayerPrefs.HasKey("new_strawberry")) PlayerPrefs.SetInt("new_strawberry", 0);
        //new_hot
        if (!PlayerPrefs.HasKey("new_hot")) PlayerPrefs.SetInt("new_hot", 0);
        //albaExp
        if (!PlayerPrefs.HasKey("albaExp")) PlayerPrefs.SetInt("albaExp", 0);
        if (!PlayerPrefs.HasKey("debugMode")) PlayerPrefs.SetInt("debugMode", 0);

        //RealTotalCardCount
        if (!PlayerPrefs.HasKey("myRealTotalCard"))
        {
            collectionPanel.GetCount();
            PlayerPrefs.SetInt("myRealTotalCard", PlayerPrefs.GetInt("myTotalCards"));
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetInt("myTotalCards") > PlayerPrefs.GetInt("myRealTotalCard"))
        {
            PlayerPrefs.SetInt("myRealTotalCard", PlayerPrefs.GetInt("myTotalCards"));
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbobgiCount")) PlayerPrefs.SetInt("bbobgiCount", 0);
        if (!PlayerPrefs.HasKey("lowDataMode"))
        {
#if UNITY_IPHONE
            PlayerPrefs.SetInt("lowDataMode", 0);
#elif UNITY_ANDROID
            PlayerPrefs.SetInt("lowDataMode", 1);
#endif
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("muteAudio"))
        {
            PlayerPrefs.SetInt("muteAudio", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("yull_friend"))
        {
            PlayerPrefs.SetInt("yull_friend", 0);
            PlayerPrefs.Save();
        }


        Application.targetFrameRate = 60;
        watchAdPanel.SetActive(false);
        front_panel.SetActive(true);
        frontCha.SetActive(true);
        phone_ui.SetActive(false);
        lower_bar.SetActive(true);
        //front_panel.SetActive(false);
        main_panel.SetActive(true);
        store_panel.GetComponent<Animator>().SetTrigger("hidden");
        store_panel.SetActive(false);
        prompter.SetActive(false);
        busPanel.SetActive(false);
        creditPanel.SetActive(false);
        settingPanel.SetActive(false);

        pmtComtrol = prompter.GetComponent<PrompterControl>();

        UpdateBbangCount();
        myAudio.PlayMusic(0);
        debugPanel.SetActive(false);
        //dangunPanel.SetActive(false);

        /*
        if (PlayerPrefs.GetInt("myTotalCards") == 116 & PlayerPrefs.GetInt("myRealTotalCard") >= 1800)
        {
            PlayerPrefs.SetInt("debugMode", 1);
        }
        */
        if (PlayerPrefs.GetInt("debugMode") == 1) SetDebugMode();

        rank.Start();
        notice.Start();
    }

    // Update is called once per frame
    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Msg.SetMsg("포켓볼빵 더게임을\n종료할까요?", 2, "QuitApp");
        }
        if (Input.GetKeyDown(KeyCode.Menu))
        {
            HideBtn();
        }
#endif
        if (Time.frameCount % 30 == 0)
            if (callBbangboy & (currentLocation == "home"))
            {
                switch (callIdx)
                {
                    case 0:
                        balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                        gameObject.GetComponent<BbangBoyContol>().Boy_In();
                        break;
                    case 1:
                        balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                        dangunCha.PlayDangunCha(callIdx);
                        break;
                    case 2:
                        balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                        dangunCha.PlayDangunCha(callIdx);
                        break;
                    case 3:
                        balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                        dangunCha.PlayDangunCha(callIdx);
                        break;
                }

                callBbangboy = false;
            }
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void DebugBtn()
    {
        debugCount += 1;
        if (debugCount >= 15) Msg.SetMsg("디버그 모드에 진입하면 영구적으로 기록이 남으며 랭킹 시스템에서 제외됩니다.\n디버그 모드에 진입하시겠습니까?", 2, "debug");
    }

    public void OpenDebug()
    {
        if (!debugPanel.activeSelf)
        {
            PlayerPrefs.SetInt("debugMode", 1);
            PlayerPrefs.Save();
            debugPanel.SetActive(true);
            debugData.SetActive(true);
        }
        else
        {
            debugPanel.SetActive(false);
            debugData.SetActive(false);
        }

        debugCount = 0;
    }

    public void CloseDebug()
    {
        debugPanel.SetActive(false);
        collection.GetComponent<Collection_Panel_Control>().debugMode = false;
        debugCount = 0;
    }

    public void SetCollectionDebugMode()
    {
        collection.GetComponent<Collection_Panel_Control>().debugMode = true;
        BtnClicked("close_phone");
        BtnClicked("collection");
    }

    public void BtnClicked(string idx)
    {
        switch (idx)
        {
            case "collection":
                collection.GetComponent<Collection_Panel_Control>().Start();
                collection.GetComponent<Collection_Panel_Control>().ShowPanel();
                debugCount = 0;
                break;
            case "store":
                if (currentLocation == "park")
                {
                    //parkToHome
                    gointToPark = false;
                    currentLocation = "park";
                    frontCha.GetComponent<Animator>().SetTrigger("walk");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");
                    newStore.GetComponent<Animator>().SetTrigger("hide");
                    main_panel.GetComponent<Animator>().SetTrigger("show");
                    myAudio.PlayMusic(0);
                }
                else if (currentLocation == "home")
                {
                    if (GetComponent<Heart_Control>().heartCount >= 1)
                    {
                        parkBtn.SetActive(false);
                        GetComponent<Heart_Control>().SetHeart(-1);
                        front_panel.SetActive(true);
                        frontCha.GetComponent<Animator>().SetTrigger("walk");
                        main_panel.GetComponent<Animator>().SetTrigger("hide");
                        lower_bar.GetComponent<Animator>().SetTrigger("hide");
                        currentLocation = "toStore";
                        myAudio.PlayMusic(1);
                        GoToStore();
                    }
                    else
                    {
                        balloon.ShowMsg("지금은 피곤하다.. \n 너튜브나 보면서 쉴까..");
                    }

                    debugCount = 0;
                }
                else if ((currentLocation == "store") | (currentLocation == "alba"))
                {
                    ActionHandler("go_home");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");
                    pmtComtrol.Reset();
                    prompter.GetComponent<Animator>().SetTrigger("hide");
                }

                break;
            case "phone":
                if (dangunIng) return;
                if (prompter.activeSelf)
                    if (prompter.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name ==
                        "Lower_Bar_Idle")
                    {
                        hidePhoneTmp = true;
                        prompter.GetComponent<Animator>().SetTrigger("hide");
                    }

                lower_bar.GetComponent<Animator>().SetTrigger("hide");
                phone_ui.SetActive(true);
                phone_ui.GetComponent<Animator>().SetTrigger("show");
                frontCha.GetComponent<Animator>().SetTrigger("phone");
                break;
            case "close_phone":
                if (hidePhoneTmp)
                {
                    prompter.GetComponent<Animator>().SetTrigger("show");
                    hidePhoneTmp = false;
                }
                else
                {
                    lower_bar.SetActive(true);
                    lower_bar.GetComponent<Animator>().SetTrigger("show");
                }

                phone_ui.GetComponent<Animator>().SetTrigger("hide");
                frontCha.GetComponent<Animator>().SetTrigger("idle");
                break;
            case "reset":
                PlayerPrefs.DeleteAll();
                balloon.ShowMsg("초기화 성공! 앱을 재실행 해주세요.");
                Application.Quit();
                break;
        }
    }

    public void BbangBoyAction(string action)
    {
        print(action);
        switch (action)
        {
            case "bbangBoyIn":
                phone_ui.GetComponent<Animator>().SetTrigger("hide");
                lower_bar.GetComponent<Animator>().SetTrigger("hide");
                break;
            case "bbangBoyOut":
                lower_bar.GetComponent<Animator>().SetTrigger("show");
                prompter.GetComponent<Animator>().SetTrigger("hide");
                pmtComtrol.Reset();
                dangunIng = false;
                break;
            case "CallBbangBoy":
                dangunIng = true;
                if (currentLocation == "home")
                {
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    gameObject.GetComponent<BbangBoyContol>().Boy_In();
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 0;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallMatBoy":
                if (currentLocation == "home")
                {
                    callIdx = 1;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 1;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallReSellBoy":
                if (currentLocation == "home")
                {
                    callIdx = 2;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 2;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallVacanceBoy":
                if (currentLocation == "home")
                {
                    callIdx = 3;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 3;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallYogurtBoy":
                if (currentLocation == "home")
                {
                    callIdx = 4;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 4;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallBuyBoy":
                if (currentLocation == "home")
                {
                    callIdx = 5;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 5;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
            case "CallSellBoy":
                if (currentLocation == "home")
                {
                    callIdx = 6;
                    balloon.ShowMsg("똑똑- 노크소리가 들린다.");
                    dangunCha.PlayDangunCha(callIdx);
                }
                else
                {
                    balloon.ShowMsg("집으로 돌아가자!");
                    callBbangboy = true;
                    callIdx = 6;
                    phone_ui.GetComponent<Animator>().SetTrigger("hide");
                    lower_bar.GetComponent<Animator>().SetTrigger("hide");

                    if (currentLocation == "park")
                        BtnClicked("store");
                    else
                        ActionHandler("go_home");
                }

                break;
        }
    }

    public void ActionHandler(string action)
    {
        print(currentLocation);
        print("ACTION " + action);

        switch (action)
        {
            case "go_home":
                if ((currentLocation != "store") & (currentLocation != "alba")) return;
                currentLocation = "toHome";
                myAudio.PlayMusic(5);
                frontCha.GetComponent<Animator>().SetTrigger("walkLeft");
                newStore.GetComponent<Animator>().SetTrigger("showR");
                main_panel.GetComponent<Animator>().SetTrigger("hideR");
                prompter.GetComponent<Animator>().SetTrigger("hide");
                albaMode = false;
                break;

            case "store_next":
                if (currentLocation != "store") return;

                if (GetComponent<Heart_Control>().heartCount >= 1)
                {
                    GetComponent<Heart_Control>().SetHeart(-1);
                    newStore.GetComponent<Animator>().SetTrigger("hide");
                    frontCha.GetComponent<Animator>().SetTrigger("walk");
                    GoToStore();
                    albaMode = false;
                    currentLocation = "store_next";
                }
                else
                {
                    balloon.ShowMsg("피곤하다.. \n 집으로 돌아가서 쉬는게 좋겠어..");
                    currentLocation = "store";
                }

                break;

            case "store_in":
                if (currentLocation != "store") return;
                currentLocation = "store_in";
                myAudio.PlayMusic(2);
                EnterStore();
                break;

            case "alba_in":
                currentLocation = "alba_in";
                AlbaEnterStore();
                break;

            case "store_out":
                domiTradePanel.HidePanel();
                currentLocation = "store";
                if ((myStoreType == 0) & (PlayerPrefs.GetInt("girl_friend") != 0))
                    achievement.UpdateAchievement("미소녀");
                else if ((myStoreType == 1) & (PlayerPrefs.GetInt("yang_friend") != 0))
                    achievement.UpdateAchievement("양아치");
                else if ((myStoreType == 2) & (PlayerPrefs.GetInt("albaExp") != 0))
                    achievement.UpdateAchievement("맛동석");
                else if ((myStoreType == 3) & (PlayerPrefs.GetInt("bi_friend") != 0))
                    achievement.UpdateAchievement("비실이");
                else if ((myStoreType == 4) & (PlayerPrefs.GetInt("yull_friend") != 0))
                    achievement.UpdateAchievement("열정맨");
                else if ((myStoreType == 7) & PlayerPrefs.GetInt("NylonDomiTutorial") == 1)
                    achievement.UpdateAchievement("나이롱마스크");
                else if (myStoreType == 8 && PlayerPrefs.GetInt("tanghuru_friend", 0) == 1)
                {
                    achievement.UpdateAchievement("왕형");
                }

                myAudio.PlayMusic(1);
                pmtComtrol.Reset();
                if (storeOutAction != "") pmtComtrol.AddString("훈이", storeOutAction);
                pmtComtrol.AddOption("다른 편의점으로 간다.", "main", "store_next");
                pmtComtrol.AddOption("집으로 돌아간다.", "main", "go_home");
                break;

            case "alba_end":
                currentLocation = "alba";
                WentToStore();
                break;

            case "park":
                //leaderboard.AuthenticateToGameCenter();
                achievement.Autheticate();
                gointToPark = true;
                goBtnText_ui.text = "집으로 돌아가기";
                currentLocation = "toPark";
                parkBtn.SetActive(false);
                frontCha.GetComponent<Animator>().SetTrigger("walkLeft");
                main_panel.GetComponent<Animator>().SetTrigger("showR");
                newStore = Instantiate(store_panel, storePanelHolder.transform);
                newStore.GetComponent<StoreControl>().UpdateStore(10);
                newStore.SetActive(true);
                newStore.GetComponent<Animator>().SetTrigger("hideR");
                lower_bar.GetComponent<Animator>().SetTrigger("hide");
                myAudio.PlayMusic(5);
                break;
        }
    }

    public void GoToStore(int target = -1)
    {
        parkBtn.SetActive(false);
        pmtComtrol.Reset();
        var str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        pmtComtrol.AddString("훈이", str);
        prompter.GetComponent<Animator>().SetTrigger("show");
        prompter.SetActive(true);
        currentLocation = "toStore";

        for (;;)
        {
            var rnd = Random.Range(0, 9);

            //DEBUG
            if (myStoreType != rnd)
            {
                myStoreType = rnd;
                break;
            }
        }

        if (target != -1) myStoreType = target;

        newStore = Instantiate(store_panel, storePanelHolder.transform);
        newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
        newStore.SetActive(true);
        newStore.GetComponent<Animator>().SetTrigger("show");
        goBtnText_ui.text = "집으로 돌아가기";
    }

    public void AlbaBtnClicked()
    {
        if (PlayerPrefs.GetInt("albaExp") == 0)
        {
            phoneMsgBox.GetComponent<PhoneMsgCtrl>().SetMsg("지금은 알바 자리가 없습니다.\n쌈마트24에 가서 맛동석 점장에게 얘기해보는 건 어떨까요?", 1);
            return;
        }

        if (gameObject.GetComponent<Heart_Control>().heartCount < 1)
        {
            balloon.ShowMsg("지금은 좀 피곤하다..");
            return;
        }

        if ((currentLocation == "home") | (currentLocation == "store"))
            phoneMsgBox.GetComponent<PhoneMsgCtrl>().SetMsg("알바를 하기 위해\n3마트24로 이동할까요?", 2, "albaGo");
        else balloon.ShowMsg("여기서는 할 수 없다..");
    }

    public void GoToAlba()
    {
        goBtnText_ui.text = "집으로 돌아가기";
        parkBtn.SetActive(false);

        if (currentLocation == "home")
        {
            front_panel.SetActive(true);
            frontCha.GetComponent<Animator>().SetTrigger("walk");
            main_panel.GetComponent<Animator>().SetTrigger("hide");
            lower_bar.GetComponent<Animator>().SetTrigger("hide");
            currentLocation = "toAlba";
            myAudio.PlayMusic(1);

            if (hidePhoneTmp)
            {
                prompter.GetComponent<Animator>().SetTrigger("show");
                hidePhoneTmp = false;
            }
            else
            {
                lower_bar.SetActive(true);
                lower_bar.GetComponent<Animator>().SetTrigger("show");
            }

            BtnClicked("close_phone");

            pmtComtrol.Reset();
            //string str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
            pmtComtrol.AddString("훈이", "알바를 하러 3마트24를 향해 걸어가고 있다.");
            prompter.GetComponent<Animator>().SetTrigger("show");
            prompter.SetActive(true);

            myStoreType = 2;

            newStore = Instantiate(store_panel, storePanelHolder.transform);
            newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
            newStore.SetActive(true);
            newStore.GetComponent<Animator>().SetTrigger("show");

            albaMode = true;
        }
        else if (currentLocation == "store")
        {
            if (myStoreType == 2)
            {
                albaMode = true;
                BtnClicked("close_phone");
                WentToStore();
            }
            else
            {
                BtnClicked("close_phone");
                frontCha.GetComponent<Animator>().SetTrigger("walk");
                newStore.GetComponent<Animator>().SetTrigger("hide");
                pmtComtrol.Reset();
                //string str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
                pmtComtrol.AddString("훈이", "알바를 하러 3마트24를 향해 걸어가고 있다.");
                prompter.GetComponent<Animator>().SetTrigger("show");

                myStoreType = 2;

                newStore = Instantiate(store_panel, storePanelHolder.transform);
                newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
                newStore.SetActive(true);
                newStore.GetComponent<Animator>().SetTrigger("show");

                albaMode = true;
            }
        }

        currentLocation = "goToAlba";
    }

    private int storeCount()
    {
        if (!PlayerPrefs.HasKey("storeCount"))
        {
            PlayerPrefs.SetInt("storeCount", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("storeCount", PlayerPrefs.GetInt("storeCount") + 1);
            PlayerPrefs.SetInt("storeDateCount", PlayerPrefs.GetInt("storeDateCount") + 1);
            PlayerPrefs.Save();
        }

        return PlayerPrefs.GetInt("storeCount");
    }

    public void WentBackHome()
    {
        parkBtn.SetActive(true);
        //front_panel.SetActive(false);
        currentLocation = "home";
        frontCha.GetComponent<Animator>().SetTrigger("idle");
        lower_bar.GetComponent<Animator>().SetTrigger("show");
        myAudio.PlayMusic(0);
        goBtnText_ui.text = "빵 사러 편의점 가기";
    }

    public void WentToPark()
    {
        if (currentLocation == "toHome") WentBackHome();
        if (!gointToPark) return;

        gointToPark = false;
        currentLocation = "park";
        frontCha.GetComponent<Animator>().SetTrigger("idle");
        lower_bar.GetComponent<Animator>().SetTrigger("show");
    }

    public void WentToStore()
    {
        currentLocation = "store";
        frontCha.GetComponent<Animator>().SetTrigger("idle");
        pmtComtrol.Reset();


        if (albaMode)
        {
            currentLocation = "alba";
            pmtComtrol.AddOption("알바하러 들어간다.", "main", "alba_in");
            pmtComtrol.AddOption("집으로 돌아간다.", "main", "go_home");
            return;
        }

        switch (myStoreType)
        {
            case 0:
                pmtComtrol.AddString("훈이", "오또기 포도씨유 앞에 왔다.");
                break;
            case 1:
                pmtComtrol.AddString("훈이", "잼미니스탑 앞에 도착했다.");
                break;
            case 2:
                pmtComtrol.AddString("훈이", "쌈마트 24에 도착했다!");
                break;
            case 3:
                pmtComtrol.AddString("훈이", "나인투식스 앞에 도착했다!");
                break;
            case 4:
                pmtComtrol.AddString("훈이", "진상25 앞에 도착했다!");
                break;
            case 5:
                pmtComtrol.AddString("훈이", "회미리마트 앞에 도착했다!");
                pmtComtrol.AddString("훈이", "문에 쪽지가 붙어있다!");
                pmtComtrol.AddOption("쪽지를 확인한다.", "store", "note");
                pmtComtrol.AddOption("뽑기 기계를 확인한다.", "store", "bbobgi");
                pmtComtrol.AddOption("집으로 돌아간다.", "main", "go_home");
                return;
            case 6:
                pmtComtrol.AddString("훈이", "머박분식집 앞에 도착했다!");
                break;
            case 7:
                pmtComtrol.AddString("훈이", "파랑새치킨 앞에 도착했다!");
                break;
            case 8:
                pmtComtrol.AddString("훈이", "왕형 탕후루 앞에 도착했다!");
                break;
        }

        pmtComtrol.AddOption("편의점으로 들어간다.", "main", "store_in");
        pmtComtrol.AddOption("다른 편의점으로 간다.", "main", "store_next");
        pmtComtrol.AddOption("집으로 돌아간다.", "main", "go_home");
    }

    private void AlbaEnterStore()
    {
        if (GetComponent<Heart_Control>().heartCount >= 1)
        {
            GetComponent<Heart_Control>().SetHeart(-1);
        }
        else
        {
            balloon.ShowMsg("지금은 좀 피곤하다..");
            currentLocation = "alba";
            return;
        }

        pmtComtrol.Reset();
        pmtComtrol.imageMode = true;
        storeOutAction = "";
        currentLocation = "alba_in";

        pmtComtrol.AddString("점주", "어! 알바하러 왔구나!");
        pmtComtrol.AddString("점주", "잘 됐다 일손이 부족했었는데.");
        pmtComtrol.AddOption("바로 시작할게요.", "store", "alba_start");
        pmtComtrol.AddOption("어떻게 하면 되나요?", "store", "alba_how");
    }

    private void EnterStore()
    {
        pmtComtrol.Reset();
        pmtComtrol.imageMode = true;
        storeOutAction = "";
        currentLocation = "store_in";
        //int chaIdx = Random.Range(0, 3);
        var chaIdx = myStoreType;

        //미소녀
        if (chaIdx == 0)
        {
            if (!PlayerPrefs.HasKey("girl_friend"))
            {
                PlayerPrefs.SetInt("girl_friend", 0);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.GetInt("girl_friend") == 0)
            {
                pmtComtrol.AddString("미소녀", "어서오세요. (포도)씨유입니다.");

                pmtComtrol.AddOption("포켓볼빵 있어요?", "store", "girl_ball");
                pmtComtrol.AddOption("남자친구 있어요?", "store", "girl_boy");
                pmtComtrol.AddOption("전화번호좀 주세요.", "store", "girl_phone");
            }
            else
            {
                //timer not yet
                if (notice.TimeGap() <= 0)
                {
                    pmtComtrol.AddString("미소녀", "어서와.");
                    pmtComtrol.AddOption("포켓볼빵 있어?", "store", "girlf_ball");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (notice.TimeGap() <= 5)
                {
                    //2-3개
                    pmtComtrol.AddString("미소녀", "우와 바로 왔구나!");
                    pmtComtrol.AddString("훈이", "아직 안팔렸지?");
                    var rnd = Random.Range(2, 4);
                    pmtComtrol.AddString("미소녀", "응 포켓볼빵 " + rnd + "개 있어!");
                    AddBBang(rnd);
                    pmtComtrol.AddString("훈이", "대박이다! 정말 고마워!");
                    pmtComtrol.AddString("미소녀", "후훗 천만에 말씀.");
                    pmtComtrol.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (notice.TimeGap() <= 30)
                {
                    //1-2개
                    pmtComtrol.AddString("미소녀", "왔구나!!");
                    pmtComtrol.AddString("훈이", "포켓볼빵 아직 있지?!");
                    var rnd = Random.Range(1, 3);
                    pmtComtrol.AddString("미소녀", "조금만 더 일찍 오지. \n 포켓볼빵 " + rnd + "개 남았어.");
                    AddBBang(rnd);
                    pmtComtrol.AddString("훈이", "우와 !정말 고마워!");
                    pmtComtrol.AddString("미소녀", "친구라면 이정도 쯤이야!");
                    pmtComtrol.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (notice.TimeGap() <= 60)
                {
                    //1개
                    pmtComtrol.AddString("미소녀", "조금만 더 일찍 오지!");
                    pmtComtrol.AddString("훈이", "허걱 벌써 다 팔렸어?");
                    pmtComtrol.AddString("미소녀", "아니 딱 한개 남았어.");
                    AddBBang(1);
                    pmtComtrol.AddString("훈이", "그래도 정말 고마워!");
                    pmtComtrol.AddString("미소녀", "다음에는 조금 더 일찍와.");
                    pmtComtrol.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else
                {
                    //late
                    pmtComtrol.AddString("미소녀", "어 왔구나!");
                    pmtComtrol.AddString("훈이", "포켓볼빵 아직 있어?");
                    pmtComtrol.AddString("미소녀", "한시간도 안 돼서 다 팔렸어..");
                    pmtComtrol.AddString("훈이", "에고 그렇구나..\n그래도 문자해줘서 고마워.");
                    pmtComtrol.AddString("미소녀", "다음에는 문자하면 바로와!");
                    pmtComtrol.AddString("훈이", "응. 고마워.");
                    pmtComtrol.AddOption("알림이 안왔어!", "store", "girlf_none");
                    pmtComtrol.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
            }
        }

        //양아치
        else if (chaIdx == 1)
        {
            if (!PlayerPrefs.HasKey("yang_friend"))
            {
                PlayerPrefs.SetInt("yang_friend", 0);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.GetInt("yang_friend") == 1)
            {
                pmtComtrol.AddString("양아치", "어 왔네.");

                pmtComtrol.AddOption("포켓볼빵 들어왔어?", "store", "yangf_ball");
                pmtComtrol.AddOption("게임은 어떻게 되어가?", "store", "yangf_game");
                pmtComtrol.AddOption("편의점을 나간다", "store", "yangf_out");
            }
            else
            {
                pmtComtrol.AddString("양아치", "어서오세요. 잼미니스톱입니다.");
                pmtComtrol.AddOption("포켓볼빵 있어요?", "store", "yang_ball");
                pmtComtrol.AddOption("무슨 게임해요?", "store", "yang_game");
                pmtComtrol.AddOption("저랑 친구 할래요?", "store", "yang_friend_ask");
            }
        }

        //점주
        else if (chaIdx == 2)
        {
            if (PlayerPrefs.GetInt("albaExp") == 0)
            {
                pmtComtrol.AddString("점주", "어서오세요! 쌈마트24입니다!");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "host_ball");
                pmtComtrol.AddOption("알바 자리 있어요?", "store", "host_alba");
                pmtComtrol.AddOption("편의점을 나간다", "store", "host_out");
            }
            else
            {
                matdongsanBuy = false;
                pmtComtrol.AddString("점주", "오 자네가 왔구만! 그래 무슨일인가?");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "hostf_ball");
                pmtComtrol.AddOption("알바하러 왔어요!", "store", "hostf_alba");
                pmtComtrol.AddOption("편의점을 나간다", "store", "hostf_out");
            }
        }

        //비실이
        else if (chaIdx == 3)
        {
            if (!PlayerPrefs.HasKey("bi_friend"))
            {
                PlayerPrefs.SetInt("bi_friend", 0);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.GetInt("bi_friend") == 0)
            {
                pmtComtrol.AddString("비실이", "어서오세요오~! 나인투식스입니다아~!");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "bi_ball");
                pmtComtrol.AddOption("나랑 친구할래?", "store", "bi_fried");
                pmtComtrol.AddOption("편의점을 나간다", "store", "bi_out");
            }
            else
            {
                pmtComtrol.AddString("비실이", "어서와아!");
                pmtComtrol.AddOption("포켓볼빵 있어?", "store", "bi_ball_2");
                pmtComtrol.AddOption("편의점을 나간다", "store", "bi_out");
            }
        }

        //열정맨
        else if (chaIdx == 4)
        {
            if (!PlayerPrefs.HasKey("yull_friend"))
            {
                PlayerPrefs.SetInt("yull_friend", 0);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.GetInt("yull_friend") == 0)
            {
                pmtComtrol.AddString("열정맨", "어서오세요! 진상25입니다!");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                pmtComtrol.AddOption("포켓볼빵 언제 들어와요?", "store", "y_when");
                pmtComtrol.AddOption("편의점을 나간다", "store", "y_out");
            }
            else
            {
                pmtComtrol.AddString("열정맨", "어서오세요! 진상25입니다!");
                pmtComtrol.AddString("열정맨", "간절한 손님이 왔네요!");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "yf_ball");
                pmtComtrol.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                pmtComtrol.AddOption("편의점을 나간다", "store", "yf_out");
            }
        }

        //박종업원
        else if (chaIdx == 6)
        {
            if (!PlayerPrefs.HasKey("park_friend"))
            {
                PlayerPrefs.SetInt("park_friend", 0);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.GetInt("park_friend") == 0)
            {
                pmtComtrol.AddString("박종업원", "어서오세유. 머박분식집이에유.");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "p_ball");
                pmtComtrol.AddOption("뭐 팔아요?", "store", "p_what");
                pmtComtrol.AddOption("편의점을 나간다", "store", "p_out");
            }
            else
            {
                //pmtComtrol.AddString("박종업원", "어서오세요! 진상25입니다!");
                //pmtComtrol.AddString("박종업원", "간절한 손님이 왔네요!");
                //pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "yf_ball");
                //pmtComtrol.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                pmtComtrol.AddOption("편의점을 나간다", "store", "yf_out");
            }
        }
        
        //나이롱
        else if (chaIdx == 7)
        {
            domiCoin.UpdateCurrentPrice();

            if (PlayerPrefs.GetInt("NylonDrawBbangCount") < 4)
            {
                if (!PlayerPrefs.HasKey("NylonInvested"))
                {
                    Nylon("ID_F_FIRST");
                }
                else
                {
                    if (PlayerPrefs.GetInt("NylonInvested") == 1)
                    {
                        Nylon("ID_F_SECOND_IFINVESTED");
                        
                    }
                    else
                    {
                        Nylon("ID_F_SECOND_IFNOTINVESTED");
                    }
                }
            }
            else
            {
                Nylon_f("ID_F_FRIEND_HELLO");
            }
        }
        
        //왕형 탕후루
        else if (chaIdx == 8)
        {
            tanghuruState = "";
            // Tanghuru("ID_T_OPEN_FRIEND"); //[진입점 2] <친구 이벤트> 친구가 되기 전 오후 12시에서 오후 6시 사이에 방문하면 여기로 진입됨.
            //
            if (PlayerPrefs.GetInt("tanghuru_friend", 0) == 1)
            {
                Tanghuru("ID_T_OPEN_AFTERFRIEND"); //[진입점 3] <친구가 된 후> 아무때나 가도 여기로 진입됨.
            }
            else
            {
                int hr = System.DateTime.Now.Hour;
                if(hr >= 12 && hr < 18) Tanghuru("ID_T_OPEN_FRIEND"); //[진입점 2] <친구 이벤트> 친구가 되기 전 오후 12시에서 오후 6시 사이에 방문하면 여기로 진입됨.
                else Tanghuru("ID_T_OPEN_STORE"); //[진입점 1] <친구 되기 전> 처음 만났을 때부터 친구가 되기 전까지. -> [연결점 1], [연결점 3]으로 연결.
            }
            
        }
    }

    public void SendtoSetting()
    {
#if UNITY_IPHONE
        OpenAppSetting();
        //Application.OpenURL("app-settings:");
#endif
    }

    public void StoreEvent(string code)
    {
        print("STORE EVENT : " + code);
        
        switch (code)
        {
            case "p_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박종업원", "분식집에서 빵을 왜 찾아유.");
                pmtComtrol.AddString("박종업원", "그런거 없어유.");
                pmtComtrol.AddString("훈이", "아 네..");
                pmtComtrol.AddOption("그러면 뭐 팔아요?", "store", "p_what");
                pmtComtrol.AddOption("편의점을 나간다", "store", "p_out");
                break;

            case "p_what":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박종업원", "분식집하면 떡볶이 아니겠슈.");
                pmtComtrol.AddString("박종업원", "떡볶이 레시피를 개발중인데\n한번 시식 해보실래유?");
                pmtComtrol.AddString("훈이", "(음.. 배가 고프긴 한 것 같은데..)");
                pmtComtrol.AddOption("네. 먹어볼게요.", "store", "p_yes");
                pmtComtrol.AddOption("아니요. 괜찮아요.", "store", "p_no");
                break;

            case "p_yes":
                dduk.StartDduk();
                break;

            case "p_again":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박종업원", "아이구. 제료가 방금 막 다 떨어졌어유.");
                pmtComtrol.AddString("훈이", "어쩔수 없죠. 다음에 또 들르면 들를게요.");
                pmtComtrol.AddString("박종업원", "다음에 언제든 또 오세유.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "p_good":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박종업원", "맛있게 먹었다니 기분이 참 좋군유.");
                pmtComtrol.AddString("박종업원", "다음에 언제든 또 오세유.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "p_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "괜찮아요. 다음에 또 들를게요.");
                pmtComtrol.AddString("박종업원", "섭섭하네유.");
                pmtComtrol.AddString("박종업원", "알았어유. 다음에 또 오세유.");
                storeOutAction = "다음에는 떡볶이를 먹어봐야겠다..";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "p_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박종업원", "다음에 또 오세유.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "girlf_none":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "이상하다..\n알림 설정이 꺼져있는거 아니야?");
                pmtComtrol.AddString("미소녀", "핸드폰 설정에서 알림이 꺼저있는 건 아닌지 확인해봐!");
                pmtComtrol.AddNextAction("store", "notiSetting");
                break;

            case "notiSetting":
#if UNITY_IPHONE
                Msg.SetMsg("아이폰 설정으로 이동할까요?\n[알림] 탭에서 [포켓볼빵 더게임] 알림 설정을 확인해주세요.", 2, "SendtoSetting");
#endif
                pmtComtrol.AddString("훈이", "고마워!");
                pmtComtrol.AddString("미소녀", "후훗 천만에 말씀.");
                pmtComtrol.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                break;


            case "hostf_ball":
                if (!matdongsanBuy)
                {
                    pmtComtrol.Reset();
                    pmtComtrol.imageMode = true;
                    pmtComtrol.AddString("점주", "그럼. 자네를 위해서라면 당연히 있지!");
                    pmtComtrol.AddString("점주", "하지만 알지? 맛동산 한 박스를 같이 사야해!");
                    pmtComtrol.AddString("훈이", "그래서 얼마라구요?");
                    pmtComtrol.AddString("점주", "맛동산은 만원이야. 어떻게 할 텐가?");
                    pmtComtrol.AddOption("산다.", "store", "hostf_buy_check");
                    pmtComtrol.AddOption("안 산다.", "store", "hostf_notbuy");
                }
                else
                {
                    pmtComtrol.Reset();
                    pmtComtrol.imageMode = true;
                    pmtComtrol.AddString("점주", "자네 조금 아까 샀잖아?");
                    pmtComtrol.AddString("점주", "다음에 또 구해둘 테니 언제든 들르라구!");
                    pmtComtrol.AddString("훈이", "네 알겠어요.");
                    pmtComtrol.AddOption("알바를 한다.", "store", "hostf_alba");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "hostf_out");
                }

                break;

            case "hostf_buy_check":
                Msg.SetMsg("맛동산 1상자와 포켓볼빵을 10,000원에 구매합니다.", 2, "matdongsanf");
                break;

            case "hostf_notbuy":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "안 살래요..");
                pmtComtrol.AddString("점주", "그래. 난 강요하지 않았어!");
                storeOutAction = "끼워팔기라니 너무한걸..";
                pmtComtrol.AddOption("알바자리 있어요?", "store", "hostf_alba");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "hostf_buy_noMoney":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "뭐야! 자네 돈이 부족하잖아?");
                pmtComtrol.AddString("점주", "용돈이 필요하면 알바를 하라구!");
                pmtComtrol.AddString("훈이", "네 그럴게요..");
                storeOutAction = "요즘 물가가 많이 올랐다..";
                pmtComtrol.AddOption("지금 알바 할게요!", "store", "hostf_alba");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "hostf_out");
                break;

            case "hostf_buy":
                gameObject.GetComponent<Heart_Control>().UpdateMoney(-10000);
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "뭔가 손해보는 것 같지만..");
                pmtComtrol.AddString("점주", "자 여기있어.");
                pmtComtrol.AddString("맛동산", "맛동산 한박스를 겟했다.");
                AddBBang(1);
                pmtComtrol.AddString("훈이", "감사합니다..");
                storeOutAction = "손해보는 것 같지만 기분은 좋다.";
                matdongsanBuy = true;

                if (GetComponent<Heart_Control>().heartCount >= 1)
                {
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "hostf_out");
                    pmtComtrol.AddOption("알바를 한다.", "store", "hostf_alba");
                }
                else
                {
                    pmtComtrol.AddNextAction("store", "hostf_out");
                }

                PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
                PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") + 1);
                PlayerPrefs.Save();

                showroom.UpdateBbangShow();
                break;

            case "hostf_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "다음에 또 보자구!");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "hostf_alba":
                if (GetComponent<Heart_Control>().heartCount >= 1)
                {
                    //GetComponent<Heart_Control>().SetHeart(-1);
                }
                else
                {
                    pmtComtrol.Reset();
                    pmtComtrol.imageMode = true;
                    pmtComtrol.AddString("훈이", "지금은 좀 피곤한데..");
                    pmtComtrol.AddString("훈이", "알바는 다음에 하는게 좋겠다.");

                    if (!matdongsanBuy & (PlayerPrefs.GetInt("money") >= 10000))
                    {
                        pmtComtrol.AddOption("포켓볼빵 살게요! 아직 있죠?", "store", "hostf_ball");
                        pmtComtrol.AddOption("편의점을 나간다.", "store", "hostf_out");
                    }
                    else
                    {
                        pmtComtrol.AddNextAction("store", "hostf_out");
                    }

                    return;
                }

                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "잘 됐다! 일손이 많이 부족했었는데!");
                pmtComtrol.AddOption("바로 시작할게요.", "store", "albaf_start");
                pmtComtrol.AddOption("어떻게 하면 되나요?", "store", "albaf_how");
                break;

            case "albaf_start":
                if (GetComponent<Heart_Control>().heartCount >= 1) GetComponent<Heart_Control>().SetHeart(-1);
                alba.StartAlba(true);
                break;

            case "albaf_end":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;

                storeOutAction = "휴.. 편의점 알바도 쉬운게 아니구나..";
                pmtComtrol.AddOption("알바 더할게요.", "store", "hostf_alba");
                if (!matdongsanBuy & (PlayerPrefs.GetInt("money") >= 10000))
                    pmtComtrol.AddOption("이제 포켓볼빵 살게요! 아직 있죠?", "store", "hostf_ball");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "hostf_out");
                break;

            case "albaf_how":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;

                pmtComtrol.AddString("점주", "손님한테 포켓볼빵 없다고만 얘기하면 돼!");
                pmtComtrol.AddString("점주", "만약 언제 들어오는지 묻는다면 모른다고 답하면 돼!");
                pmtComtrol.AddString("점주", "참 별거 없네요.");
                pmtComtrol.AddString("점주", "허허 그래도 상당한 어려운 일이라구!");
                pmtComtrol.AddString("점주", "준비 됐나 이제?");
                pmtComtrol.AddOption("네 시작할게요.", "store", "albaf_start");
                pmtComtrol.AddOption("다시 설명해주세요!", "store", "albaf_how");
                break;

            case "note":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("박법학박사", "포켓볼빵은 품절되었단다!\n자 다음 편의점으로 당장 이동하렴!");
                pmtComtrol.AddString("훈이", "품절인가보다..");
                pmtComtrol.AddNextAction("store", "bbobgiStore");
                break;

            case "bbobgi":
                if (currentLocation == "store")
                {
                    currentLocation = "bbobgi";
                    newStore.GetComponent<StoreControl>().StartBbobgi();
                }

                break;

            case "bbobgiStore":
                pmtComtrol.Reset();
                pmtComtrol.AddOption("뽑기 기계를 확인한다.", "store", "bbobgi");
                pmtComtrol.AddOption("다른 편의점으로 간다.", "main", "store_next");
                pmtComtrol.AddOption("집으로 돌아간다.", "main", "go_home");
                break;

            case "alba_start":
                alba.StartAlba();
                break;

            case "alba_how":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;

                pmtComtrol.AddString("점주", "별거 없어! 요새 사람들이 다들 포켓볼빵만 찾으니까");
                pmtComtrol.AddString("점주", "포켓볼빵 없다고만 얘기하면 돼!");
                pmtComtrol.AddString("훈이", "그렇게만 얘기하면 되나요?");
                pmtComtrol.AddString("점주", "그래 그게 다야!");
                pmtComtrol.AddString("점주", "만약 언제 들어오는지 묻는다면 모른다고 답하면 돼!");
                pmtComtrol.AddString("점주", "별거 없네요.");
                pmtComtrol.AddString("점주", "허허 그래도 상당한 일이라구!");
                pmtComtrol.AddString("점주", "준비 됐나 이제?");
                pmtComtrol.AddOption("네 시작할게요.", "store", "alba_start");
                pmtComtrol.AddOption("다시 설명해주세요!", "store", "alba_how");
                break;

            case "y_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "요즘 그 핫한 포켓볼빵..");
                pmtComtrol.AddString("열정맨", "포켓볼빵에 대해 얼마나 간절하십니까?");

                pmtComtrol.AddOption("엄청 간절해요!", "store", "y_yes");
                pmtComtrol.AddOption("간절해요!", "store", "y_no");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "그래요 그렇겠죠.");
                pmtComtrol.AddString("열정맨", "포켓볼빵을.. 왜 드시고 싶습니까?");

                pmtComtrol.AddOption("유행이라고 하니까요!", "store", "y_no");
                pmtComtrol.AddOption("추억의 맛이 생각나서요!", "store", "y_yes2");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "옛날에 먹던 그 추억의 맛이..");
                pmtComtrol.AddString("훈이", "너무도 그리워서 계속 찾게 되더라구요..");

                pmtComtrol.AddString("열정맨", "마지막으로 하고싶은 말 없습니까?");

                pmtComtrol.AddString("훈이", "그리고 스티커를 모으는 건 마치..");

                pmtComtrol.AddOption("추억을 모으는 것 같아요.", "store", "y_yes3");
                pmtComtrol.AddOption("중독적이에요.", "store", "y_no");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes3":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "스티커를 모으는 건 마치..");
                pmtComtrol.AddString("훈이", "제 어릴적 추억을 모으는 것 같아요.");
                pmtComtrol.AddString("훈이", "하나 둘 스티커를 모으다보면..");
                pmtComtrol.AddString("훈이", "빵 하나에도 기뻐하던 어린 아이였던 제 모습이 떠올라요.");
                pmtComtrol.AddString("훈이", "그래서 포켓볼빵을 다시 맛보고 싶어요.");

                pmtComtrol.AddString("열정맨", "...");
                pmtComtrol.AddString("열정맨", "합격!!");
                pmtComtrol.AddString("열정맨", "간절함을 잘 느껴졌어요. 빵을 드릴게요!!");
                pmtComtrol.AddString("열정맨", "하지만 우리 매장에는 메이풀빵만 들어와요.");

                pmtComtrol.AddString("훈이", "메이풀빵이요..?");
                pmtComtrol.AddString("열정맨", "네 추억의 과자 풀빵이죠.");

                AddBBang(1, "maple");
                storeOutAction = "면접을 합격한 기분이다.";
                PlayerPrefs.SetInt("yull_friend", 1);
                PlayerPrefs.Save();

                pmtComtrol.AddString("열정맨", "다음에도 지나갈때 또 들러요.");
                pmtComtrol.AddString("훈이", "네. 감사합니다!");


                pmtComtrol.AddOption("편의점을 나간다.", "store", "yf_out");
                break;

            case "y_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "간절함이 없네요!!");
                pmtComtrol.AddString("열정맨", "당신에게 판매할 빵은 없어요.");

                storeOutAction = "아니. 빵을 사는데 면접을 본다고..?!";

                pmtComtrol.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_ask":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "저희 가게에는 포켓볼빵이 들어오지 않아요.");
                pmtComtrol.AddString("훈이", "아.. 네..");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                pmtComtrol.AddOption("편의점을 나간다", "store", "y_out");
                break;

            case "y_when":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "저희 가게에는 포켓볼빵이 들어오지 않아요.");
                pmtComtrol.AddString("훈이", "아.. 네..");
                pmtComtrol.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                pmtComtrol.AddOption("편의점을 나간다", "store", "y_out");
                break;

            case "y_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "안녕하가세요.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yf_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "간절한 손님이라면 언제든 또 오세요!");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yf_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "저희 가게에는 포켓볼빵이 안 들어와요.");
                pmtComtrol.AddString("열정맨", "대신 풀빵이 있지요.");
                pmtComtrol.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                pmtComtrol.AddOption("편의점을 나간다", "store", "yf_out");
                break;

            case "yf_maple":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("열정맨", "아, 추억의 풀빵이요! 재고 있나 확인해볼게요!");

                var rnd00 = Random.Range(0, 4);
                if (rnd00 == 0)
                {
                    pmtComtrol.AddString("열정맨", "아이고.. 오늘은 빵이 다 떨어졌네요.");
                    pmtComtrol.AddString("열정맨", "다음에 다시 와야겠어요.");
                    pmtComtrol.AddString("훈이", "네, 다음에 다시 들를게요!");
                    pmtComtrol.AddNextAction("store", "yf_out");
                }
                else
                {
                    pmtComtrol.AddString("열정맨", "여기 하나 있네요!");
                    AddBBang(1, "maple");
                    storeOutAction = "추억의 메이풀빵을 얻었다!";
                    pmtComtrol.AddString("훈이", "네! 감사합니다!");
                    pmtComtrol.AddNextAction("store", "yf_out");
                }

                break;

            case "host_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "그럼. 당연히 있지!");
                pmtComtrol.AddString("점주", "하지만 맛동산 한 박스를 같이 사야해!");
                pmtComtrol.AddString("훈이", "네? 맛동산을 사야 포켓볼빵을 살 수 있다고요?");
                pmtComtrol.AddString("훈이", "얼마인데요?");
                pmtComtrol.AddString("점주", "맛동산은 만원이야. 어떻게 할 텐가?");
                pmtComtrol.AddOption("산다.", "store", "host_buy_check");
                pmtComtrol.AddOption("안 산다.", "store", "host_notbuy");
                break;

            case "host_buy_check":
                Msg.SetMsg("맛동산 1상자와 포켓볼빵을 10,000원에 구매합니다.", 2, "matdongsan");
                break;

            case "host_buy_noMoney":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "뭐야! 자네 돈이 부족하잖아?");
                pmtComtrol.AddString("점주", "용돈이 필요하면 알바를 하러 오라구!");
                pmtComtrol.AddString("훈이", "네 그럴게요..");
                storeOutAction = "요즘 물가가 많이 올랐다..";
                pmtComtrol.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "host_buy":
                gameObject.GetComponent<Heart_Control>().UpdateMoney(-10000);
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "뭔가 손해보는 것 같지만..");
                pmtComtrol.AddString("점주", "자 여기있어.");
                pmtComtrol.AddString("맛동산", "맛동산 한박스를 겟했다.");
                AddBBang(1);
                pmtComtrol.AddString("훈이", "감사합니다..");
                storeOutAction = "손해보는 것 같지만 기분은 좋다.";
                pmtComtrol.AddOption("편의점을 나간다.", "store", "host_out");

                PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
                PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") + 1);
                PlayerPrefs.Save();

                showroom.UpdateBbangShow();

                break;

            case "host_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "언제든 또 오라구!");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "host_notbuy":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "안 살래요..");
                pmtComtrol.AddString("점주", "그래. 난 강요하지 않았어!");
                storeOutAction = "끼워팔기라니 너무한걸..";
                pmtComtrol.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "host_alba":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                if (PlayerPrefs.GetInt("albaExp") > 0)
                {
                    pmtComtrol.AddString("점주", "당연히 있지!");
                    pmtComtrol.AddString("점주", "이렇게 불쑥 찾아오지 말고 앱으로 신청하고 오라구!");
                    pmtComtrol.AddString("점주", "알밥천국 앱 알지?");
                    pmtComtrol.AddOption("네 알아요.", "store", "host_alba_ok");
                    pmtComtrol.AddOption("잘 모르는데요.", "store", "host_alba_dontknow");
                }
                else
                {
                    pmtComtrol.AddString("점주", "자네 알바 자리 관심있나?");
                    pmtComtrol.AddString("훈이", "네..");
                    pmtComtrol.AddString("점주", "마침 일손이 필요했는데 잘됐어!");
                    pmtComtrol.AddString("점주", "요새 포켓볼빵을 찾는 사람이 워낙 많아서 말이야!");
                    pmtComtrol.AddString("점주", "핸드폰에 알밥천국 앱 있지?");
                    pmtComtrol.AddOption("네 있어요.", "store", "host_alba_ok");
                    pmtComtrol.AddOption("그게 뭔데요?", "store", "host_alba_dontknow");
                    PlayerPrefs.SetInt("albaExp", 1);
                }

                break;

            case "host_alba_ok":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "그래. 알밥천국. 그 앱으로 신청할 수도 있고 다음에 지나가다 들러도 되고!");
                pmtComtrol.AddString("훈이", "네. 알겠어요.");
                pmtComtrol.AddString("점주", "언제든 또 오라구!");
                storeOutAction = "편의점 알바로 용돈을 좀 모아볼까..?";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "host_alba_dontknow":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("점주", "젊은 사람이 그것도 모르나!");
                pmtComtrol.AddString("점주", "핸드폰을 보면 '알밥천국'이라는 앱이 있을거야.");
                pmtComtrol.AddString("점주", "그 앱으로 알바를 신청하고 나한테 찾아오면 돼.");
                pmtComtrol.AddString("점주", "그러면 알바를 시켜주지.");
                pmtComtrol.AddString("훈이", "네. 알겠어요.");
                pmtComtrol.AddString("점주", "그래 이해가 빠르구먼!");
                storeOutAction = "편의점 알바로 용돈을 좀 모아볼까..?";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "girl_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;

                if (Random.Range(0, 10) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        pmtComtrol.AddString("미소녀", "포켓볼빵 1개 있어요.");
                        AddBBang(1);
                        storeOutAction = "기분이 너무 좋다.";
                        pmtComtrol.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                    }
                    else
                    {
                        pmtComtrol.AddString("미소녀", "포켓볼빵 2개 있어요.");
                        AddBBang(2);
                        storeOutAction = "기분이 너무 너무 좋다.";
                        pmtComtrol.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                    }
                }
                else
                {
                    pmtComtrol.AddString("미소녀", "포켓볼빵 없어요.");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                }

                break;
            case "girl_boy":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "남자친구 있어요.");
                pmtComtrol.AddOption("편의점을 나간다.", "main", "store_out");
                storeOutAction = "(난 괜찮아....)";
                break;
            case "girl_phone":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "네..? 왜요..?");
                pmtComtrol.AddOption("포켓몬 빵 들어오면 알려주세요.", "store", "girl_call");
                pmtComtrol.AddOption("제 이상형이에요.", "store", "girl_type");
                pmtComtrol.AddOption("저랑 친구 할래요?", "store", "girl_friend");
                break;
            case "girl_call":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "제가 왜 그래야 하죠?");
                pmtComtrol.AddString("미소녀", "귀찮게 하지말고 나가세요.");
                storeOutAction = "(포켓볼 빵을 구하는 것은 어렵구나.)";
                pmtComtrol.AddOption("편의점을 나간다.", "main", "store_out");
                break;
            case "girl_type":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "미안한데 제 스타일 아니에요.");
                pmtComtrol.AddString("미소녀", "볼 일 다 보셨으면 나가주세요.");
                storeOutAction = "(눈에서 땀이 흐른다.)";
                pmtComtrol.AddOption("편의점을 나간다.", "main", "store_out");
                break;
            case "girl_friend":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "응 그래.");
                pmtComtrol.AddString("훈이", "(소녀와 친구가 되었다.)");
                PlayerPrefs.SetInt("girl_friend", 1);
                PlayerPrefs.Save();
                pmtComtrol.AddOption("편의점을 나간다.", "store", "girl_out");
                break;
            case "girl_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "또 와.");
                storeOutAction = "다음에 만나면 빵이 언제 들어오나 물어봐야 겠다.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;
            case "girl_noBbang":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "또 오세요.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;
            case "girlf_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;

                if (Random.Range(0, 6) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        pmtComtrol.AddString("미소녀", "응, 포켓볼빵 1개 있어.");
                        AddBBang(1);
                        storeOutAction = "기분이 너무 좋다.";
                        pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                    }
                    else
                    {
                        pmtComtrol.AddString("미소녀", "응, 포켓볼빵 2개 있어.");
                        AddBBang(2);
                        storeOutAction = "기분이 너무 너무 좋다.";
                        pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                    }
                }
                else
                {
                    pmtComtrol.AddString("미소녀", "포켓볼빵 없어.");
                    pmtComtrol.AddOption("언제 들어와?.", "store", "girlf_when");
                    pmtComtrol.AddOption("거짓말.", "store", "girlf_lie");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }

                break;
            case "girlf_noBbang":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "다음에 또 와.");
                pmtComtrol.AddNextAction("main", "store_out");
                storeOutAction = "포켓볼빵은 참 구하기 어렵다.";
                break;
            case "girlf_when":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "잘 모르겠는데.");
                pmtComtrol.AddOption("그러면 들어올 때 알려줄래?", "store", "girlf_when2");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                break;
            case "girlf_when2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "그래. 들어오면 문자 할까?");
                pmtComtrol.AddOption("응 부탁해.", "store", "girlf_yes");
                pmtComtrol.AddOption("아니 괜찮아.", "store", "girlf_no");
                break;
            case "girlf_yes":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "알겠어. 빵 들어오면 문자 할게.");
                pmtComtrol.AddString("훈이", "정말 고마워!");
                pmtComtrol.AddString("미소녀", "응. 다음에 또 봐.");
                storeOutAction = "좋은 친구다.\n연락이 오면 바로 달려와야겠다.";
                pmtComtrol.AddNextAction("main", "store_out");
                AddAlarm();
                break;
            case "girlf_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("미소녀", "그래. 뭐 더 필요한 거 있어?");
                pmtComtrol.AddString("훈이", "아니 없어.");
                pmtComtrol.AddString("미소녀", "응. 다음에 또 봐.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;
            case "girlf_lie":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                if (Random.Range(0, 2) == 0)
                {
                    pmtComtrol.AddString("미소녀", "헤헷 들켰네. 자 여기.");
                    AddBBang(1);
                    pmtComtrol.AddString("훈이", "고마워.");
                    pmtComtrol.AddOption("언제 또 들어와?", "store", "girlf_when");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else
                {
                    pmtComtrol.AddString("미소녀", "무슨 소리 하는거야?");
                    pmtComtrol.AddString("미소녀", "귀찮게 하지말고 빨리 나가.");
                    pmtComtrol.AddNextAction("main", "store_out");
                }

                break;

            case "yang_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "거기 없으면 없는거예요.");
                pmtComtrol.AddOption("편의점을 둘러본다.", "store", "yang_find_1");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_exit":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "안녕히가세요.");
                storeOutAction = "무슨 게임을 하는지 궁금하다.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yang_exit2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "안녕히가세요.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yang_find_1":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                if (Random.Range(0, 10) == 0)
                {
                    pmtComtrol.AddString("훈이", "포켓볼빵 1개가 있다!");
                    AddBBang(1);
                    storeOutAction = "기분이 너무 좋다.";
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit2");
                    pmtComtrol.AddOption("빵을 더 찾아본다.", "store", "yang_find_2");
                }
                else
                {
                    pmtComtrol.AddString("훈이", "빵을 찾아봤지만 보이지 않는다.");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                    pmtComtrol.AddOption("빵을 더 찾아본다.", "store", "yang_find_2");
                }

                break;

            case "yang_find_2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                if (Random.Range(0, 10) == 0)
                {
                    pmtComtrol.AddString("훈이", "구석진 곳까지 잘 찾아봐야겠다.");
                    pmtComtrol.AddString("훈이", "구석진 곳에서 포켓볼빵 1개를 발견했다!");
                    AddBBang(1);
                    storeOutAction = "좋았어! 운이 정말 좋구나!";
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit2");
                }
                else
                {
                    pmtComtrol.AddString("훈이", "구석진 곳까지 잘 찾아봐야겠다.");
                    pmtComtrol.AddString("훈이", "열심히 찾아봤지만 보이지 않는다.");
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                    storeOutAction = "부지런한 사람들이 많나보다..";
                }

                break;

            case "yang_game":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "[정자 키우기]라고 요즘 핫한 게임 있어요.");
                pmtComtrol.AddOption("들어보지 못 한 게임이네요.", "store", "yang_game_no");
                pmtComtrol.AddOption("정자를 키우는 게임이에요?", "store", "yang_game_jungja");
                pmtComtrol.AddOption("요즘 완전 핫한 게임이잖아요!", "store", "yang_game_hot");
                break;

            case "yang_game_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "무슨 그런 게임이 다 있어요?");
                pmtComtrol.AddString("양아치", "알지도 못하면서 왜 물어봐요?");
                pmtComtrol.AddString("양아치", "귀찮게 하지 말고 나가세요.");
                pmtComtrol.AddString("양아치", "[정자 키우기] 해야돼요.");
                pmtComtrol.AddOption("밖으로 나간다.", "store", "yang_exit");
                break;

            case "yang_game_jungja":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "헐 맞아요! 어떻게 아세요?");
                pmtComtrol.AddString("양아치", "4억마리 정자와 펼치는 레이싱 게임이죠.");
                pmtComtrol.AddString("양아치", "1등으로 난자를 만나야 승리하는 게임이에요.");
                pmtComtrol.AddOption("재미있어 보이네요.", "store", "yang_game_fun");
                pmtComtrol.AddOption("이상한 게임 아니에요?", "store", "yang_game_weird");
                break;

            case "yang_game_fun":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "맞아요 굉장히 재미있는 게임이죠.");
                pmtComtrol.AddOption("이상한 게임 아니에요?", "store", "yang_game_fun2");
                break;

            case "yang_game_fun2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "이상해 보일지 몰라도 교육용 컨텐츠랍니다.");
                pmtComtrol.AddOption("꼭 한번 플레이해볼게요.", "store", "yang_game_play");
                break;

            case "yang_game_weird":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "이상한 게임이라뇨!");
                pmtComtrol.AddString("양아치", "성에 대한 다양한 상식을 담은 잡학사전도 있다구요.");
                pmtComtrol.AddString("양아치", "이래보여도 교육용 컨텐츠라구요!");
                pmtComtrol.AddOption("재미있을 것 같아 보이네요.", "store", "yang_game_weird2");
                break;

            case "yang_game_weird2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "맞아요 굉장히 재미있는 게임이죠.");
                pmtComtrol.AddOption("꼭 한번 플레이해볼게요.", "store", "yang_game_play");
                break;

            case "yang_game_play":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "말이 생각보다 잘 통하시는 분이네요.");
                pmtComtrol.AddString("양아치", "우리 친구 할까요?");
                pmtComtrol.AddOption("좋아요.", "store", "yang_friend_yes");
                pmtComtrol.AddOption("아니요.", "store", "yang_friend_no");
                break;

            case "yang_friend_yes":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "그래 반가워.");
                pmtComtrol.AddOption("혹시 포켓볼빵은 없을까?", "store", "yang_friend_yes2");
                pmtComtrol.AddOption("반말은 좀 그런것 같은데.", "store", "yang_friend_no");
                break;

            case "yang_friend_yes2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "아쉽지만 오늘은 포켓볼빵이 없어.");
                pmtComtrol.AddString("양아치", "다음에 들어오면 내가 몇 개 챙겨둘게.");
                pmtComtrol.AddOption("고마워.", "store", "yang_friend_thanks");
                pmtComtrol.AddOption("필요 없어.", "store", "yang_friend_no");
                break;

            case "yang_friend_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "그래요.");
                pmtComtrol.AddString("양아치", "볼 일 다 봤으면 나가주세요.");
                pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_friend_thanks":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                PlayerPrefs.SetInt("yang_friend", 1);
                PlayerPrefs.Save();
                pmtComtrol.AddString("훈이", "정말 고마워!");
                pmtComtrol.AddString("양아치", "그래. 더 볼일 있어?");
                pmtComtrol.AddOption("다음에 다시 올게.", "store", "yang_friend_exit");
                pmtComtrol.AddOption("아니 없어.", "store", "yang_friend_exit");
                break;

            case "yang_friend_exit":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "그래 조심히 가고 또 와.");
                storeOutAction = "좋은 친구가 생겼다.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yang_friend_ask":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "누구신데요. 저 아세요?");
                pmtComtrol.AddOption("알아요.", "store", "yang_friend_ask_yes");
                pmtComtrol.AddOption("잘 몰라요.", "store", "yang_friend_ask_no");
                break;

            case "yang_friend_ask_yes":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "누구신데요? 난 모르겠는데.");
                pmtComtrol.AddString("양아치", "공통의 관심사도 없는데 어떻게 친구를 해요.");
                pmtComtrol.AddString("양아치", "귀찮게 하지 말고 나가세요.");

                pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_friend_ask_no":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "서로 아는게 없는데 어떻게 친구를 해요.");
                pmtComtrol.AddString("양아치", "귀찮게 하지 말고 나가세요.");

                pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_game_hot":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("훈이", "요즘 완전 인기 많은 게임이잖아요?");
                pmtComtrol.AddString("양아치", "하는 사람 아무도 없는 망겜이에요.");
                pmtComtrol.AddString("양아치", "아는 척 하지 마세요.");
                pmtComtrol.AddString("훈이", "...");

                pmtComtrol.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yangf_ball":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                if (Random.Range(0, 2) == 0)
                {
                    pmtComtrol.AddString("양아치", "아 응. 빵 하나 내가 빼뒀어.");
                    AddBBang(1);
                    pmtComtrol.AddString("훈이", "고맙다 친구야!");
                    pmtComtrol.AddString("양아치", "뭐 이런걸 갖고.");
                    storeOutAction = "좋은 친구를 두니 좋다.";
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yangf_out");
                }
                else
                {
                    pmtComtrol.AddString("양아치", "아직 안 들어왔어.");
                    pmtComtrol.AddString("양아치", "들어오면 몇개 빼둘게.");
                    pmtComtrol.AddString("훈이", "그래 고맙다!");
                    storeOutAction = "다음에 또 들러야 겠다.";
                    pmtComtrol.AddOption("편의점을 나간다.", "store", "yangf_out");
                }

                break;

            case "yangf_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "또 와.");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "yangf_game":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "지금 열심히 정자를 키우고있어.");
                pmtComtrol.AddString("양아치", "너도 해봐. 앱스토어에 있어.");
                pmtComtrol.AddOption("게임 이름이 뭐였지?", "store", "yangf_game2");
                pmtComtrol.AddOption("나도 해봤어 그 게임", "store", "yangf_game3");
                break;

            case "yangf_game2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "게임 이름은 [정자키우기]야.");
                pmtComtrol.AddString("훈이", "응. 꼭 해볼게.");
                pmtComtrol.AddOption("혹시 빵 들어왔어?", "store", "yangf_ball");
                break;

            case "yangf_game3":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("양아치", "[정자키우기] 너무 재밌지 않아?");
                pmtComtrol.AddString("양아치", "엔딩이 굉장히 감동적이라던데.");
                pmtComtrol.AddString("훈이", "응. 중독성이 있는 게임이더라.");
                pmtComtrol.AddString("양아치", "맞아. 이런 게임 처음이야.");
                pmtComtrol.AddOption("그건 그렇고.. 혹시 빵 들어왔어?", "store", "yangf_ball");
                break;


            // 비실이
            case "bi_ball":
                if (Random.Range(0, 2) == 0)
                {
                    pmtComtrol.Reset();
                    pmtComtrol.imageMode = true;
                    pmtComtrol.AddString("비실이", "오늘 들어온 빵은 다 나갔어요!");
                    pmtComtrol.AddString("훈이", "그렇군요. 아쉽네요.");
                    pmtComtrol.AddString("훈이", "그러면 다음에 또 올게요.");
                    pmtComtrol.AddString("비실이", "다음에 또 오세요우~");
                    storeOutAction = "말투가 참 이상하다. 친해지면 재밌을것 같다.";
                    pmtComtrol.AddNextAction("main", "store_out");
                }
                else
                {
                    pmtComtrol.Reset();
                    pmtComtrol.imageMode = true;
                    pmtComtrol.AddString("비실이", "저랑 가위바위보 해서 이기시면 빵 하나를 드릴게요오~");
                    pmtComtrol.AddString("훈이", "아 진짜요?");
                    pmtComtrol.AddString("비실이", "그럼 진짜죠우~");
                    pmtComtrol.AddString("비실이", "준비 됐나요우~");
                    pmtComtrol.AddOption("가위를 낸다.", "store", "bi_game");
                    pmtComtrol.AddOption("바위를 낸다.", "store", "bi_game");
                    pmtComtrol.AddOption("보를 낸다.", "store", "bi_game");
                }

                break;
            case "bi_game":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                var rnd = Random.Range(0, 3);
                switch (rnd)
                {
                    case 0:
                        pmtComtrol.AddString("비실이", "앗 비겼다아!!");
                        pmtComtrol.AddString("훈이", "한판 더!!");
                        pmtComtrol.AddString("비실이", "준비 됐나요우~");
                        pmtComtrol.AddOption("가위를 낸다.", "store", "bi_game");
                        pmtComtrol.AddOption("바위를 낸다.", "store", "bi_game");
                        pmtComtrol.AddOption("보를 낸다.", "store", "bi_game");
                        break;
                    case 1:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", "이겼다!!!!");
                        pmtComtrol.AddString("비실이", "좋아요우 졌으니까 약속대로 포켓볼빵 하나 드릴게요우.");
                        AddBBang(1);
                        pmtComtrol.AddString("훈이", "오예 신난다!");
                        storeOutAction = "신난다!! 저 알바와 친해지면 재밌을것 같다.";
                        pmtComtrol.AddNextAction("store", "bi_out");
                        break;
                    default:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", "졌다......");
                        pmtComtrol.AddString("비실이", "좋은 승부였어요우.");
                        pmtComtrol.AddString("훈이", "분하다.. 다음에 한판 더 해요.");
                        storeOutAction = "정말 이상한 알바다... 그치만 친해지면 재밌을 것 같다.";
                        pmtComtrol.AddNextAction("store", "bi_out");
                        break;
                }

                break;

            case "bi_out":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "다음에 또 오세요우!");
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "bi_fried":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "...!!");
                pmtComtrol.AddString("비실이", "뭐라고 했나요우 방금?");
                pmtComtrol.AddString("훈이", "친구 하자구요.");
                pmtComtrol.AddString("비실이", "...!!!!");
                pmtComtrol.AddString("비실이", "정말인가요오오!!!!");
                pmtComtrol.AddOption("네!", "store", "bi_yes");
                pmtComtrol.AddOption("농담이에요.", "store", "bi_out");
                storeOutAction = "참 이상한 알바다....";
                break;

            case "bi_yes":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                storeOutAction = "재미있는 친구가 생겼다!";
                pmtComtrol.AddString("비실이", "너무 죠와.");
                pmtComtrol.AddString("비실이", "기념으로 빵 하나 줄겡.");
                AddBBang(1);
                pmtComtrol.AddString("훈이", "고마워 친구야!");
                pmtComtrol.AddString("비실이", "언제든 또 놀러왕.");
                pmtComtrol.AddNextAction("main", "store_out");
                PlayerPrefs.SetInt("bi_friend", 1);
                PlayerPrefs.Save();
                break;

            case "bi_ball_2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "자. 게임을 시작해볼까?");
                pmtComtrol.AddString("비실이", "지금 포켓볼빵 1개를 갖고 가거나");
                pmtComtrol.AddString("비실이", "가위바위보에서 이기면 포켓볼빵 2개를 줄게.");
                pmtComtrol.AddString("비실이", "대신 지면 아무것도 못 가져가는거얌!");
                pmtComtrol.AddString("비실이", "어떻게 할래?");
                pmtComtrol.AddOption("좋아. 게임을 시작하자.", "store", "bif_game");
                pmtComtrol.AddOption("하나만 받고 끝낼래.", "store", "bif_nogame");
                break;

            case "bif_game":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "좋아. 준비되면 시작해!");
                pmtComtrol.AddOption("가위를 낸다.", "store", "bif_game_2");
                pmtComtrol.AddOption("바위를 낸다.", "store", "bif_game_2");
                pmtComtrol.AddOption("보를 낸다.", "store", "bif_game_2");
                break;

            case "bif_nogame":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "칫 시시하긴.");
                pmtComtrol.AddString("비실이", "언제든 또 오라구!");
                AddBBang(1);
                storeOutAction = "도박은 역시 좋지 않아.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "bif_game_2":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                var rnd2 = Random.Range(0, 3);
                switch (rnd2)
                {
                    case 0:
                        pmtComtrol.AddString("비실이", "앗 비겼다!!");
                        pmtComtrol.AddString("훈이", "한판 더!!");
                        pmtComtrol.AddOption("가위를 낸다.", "store", "bif_game_2");
                        pmtComtrol.AddOption("바위를 낸다.", "store", "bif_game_2");
                        pmtComtrol.AddOption("보를 낸다.", "store", "bif_game_2");
                        break;
                    case 1:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", "이겼다!!!!");
                        pmtComtrol.AddString("비실이", "좋아 그러면 두번째 게임을 시작할까?");
                        pmtComtrol.AddString("비실이", "가위바위보를 한번 더 이기면 빵 4개를 줄게.");
                        pmtComtrol.AddString("비실이", "대신 지게되면 다 꽝이야.");
                        pmtComtrol.AddString("훈이", "흠.. 어떻게 하는 게 좋으려나..");

                        pmtComtrol.AddOption("두 번째 게임을 시작한다.", "store", "bif_game_3_start");
                        pmtComtrol.AddOption("빵 2개를 받고 끝낸다.", "store", "bif_finish_game");

                        score = 2;
                        break;
                    default:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", "졌다......");
                        pmtComtrol.AddString("비실이", "좋은 승부였어.");
                        pmtComtrol.AddString("훈이", "분하다!! 다음에 한판 더 해!");
                        pmtComtrol.AddString("비실이", "좋아. 언제든 또 오라구!");
                        storeOutAction = "졌지만 좋은 승부였다..";
                        pmtComtrol.AddNextAction("main", "store_out");
                        break;
                }

                break;

            case "bif_endgame":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "언제든 또 오라구!");
                storeOutAction = "도박은 역시 좋지 않아.";
                break;

            case "bif_finish_game":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "좋은 승부였어!");
                AddBBang(score);
                pmtComtrol.AddString("훈이", "오예 신난다!");
                pmtComtrol.AddString("비실이", "언제든 또 오라구!");
                storeOutAction = "빵을 많이 얻었다! 역시 친구가 좋다!";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "bif_game_3_start":
                score += 2;
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                pmtComtrol.AddString("비실이", "우왕 긴장 되는걸!!");
                pmtComtrol.AddOption("가위를 낸다.", "store", "bif_game_3");
                pmtComtrol.AddOption("바위를 낸다.", "store", "bif_game_3");
                pmtComtrol.AddOption("보를 낸다.", "store", "bif_game_3");
                break;

            case "bif_game_3":
                pmtComtrol.Reset();
                pmtComtrol.imageMode = true;
                var rnd3 = Random.Range(0, 3);
                switch (rnd3)
                {
                    case 0:
                        pmtComtrol.AddString("비실이", "앗 비겼다!!");
                        pmtComtrol.AddString("훈이", "한판 더!!");
                        pmtComtrol.AddOption("가위를 낸다.", "store", "bif_game_3");
                        pmtComtrol.AddOption("바위를 낸다.", "store", "bif_game_3");
                        pmtComtrol.AddOption("보를 낸다.", "store", "bif_game_3");
                        break;
                    case 1:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", ".....!!!");
                        pmtComtrol.AddString("훈이", "이겼다!!!!!!!");
                        pmtComtrol.AddString("비실이", "좋아 그러면 다음 게임을 시작할까?");
                        pmtComtrol.AddString("비실이", "가위바위보를 한번 더 이기면 빵 " + (score + 2) + "개를 줄게.");
                        pmtComtrol.AddString("비실이", "지면 어떻게 되는 지 알지?");
                        pmtComtrol.AddString("훈이", "우와.. 어떻게 하는 게 좋으려나..");

                        pmtComtrol.AddOption("빵 " + score + "개를 받고 끝낸다.", "store", "bif_finish_game");
                        pmtComtrol.AddOption("다음 게임을 시작한다.", "store", "bif_game_3_start");
                        break;

                    default:
                        pmtComtrol.AddString("훈이", "...!!");
                        pmtComtrol.AddString("훈이", ".....!!!");
                        pmtComtrol.AddString("훈이", "졌다.........");
                        pmtComtrol.AddString("비실이", "좋은 승부였어.");
                        pmtComtrol.AddString("훈이", "분하다!! 다음에 한판 더 해!");
                        pmtComtrol.AddString("비실이", "좋아. 언제든 또 오라구!");
                        storeOutAction = "졌지만 좋은 승부였다..";
                        pmtComtrol.AddNextAction("main", "store_out");
                        break;
                }

                break;
        }
    }

    public void AddBBang(int amount, string idx = "")
    {
        print("AddBBang amt:" + amount + " idx:" + idx);
        if (amount > 0) myAudio.PlaySoundFx(1);

        if (amount < 0)
        {
            if (idx == "choco")
            {
                print("choco --");
                PlayerPrefs.SetInt("new_choco", PlayerPrefs.GetInt("new_choco") + amount);
            }
            else if (idx == "strawberry")
            {
                print("strawberry --");
                PlayerPrefs.SetInt("new_strawberry", PlayerPrefs.GetInt("new_strawberry") + amount);
            }
            else if (idx == "hot")
            {
                print("hot --");
                PlayerPrefs.SetInt("new_hot", PlayerPrefs.GetInt("new_hot") + amount);
            }
            else if (idx == "bingle")
            {
                print("bingle --");
                PlayerPrefs.SetInt("new_bingle", PlayerPrefs.GetInt("new_bingle") + amount);
            }
            else if (idx == "maple")
            {
                print("maple --");
                PlayerPrefs.SetInt("new_maple", PlayerPrefs.GetInt("new_maple") + amount);
            }
            else if (idx == "purin")
            {
                print("purin --");
                PlayerPrefs.SetInt("new_purin", PlayerPrefs.GetInt("new_purin") + amount);
            }
            else
            {
                print("ERROR: ADD BBANG IDX ERROR : " + idx);
            }

            PlayerPrefs.SetInt("bbang",
                PlayerPrefs.GetInt("new_choco") + PlayerPrefs.GetInt("new_strawberry") + PlayerPrefs.GetInt("new_hot") +
                PlayerPrefs.GetInt("new_bingle") + PlayerPrefs.GetInt("new_maple") + PlayerPrefs.GetInt("new_purin"));
            PlayerPrefs.Save();
            UpdateBbangCount();
            return;
        }

        var rnd = Random.Range(0, 6);
        if (rnd == 4) rnd = 5;

        if (idx == "choco") rnd = 0;
        if (idx == "strawberry") rnd = 1;
        if (idx == "hot") rnd = 2;
        if (idx == "bingle") rnd = 3;
        if (idx == "maple") rnd = 4;
        if (idx == "purin") rnd = 5;

        if (rnd == 5)
        {
            print("purin ++");
            pmtComtrol.AddString("푸린글스빵", "푸린글스빵 " + amount + "개를 겟했다!");
            PlayerPrefs.SetInt("new_purin", PlayerPrefs.GetInt("new_purin") + amount);
        }

        if (rnd == 4)
        {
            print("maple ++");
            pmtComtrol.AddString("메이풀빵", "메이풀빵 " + amount + "개를 겟했다!");
            PlayerPrefs.SetInt("new_maple", PlayerPrefs.GetInt("new_maple") + amount);
        }
        else if (rnd == 3)
        {
            print("bingle ++");
            PlayerPrefs.SetInt("new_bingle", PlayerPrefs.GetInt("new_bingle") + amount);
        }
        else if (rnd == 2)
        {
            print("hot ++");
            pmtComtrol.AddString("핫소스빵", "핫소스빵 " + amount + "개를 겟했다!");
            PlayerPrefs.SetInt("new_hot", PlayerPrefs.GetInt("new_hot") + amount);
        }
        else if (rnd == 1)
        {
            print("strawberry ++");
            pmtComtrol.AddString("딸기크림빵", "딸기크림빵 " + amount + "개를 겟했다!");
            PlayerPrefs.SetInt("new_strawberry", PlayerPrefs.GetInt("new_strawberry") + amount);
        }
        else if (rnd == 0)
        {
            print("choco ++");
            pmtComtrol.AddString("초코롤빵", "초코롤빵 " + amount + "개를 겟했다!");
            PlayerPrefs.SetInt("new_choco", PlayerPrefs.GetInt("new_choco") + amount);
        }
        else
        {
            print("ERROR: ADD BBANG IDX ERROR : " + rnd);
        }

        PlayerPrefs.SetInt("bbang",
            PlayerPrefs.GetInt("new_choco") + PlayerPrefs.GetInt("new_strawberry") + PlayerPrefs.GetInt("new_hot") +
            PlayerPrefs.GetInt("new_bingle") + PlayerPrefs.GetInt("new_maple") + PlayerPrefs.GetInt("new_purin"));
        PlayerPrefs.Save();
        UpdateBbangCount();

        print("| CHOCO " + PlayerPrefs.GetInt("new_choco") + " BERRY " + PlayerPrefs.GetInt("new_strawberry"));
    }

    public void AddAlarm()
    {
        notice.MakeAndSetNotification();
    }

    public void UpdateBbangCount()
    {
        if (!PlayerPrefs.HasKey("bbang"))
        {
            PlayerPrefs.SetInt("bbang", 0);
            PlayerPrefs.Save();
        }

        bbangCount = PlayerPrefs.GetInt("bbang");
        bbangCount_ui.GetComponent<Text>().text = "" + bbangCount;
        if (bbangCount == 0)
            bbangCountPanel.GetComponent<Animator>().SetTrigger("stop");
        else
            bbangCountPanel.GetComponent<Animator>().SetTrigger("play");
    }

    public void OpenBbang()
    {
        if (PlayerPrefs.GetInt("bbang") < 1) return;
        //AddBBang(-1);
        if (currentLocation == "bbobgi") newStore.GetComponent<StoreControl>().bbobgiPanel.bbobgi.HideObjs();

        if (PlayerPrefs.GetInt("new_purin") > 0)
        {
            AddBBang(-1, "purin");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(5);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 1);
            PlayerPrefs.SetInt("bbang_purin", PlayerPrefs.GetInt("bbang_purin") + 1);
        }
        else if (PlayerPrefs.GetInt("new_maple") > 0)
        {
            AddBBang(-1, "maple");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(4);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 1);
            PlayerPrefs.SetInt("bbang_maple", PlayerPrefs.GetInt("bbang_maple") + 1);
        }
        else if (PlayerPrefs.GetInt("new_bingle") > 0)
        {
            AddBBang(-1, "bingle");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(3);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 1);
            PlayerPrefs.SetInt("bbang_bingle", PlayerPrefs.GetInt("bbang_bingle") + 1);
        }
        else if (PlayerPrefs.GetInt("new_hot") > 0)
        {
            AddBBang(-1, "hot");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(2);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 1);
            PlayerPrefs.SetInt("bbang_hot", PlayerPrefs.GetInt("bbang_hot") + 1);
        }
        else if (PlayerPrefs.GetInt("new_strawberry") > 0)
        {
            AddBBang(-1, "strawberry");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(1);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 1);
            PlayerPrefs.SetInt("bbang_strawberry", PlayerPrefs.GetInt("bbang_strawberry") + 1);
        }
        else if (PlayerPrefs.GetInt("new_choco") > 0)
        {
            AddBBang(-1, "choco");
            var myBang = new GameObject();
            myBang = Instantiate(bbang_panel, bbang_holder.transform);
            myBang.SetActive(true);
            myBang.GetComponent<BbangControl>().SetBbang(0);

            PlayerPrefs.SetInt("currentBbangCount", PlayerPrefs.GetInt("currentBbangCount") + 2);
            PlayerPrefs.SetInt("bbang_choco", PlayerPrefs.GetInt("bbang_choco") + 2);
        }

        originalMusic = myAudio.currentPlaying;
        myAudio.PlayMusic(3);

        PlayerPrefs.SetInt("totalBbangCount", PlayerPrefs.GetInt("totalBbangCount") + 1);
        PlayerPrefs.Save();
        showroom.UpdateBbangShow();
    }

    public void BBangClosed()
    {
        myAudio.PlayMusic(originalMusic);
    }

    public void WatchAdBtnClicked()
    {
        watchAdPanel.SetActive(true);
        debugCount = 1;
    }

    public void AdsYes()
    {
        if (gameObject.GetComponent<Heart_Control>().heartCount >= 6)
        {
            balloon.ShowMsg("이제 그만 쉬어도 되겠다.");
            return;
        }

        gameObject.GetComponent<Ad_Control>().PlayAds(Ad_Control.AdsType.heart);
        watchAdPanel.SetActive(false);
    }

    public void AdsNo()
    {
        watchAdPanel.SetActive(false);
    }

    public void BusBtnClicked()
    {
        busPanel.SetActive(true);
        BusBtnUpdate();
        gameObject.GetComponent<Heart_Control>().Bus_SetAnim();
    }

    public void BusExit()
    {
        busPanel.SetActive(false);
    }

    public void BusLaunch()
    {
        if (lauchText.text == "빵 받기")
        {
            gameObject.GetComponent<Heart_Control>().Bus_Receive();
        }
        else
        {
            gameObject.GetComponent<Heart_Control>().Bus_launch();
            if (PlayerPrefs.HasKey("ShuttleCount"))
            {
                PlayerPrefs.SetInt("ShuttleCount", Mathf.RoundToInt(PlayerPrefs.GetInt("ShuttleCount") + 1));
                PlayerPrefs.Save();
            }
        }

        BusBtnUpdate();
    }

    public void BusBtnUpdate()
    {
        debugCount = 0;
        //gameObject.GetComponent<Heart_Control>().Bus_SetAnim();
        if (gameObject.GetComponent<Heart_Control>().went)
        {
            launchBtn.GetComponent<Button>().interactable = false;
            lauchText.text = "빵셔틀 보내기";
        }
        else
        {
            if (gameObject.GetComponent<Heart_Control>().received)
            {
                lauchText.text = "빵셔틀 보내기";
                launchBtn.GetComponent<Button>().interactable = true;
                launchBtn.GetComponent<Image>().color = Color.white;
            }
            else
            {
                lauchText.text = "빵 받기";
                launchBtn.GetComponent<Button>().interactable = true;
                launchBtn.GetComponent<Image>().color = Color.yellow;
            }
        }
    }


    public void DangunBtnClicked()
    {
        dangunPanel.GetComponent<DangunControl>().OpenDangun();
    }

    public void CreditBtnClicked()
    {
        collection.GetComponent<Collection_Panel_Control>().GetCount();
        creditPanel.SetActive(true);
        creditPanel.GetComponent<CreditControl>().Play();
    }

    public void SettingBtnClicked()
    {
        settingPanel.GetComponent<SettingPanelControl>().Start();
        settingPanel.SetActive(true);
    }

    public void HideBtn()
    {
        if (lower_bar.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Lower_Bar_Idle")
        {
            lower_bar.GetComponent<Animator>().SetTrigger("hide");
            return;
        }

        if ((currentLocation == "home") | (currentLocation == "store") | (currentLocation == "alba"))
            if (lower_bar.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Lower_Bar_Hidden")
            {
                if (dangunIng) return;
                lower_bar.GetComponent<Animator>().SetTrigger("show");
            }
    }

    public void Debug_addBbang(string idx)
    {
        AddBBang(1, idx);
    }

    public void Debug_BbangEatCount()
    {
        PlayerPrefs.SetInt("bbangEatCount", 0);
    }

    public void Debug_RestoreDebugData()
    {
        PlayerPrefs.SetInt("debugMode", 0);
        balloon.ShowMsg("디버그 기록 삭제");
    }

    public void Debug_AddVacance()
    {
        PlayerPrefs.SetInt("bbang_vacance", PlayerPrefs.GetInt("bbang_vacance") + 1);
        PlayerPrefs.Save();
        showroom.UpdateBbangShow();
    }

    public void Debug_AddYogurt()
    {
        PlayerPrefs.SetInt("bbang_yogurt", PlayerPrefs.GetInt("bbang_yogurt") + 1);
        PlayerPrefs.Save();
        showroom.UpdateBbangShow();
    }

    public void Debug_MakeCards()
    {
        PlayerPrefs.SetInt("card_0", 15);
        PlayerPrefs.SetInt("card_3", 15);
        PlayerPrefs.SetInt("card_5", 15);
        PlayerPrefs.SetInt("card_18", 15);
        PlayerPrefs.SetInt("card_20", 15);
        PlayerPrefs.SetInt("card_22", 15);
        balloon.ShowMsg("카드 15장 됐어요!");
    }

    public void Debug_AddMoney()
    {
        gameObject.GetComponent<Heart_Control>().UpdateMoney(10000);
    }

    public void Debug_RemoveMoney()
    {
        gameObject.GetComponent<Heart_Control>().UpdateMoney(-10000);
    }

    public void SetDebugMode()
    {
        Msg.SetMsg("디버그 모드를 사용한 이력이 있어 랭킹 시스템에서 제외됩니다. 랭킹을 사용하려면 데이터를 초기화 해주세요.", 1);
        debugPanlShowBtn.SetActive(true);
        //leaderboard.AuthenticateToGameCenter();
        achievement.Autheticate();
    }

    private void Nylon_TryInvestBbang(string inputString, string id_y, string id_n, string parent_id = "Nylon")
    {
        int price = 10000;
        if(PlayerPrefs.GetInt("money") >= price)
            pmtComtrol.AddOption(inputString , parent_id, id_y); 
        else
            pmtComtrol.AddOption(inputString , parent_id, id_n); 
    }
    

    private void Nylon_DrawBbang(string id0, float weight0, string id1, float weight1, string id2, float weight2,
        string id3, float weight3)
    {
        int count = PlayerPrefs.GetInt("NylonDrawBbangCount", 0);
        count++;
        PlayerPrefs.SetInt("NylonDrawBbangCount", count);
        
        float totalWeight = weight0 + weight1 + weight2 + weight3;
        float rnd = Random.Range(0, totalWeight);
        
        if(rnd<weight0) pmtComtrol.AddNextAction("Nylon", id0);
        else if(rnd<weight0+weight1) pmtComtrol.AddNextAction("Nylon", id1);
        else if(rnd<weight0+weight1+weight2) pmtComtrol.AddNextAction("Nylon", id2);
        else pmtComtrol.AddNextAction("Nylon", id3);
    }
    
    private void Nylon_DrawBbang_f(string id0, float weight0, string id1, float weight1, string id2, float weight2,
        string id3, float weight3)
    {
        int count = PlayerPrefs.GetInt("NylonDrawBbangCount", 0);
        count++;
        PlayerPrefs.SetInt("NylonDrawBbangCount", count);
        
        float totalWeight = weight0 + weight1 + weight2 + weight3;
        float rnd = Random.Range(0, totalWeight);
        
        if(rnd<weight0) pmtComtrol.AddNextAction("Nylon_f", id0);
        else if(rnd<weight0+weight1) pmtComtrol.AddNextAction("Nylon_f", id1);
        else if(rnd<weight0+weight1+weight2) pmtComtrol.AddNextAction("Nylon_f", id2);
        else pmtComtrol.AddNextAction("Nylon_f", id3);
    }
    //WangHyung

    public void DialogueCheckMoney(int price, string id_y, string id_n, string inputString, string parent_id)
    {
        if(PlayerPrefs.GetInt("money") >= price)
            pmtComtrol.AddOption(inputString , parent_id, id_y); 
        else
            pmtComtrol.AddOption(inputString , parent_id, id_n); 
    }
    public void Tanghuru(string ID)
    {
        print("EVENT ID Tanghuru : " + ID);
        pmtComtrol.Reset();
        pmtComtrol.imageMode = true;

        switch (ID)
        {
            case "ID_T_OPEN_STORE":
                pmtComtrol.AddString("왕형", "안녕하시오.");
                pmtComtrol.AddString("왕형", "왕형 탕후루에 당도한 것을 환영하오, 낯선 이여.");
                pmtComtrol.AddString("왕형", "나는 당이 떨어진 자들을 굽어살피는 달콤한 사업가, 왕형이오.");
                tanghuruState = "ID_T_OPEN_STORE";
                pmtComtrol.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU");
                pmtComtrol.AddOption("포켓볼 빵 있어요?", "Tanghuru", "ID_T_OPEN_STORE_BREAD");
                pmtComtrol.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_STORE_STOREOUT");
                break;

            case "ID_T_OPEN_STORE_STOREOUT":
                storeOutAction = "";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_TANGHURU":
                pmtComtrol.AddString("왕형", "탕후루는 당 함량이 높아 원기를 회복시켜 주는 효과가 있소.");
                pmtComtrol.AddString("왕형", "먹으면 편의점 하나 정도 더 찾아갈 수 있는 힘이 날 것이오.");
                pmtComtrol.AddNextAction("Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_2":
                pmtComtrol.AddString("왕형", "탕후루는 하나에 3,000원이오. 구매하겠소?");
                DialogueCheckMoney(3000, "ID_T_OPEN_STORE_TANGHURU_BUY_YES", "ID_T_OPEN_STORE_TANGHURU_BUY_NOMONEY",
                    "네 주세요.", "Tanghuru");
                pmtComtrol.AddOption("그냥 다음에 살게요.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU_STOREOUT");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_BUY_YES":
                pmtComtrol.AddString("왕형", "고맙소.");
                pmtComtrol.AddString("왕형", "탕후루 여기 있소.");
                heartControl.UpdateMoney(-3000);
                heartControl.SetHeart(1);
                heartControl.SetHeart(1);
                pmtComtrol.AddString("훈이", "(탕후루를 먹고 하트가 두 개 회복되었다!)");
                pmtComtrol.AddString("왕형", "또 오시오.");
                if (tanghuruState == "ID_T_OPEN_STORE")
                {
                    pmtComtrol.AddString("왕형", "단, 오후 12시부터 오후 6시 사이는 손님이 많으니 되도록 피해 주시오.");
                }

                storeOutAction = "달고 맛있는 탕후루! 다음에 또 사러 와야겠다.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_BUY_NOMONEY":
                pmtComtrol.AddString("왕형", "그대가 소지하고 있는 돈이 부족하오.");
                pmtComtrol.AddNextAction("Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_STOREOUT":
                pmtComtrol.AddString("왕형", "그럼 그렇게 하시오.");
                pmtComtrol.AddString("왕형", "또 오시오.");
                if (tanghuruState == "ID_T_OPEN_STORE")
                {
                    pmtComtrol.AddString("왕형", "단, 오후 12시부터 오후 6시 사이는 손님이 많으니 되도록 피해 주시오.");
                }

                storeOutAction = "달달한 탕후루가 먹고 싶긴 하지만... 다음에 사먹도록 하자.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_BREAD":
                pmtComtrol.AddString("훈이", "포켓볼 빵 있어요?");
                pmtComtrol.AddString("왕형", "어허. 말세로다. 어찌 탕후루 가게에서 빵 따위를 팔겠는가?");
                pmtComtrol.AddString("훈이", "혹시 몰라서 여쭤봤어요...");
                pmtComtrol.AddString("왕형", "어림없는 소리!");
                pmtComtrol.AddString("왕형", "사대부의 나라가 어찌 오랑케의 음식을 팔겠는가. 당치도 않다!");
                storeOutAction = "괜히 호통만 들었다... 다음에는 탕후루를 사러 오자.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_FRIEND":
                pmtComtrol.AddString("왕형", "또 손님이 들이닥치다니, 난세로다!");
                pmtComtrol.AddString("왕형", "이제 나의 워라벨은 누가 지켜준단 말인가! 암흑의 시대가 도래하였구나!");
                tanghuruState = "ID_T_OPEN_FRIEND";
                pmtComtrol.AddNextAction("Tanghuru", "ID_T_OPEN");
                break;

            case "ID_T_OPEN_AFTERFRIEND":
                pmtComtrol.AddString("왕형", "어서오시오. 환영하오.");
                pmtComtrol.AddString("왕형", "그대가 다시 찾아 주니 기쁘구려.");
                // No command found on line 57 : 
                pmtComtrol.AddNextAction("Tanghuru", "ID_T_OPEN");
                break;

            case "ID_T_OPEN":
                pmtComtrol.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_ORDER");
                pmtComtrol.AddOption("포켓볼 빵 있어요?", "Tanghuru", "ID_T_OPEN_BBANG");
                pmtComtrol.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_ORDER":
                pmtComtrol.AddString("왕형", "아아. 지금은 주문이 폭주해서 당장 드릴 수가 없소.");
                pmtComtrol.AddString("왕형", "혹시 탕후루 만드는 것을 좀 도와주겠소? 도와준다면 내 넉넉하게 사례하겠소.");
                pmtComtrol.AddOption("네, 도와 드릴게요!", "Tanghuru", "ID_T_OPEN_ORDER_HELP");
                pmtComtrol.AddOption("다음에 다시 올게요.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_ORDER_HELP":
                tanghuruGameManager.EnterGame();
                break;

            case "ID_T_OPEN_BBANG":
                pmtComtrol.AddString("왕형", "어찌 탕후루 가게에서 빵 따위를 팔겠는가? 내 지금 바쁘니 당치 않은 이야기는 삼가 주시게.");
                pmtComtrol.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_ORDER");
                pmtComtrol.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_END":
                pmtComtrol.AddString("왕형", "미안하오. 다음에 다시 와 주시오.");
                storeOutAction = "괜히 호통만 들었다... 다음에는 탕후루를 사러 오자.";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_ORDER_HELP_NOHEART":
                break;

            case "ID_T_OPEN_ORDER_HELP_NOHEART_STOREOUT":
                break;

            case "ID_T_GAME_OVER_LINK":
                //앞으로는 알밥 천국을 통해서도 왕형 탕후루에 일을 도와주러 올 수 있소.
                if (PlayerPrefs.GetInt("tanghuru_friend", 0) == 0)
                {
                    if (PlayerPrefs.GetString("tanghuru_rank") == "F")
                    {
                        pmtComtrol.AddString("왕형", "내가 괜한 부탁을 한 모양이오.");
                        pmtComtrol.AddString("왕형", "돈이 궁할 때 와서 알바를 하든지 말든지 알아서 하시오.");
                    }
                    else
                    {
                        pmtComtrol.AddString("왕형", "고맙소.. 자네 덕분에 살았소.");
                        pmtComtrol.AddString("왕형", "앞으로도 용돈이 필요할 때 가끔씩 들러서 알바를 해주시오.");
                    }
                    PlayerPrefs.SetInt("tanghuru_friend", 1);
                }
                else
                {
                    if (PlayerPrefs.GetString("tanghuru_rank") == "F")
                    {
                        pmtComtrol.AddString("왕형", "내가 괜한 부탁을 한 모양이오.");
                    }
                    else
                    {
                        pmtComtrol.AddString("왕형", "고생많았소.");
                    }
                }
                pmtComtrol.AddString("왕형", "이제 좀 한숨 돌릴 여유가 생긴 듯 한데 탕후루를 구매하겠소?");
                pmtComtrol.AddOption("탕후루를 산다.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                pmtComtrol.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_GAME_CLOSE_END");
                break;

            case "ID_T_GAME_CLOSE_END":
                pmtComtrol.AddString("왕형", "또 오시오.");
                storeOutAction = "";
                pmtComtrol.AddNextAction("main", "store_out");
                break;

            case "ID_T_ALBAB":
                pmtComtrol.AddString("왕형", "어서오시오. 환영하오.");
                pmtComtrol.AddString("왕형", "그대가 일부러 찾아 주니 기쁘구려.");
                pmtComtrol.AddString("왕형", "탕후루 만드는 것을 좀 도와주겠소?");
                pmtComtrol.AddOption("네, 도와 드릴게요!", "Tanghuru", "ID_T_ALBAB_HELP");
                pmtComtrol.AddOption("다음에 다시 올게요.", "Tanghuru", "ID_T_ALBAB_STOREOUT");
                break;

            case "ID_T_ALBAB_HELP":
                tanghuruGameManager.EnterGame();
                break;

            case "ID_T_ALBAB_STOREOUT":
                pmtComtrol.AddString("왕형", "다음엔 꼭 도와주길 바라오.");
                storeOutAction = "";
                pmtComtrol.AddNextAction("main", "store_out");
                break;
        }
    }
    
    //!nylon
    public enum TradeType {buy, sell}
    private void Nylon_TradeCoin(TradeType type, string confirmID, string cancelID, string noMoney = null) {
        domiTradePanel.OpenPanel(type, confirmID, cancelID, noMoney);
    }

    private void Nylon_OpenTradePanel(bool flag)
    {
        // if(domiCoin.GetAmount() == 0) return;
        if(flag) domiCoin.ShowPanel();
            else domiCoin.HidePanel();
    }
    
    public void Nylon(string ID)
{
    print("EVENT ID Nylon : " + ID);
    pmtComtrol.Reset();
    pmtComtrol.imageMode = true;

    switch(ID)
    {
        case "ID_F_NOMONEY_BEFORECOIN":
            pmtComtrol.AddString("나이롱마스크", "오우~ 당신~ 빈털털이였네요우~");
            pmtComtrol.AddString("나이롱마스크", "돈이 없으면 투자할 수 없어요우~ 돈 벌어와요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_NOMONEY_BEFORECOIN_END");
            break;

        case "ID_F_NOMONEY_BEFORECOIN_END":
            storeOutAction = "(돈을 벌어서 빵에 투자해 보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_NOMONEY_AFTERECOIN_BBANGLAST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 손님~ 돈이 부족하네요우~");
            pmtComtrol.AddString("나이롱마스크", "세상엔 돈을 버는 방법이 많이 있어요우~ 행운을 빌어요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST_END");
            break;

        case "ID_F_NOMONEY_AFTERECOIN_BBANGLAST_END":
            storeOutAction = "(돈이 부족해서 투자를 못하다니. 어서 돈을 벌어오자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_FIRST":
            pmtComtrol.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
            pmtComtrol.AddOption("포켓볼 빵 있어요?", "Nylon", "ID_F_BBANG");
            pmtComtrol.AddOption("치킨 한 마리 주세요", "Nylon", "ID_F_CHICKEN");
            pmtComtrol.AddOption("여기선 뭐 팔아요?", "Nylon", "ID_F_WHAT");
            break;

        case "ID_F_STOREOUT":
            pmtComtrol.AddString("나이롱마스크", "나중에 또 오세요우~");
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_BBANG":
            pmtComtrol.AddString("나이롱마스크", "오우~ 포켓볼 뽱~ 그냥은 안 파라요우~");
            pmtComtrol.AddString("훈이", "그러면 어떻게 하면 팔아요?");
            pmtComtrol.AddString("나이롱마스크", "사지 말고 투자하세요우~");
            pmtComtrol.AddString("나이롱마스크", "지금 만 원을 내면 나중에 포켓볼 뽱으로 돌려줄게요우~");
            pmtComtrol.AddString("훈이", "몇 개 줄 건데요?");
            pmtComtrol.AddString("나이롱마스크", "그때그때 달라요우~");
            pmtComtrol.AddString("훈이", "(왠지 사기꾼 같은데...)");
            Nylon_TryInvestBbang("네, 만 원 투자할게요.", "ID_F_BBANG_INVEST", "ID_F_NOMONEY_BEFORECOIN");
            pmtComtrol.AddOption("아니요, 괜찮아요.", "Nylon", "ID_F_BBANG_NOTINVEST");
            break;

        case "ID_F_BBANG_INVEST":
            pmtComtrol.AddString("나이롱마스크", "감솨합니다~! 제가 뽱 많이 가져올게요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_INVEST_END");
            break;

        case "ID_F_BBANG_INVEST_END":
            storeOutAction = "(이상한 가게지만 일단 투자를 했으니 다시 와야겠다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_BBANG_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 아쉽네요우~ 저한테 투자하시면 한번에 포켓볼 뽱 세 개까지도 얻을 수 있는데~");
            pmtComtrol.AddString("나이롱마스크", "물론 하나도 못 얻을 수도 있지만...");
            pmtComtrol.AddString("훈이", "네? 뭐라고요?");
            pmtComtrol.AddString("나이롱마스크", "손님 참 운이 좋아보인다고요우~");
            Nylon_TryInvestBbang("정말요? 그럼 당장 투자할게요.", "ID_F_BBANG_NOTINVEST_INVEST", "ID_F_NOMONEY_BEFORECOIN");
            pmtComtrol.AddOption("괜찮다니까요.", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST");
            break;

        case "ID_F_BBANG_NOTINVEST_INVEST":
            pmtComtrol.AddString("나이롱마스크", "정말 현명한 선택이에요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddString("나이롱마스크", "뽱 많이 가져올 테니까 다음에 봐요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_BBANG_NOTINVEST_INVEST_END");
            break;

        case "ID_F_BBANG_NOTINVEST_INVEST_END":
            storeOutAction = "(빵을 세 개나 얻을 수 있다니 너무 기대된다. 그런데 투자가 망하면 어떡하지?)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_BBANG_NOTINVEST_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "그렇다면 어쩔 수 없죠우~");
            pmtComtrol.AddOption("그냥 치킨 한 마리 주세요", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN");
            pmtComtrol.AddOption("안녕히 계세요.", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE");
            break;

        case "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN":
            pmtComtrol.AddString("나이롱마스크", "재료가 다 떨어졌어요우~");
            pmtComtrol.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN_END");
            break;

        case "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN_END":
            storeOutAction = "(이 시간에 재료가 다 떨어졌다? 역시 뭔가 수상한 가게다. 치킨을 팔긴 하는 걸까? 그래도 빵을 세 개나 얻을 수 있는 투자는 흥미롭다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE":
            pmtComtrol.AddString("나이롱마스크", "또 오세요우~");
            pmtComtrol.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE_END");
            break;

        case "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE_END":
            storeOutAction = "(수상한 가게다. 그런데 빵을 세 개나 얻을 수 있다고 하니 욕심이 난다. 여유가 생기면 다시 와보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_CHICKEN":
            pmtComtrol.AddString("나이롱마스크", "오우~ 취킨~ 재료가 다 떨어졌어요우~ 안 팔아요우~");
            pmtComtrol.AddOption("그럼 뭐 팔아요?", "Nylon", "ID_F_CHICKEN_WHAT");
            pmtComtrol.AddOption("그럼 다음에 올게요.", "Nylon", "ID_F_CHICKEN_NOTINVEST");
            break;

        case "ID_F_CHICKEN_WHAT":
            pmtComtrol.AddString("나이롱마스크", "취킨 사지 말고 저한테 투자하세요우~");
            pmtComtrol.AddString("나이롱마스크", "지금 만 원을 내면 나중에 포켓볼 뽱으로 돌려줄게요우~");
            pmtComtrol.AddString("훈이", "몇 개 줄 건데요?");
            pmtComtrol.AddString("나이롱마스크", "그때그때 달라요우~");
            pmtComtrol.AddString("훈이", "(왠지 사기꾼 같은데...)");
            Nylon_TryInvestBbang("네, 만 원 투자할게요.", "ID_F_CHICKEN_WHAT_INVEST", "ID_F_NOMONEY_BEFORECOIN");
            pmtComtrol.AddOption("아니요, 괜찮아요.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST");
            break;

        case "ID_F_CHICKEN_WHAT_INVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 잘 선택하셨어요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddString("나이롱마스크", "제가 뽱 잔뜩 가져올게요우~ 최대 3개까지 가져올 수 있어요우~");
            pmtComtrol.AddString("나이롱마스크", "물론 하나도 못 가져올 수도 있지만...");
            pmtComtrol.AddString("훈이", "네? 뭐라고요?");
            pmtComtrol.AddString("나이롱마스크", "아무것도 아니에요우~");
            pmtComtrol.AddString("나이롱마스크", "다음에 봐요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_INVEST_END");
            break;

        case "ID_F_CHICKEN_WHAT_INVEST_END":
            storeOutAction = "(뭔가 하나도 못 가져올 수도 있다는 말을 들은 것 같은데... 잘못 들은 거겠지? 투자가 잘 되길 기도하자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_CHICKEN_WHAT_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 아쉽네요우~");
            pmtComtrol.AddString("나이롱마스크", "저한테 투자하시면 한번에 포켓볼 뽱 세 개까지도 얻을 수 있는데~");
            Nylon_TryInvestBbang("정말요? 그럼 당장 투자할게요.", "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST", "ID_F_NOMONEY_BEFORECOIN");
            pmtComtrol.AddOption("괜찮다니까요.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            break;

        case "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST":
            pmtComtrol.AddString("나이롱마스크", "좋아요우~ 제가 뽱 많이 가져올게요우~");
            pmtComtrol.AddString("나이롱마스크", "세 개까지 가져올 수 있어요우~");
            pmtComtrol.AddString("나이롱마스크", "하나도 못 가져올 수도 있지만...");
            pmtComtrol.AddString("훈이", "네?");
            pmtComtrol.AddString("나이롱마스크", "아무말도 안 했어요우~ 다음에 봐요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST_END");
            break;

        case "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST_END":
            storeOutAction = "(빵을 한번에 세 개나 얻을 수 있다니! 그런데 하나도 못 가져올 수도 있다는 건... 잘못 들은 거겠지?)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "어쩔 수 없죠우~ 그래도 잘 생각해 봐요우~");
            pmtComtrol.AddString("훈이", "포켓볼 빵을 팔지는 않으시나요?");
            pmtComtrol.AddString("나이롱마스크", "안 팔아요우~!");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST_END");
            break;

        case "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST_END":
            storeOutAction = "(수상한 가게에 무턱대고 돈을 맡길 수는 없다. 그래도 빵을 세 개나 얻을 수 있다고 하니 더 고민해보고 방문하자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_CHICKEN_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 잠깐 기다려봐요우~");
            pmtComtrol.AddString("나이롱마스크", "저는 사실 투자자에요우~");
            pmtComtrol.AddString("나이롱마스크", "저한테 만 원을 투자하면 포켓볼 뽱을 세 개까지 가져올 수 있어요우~");
            pmtComtrol.AddOption("진짜요? 그럼 당장 투자할게요.", "Nylon", "ID_F_CHICKEN_NOTINVEST_INVEST");
            pmtComtrol.AddOption("필요 없어요.", "Nylon", "ID_F_CHICKEN_NOTINVEST_NOTINVEST");
            break;

        case "ID_F_CHICKEN_NOTINVEST_INVEST":
            pmtComtrol.AddString("나이롱마스크", "탁월한 선택이에요우~ 최선을 다할게요우~");
            pmtComtrol.AddString("나이롱마스크", "물론 투자 실패 가능성도 있지만...");
            pmtComtrol.AddString("훈이", "투자 실패요?");
            pmtComtrol.AddString("나이롱마스크", "저는 그런 말 안 했어요우~ 다음에 또 와요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_NOTINVEST_INVEST_END");
            break;

        case "ID_F_CHICKEN_NOTINVEST_INVEST_END":
            storeOutAction = "(빵을 세 개나 한번에 얻을 수 있다니 기대된다. 그런데 투자에 실패하면 어떻게 되는 거지?)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_CHICKEN_NOTINVEST_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "그렇다면 어쩔 수 없죠우~");
            pmtComtrol.AddString("훈이", "그냥 포켓볼 빵을 팔지는 않아요?");
            pmtComtrol.AddString("나이롱마스크", "안 팔아요우~! 투자하고 싶을 때 다시 오세요우~!");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_NOTINVEST_NOTINVEST_END");
            break;

        case "ID_F_CHICKEN_NOTINVEST_NOTINVEST_END":
            storeOutAction = "(아저씨가 치킨 파는 데는 관심이 없나보다. 투자를 하고 싶어지면 다시 오자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_WHAT":
            pmtComtrol.AddString("나이롱마스크", "아무것도 안 팔아요우~ 대신 투자해요우~");
            pmtComtrol.AddString("훈이", "투자요?");
            pmtComtrol.AddString("나이롱마스크", "포켓볼 뽱 투자에요우~ 만 원 투자로 최대 세 개까지 얻을 수 있어요우~");
            pmtComtrol.AddString("나이롱마스크", "아무것도 못 얻을 수도 있지만...");
            pmtComtrol.AddString("훈이", "뭐라고요?");
            pmtComtrol.AddString("나이롱마스크", "아무것도 아니에요우~ 투자 하실래요우~? 뽱 많이 가져올 수 있어요우~");
            Nylon_TryInvestBbang("좋아요. 만 원 투자할게요.", "ID_F_WHAT_INVEST", "ID_F_NOMONEY_BEFORECOIN");
            pmtComtrol.AddOption("아니요. 괜찮아요.", "Nylon", "ID_F_WHAT_NOTINVEST");
            break;

        case "ID_F_WHAT_INVEST":
            pmtComtrol.AddString("나이롱마스크", "완죤 좋은 선택이에요우~ 열심히 투자해 볼게요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddString("나이롱마스크", "다음에 또 봐요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_WHAT_INVEST_END");
            break;

        case "ID_F_WHAT_INVEST_END":
            storeOutAction = "(빵을 세 개나 얻을 수 있다니 기대된다. 그런데 투자에 실패해서 아무것도 못 돌려받는 건 아니겠지?)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_WHAT_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 겁쟁이군요우~ 투자할 생각이 생기면 다시 와요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_WHAT_NOTINVEST_END");
            break;

        case "ID_F_WHAT_NOTINVEST_END":
            storeOutAction = "(겁쟁이라니. 기분은 별로 안 좋지만 왠지 오기가 생긴다. 다음번엔 투자해 봐야겠다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED":
            pmtComtrol.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
            pmtComtrol.AddOption("투자는 잘 됐나요?", "Nylon", "ID_F_SECOND_IFINVESTED_BREADS_???");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_STOREOUT");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_???":
            Nylon_DrawBbang("ID_F_SECOND_IFINVESTED_BREADS_0", 20,
            "ID_F_SECOND_IFINVESTED_BREADS_1", 45,
            "ID_F_SECOND_IFINVESTED_BREADS_2", 30, 
            "ID_F_SECOND_IFINVESTED_BREADS_3", 15);
            PlayerPrefs.SetInt("NylonInvested", 0);
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_3":
            pmtComtrol.AddString("나이롱마스크", "오우~ 예~ 완죤 잘 됐어요우~ 마치 투자의 신이 된 기분이에요우~");
            AddBBang(3);
            pmtComtrol.AddString("훈이", "와 3개씩이나!");
            pmtComtrol.AddString("나이롱마스크", "이 정돈 기본이에요우~! 또 투자해 볼래요우~?");
            Nylon_TryInvestBbang("당연하죠! 당장 투자할래요.", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
            pmtComtrol.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "똑똑한 선택이네요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 대박의 기운이 가득한 이몸만 믿으세요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_END":
            storeOutAction = "(투자 한 번에 빵을 세 개나 얻다니, 완전 개이득이다. 다음 투자 결과도 무척 기대된다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "지금 투자의 신을 못 믿는 건가요우~? 어리석기 짝이 없군요우~ 아마 오늘 투자하지 않은 걸 후회할 거에요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_END":
            storeOutAction = "(투자 한 번에 빵을 세 개나 얻다니, 완전 개이득이다. 그래도 두 번 연속 세 개를 얻긴 힘들겠지. 투자는 다음에 하자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_2":
            pmtComtrol.AddString("나이롱마스크", "오우~ 꽤 잘 됐어요우~ 저는 투자 고수거든요우~");
            AddBBang(2);
            pmtComtrol.AddString("훈이", "오 2개나!");
            pmtComtrol.AddString("나이롱마스크", "나름 만족스럽죠우~? 또 투자해보는 건 어때요우~?");
            Nylon_TryInvestBbang("좋아요!", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
            pmtComtrol.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "투자의 묘미를 알아주는 분이군요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 제가 알아서 잘 투자해드릴테니 다음에 봐요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_END":
            storeOutAction = "(투자도 빵을 얻는 데 좋은 방법인 것 같다. 다음 투자 결과도 기대된다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "이번 투자 결과가 마음에 안 들었나요우~? 이 정도면 정말 잘 한 거라고요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_END":
            storeOutAction = "(투자 한 번에 빵 두 개 정도면 만족스럽다. 욕심을 너무 많이 부리는 건 좋지 않겠지. 투자는 다음에 하자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_1":
            pmtComtrol.AddString("나이롱마스크", "오우~ 손해는 안 봤어요우~");
            AddBBang(1);
            pmtComtrol.AddString("훈이", "1개네요.");
            pmtComtrol.AddString("나이롱마스크", "맞아요우~ 이번에는 좀 아쉬운 결과지만 다음 번엔 더 잘할 수 있어요우~ 다시 한번 투자해 볼래요우~?");
            Nylon_TryInvestBbang("한번만 더 믿어볼게요.", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
            pmtComtrol.AddOption("아니요. 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST":
            pmtComtrol.AddString("나이롱마스크", "한 번 더 기회를 줘서 고마워요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 이번엔 더 잘해 볼게요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_END":
            storeOutAction = "(만 원이나 투자해서 빵을 한 개밖에 못 얻다니. 조금 아쉽지만 다음 결과를 기다려보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "정말 매정한 사람이네요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_END":
            storeOutAction = "(끼워 파는 포켓볼 빵이랑 가격이 같다니 너무 비싸다. 조금 여유가 생기면 다시 투자해 보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_0":
            pmtComtrol.AddString("나이롱마스크", "오우~ 이번에 상황이 좀 좋지 않았어요우~");
            AddBBang(0);
            pmtComtrol.AddString("훈이", "한 개도 못 얻었다고요?");
            pmtComtrol.AddString("나이롱마스크", "투자라는 게 원래 그런 거 아니겠어요우~? 맑은 날이 있으면 흐린 날이 있는 거죠우~");
            pmtComtrol.AddString("나이롱마스크", "이번에는 정말 느낌이 좋은데 한번 더 투자해 볼래요우?");
            Nylon_TryInvestBbang("정말이죠...? 그럼 딱 한번만 더 투자해 볼게요.", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
            pmtComtrol.AddOption("미쳤어요?", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "물론 정말이죠우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 이번엔 진짜 잘 될 거니까 저만 믿으세요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_END":
            storeOutAction = "(만 원이나 투자해서 빵을 하나도 못 얻다니 이거 완전 손해잖아? 다음 투자는 잘 되기를 기도한다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 그렇게 험한 말을~! 원숭이도 나무에서 떨어질 때가 있는 법이라고요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_END");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_END":
            storeOutAction = "(완전 사기꾼이잖아? 분하다. 하지만 뭔가 또 투자하고 싶어서 손이 근질근질하다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFNOTINVESTED":
            pmtComtrol.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
            pmtComtrol.AddString("나이롱마스크", "포켓볼 뽱에 투자해볼 생각이 생기셨나요우~? 만 원이면 포켓볼 뽱을 최대 3개 까지 얻을 수 있어요우~");
            Nylon_TryInvestBbang("네, 투자할게요.", "ID_F_SECOND_IFNOTINVESTED_INVEST", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
            pmtComtrol.AddOption("아니요, 투자 안 할 건데요?", "Nylon", "ID_F_SECOND_IFNOTINVESTED_NOTINVEST");
            break;

        case "ID_F_SECOND_IFNOTINVESTED_INVEST":
            pmtComtrol.AddString("나이롱마스크", "역시 투자할줄 있었어요우~ 저만 믿으세요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFNOTINVESTED_INVEST_END");
            break;

        case "ID_F_SECOND_IFNOTINVESTED_INVEST_END":
            storeOutAction = "(결국 투자했다. 왠지 포켓볼 빵을 잔뜩 얻을 수 있을 것 같은 기분이 든다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_SECOND_IFNOTINVESTED_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "그럼 뭐하러 온 거에요우~ 투자할 생각 생기면 다시 와요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFNOTINVESTED_NOTINVEST_END");
            break;

        case "ID_F_SECOND_IFNOTINVESTED_NOTINVEST_END":
            storeOutAction = "(역시 투자는 위험하다. 빵을 세 개나 얻을 수 있다는 건 매력적이지만 역시 돈이 아깝다. 좀 더 고민해본 뒤 투자하자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;
    }
}
    public void Nylon_f(string ID)
{
    print("EVENT ID Nylon_f : " + ID);
    pmtComtrol.Reset();
    pmtComtrol.imageMode = true;

    switch(ID)
    {
        case "ID_F_FRIEND_HELLO":
            pmtComtrol.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~ ");
            pmtComtrol.AddString("나이롱마스크", "단골 손님~ 오늘도 이렇게 방문해 주셔서 캄사합니돠~");
            if (PlayerPrefs.GetInt("NylonInvested") == 0)
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_BREADS_???");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_???":
            Nylon_DrawBbang_f("ID_F_SECOND_IFINVESTED_BREADS_0", 20,
            "ID_F_SECOND_IFINVESTED_BREADS_1", 50,
            "ID_F_SECOND_IFINVESTED_BREADS_2", 20, 
            "ID_F_SECOND_IFINVESTED_BREADS_3", 10);
            PlayerPrefs.SetInt("NylonInvested", 0);
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_3":
            pmtComtrol.AddString("훈이", "투자는 잘 됐나요?");
            pmtComtrol.AddString("나이롱마스크", "오우~ 예~ 완죤 잘 됐어요우~ 마치 투자의 신이 된 기분이에요우~");
            AddBBang(3);
            pmtComtrol.AddString("훈이", "와 3개씩이나!");
            pmtComtrol.AddString("나이롱마스크", "이 정돈 기본이에요우~! 또 투자해 볼래요우~?");
            Nylon_TryInvestBbang("당연하죠! 당장 투자할래요.", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST", "ID_F_NOMONEY_BREAD", "Nylon_f");
            pmtComtrol.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "똑똑한 선택이네요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 이번에도 투자 잘해서 대봑 한번 터뜨려 볼게요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "지금 투자의 신을 못 믿는 건가요우~? 어리석기 짝이 없군요우~ 오늘 투자하지 않으면 후회할 거에요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_2":
            pmtComtrol.AddString("훈이", "투자는 잘 됐나요?");
            pmtComtrol.AddString("나이롱마스크", "오우~ 꽤 잘 됐어요우~ 저는 투자 고수거든요우~");
            AddBBang(2);
            pmtComtrol.AddString("훈이", "오 2개나!");
            pmtComtrol.AddString("나이롱마스크", "나름 만족스럽죠우~? 또 투자해보는 건 어때요우~?");
            Nylon_TryInvestBbang("좋아요!", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST", "ID_F_NOMONEY_BREAD", "Nylon_f");
            pmtComtrol.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "투자의 묘미를 알아주는 분이군요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 다음번엔 뽱을 더 많이 가져오도록 노력할게요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "이번 투자 결과가 마음에 안 들었나요우~? 이 정도면 정말 잘 한 거라고요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_1":
            pmtComtrol.AddString("훈이", "투자는 잘 됐나요?");
            pmtComtrol.AddString("나이롱마스크", "오우~ 손해는 안 봤어요우~");
            AddBBang(1);
            pmtComtrol.AddString("훈이", "1개네요.");
            pmtComtrol.AddString("나이롱마스크", "맞아요우~ 이번에는 좀 아쉬운 결과지만 다음 번엔 더 잘 할 수 있어요우~ 다시 한번 투자해 볼래요우~?");
            Nylon_TryInvestBbang("한번만 더 믿어볼게요.", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST", "ID_F_NOMONEY_BREAD", "Nylon_f");
            pmtComtrol.AddOption("아니요. 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST":
            pmtComtrol.AddString("나이롱마스크", "한번 더 기회를 줘서 고마워요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 다음번엔 확실히 뽱을 더 많이 가져올 수 있을 거에요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "정말 매정한 사람이네요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_SECOND_IFINVESTED_BREADS_0":
            // No command found on line 89 : 
            pmtComtrol.AddString("나이롱마스크", "오우~ 이번에 상황이 좀 좋지 않았어요우~");
            AddBBang(0);
            pmtComtrol.AddString("훈이", "한 개 도 못 얻었다고요?");
            pmtComtrol.AddString("나이롱마스크", "투자라는 게 원래 그런 거 아니겠어요우~? 맑은 날이 있으면 흐린 날이 있는 거죠우~");
            pmtComtrol.AddString("나이롱마스크", "이번에는 정말 느낌이 좋은데 한번 더 투자해 볼래요우?");
            Nylon_TryInvestBbang("정말이죠...? 그럼 딱 한번만 더 투자해 볼게요.", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST", "ID_F_NOMONEY_BREAD", "Nylon_f");
            pmtComtrol.AddOption("미쳤어요?", "Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST":
            pmtComtrol.AddString("나이롱마스크", "물론 정말이죠우~ I am 신뢰에요우~");
            pmtComtrol.AddString("나이롱마스크", "만 원 잘 받았어요우~ 다음 번엔 절대 투자 실패 같은 거 안 할 거에요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_2":
            pmtComtrol.AddString("", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 그렇게 험한 말을~! 원숭이도 나무에서 떨어질 때가 있는 법이라고요우~");
            if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2");
            else
                pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
            break;

        case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2":
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN":
            pmtComtrol.AddString("나이롱마스크", "무엇에 투자하시겠어요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_INTERSECTION":
            if (PlayerPrefs.GetInt("NylonInvested") == 0) 
                Nylon_TryInvestBbang("빵에 투자하기", "ID_F_FRIEND_MAIN_BREAD", "ID_F_NOMONEY_BREAD", "Nylon_f"); 
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddOption("코인 거래하기", "Nylon_f", "ID_F_FRIEND_MAIN_COIN");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_MAIN_OUT");
            break;

        case "ID_F_FRIEND_MAIN_BREAD":
            pmtComtrol.AddString("나이롱마스크", "포켓볼 뽱~에 투자하고 싶으시군요우~ 만 원이면 포켓볼 뽱을 최대 3개 까지 얻을 수 있어요우~");
            pmtComtrol.AddString("나이롱마스크", "투자를 확정하시겠어요우~?");
            pmtComtrol.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_BREAD_YES");
            pmtComtrol.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_BREAD_NO");
            break;

        case "ID_F_FRIEND_MAIN_BREAD_YES":
            pmtComtrol.AddString("나이롱마스크", "역시 단골손님이에요우~ 화끈하네요우~");
            heartControl.UpdateMoney(-10000);
            PlayerPrefs.SetInt("NylonInvested", 1);
            pmtComtrol.AddString("나이롱마스크", "다음에 들를 때 투자 결과를 알려드릴게요우~");
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_BREAD_NO":
            pmtComtrol.AddString("나이롱마스크", "그럼 뭐하러 온 거에요우~");
            pmtComtrol.AddString("나이롱마스크", "다른 투자에 관심이 있는 건가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_COIN":
            Nylon_OpenTradePanel(true);
            int balancePercent = domiCoin.GetBalancePercent();
            if (domiCoin.GetAmount() > 0)
                        {
                            if(balancePercent > 70) 
                                pmtComtrol.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 엄청나요우~ 이러다 정말 화성 가겠어요우~");
                            else if(balancePercent > 30) 
                                pmtComtrol.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 완전 떡상했네요우~");
                            else if(balancePercent > -30) 
                                pmtComtrol.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~");
                            else if(balancePercent > -70) 
                                pmtComtrol.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 떡락도 이런 떡락이 없네요우~");
                            else 
                                pmtComtrol.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 오우~ 투자 정말 개못하시네요우~");
                            
                        }
            pmtComtrol.AddOption("도미 코인을 사고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY");
            if (domiCoin.GetAmount() > 0)
                            pmtComtrol.AddOption("도미 코인을 팔고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL");
            pmtComtrol.AddOption("다른 거래를 하고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_ANOTHER");
            break;

        case "ID_F_FRIEND_MAIN_COIN_ANOTHER":
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY":
            pmtComtrol.AddString("나이롱마스크", "지금 코인 가격은 "+domiCoin.GetPrice()+"원 이에요우~");
            Nylon_OpenTradePanel(true);
            if(domiCoin.diff < 0.6) { 
              pmtComtrol.AddString("나이롱마스크", "지금이니~? 평생에 한번 있을까말까한 기회에요우~ 저라면 몰빵했어요우~");
              pmtComtrol.AddString("나이롱마스크", "차가 있다면 차를 팔고 집이 있다면 집을 팔아서 도미 코인을 사야해요우~");
            }
            else if(domiCoin.diff < 0.7) pmtComtrol.AddString("나이롱마스크", "오늘이 블랙프라이데이인가요우~? 도미 코인이 미친듯한 할인을 하고 있네요우~");
            else if(domiCoin.diff < 0.85) pmtComtrol.AddString("나이롱마스크", "바닥을 다지는 중이네요우~ 저라면 추매했어요우~ 물을 탈 때에요우~");
            else if(domiCoin.diff < 1) pmtComtrol.AddString("나이롱마스크", "도미 코인이 손님을 낚아먹으려고 하는군요우~ 물론 오를수도 있고 내릴 수도 있죠우~");
            else if(domiCoin.diff < 1.15) pmtComtrol.AddString("나이롱마스크", "이건 오른 것도 아니에요우~ 상승세 한번 타면 오늘이 제일 싸다는 걸 알게 될 거에요우~");
            else if(domiCoin.diff < 1.4) pmtComtrol.AddString("나이롱마스크", "꽤 올랐네요우~ 하지만 아직 늦지 않았어요우~ 이 가격보다 오르기만 하면 이득이에요우~");
            else if(domiCoin.diff < 1.8) pmtComtrol.AddString("나이롱마스크", "이미 버스 떠나버렸네요우~ 다음 버스를 기다리는 게 어떨까요우~? 물론 다음 버스가 온다면 말이죠우~");
            else {
              pmtComtrol.AddString("나이롱마스크", "오우~ 은하수에 살던 도미가 안드로메다로 떠나갔군요우~ 배 아파해도 소용 없어요우~");
              pmtComtrol.AddString("나이롱마스크", "진작에 샀어야죠우~");
            }
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_TRADE");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY_TRADE":
            Nylon_TradeCoin(TradeType.buy, "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER", "ID_F_FRIEND_MAIN_COIN_BUY_NOTBUY", "ID_F_NOMONEY_COIN");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER":
            pmtComtrol.AddString("나이롱마스크", "총 "+domiCoin.BuyAmount * domiCoin.GetPrice()+"원이에요우~ 구매를 확정하시겠어요우~?");
            pmtComtrol.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_YES");
            pmtComtrol.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_NO");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_YES":
            domiCoin.BuyCoin(domiCoin.BuyAmount);
            pmtComtrol.AddString("나이롱마스크", "여기 도미 코인 "+domiCoin.BuyAmount+"개에요우~ ");
            pmtComtrol.AddString("나이롱마스크", "이제 도미 코인을 "+domiCoin.GetAmount()+"개 가지고 계시네요우~");
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_NO":
            pmtComtrol.AddString("나이롱마스크", "변덕이 심하시군요우~ 몇 개 살지 확실히 정하세요우~");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_TRADE");
            break;

        case "ID_F_FRIEND_MAIN_COIN_BUY_NOTBUY":
            pmtComtrol.AddString("나이롱마스크", "신중한 성격이시군요우~ 하지만 때론 과감할 필요도 있어요우~ 화성 갈끄니까~");
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL":
            pmtComtrol.AddString("나이롱마스크", "지금 코인 가격은 "+domiCoin.GetPrice()+"원 이에요우~");
            Nylon_OpenTradePanel(true);
            if(domiCoin.diff < 0.6) { 
              pmtComtrol.AddString("나이롱마스크", "오우~ 쥐저스~! 대공황이에요우~ 제 돈 아니라서 다행이에요우~");
              pmtComtrol.AddString("나이롱마스크", "만약 제가 지금 코인을 들고 있었다면 정신차리려고 세수하다가 화가나서 세면대를 부수고 말았을 거에요우~");
            }
            else if(domiCoin.diff < 0.7) pmtComtrol.AddString("나이롱마스크", "바닥 밑에 지하실이 있었네요우~ 당신이 선택한 코인이에요우~ 악으로 깡으로 버티세요우~!");
            else if(domiCoin.diff < 0.85) pmtComtrol.AddString("나이롱마스크", "마음이 아프네요우~ 하지만 아프니까 청춘이죠우~ 단골 손님은 이팔청춘이네요우~ 너무 아프니까요우~");
            else if(domiCoin.diff < 1) pmtComtrol.AddString("나이롱마스크", "약간의 손실이 있네요우~ 하지만 이정도로 엄살 부리는 찌질이는 없겠죠우~?");
            else if(domiCoin.diff < 1.15) pmtComtrol.AddString("나이롱마스크", "오늘 장은 심심하네요우~ 차라리 적금을 드는 게 낫겠어요우~");
            else if(domiCoin.diff < 1.4) pmtComtrol.AddString("나이롱마스크", "나쁘지 않은 수익이에요우~ 이 정도는 돼야 코인할 맛이 나죠우~");
            else if(domiCoin.diff < 1.8) pmtComtrol.AddString("나이롱마스크", "가즈아~ 불꽃 같은 상승에 제 마음이 뜨거워지는군요우~");
            else {
              pmtComtrol.AddString("나이롱마스크", "오우~ 마취 로켓이 발사되는 것 같은 급등세로군요우~ 축하해요우~");
              pmtComtrol.AddString("나이롱마스크", "이정도면 하늘나라 천사 똥침도 찌를 수 있겠어요우~ ");
            }
            pmtComtrol.AddString("나이롱마스크", "지금 도미 코인을 "+domiCoin.GetAmount()+"개 가지고 있으시네요우~ 몇 개 파시겠어요우~? ");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_TRADE");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL_TRADE":
            Nylon_TradeCoin(TradeType.sell, "ID_F_FRIEND_MAIN_COIN_SELL_NUMBER", "ID_F_FRIEND_MAIN_COIN_SELL_NOTSELL", "");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL_NUMBER":
            pmtComtrol.AddString("나이롱마스크", "총 "+domiCoin.SellAmount*domiCoin.GetPrice()+"원을 얻을 수 있어요우~ 판매를 확정하시겠어요우~?");
            pmtComtrol.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_YES");
            pmtComtrol.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_NO");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL_YES":
            domiCoin.SellCoin(domiCoin.SellAmount);
            pmtComtrol.AddString("나이롱마스크", "도미 코인을 "+domiCoin.SellAmount+"개 팔았어요우~ ");
            pmtComtrol.AddString("나이롱마스크", "이제 도미 코인이 "+domiCoin.GetAmount()+"개 남았네요우~");
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL_NO":
            pmtComtrol.AddString("나이롱마스크", "완전 변덕쟁이군요우~ 몇 개 팔지 확실히 정하세요우~");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_TRADE");
            break;

        case "ID_F_FRIEND_MAIN_COIN_SELL_NOTSELL":
            pmtComtrol.AddString("나이롱마스크", "지금 팔긴 좀 아깝긴 하죠우~ 하지만 언제나 떨어질 수 있다는 걸 명심하세요우~");
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_MAIN_OUT":
            storeOutAction = "(투자는 개인의 선택이다. 나는 오늘의 선택에 후회하지 않는다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_NOMONEY_BREAD":
            pmtComtrol.AddString("나이롱마스크", "오우~ 손님~ 돈이 부족하네요우~");
            pmtComtrol.AddString("나이롱마스크", "돈을 벌어서 포켓볼 뽱에 투자해 보세요우~");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_NOMONEY_COIN":
            pmtComtrol.AddString("나이롱마스크", "오우~ 돈이 없잖아요우~");
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddString("나이롱마스크", "티끌 모아 태산이라는 말이 있죠우~");
            pmtComtrol.AddString("나이롱마스크", "티끌이라도 있어야 태산이 된다는 뜻이에요우~");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
            break;

        case "ID_F_FRIEND_COINEXPLAIN":
            pmtComtrol.AddString("나이롱마스크", "그나저나 우리가 투자로 인연을 쌓은지도 꽤 오래됐군요우~ 당신은 단골 손님이에요우~");
            //become friend
            PlayerPrefs.SetInt("NylonDomiTutorial", 1);
            pmtComtrol.AddString("나이롱마스크", "그래서 제가 새로운 투자 상품을 준비해 봤어요우~ 한번 들어볼래요우~?");
            pmtComtrol.AddString("훈이", "오! 어떤 거죠?");
            pmtComtrol.AddString("나이롱마스크", "바로 도미 코인이에요우~");
            pmtComtrol.AddString("훈이", "도미 코인이요?");
            pmtComtrol.AddString("나이롱마스크", "비트코인 같은 거에요우~ 도미 코인을 사면 실시간으로 가격이 바뀌어요우~ 쌀 때 사서 비쌀 때 사면 되요우~");
            pmtComtrol.AddString("훈이", "이것도 만 원씩 투자하면 되나요?");
            pmtComtrol.AddString("나이롱마스크", "아니요우~ 원하는 금액만큼 자유롭게 투자할 수 있어요우~ 한번 질러 볼래요우~? 벼락 부자 될 수 있어요우~");
            pmtComtrol.AddOption("좋아요. 한번 해볼게요.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST");
            pmtComtrol.AddOption("아니요. 돈은 알바해서 정직하게 벌어야죠.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_NOTINVEST");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST":
            pmtComtrol.AddString("나이롱마스크", "역시 손님이라면 코인의 가치를 알아봐 줄줄 알았어요우~");
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_2":
            pmtComtrol.AddString("나이롱마스크", "지금 코인 가격은 "+domiCoin.GetPrice()+"원 이에요우~ 코인을 몇 개 구매할래요우~?");
            Nylon_OpenTradePanel(true);
            pmtComtrol.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2_TRADE");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_2_TRADE":
            Nylon_TradeCoin(TradeType.buy, "ID_F_FRIEND_COINEXPLAIN_INVEST_NUMBER", "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST", "ID_F_NOMONEY_COIN");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_NUMBER":
            pmtComtrol.AddString("나이롱마스크", "총 "+domiCoin.GetPrice() * domiCoin.BuyAmount+"원이에요우~ 구매를 확정하시겠어요우~?");
            pmtComtrol.AddOption("네.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_YES");
            pmtComtrol.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_YES":
            pmtComtrol.AddString("나이롱마스크", "여기 도미 코인 "+domiCoin.BuyAmount+"개에요우~ 역시 손님은 도전 정신이 투철하시군요우~ 도전하는 사람은 아름다워요우~");
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddString("훈이", "가격이 이것보다 떨어지진 않겠죠?");
            pmtComtrol.AddString("나이롱마스크", "걱정하지 마요우~ 떨어지면 또 사면 되죠우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_YES_OUT");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_YES_OUT":
            storeOutAction = "(도미 코인을 샀다. 올랐으면 좋겠다.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "오우~ 쫄보가 따로 없네요우~ 다음에 왔을 땐 도미 코인 가격 따따블 돼 있을 거에요우~ 그땐 후회해도 소용 없어요우~");
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_NOTINVEST_OUT");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_NOTINVEST_OUT":
            storeOutAction = "(도미 코인에 투자할 수 있게 됐다. 다음에 한번 투자해 보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST":
            pmtComtrol.AddString("나이롱마스크", "답답하네요우~! 그런 푼돈 벌어서 언제 부자 될래요우~?");
            Nylon_OpenTradePanel(false);
            pmtComtrol.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST_OUT");
            break;

        case "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST_OUT":
            storeOutAction = "(코인이라니. 열심히 번 돈을 하루 아침에 날려 버릴 수는 없다. 그래도 인생은 한방이긴 한데... 다음에 투자해 보자.)";
            pmtComtrol.AddNextAction("main", "store_out");
            break;
    }
}
    
    public void Debug_gotoBbobgi()
    {
        if (currentLocation == "bbobgi")
        {
            Debug_reloadBbobgi();
            return;
        }

        parkBtn.SetActive(false);
        GetComponent<Heart_Control>().SetHeart(-1);
        front_panel.SetActive(true);
        frontCha.GetComponent<Animator>().SetTrigger("walk");
        main_panel.GetComponent<Animator>().SetTrigger("hide");
        lower_bar.GetComponent<Animator>().SetTrigger("hide");
        currentLocation = "toStore";
        myAudio.PlayMusic(1);

        parkBtn.SetActive(false);
        pmtComtrol.Reset();
        var str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        pmtComtrol.AddString("훈이", str);
        prompter.GetComponent<Animator>().SetTrigger("show");
        prompter.SetActive(true);
        currentLocation = "toStore";

        for (;;)
        {
            var rnd = 5;

            if (myStoreType != rnd)
            {
                myStoreType = rnd;
                break;
            }
        }


        newStore = Instantiate(store_panel, storePanelHolder.transform);
        newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
        newStore.SetActive(true);
        newStore.GetComponent<Animator>().SetTrigger("show");
        goBtnText_ui.text = "집으로 돌아가기";
    }

    public void GotoCu()
    {
        if (currentLocation == "home")
        {
            if (GetComponent<Heart_Control>().heartCount >= 1)
            {
                parkBtn.SetActive(false);
                GetComponent<Heart_Control>().SetHeart(-1);
                front_panel.SetActive(true);
                frontCha.GetComponent<Animator>().SetTrigger("walk");
                main_panel.GetComponent<Animator>().SetTrigger("hide");
                lower_bar.GetComponent<Animator>().SetTrigger("hide");
                currentLocation = "toStore";
                myAudio.PlayMusic(1);
                GoToStore(0);
                notice.HideNoticePanel();
            }
            else
            {
                balloon.ShowMsg("지금은 피곤하다.. \n 너튜브나 보면서 쉴까..");
            }
        }
        else if (currentLocation == "store")
        {
            if (GetComponent<Heart_Control>().heartCount >= 1)
            {
                GetComponent<Heart_Control>().SetHeart(-1);
                newStore.GetComponent<Animator>().SetTrigger("hide");
                frontCha.GetComponent<Animator>().SetTrigger("walk");
                GoToStore(0);
                albaMode = false;
                currentLocation = "store_next";
                notice.HideNoticePanel();
            }
            else
            {
                balloon.ShowMsg("피곤하다.. \n 집으로 돌아가서 쉬는게 좋겠어..");
                currentLocation = "store";
            }
        }
        else
        {
            balloon.ShowMsg("여기서는 이동할 수 없어..\n집으로 돌아갈까?");
        }
    }

    public void Debug_reloadBbobgi()
    {
        newStore.GetComponent<StoreControl>().bbobgiPanel.bbobgi.StartGame();
    }

    public void OpenAppSetting()
    {
        Utilities.OpenApplicationSettings();
    }

    public void Debug_addMaple()
    {
        AddBBang(1, "maple");
    }

    public void Debug_addPurin()
    {
        AddBBang(1, "purin");
    }

    public void Debug_gotoGS()
    {
        if (currentLocation == "bbobgi")
        {
            Debug_reloadBbobgi();
            return;
        }

        parkBtn.SetActive(false);
        GetComponent<Heart_Control>().SetHeart(-1);
        front_panel.SetActive(true);
        frontCha.GetComponent<Animator>().SetTrigger("walk");
        main_panel.GetComponent<Animator>().SetTrigger("hide");
        lower_bar.GetComponent<Animator>().SetTrigger("hide");
        currentLocation = "toStore";
        myAudio.PlayMusic(1);

        parkBtn.SetActive(false);
        pmtComtrol.Reset();
        var str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        pmtComtrol.AddString("훈이", str);
        prompter.GetComponent<Animator>().SetTrigger("show");
        prompter.SetActive(true);
        currentLocation = "toStore";

        myStoreType = 6;

        newStore = Instantiate(store_panel, storePanelHolder.transform);
        newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
        newStore.SetActive(true);
        newStore.GetComponent<Animator>().SetTrigger("show");
        goBtnText_ui.text = "집으로 돌아가기";
    }
    
    public void Debug_gotoBlurBird()
    {
        if (currentLocation == "bbobgi")
        {
            Debug_reloadBbobgi();
            return;
        }

        parkBtn.SetActive(false);
        GetComponent<Heart_Control>().SetHeart(-1);
        front_panel.SetActive(true);
        frontCha.GetComponent<Animator>().SetTrigger("walk");
        main_panel.GetComponent<Animator>().SetTrigger("hide");
        lower_bar.GetComponent<Animator>().SetTrigger("hide");
        currentLocation = "toStore";
        myAudio.PlayMusic(1);

        parkBtn.SetActive(false);
        pmtComtrol.Reset();
        var str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        pmtComtrol.AddString("훈이", str);
        prompter.GetComponent<Animator>().SetTrigger("show");
        prompter.SetActive(true);
        currentLocation = "toStore";

        myStoreType = 7;

        newStore = Instantiate(store_panel, storePanelHolder.transform);
        newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
        newStore.SetActive(true);
        newStore.GetComponent<Animator>().SetTrigger("show");
        goBtnText_ui.text = "집으로 돌아가기";
    }
    
    public void Debug_gotoTanghuru()
    {
        if (currentLocation == "bbobgi")
        {
            Debug_reloadBbobgi();
            return;
        }

        parkBtn.SetActive(false);
        GetComponent<Heart_Control>().SetHeart(-1);
        front_panel.SetActive(true);
        frontCha.GetComponent<Animator>().SetTrigger("walk");
        main_panel.GetComponent<Animator>().SetTrigger("hide");
        lower_bar.GetComponent<Animator>().SetTrigger("hide");
        currentLocation = "toStore";
        myAudio.PlayMusic(1);

        parkBtn.SetActive(false);
        pmtComtrol.Reset();
        var str = "" + storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        pmtComtrol.AddString("훈이", str);
        prompter.GetComponent<Animator>().SetTrigger("show");
        prompter.SetActive(true);
        currentLocation = "toStore";

        myStoreType = 8;

        newStore = Instantiate(store_panel, storePanelHolder.transform);
        newStore.GetComponent<StoreControl>().UpdateStore(myStoreType);
        newStore.SetActive(true);
        newStore.GetComponent<Animator>().SetTrigger("show");
        goBtnText_ui.text = "집으로 돌아가기";
    }
}