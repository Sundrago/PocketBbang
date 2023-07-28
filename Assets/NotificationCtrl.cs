using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class NotificationCtrl : MonoBehaviour
{
    const string format = "yyyy/MM/dd HH:mm";
    System.IFormatProvider provider;
    System.DateTime CULastNotiDate, CUArriveTime;
    System.TimeSpan timeGap;
    bool notificationSent;
    public Heart_Control heart;
    public PhoneMsgCtrl msg;

    bool started = false;
    public Text time_text_ui;


    // Start is called before the first frame update

    public void Start()
    {
        /*
        if (!PlayerPrefs.HasKey("flag_notification"))
            PlayerPrefs.SetInt("flag_notification", 1);
        */

        if (started) return;

        /*
        if(PlayerPrefs.GetInt("flag_notification") == 0) {
            msg.SetMsg("앱이 충돌한 이력이 있어 알림 기능이 꺼졌습니다. 설정에서 다시 켤 수 있습니다.", 1);
            return;
        }
        PlayerPrefs.SetInt("flag_notification", 0);
        PlayerPrefs.Save();
        */

        //if (gameObject.activeSelf) gameObject.SetActive(false);
        if (!PlayerPrefs.HasKey("CUArriveTime"))
        {
            PlayerPrefs.SetString("CUArriveTime", System.DateTime.Now.AddDays(-1).ToString(format));
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("notificationSent"))
        {
            PlayerPrefs.SetInt("notificationSent", 1);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("CULastNotiDate"))
        {
            CULastNotiDate = System.DateTime.Now.AddDays(-1);
            PlayerPrefs.SetString("CULastNotiDate", CULastNotiDate.ToString(format));
            PlayerPrefs.Save();
        }

        ReadData();
        NotificationInitailize();
        started = true;

        //PlayerPrefs.SetInt("flag_notification", 1);
        //PlayerPrefs.Save();
    }

    // Update is called once per frame
    public void UpdateCUTime()
    {
        timeGap = CUArriveTime - System.DateTime.Now;

        if (timeGap.TotalHours <= 0 & notificationSent == false)
        {
            print("|| UpdateCUTime | Time past detected");
            SendNotificationUI();
        }
    }

    //Call Once in a launce
    public void NotificationInitailize()
    {
        VoxelBusters.EssentialKit.NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge, callback: (result, error) =>
        {
            Debug.Log("Request for access finished.");
            Debug.Log("Notification access status: " + result.PermissionStatus);
        });

        VoxelBusters.EssentialKit.NotificationServices.CancelAllScheduledNotifications();
        VoxelBusters.EssentialKit.NotificationServices.RemoveAllDeliveredNotifications();

        //GleyNotifications.Initialize();
        timeGap = CUArriveTime - System.DateTime.Now;
        print("|| NotificationInitailize | notificationSent : " + notificationSent + " | timeGap : " + timeGap.ToString());
        if (notificationSent) return;
        
        //Time hasn't passed
        if (timeGap.TotalHours > 0)
        {
            print("|| NotificationInitailize | Time hasn't passed");

            SetCUNotification(timeGap);
            //CULastNotiDate = System.DateTime.Today;
            //Notification not yet Time
        } else //Time has passed
        {
            print("|| NotificationInitailize | Time has past");
            SendNotificationUI();
        }
    }

    public void SendNotificationUI()
    {
        notificationSent = false;
        CULastNotiDate = System.DateTime.Now;
        timeGap = CUArriveTime - System.DateTime.Now;
        System.TimeSpan PastTime = System.DateTime.Now - CUArriveTime;
        time_text_ui.text = "("; // + timeGap.Days + ""
        if (PastTime.Days > 0) time_text_ui.text += PastTime.Days + "일 ";
        if (PastTime.Hours > 0) time_text_ui.text += PastTime.Hours + "시간 ";
        time_text_ui.text += PastTime.Minutes + "분 전)";
        print("|| SendNotificationUI | UI MSG SHOW | timegap : " + PastTime.ToString());
        ShowNoticePanel();
    }

    public void NotificationSent()
    {
        notificationSent = true;
        CULastNotiDate = System.DateTime.Now;
        timeGap = CUArriveTime - System.DateTime.Now;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);

        System.TimeSpan PastTime = System.DateTime.Now - CUArriveTime;
        time_text_ui.text = "("; // + timeGap.Days + ""
        if (PastTime.Days > 0) time_text_ui.text += PastTime.Days + "일 ";
        if (PastTime.Hours > 0) time_text_ui.text += PastTime.Hours + "시간 ";
        time_text_ui.text += PastTime.Minutes + "분 전)";
        print("|| NotificationSent | UI MSG SHOW | timegap : " + PastTime.ToString());
    }

    //Make Notification based on CUArriveTime data
    public void SetCUNotification(System.TimeSpan timeGap)
    {
        VoxelBusters.EssentialKit.NotificationServices.GetSettings((result) =>
        {
            NotificationSettings settings = result.Settings;
            Debug.Log(settings.ToString());
            Debug.Log("Permission status : " + settings.PermissionStatus);
            if (settings.PermissionStatus == NotificationPermissionStatus.Denied)
            {
                msg.SetMsg("시스템 설정에서 푸시 알림이 꺼져있어 알림을 받을 수 없습니다.\n시스템 설정으로 이동할까요?", 2, "NotificationSetting");
            }
        });


        print("|| SetCUNotification | Notification Set At timespan.hrs : " + timeGap.TotalHours);

        INotification notification = NotificationBuilder.CreateNotification("notif-id-1")
    .SetTitle("포켓볼빵 더게임")
    .SetTimeIntervalNotificationTrigger(timeGap.TotalSeconds) //Setting the time interval to 10 seconds
    .SetBody("미소녀 알바생으로부터 문자 메시지가 도착했습니다!")
    .Create();

        VoxelBusters.EssentialKit.NotificationServices.ScheduleNotification(notification, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to schedule notification finished successfully.");
                //msg.SetMsg("Request to schedule notification finished successfully.", 1);
            }
            else
            {
                Debug.Log("Request to schedule notification failed with error. Error: " + error);
                msg.SetMsg("알림을 설정하는 데 오류가 발생했습니다. 오류코드: " + error, 1);
            }
        });
        //GleyNotifications.SendNotification("포켓볼빵 더게임", "미소녀 알바생으로부터 문자 메시지가 도착했습니다!", timeGap, "icon_0", "icon_1", "CUNotification");
    }

    public void NotificationSetting()
    {
        Utilities.OpenApplicationSettings();
        msg.SetMsg("확인 버튼을 누르면 알림을 재설정 합니다.", 1, "NotificationReset");
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
        timeGap = CUArriveTime - System.DateTime.Now;
        print("|| MakeAndSetNotification | New Notification Set : " + CUArriveTime.ToString(format));
        SetCUNotification(timeGap);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
    }

    public System.DateTime UpdateTime()
    {
        System.DateTime CUArriveTime;
        if (CULastNotiDate.Date == System.DateTime.Today.Date)
        {
            CUArriveTime = System.DateTime.Today.AddDays(1).AddHours(Random.Range(7, 23)).AddMinutes(Random.Range(0, 60));
            print(CUArriveTime.ToString(format));
        } else
        {
            System.DateTime startTime = System.DateTime.Today.AddHours(8);
            System.DateTime endTime = System.DateTime.Today.AddHours(22);
            System.TimeSpan endTimeGap = endTime - System.DateTime.Now;

            if (endTimeGap.TotalHours > 1)
            {
                CUArriveTime = System.DateTime.Today.AddHours(Random.Range(0f, (float)endTimeGap.TotalHours)).AddHours(1);
                System.TimeSpan startTimeGap = CUArriveTime - startTime;
                if (startTimeGap.TotalHours < 0)
                {
                    CUArriveTime = CUArriveTime.AddHours(8);
                }
            }
            else
            {
                CUArriveTime = System.DateTime.Today.AddDays(1).AddHours(Random.Range(7, 23)).AddMinutes(Random.Range(0, 60));
            }
        }
        print("|| UpdateTime | New CUArriveTime : " + CUArriveTime.ToString(format));
        return CUArriveTime;
    }

    public void ParseData(System.DateTime CULastNotiDate, System.DateTime CUArriveTime, bool notificationSent)
    {
        int mySent = notificationSent ? 1 : 0;
        PlayerPrefs.SetString("CULastNotiDate", CULastNotiDate.ToString(format));
        PlayerPrefs.SetString("CUArriveTime", CUArriveTime.ToString(format));
        PlayerPrefs.SetInt("notificationSent", mySent);
        PlayerPrefs.Save();

        heart.UpdateBbangInfo();
        ReadData();
    }

    public void ReadData()
    {
        if (PlayerPrefs.GetString("CULastNotiDate") == null) Start();

        CULastNotiDate = System.DateTime.ParseExact(PlayerPrefs.GetString("CULastNotiDate"), format, provider);
        CUArriveTime = System.DateTime.ParseExact(PlayerPrefs.GetString("CUArriveTime"), format, provider);
        notificationSent = (PlayerPrefs.GetInt("notificationSent") == 1);

        print("|| ReadData | CULastNotiDate : " + CULastNotiDate.ToString(format));
        print("|| ReadData | CUArriveTime : " + CUArriveTime.ToString(format));
        print("|| ReadData | notificationSent : " + notificationSent);
    }

    public void ShowNoticePanel()
    {
        if(gameObject.activeSelf == false || !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("notification_show"))
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
        CULastNotiDate = System.DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = System.DateTime.Now.AddMinutes(1);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - System.DateTime.Now;
        SetCUNotification(timeGap);
    }

    public void Debug_SetSubNegative60min()
    {
        CULastNotiDate = System.DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = System.DateTime.Now.AddMinutes(-60);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - System.DateTime.Now;
        SetCUNotification(timeGap);
    }

    public void Debug_SetSubMinutes()
    {
        CULastNotiDate = System.DateTime.Now;
        CULastNotiDate = CULastNotiDate.AddDays(-1);

        CUArriveTime = System.DateTime.Now.AddMinutes(-1);
        notificationSent = false;
        ParseData(CULastNotiDate, CUArriveTime, notificationSent);
        timeGap = CUArriveTime - System.DateTime.Now;
        SetCUNotification(timeGap);
    }

    public float TimeGap()
    {
        System.TimeSpan PastTime = System.DateTime.Now - CUArriveTime;
        return (float)PastTime.TotalMinutes;
    }
}
