using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PhoneMessageController : MonoBehaviour
{
    [Header("Managers and Controllers")] [SerializeField]
    private BbangInteractionUIController bbangBtn;

    [SerializeField] private DangunManager dangun;

    [FormerlySerializedAs("dangunCharacterController")] [FormerlySerializedAs("dangunCha")] [SerializeField]
    private DangunCharacterManager dangunCharacterManager;

    [FormerlySerializedAs("notice")] [SerializeField]
    private DangunNotificationManager dangunNotificationManager;

    [FormerlySerializedAs("cloud")] [SerializeField]
    private CloudSaving cloudSaving;

    [FormerlySerializedAs("iron")] [SerializeField]
    private IronSourceAd ironSourceAd;

    [Header("UI Elements")] [SerializeField]
    private GameObject yes_ui, no_ui, ok_ui, main, msg_ui, parentPanel;

    public string actionCode;
    public bool initiateMode = true;
    private bool started;

    public void Start()
    {
        if (started) return;
        started = true;
    }

    public void OkbtnClicked()
    {
        if (actionCode == "eat")
            bbangBtn.Eat();
        else if (actionCode == "OpenCardSelection")
            dangunCharacterManager.OpenCardSelection();
        else if (actionCode == "NotificationReset")
            dangunNotificationManager.NotificationReset();
        else if (actionCode == "RequestAuthorizationTracking") ironSourceAd.RequestAuthorizationTracking();
        gameObject.SetActive(false);
        if (initiateMode) Destroy(gameObject);
    }

    public void SetMsg(string msg, int btnCount, string action = "")
    {
        if (initiateMode)
        {
            var newMsg = Instantiate(gameObject, parentPanel.transform);

            var phoneMessageController = newMsg.GetComponent<PhoneMessageController>();

            phoneMessageController.msg_ui.GetComponent<Text>().text = msg;
            if (btnCount == 1)
            {
                phoneMessageController.yes_ui.SetActive(false);
                phoneMessageController.no_ui.SetActive(false);
                phoneMessageController.ok_ui.SetActive(true);
            }
            else if (btnCount == 2)
            {
                phoneMessageController.yes_ui.SetActive(true);
                phoneMessageController.no_ui.SetActive(true);
                phoneMessageController.ok_ui.SetActive(false);
            }

            phoneMessageController.actionCode = action;
            phoneMessageController.gameObject.SetActive(true);
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

            GetComponent<PhoneMessageController>().actionCode = action;
            gameObject.SetActive(true);
        }
    }

    public void YesBtnClicked()
    {
        switch (actionCode)
        {
            case "albaGo":
                AlbaPhoneManager.Instance.gameObject.SetActive(false);
                main.GetComponent<GameManager>().GoToAlba();
                break;
            case "eat":
                bbangBtn.Eat();
                break;
            case "debug":
                main.GetComponent<GameManager>().OpenDebug();
                break;
            case "matdongsan":
                if (PlayerPrefs.GetInt("money") >= 10000)
                    main.GetComponent<GameManager>().StoreEvent("host_buy");
                else
                    main.GetComponent<GameManager>().StoreEvent("host_buy_noMoney");

                break;
            case "matdongsanf":
                if (PlayerPrefs.GetInt("money") >= 10000)
                    main.GetComponent<GameManager>().StoreEvent("hostf_buy");
                else
                    main.GetComponent<GameManager>().StoreEvent("hostf_buy_noMoney");

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
                dangunCharacterManager.SelectAgain(true);
                break;
            case "WatchAdsToResetDangun":
                dangun.WatchAdsToResetDangun();
                break;
            case "QuitApp":
                main.GetComponent<GameManager>().QuitApp();
                break;
            case "DangunChaMsgCallBack":
                dangunCharacterManager.DangunChaMsgCallBack(-1);
                break;
            case "SendtoSetting":
                main.GetComponent<GameManager>().SendtoSetting();
                break;
            case "NotificationSetting":
                dangunNotificationManager.NotificationSetting();
                break;
            case "AskToLoad":
                cloudSaving.AskToLoad_Yes();
                break;
            case "AskToLoad_No":
                cloudSaving.AskToLoad_No2();
                break;
            case "RequestAuthorizationTracking":
                ironSourceAd.RequestAuthorizationTracking();
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
                main.GetComponent<GameManager>().StoreEvent("host_notbuy");
                break;
            case "SelectAgain":
            {
                dangunCharacterManager.SelectAgain(false);
            }
                break;
            case "DangunChaMsgCallBack":
                dangunCharacterManager.EndDangun = true;
                dangunCharacterManager.DangunChaMsgCallBack(-1);
                break;
            case "matdongsanf":
                main.GetComponent<GameManager>().StoreEvent("hostf_notbuy");
                break;
            case "AskToLoad":
                cloudSaving.AskToLoad_No();
                break;
            case "AskToLoad_No":
                cloudSaving.AskToLoad();
                break;
        }

        gameObject.SetActive(false);
        if (initiateMode) Destroy(gameObject);
    }
}