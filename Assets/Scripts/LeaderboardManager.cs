using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

#if UNITY_IPHONE
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#elif UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LeaderboardManager : MonoBehaviour
{
    public Collection_Panel_Control collection;
    public BalloonControl balloon;
    public PhoneMsgCtrl msg;
    bool logInSuccess = false;
    int failCount = 0;

    #region GAME_CENTER
    //Call When gotoPark
    public void AuthenticateToGameCenter()
    {
#if UNITY_IPHONE
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                balloon.ShowMsg("애플 게임센터 로그인 성공!");
                Debug.Log("Authentication successful");
                OnPostScore();
                logInSuccess = true;
                failCount = 0;
            }
            else
            {
                balloon.ShowMsg("애플 게임센터 로그인 실패..");
                failCount += 1;
                Debug.Log("Authentication failed");

                if(failCount >= 3)
                {
                    msg.SetMsg("아이폰 설정에서 Game Center가 켜져 있는지 확인해주세요!", 1);
                    failCount = 0;
                }
            }
        });
#elif UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif
    }

#if UNITY_ANDROID
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            balloon.ShowMsg("플레이 게임 서비스 로그인 성공!");
            Debug.Log("Authentication successful");
            OnPostScore();
            logInSuccess = true;
            failCount = 0;
        }
        else
        {
            balloon.ShowMsg("플레이 게임 서비스 로그인 실패!");
            failCount += 1;
            Debug.Log("Authentication failed");

            msg.SetMsg("랭킹을 보기 위해서는 로그인이 필요합니다.\nPlay Games Services에\n로그인 하시겠습니까?", 2, "PlayGamesLogIn");
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
#endif

    public void PlayGamesLogIn()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
#endif
    }

    void ReportScore(long score, string leaderboardID)
    {
        //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        Social.ReportScore(score, leaderboardID, success =>
        {
            if (success)
            {
                Debug.Log("Reported score successfully");
            }
            else
            {
                failCount += 1;
                balloon.ShowMsg("로그인 실패");
                Debug.Log("Failed to report score");
            }

            Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:" + score);
        });
    }

    static void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion

    void OnPostScore()
    {
        collection.GetCount();

        int typeScore, totalScore, storeScore;
        if (PlayerPrefs.GetInt("debugMode") == 1)
        {
            typeScore = -1;
            totalScore = -1;
            storeScore = -1;
        }
        else
        {
            typeScore = PlayerPrefs.GetInt("myTotalCards");
            totalScore = PlayerPrefs.GetInt("myRealTotalCard");
            storeScore = PlayerPrefs.GetInt("storeDateCount");
        }

#if UNITY_ANDROID
        ReportScore(typeScore, "CgkIptWu0egOEAIQAQ");
        ReportScore(totalScore, "CgkIptWu0egOEAIQAg");
        ReportScore(storeScore, "CgkIptWu0egOEAIQBw");
#elif UNITY_IPHONE
        ReportScore(typeScore, "LeaderBoard");
        ReportScore(totalScore, "LeaderBoardTotal");
        ReportScore(storeScore, "net.sundragon.bbang.store");
#endif

    }

    //Open Leaderboard Btn Clicked
    public void OpenLeaderBoard()
    {
        OnPostScore();

        if (logInSuccess)
        {
            ShowLeaderboard();
        } else
        {
            AuthenticateToGameCenter();
            ShowLeaderboard();
        }
    }
}
*/