using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class CloudSaving : MonoBehaviour
{
    [SerializeField] CollectionControl collection;
    [SerializeField] BalloonControl balloon;
    [SerializeField] PhoneMsgCtrl msg;
    [SerializeField] GameObject chaSelect;
    [SerializeField] Bbang_showroom bbang_Showrooml;
    [SerializeField] Main_control main;
    [SerializeField] SettingPanelControl setting;

    public bool save = true;
    const string format = "yyyy/MM/dd HH:mm:ss";

    private void OnEnable()
    {
        // register for events
        CloudServices.OnUserChange += OnUserChange;
        CloudServices.OnSavedDataChange += OnSavedDataChange;
        CloudServices.OnSynchronizeComplete += OnSynchronizeComplete;
    }

    private void OnDisable()
    {
        
        // unregister from events
        CloudServices.OnUserChange -= OnUserChange;
        CloudServices.OnSavedDataChange -= OnSavedDataChange;
        CloudServices.OnSynchronizeComplete -= OnSynchronizeComplete;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("autoSave")) {
            PlayerPrefs.SetInt("autoSave", 1);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("autoSave") == 0)
            return;

        DontDestroyOnLoad(gameObject);
        if (CloudServices.IsAvailable())
        {
            CloudServices.Synchronize(); // First call to syncronize triggers google play services login prompt if required - on Android.
        } else
        {
#if UNITY_IPHONE
            balloon.ShowMsg("아이폰 설정에서 앺애플 게임 센터를 켜주세요!");
#elif UNITY_ANDROID
            balloon.ShowMsg("플레이 게임 서비스에 로그인해주세요!");
#endif
        }
    }

    private void OnSynchronizeComplete(CloudServicesSynchronizeResult result)
    {
        Debug.Log("Received synchronize finish callback.");
        Debug.Log("Status: " + result.Success);
        // By this time, you have the latest data from cloud and you can start reading.

        print("CLOUD myRealTotalCard : " + CloudServices.GetInt("myRealTotalCard"));
        print("PLAYER myRealTotalCard : " + PlayerPrefs.GetInt("myRealTotalCard"));

        if (CloudServices.GetInt("myRealTotalCard") > PlayerPrefs.GetInt("myRealTotalCard"))
        {
            AskToLoad();
        }
        else if (CloudServices.GetInt("myRealTotalCard") < PlayerPrefs.GetInt("myRealTotalCard"))
        {
            SaveData(true);
            balloon.ShowMsg("데이터 저장 완료");
        } else
        {
            // balloon.ShowMsg("데이터 동기화 완료");
        }
    }

    public void AskToLoad()
    {
        msg.SetMsg("클라우드에 저장된 데이터를 찾았습니다.\n" + CloudServices.GetString("SavedTime") + "에 저장된 데이터를 불러올까요?\n\n (닉네임 : " + CloudServices.GetString("ChaName") + ")\n(전체 카드 숫자 : " + CloudServices.GetInt("myRealTotalCard") + ")", 2, "AskToLoad");
    }

    public void AskToLoad_No()
    {
        msg.SetMsg("기기의 데이터를 업로드합니다.\n클라우드에 저장된 데이터는 유실됩니다.", 2, "AskToLoad_No");
    }

    public void AskToLoad_No2()
    {
        SaveData(true);
    }

    public void AskToLoad_Yes()
    {
        SaveData(false);
        if (chaSelect.activeSelf)
        {
            chaSelect.SetActive(false);
        }
        bbang_Showrooml.UpdateBbangShow();
        main.UpdateBbangCount();
        setting.Start();
        balloon.ShowMsg("데이터 로드 완료");
    }

    private void OnUserChange(CloudServicesUserChangeResult result, Error error)
    {
        Debug.Log(result.User);
    }

    //This gets triggered if the data changed externally on another device while playing on this device.
    private void OnSavedDataChange(CloudServicesSavedDataChangeResult result)
    {
    }

    private void SaveInt(string name)
    {
        if(save)
        {
            if (CloudServices.GetInt(name) != PlayerPrefs.GetInt(name))
            {
                if (PlayerPrefs.HasKey(name))
                {
                    print("SAVE : " + name + "(" + PlayerPrefs.GetInt(name) + "->" + CloudServices.GetInt(name) + ")");
                    CloudServices.SetInt(name, PlayerPrefs.GetInt(name));
                }
            }
        } else
        {
            if (CloudServices.GetInt(name) != PlayerPrefs.GetInt(name))
            {
                print("SAVE : " + name + "(" + PlayerPrefs.GetInt(name) + "<-" + CloudServices.GetInt(name) + ")");
                PlayerPrefs.SetInt(name, CloudServices.GetInt(name));
            }
        }
        
    }

    private void SaveString(string name)
    {
        if (save)
        {
            if (CloudServices.GetString(name) != PlayerPrefs.GetString(name))
                if(PlayerPrefs.HasKey(name))
                    CloudServices.SetString(name, PlayerPrefs.GetString(name));
        } else
        {
            if (CloudServices.GetString(name) != PlayerPrefs.GetString(name))
                if(CloudServices.GetString(name) != null)
                    PlayerPrefs.SetString(name, CloudServices.GetString(name));
        }
    }

    private void SaveData(bool mySave)
    {
        save = mySave;

        if(save)
        {
            CloudServices.SetString("SavedTime", System.DateTime.Now.ToString(format));
        }
        SaveInt("myRealTotalCard");
        SaveInt("CollectionTab");
        SaveInt("heartCount");
        SaveInt("debugMode");

        SaveInt("sellPrice0");
        SaveInt("sellPrice1");
        SaveInt("sellIdx0");
        SaveInt("sellIdx1");
        SaveInt("sellUpdated0");
        SaveInt("sellUpdated1");

        SaveInt("bbang");
        SaveInt("bbang_yogurt");
        SaveInt("bbang_mat");
        SaveInt("bbang_choco");
        SaveInt("bbang_bingle");
        SaveInt("bbang_hot");
        SaveInt("bbang_vacance");
        SaveInt("bbang_strawberry");
        SaveInt("bbang_maple");

        SaveInt("bbangEatCount");

        SaveInt("bbang_yogurt_total");
        SaveInt("bbang_bingle_total");
        SaveInt("bbang_vacance_total");

        SaveInt("new_choco");
        SaveInt("new_strawberry");
        SaveInt("new_bingle");
        SaveInt("new_maple");

        SaveInt("Matdongsan");
        SaveInt("TotalCards");
        SaveInt("myTotalCards");
        SaveInt("totalBbangCount");
        SaveInt("storeCount");
        SaveInt("currentBbangCount");
        SaveInt("flag_notification");
        SaveInt("money");
        SaveInt("totalMoney"); //
        SaveInt("went");
        SaveInt("new_hot");
        SaveInt("lowDataMode");
        SaveInt("received");
        SaveInt("muteAudio");
        SaveInt("ChaIdx");
        SaveInt("bbobgiCount"); //
        SaveInt("ShuttleCount");
        SaveInt("adCount");
        SaveInt("storeDateCount");
        SaveInt("albaTime"); //
        SaveInt("albaCustomerCount");
        SaveInt("notificationSent");
        SaveInt("autoSave");

        SaveInt("girl_friend");
        SaveInt("bi_friend");
        SaveInt("yang_friend");
        SaveInt("albaExp");


        SaveString("CUArriveTime");
        SaveString("oldTime");
        SaveString("ChaName");
        SaveString("arriveTime");
        SaveString("CULastNotiDate");
        SaveString("storeDate");

        SaveString("dangun0");
        SaveString("dangun1");
        SaveString("dangun2");
        SaveString("dangun3");
        SaveString("dangun4");
        SaveString("dangun5");
        SaveString("dangun6");

        collection.Start();

        for (int i = 0; i < collection.myCard.Count; i++)
        {
            SaveInt("card_" + i);
        }

        PlayerPrefs.Save();
    }

    public void SyncNow()
    {
        CloudServices.Synchronize();
    }

    private void OnApplicationPause(bool pause)
    {
        if(PlayerPrefs.GetInt("autoSave") == 1)
        SyncNow();
    }

    private void OnApplicationQuit()
    {
        if (PlayerPrefs.GetInt("autoSave") == 1)
            SyncNow();
    }
}