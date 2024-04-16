using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;
using NotificationServices = VoxelBusters.EssentialKit.NotificationServices;
using Random = UnityEngine.Random;

public class DangunNotificationManager : MonoBehaviour
{
    private const string format = "yyyy/MM/dd HH:mm";

    [FormerlySerializedAs("heart")] [SerializeField]
    private PlayerHealthManager playerHealthManager;

    [FormerlySerializedAs("msg")] [SerializeField]
    private PhoneMessageController phoneMessageController;

    [SerializeField] private Text time_text_ui;
    private DateTime CULastNotiDate, CUArriveTime;
    private bool notificationSent;
    private IFormatProvider provider;

    private bool started;
    private TimeSpan timeGap;

    public void Start()
    {
        if (started) return;

        if (!PlayerPrefs.HasKey("CUArriveTime"))
        {
            PlayerPrefs.SetString("CUArriveTime", DateTime.Now.AddDays(-1).ToString(format));
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("notificationSent"))
        {
            PlayerPrefs.SetInt("notificationSent", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("CULastNotiDate"))
        {
            CULastNotiDate = DateTime.Now.AddDays(-1);
            PlayerPrefs.SetString("CULastNotiDate", CULastNotiDate.ToString(format));
            PlayerPrefs.Save();
        }

        ReadData();
        NotificationInitailize();
        started = true;
    }

    // Update is called once per frame
    public void UpdateCUTime()
    {
        timeGap = CUArriveTime - DateTime.Now;

        if ((timeGap.TotalHours <= 0) & (notificationSent == false))
        {
            print("|| UpdateCUTime | Time past detected");
            SendNotificationUI();
        }
    }

    //Call Once in a launce
    public void NotificationInitailize()
    {
        NotificationServices.RequestPermission(
            NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound |
            NotificationPermissionOptions.Badge, callback: (result, error) =>
            {
                Debug.Log("Request for access finished.");
                Debug.Log("Notification access status: " + result.PermissionStatus);
            });

        NotificationServices.CancelAllScheduledNotifications();
        NotificationServices.RemoveAllDeliveredNotifications();

        //GleyNotifications.Initialize();
        timeGap = CUArriveTime - DateTime.Now;
        print("|| NotificationInitailize | notificationSent : " + notificationSent + " | timeGap : " + timeGap);
        if (notificationSent) return;

        //Time hasn't passed
        if (timeGap.TotalHours > 0)
        {
            print("|| NotificationInitailize | Time hasn't passed");

            SetCUNotification(timeGap);
            //CULastNotiDate = System.DateTime.Today;
            //Notification not yet Time
        }
        else //Time has passed
        {
            print("|| NotificationInitailize | Time has past");
            SendNotificationUI();
        }
    }

    public void SendNotificationUI()
    {
        notificationSent = false;
        CULastNotiDate = DateTime.Now;
        timeGap = CUArriveTime - DateTime.Now;
        var PastTime = DateTime.Now - CUArriveTime;
        time_text_ui.text = "("; // + timeGap.Days + ""
        if (PastTime.Days > 0) time_text_ui.text += PastTime.Days + "일 ";
        if (PastTime.Hours > 0) time_text_ui.text += PastTime.Hours + "시간 ";
        time_text_ui.text += PastTime.Minutes + "분 전)";
        print("|| SendNotificationUI | UI MSG SHOW | timegap : " + PastTime);
        ShowNoticePanel();
    }

    public void NotificationSent()
    {
        notificationSent = true;
        CULastNotiDate = DateTime.Now;
        timeGap = CUArriveTime - DateTime.Now;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);

        var PastTime = DateTime.Now - CUArriveTime;
        time_text_ui.text = "("; // + timeGap.Days + ""
        if (PastTime.Days > 0) time_text_ui.text += PastTime.Days + "일 ";
        if (PastTime.Hours > 0) time_text_ui.text += PastTime.Hours + "시간 ";
        time_text_ui.text += PastTime.Minutes + "분 전)";
        print("|| NotificationSent | UI MSG SHOW | timegap : " + PastTime);
    }

    //Make Notification based on CUArriveTime data
    public void SetCUNotification(TimeSpan timeGap)
    {
        NotificationServices.GetSettings(result =>
        {
            var settings = result.Settings;
            Debug.Log(settings.ToString());
            Debug.Log("Permission status : " + settings.PermissionStatus);
            if (settings.PermissionStatus == NotificationPermissionStatus.Denied)
                phoneMessageController.SetMsg("시스템 설정에서 푸시 알림이 꺼져있어 알림을 받을 수 없습니다.\n시스템 설정으로 이동할까요?", 2,
                    "NotificationSetting");
        });

        var notification = NotificationBuilder.CreateNotification("notif-id-1")
            .SetTitle("포켓볼빵 더게임")
            .SetTimeIntervalNotificationTrigger(timeGap.TotalSeconds) //Setting the time interval to 10 seconds
            .SetBody("미소녀 알바생으로부터 문자 메시지가 도착했습니다!")
            .Create();

        NotificationServices.ScheduleNotification(notification, error =>
        {
            if (error == null)
            {
                Debug.Log("Request to schedule notification finished successfully.");
                //phoneMessageController.SetMsg("Request to schedule notification finished successfully.", 1);
            }
            else
            {
                Debug.Log("Request to schedule notification failed with error. Error: " + error);
                phoneMessageController.SetMsg("알림을 설정하는 데 오류가 발생했습니다. 오류코드: " + error, 1);
            }
        });
        //GleyNotifications.SendNotification("포켓볼빵 더게임", "미소녀 알바생으로부터 문자 메시지가 도착했습니다!", timeGap, "icon_0", "icon_1", "CUNotification");
    }

    public void NotificationSetting()
    {
        Utilities.OpenApplicationSettings();
        phoneMessageController.SetMsg("확인 버튼을 누르면 알림을 재설정 합니다.", 1, "NotificationReset");
    }

    public void NotificationReset()
    {
        NotificationInitailize();
        MakeAndSetNotification();
    }

    //Create new CU Notification
    public void MakeAndSetNotification()
    {
        CUArriveTime = UpdateTime();
        timeGap = CUArriveTime - DateTime.Now;
        print("|| MakeAndSetNotification | New Notification Set : " + CUArriveTime.ToString(format));
        SetCUNotification(timeGap);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
    }

    public DateTime UpdateTime()
    {
        DateTime CUArriveTime;
        if (CULastNotiDate.Date == DateTime.Today.Date)
        {
            CUArriveTime = DateTime.Today.AddDays(1).AddHours(Random.Range(7, 23)).AddMinutes(Random.Range(0, 60));
            print(CUArriveTime.ToString(format));
        }
        else
        {
            var startTime = DateTime.Today.AddHours(8);
            var endTime = DateTime.Today.AddHours(22);
            var endTimeGap = endTime - DateTime.Now;

            if (endTimeGap.TotalHours > 1)
            {
                CUArriveTime = DateTime.Today.AddHours(Random.Range(0f, (float)endTimeGap.TotalHours)).AddHours(1);
                var startTimeGap = CUArriveTime - startTime;
                if (startTimeGap.TotalHours < 0) CUArriveTime = CUArriveTime.AddHours(8);
            }
            else
            {
                CUArriveTime = DateTime.Today.AddDays(1).AddHours(Random.Range(7, 23)).AddMinutes(Random.Range(0, 60));
            }
        }

        print("|| UpdateTime | New CUArriveTime : " + CUArriveTime.ToString(format));
        return CUArriveTime;
    }

    public void ParseData(DateTime CULastNotiDate, DateTime CUArriveTime, bool notificationSent)
    {
        var mySent = notificationSent ? 1 : 0;
        PlayerPrefs.SetString("CULastNotiDate", CULastNotiDate.ToString(format));
        PlayerPrefs.SetString("CUArriveTime", CUArriveTime.ToString(format));
        PlayerPrefs.SetInt("notificationSent", mySent);
        PlayerPrefs.Save();

        playerHealthManager.UpdateBbangInfo();
        ReadData();
    }

    public void ReadData()
    {
        if (PlayerPrefs.GetString("CULastNotiDate") == null) Start();

        CULastNotiDate = DateTime.ParseExact(PlayerPrefs.GetString("CULastNotiDate"), format, provider);
        CUArriveTime = DateTime.ParseExact(PlayerPrefs.GetString("CUArriveTime"), format, provider);
        notificationSent = PlayerPrefs.GetInt("notificationSent") == 1;

        print("|| ReadData | CULastNotiDate : " + CULastNotiDate.ToString(format));
        print("|| ReadData | CUArriveTime : " + CUArriveTime.ToString(format));
        print("|| ReadData | notificationSent : " + notificationSent);
    }

    public void ShowNoticePanel()
    {
        if (gameObject.activeSelf == false ||
            !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("notification_show"))
            gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    public void HideNoticePanel()
    {
        gameObject.GetComponent<Animator>().SetTrigger("hide");
    }

    public void GoToCuStore()
    {
        //
    }

    public void Debug_SetAddMinutes()
    {
        CULastNotiDate = DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = DateTime.Now.AddMinutes(1);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - DateTime.Now;
        SetCUNotification(timeGap);
    }

    public void Debug_SetSubNegative60min()
    {
        CULastNotiDate = DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = DateTime.Now.AddMinutes(-60);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - DateTime.Now;
        SetCUNotification(timeGap);
    }

    public void Debug_SetSubMinutes()
    {
        CULastNotiDate = DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = DateTime.Now.AddMinutes(-1);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - DateTime.Now;
        SetCUNotification(timeGap);
    }

    public float TimeGap()
    {
        var PastTime = DateTime.Now - CUArriveTime;
        return (float)PastTime.TotalMinutes;
    }
}