using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneMsgCtrl : MonoBehaviour
{
    [SerializeField] BbangBtnControl bbangBtn;
    [SerializeField] DangunControl dangun;
    [SerializeField] DangunChaCtrl dangunCha;
    [SerializeField] NotificationCtrl notice;
    [SerializeField] CloudSaving cloud;
    [SerializeField] IronSourceAd iron;
    [SerializeField] GameObject yes_ui, no_ui, ok_ui, main, msg_ui, parentPanel;
    
    public string actionCode;
    public bool initiateMode = true;
    bool started = false;

    public void Start()
    {
        if (started) return;
        started = true;
    }

    public void OkbtnClicked()
    {
        print("action Code : " + actionCode);
        if (actionCode == "eat") {
            bbangBtn.Eat();
        } else if (actionCode == "OpenCardSelection")
        {
            print("OpenCardSelection");
            dangunCha.OpenCardSelection();
        } else if (actionCode == "NotificationReset")
        {
            notice.NotificationReset();
        } else if (actionCode == "RequestAuthorizationTracking")
        {
            iron.RequestAuthorizationTracking();
        }
        gameObject.SetActive(false);
        if(initiateMode) Destroy(gameObject);
    }

    public void SetMsg(string msg, int btnCount, string action = "")
    {
        if (initiateMode)
        {
            GameObject newMsg = Instantiate(gameObject, parentPanel.transform);

            newMsg.GetComponent<PhoneMsgCtrl>().msg_ui.GetComponent<Text>().text = msg;
            if (btnCount == 1)
            {
                newMsg.GetComponent<PhoneMsgCtrl>().yes_ui.SetActive(false);
                newMsg.GetComponent<PhoneMsgCtrl>().no_ui.SetActive(false);
                newMsg.GetComponent<PhoneMsgCtrl>().ok_ui.SetActive(true);
            }
            else if (btnCount == 2)
            {
                newMsg.GetComponent<PhoneMsgCtrl>().yes_ui.SetActive(true);
                newMsg.GetComponent<PhoneMsgCtrl>().no_ui.SetActive(true);
                newMsg.GetComponent<PhoneMsgCtrl>().ok_ui.SetActive(false);
            }

            newMsg.GetComponent<PhoneMsgCtrl>().actionCode = action;
            newMsg.GetComponent<PhoneMsgCtrl>().gameObject.SetActive(true);
        }
        else
        {
            msg_ui.GetComponent<Text>().text = msg;
            if (btnCount == 1)
            {
                yes_ui.SetActive(false);
                no_ui.SetActive(false);
                ok_ui.SetActive(true);
            }
            else if (btnCount == 2)
            {
                yes_ui.SetActive(true);
                no_ui.SetActive(true);
                ok_ui.SetActive(false);
            }

            GetComponent<PhoneMsgCtrl>().actionCode = action;
            gameObject.SetActive(true);
        }
    }

    public void YesBtnClicked()
    {
        switch (actionCode)
        {
            case "albaGo":
                AlbabPhoneManager.Instance.gameObject.SetActive(false);
                main.GetComponent<Main_control>().GoToAlba();
                break;
            case "eat":
                bbangBtn.Eat();
                break;
            case "debug":
                main.GetComponent<Main_control>().OpenDebug();
                break;
            case "matdongsan":
                if (PlayerPrefs.GetInt("money") >= 10000)
                {
                    main.GetComponent<Main_control>().StoreEvent("host_buy");
                } else
                {
                    main.GetComponent<Main_control>().StoreEvent("host_buy_noMoney");
                }
                
                break;
            case "matdongsanf":
                if (PlayerPrefs.GetInt("money") >= 10000)
                {
                    main.GetComponent<Main_control>().StoreEvent("hostf_buy");
                }
                else
                {
                    main.GetComponent<Main_control>().StoreEvent("hostf_buy_noMoney");
                }

                break;
            case "BbangBoyCallBack":
                dangun.BbangBoyCallBack();
                break;
            case "SellMatCallBack":
                dangun.SellMatCallBack();
                break;
            case "BuyHotCallBack":
                dangun.BuyHotCallBack();
                break;
            case "BuyVacanceCallBack":
                dangun.BuyVacanceCallBack();
                break;
            case "BuyCardCallBack":
                dangun.BuyCardCallBack();
                break;
            case "SellCardCallBack":
                dangun.SellCardCallBack();
                break;
            case "BuyYogurtCallBack":
                dangun.BuyYogurtCallBack();
                break;
            case "SelectAgain":
                    dangunCha.SelectAgain(true);
                break;
            case "WatchAdsToResetDangun":
                dangun.WatchAdsToResetDangun();
                break;
            case "QuitApp":
                main.GetComponent<Main_control>().QuitApp();
                break;
            case "DangunChaMsgCallBack":
                dangunCha.DangunChaMsgCallBack(-1);
                break;
            case "SendtoSetting":
                main.GetComponent<Main_control>().SendtoSetting();
                break;
            case "NotificationSetting":
                notice.NotificationSetting();
                break;
            //AskToLoad
            case "AskToLoad":
                cloud.AskToLoad_Yes();
                break;
            case "AskToLoad_No":
                cloud.AskToLoad_No2();
                break;
            case "RequestAuthorizationTracking":
                iron.RequestAuthorizationTracking();
                break;
        }
        gameObject.SetActive(false);
        if (initiateMode) Destroy(gameObject);
    }

    public void NoBtnClicked()
    {
        switch (actionCode)
        {
            case "matdongsan":
                main.GetComponent<Main_control>().StoreEvent("host_notbuy");
                break;
            case "SelectAgain":
                {
                    dangunCha.SelectAgain(false);
                }
                break;
            case "DangunChaMsgCallBack":
                dangunCha.endDangun = true;
                dangunCha.DangunChaMsgCallBack(-1);
                break;
            case "matdongsanf":
                main.GetComponent<Main_control>().StoreEvent("hostf_notbuy");
                break;
            case "AskToLoad":
                cloud.AskToLoad_No();
                break;
            case "AskToLoad_No":
                cloud.AskToLoad();
                break;
        }
        gameObject.SetActive(false);
        if (initiateMode) Destroy(gameObject);
    }
}
