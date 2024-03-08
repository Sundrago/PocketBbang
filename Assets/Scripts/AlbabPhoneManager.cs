using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbabPhoneManager : MonoBehaviour
{
    [SerializeField] private PhoneMsgCtrl phoneMsgCtrl;
    [SerializeField] private List<Image> btnImgs;
    [SerializeField] private Main_control main;
    [SerializeField] private BalloonControl balloon;
    private List<bool> available;
    public static AlbabPhoneManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        available = new List<bool>() {false, false, false};
        available[0] = (PlayerPrefs.GetInt("albaExp", 0) != 0);
        available[1] = (PlayerPrefs.GetInt("tanghuru_friend", 0) != 0);
        available[2] = (PlayerPrefs.GetInt("bossam_friend", 0) != 0);

        for (int i = 0; i < 3; i++)
        {
            btnImgs[i].color = available[i] ? Color.white : new Color(1, 1, 1, 0.5f);
        }
    }
    
    public void AbabBtnClicked(int idx)
    {
        if (!available[idx])
        {
            switch (idx)
            {
                case 0:
                    phoneMsgCtrl.SetMsg("지금은 알바 자리가 없습니다.\n쌈마트24에 가서 맛동석 점장에게 얘기해보는 건 어떨까요?", 1);
                    return;
                    break;
                case 1:
                    phoneMsgCtrl.SetMsg("지금은 알바 자리가 없습니다.\n탕후루 가게에서 왕형을 열심히 도와보는 건 어떨까요?", 1);
                    return;
                    break;
                case 2:
                    phoneMsgCtrl.SetMsg("지금은 알바 자리가 없습니다.\n장족발보쌈 가게에서 장왕을 열심히 도와보는 건 어떨까요?", 1);
                    return;
                    break;
            }
        }
        
        if (Heart_Control.Instance.IsHeartEmpty())
        {
            balloon.ShowMsg("지금은 좀 피곤하다..");
            return;
        }

        if ((main.currentLocation != "home") && (main.currentLocation != "store"))
        {
            balloon.ShowMsg("여기서는 할 수 없다..");
            return;
        }
        
        PlayerPrefs.SetInt("GotoAlbaIdx", idx);
        switch (idx)
        {
            case 0:
                phoneMsgCtrl.SetMsg("알바를 하기 위해\n3마트24로 이동할까요?", 2, "albaGo");
                break;
            case 1:
                phoneMsgCtrl.SetMsg("알바를 하기 위해\n탕후루가게로 이동할까요?", 2, "albaGo");
                break;
            case 2:
                phoneMsgCtrl.SetMsg("알바를 하기 위해\n왕충동장보쌈으로 이동할까요?", 2, "albaGo");
                break;
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    /*
     * if ((myStoreType == 0) & (PlayerPrefs.GetInt("girl_friend") != 0))
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
       else if (myStoreType == 9 && PlayerPrefs.GetInt("bossam_friend", 0) == 1)
       {
       achievement.UpdateAchievement("장왕");
       }
     */
}
