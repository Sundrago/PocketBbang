using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class AchievementCtrl : MonoBehaviour
{
    [SerializeField] BalloonControl balloon;
    [SerializeField] PhoneMsgCtrl msg;
    [SerializeField] Collection_Panel_Control collection;
    [SerializeField] RankCtrl rank;

    private void OnEnable()
    {
        // register for events
        GameServices.OnAuthStatusChange += OnAuthStatusChange;
    }

    private void OnDisable()
    {
        //base.OnDisable();

        // unregister from events
        GameServices.OnAuthStatusChange -= OnAuthStatusChange;
    }

    private void OnAuthStatusChange(GameServicesAuthStatusChangeResult result, Error error)
    {
        if (error == null)
        {
            Debug.Log("Received auth status change event");
            Debug.Log("Auth status: " + result.AuthStatus);

            if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
            {
#if UNITY_IPHONE
                balloon.ShowMsg("애플 게임센터 로그인 성공!");
#elif UNITY_ANDROID
                balloon.ShowMsg("플레이 게임 서비스 로그인 성공!");
#endif
                UpdateScore();
                Debug.Log("Local player: " + result.LocalPlayer);
            }
        }
        else
        {
#if UNITY_IPHONE
            balloon.ShowMsg("애플 게임센터 로그인 실패 : " + error);
#elif UNITY_ANDROID
                balloon.ShowMsg("플레이 게임 서비스 로그인 실패 : " + error);
#endif
            msg.SetMsg("Failed login with error : " + error, 1);
            Debug.LogError("Failed login with error : " + error);
        }
    }

    private void Start()
    {
        Autheticate();
    }

    public void Autheticate()
    {
        if (GameServices.IsAvailable())
        {
            if(!GameServices.IsAuthenticated)
            GameServices.Authenticate();
            ReceiveRank();
        } else
            {
#if UNITY_IPHONE
                msg.SetMsg("아이폰 설정에서 Game Center가 켜져 있는지 확인해주세요!", 1);
#elif UNITY_ANDROID
                msg.SetMsg("Play Games Services에 로그인이 되어 있는지 확인해주세요!", 1);
#endif
                msg.SetMsg("GameServices.IsAvailable false", 1);
                Debug.LogError("!IsAvailable");
                if(!GameServices.IsAuthenticated)
                    GameServices.Authenticate();
            }
    }

    public void ShowLeaderBoard()
    {
        if (GameServices.IsAuthenticated)
        {
            UpdateScore();
            GameServices.ShowLeaderboards(callback: (result, error) =>
            {
                Debug.Log("Leaderboards UI closed");
            });
        } else
        {
            Autheticate();
            GameServices.ShowLeaderboards(callback: (result, error) =>
            {
                Debug.Log("Leaderboards UI closed");
            });
        }
    }

    public void UpdateScore()
    {
        int typeScore, totalScore, storeScore;
        collection.GetCount();

        if (PlayerPrefs.GetInt("debugMode") == 1)
        {
            typeScore = PlayerPrefs.GetInt("myTotalCards");
            totalScore = -1;
            storeScore = -1;
        }
        else
        {
            typeScore = PlayerPrefs.GetInt("myTotalCards");
            totalScore = PlayerPrefs.GetInt("myRealTotalCard");
            storeScore = PlayerPrefs.GetInt("storeDateCount");
        }

        GameServices.ReportScore("LeaderBoard", typeScore, (error) =>
        {
            if (error != null)
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });
        GameServices.ReportScore("LeaderBoardTotal", totalScore, (error) =>
        {
            if (error != null)
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });
        GameServices.ReportScore("LeaderBoardStore", storeScore, (error) =>
        {
            if (error != null)
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });
    }

    public void UpdateAchievement(string code)
    {
        string achievementId = null; //This is the Id set in Setup for each achievement
        double percentageCompleted = 100;// This is in the range [0 - 100]
        switch (code)
        {
            case "미소녀":
                achievementId = "alba0";
                break;
            case "맛동석":
                achievementId = "alba1";
                break;
            case "양아치":
                achievementId = "alba2";
                break;
            case "비실이":
                achievementId = "alba3";
                break;
            case "열정맨":
                achievementId = "alba4";
                break;
            case "박종업원 아저씨":
                achievementId = "alba5";
                break;
            case "나이롱마스크":
                achievementId = "alba6";
                break;
            case "왕형":
                achievementId = "alba7";
                break;
            case "장왕":
                achievementId = "alba8";
                break;
        }

        GameServices.ReportAchievementProgress(achievementId, percentageCompleted, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to submit progress finished successfully.");
            }
            else
            {
                Debug.Log("Request to submit progress failed with error. Error: " + error);
            }
        });
    }

    public void ShowAchievement()
    {
        if (GameServices.IsAuthenticated)
        {
            GameServices.ShowAchievements((result, error) =>
            {
                Debug.Log("Achievements view closed");
            });
        }
        else
        {
            Autheticate();
            GameServices.ShowAchievements((result, error) =>
            {
                Debug.Log("Achievements view closed");
            });
        }
    }

    public void ReceiveRank()
    {
        StartCoroutine(UpdateDataAndReceiveRank());
    }

    public IEnumerator UpdateDataAndReceiveRank()
    {
        if (GameServices.IsAvailable())
        {
            UpdateScore();

            GameServices.LoadLeaderboards((result, error) =>
            {
                if (error == null)
                {
                    // show console messages
                    ILeaderboard[] leaderboards = result.Leaderboards;
                    ILeaderboard leaderboard1;
                    for (int iter = 0; iter < leaderboards.Length; iter++)
                    {
                        leaderboard1 = leaderboards[iter];
                        Debug.Log(string.Format("[{0}]: {1}", iter, leaderboard1));
                    }

                    leaderboard1 = leaderboards[0];

#if UNITY_ANDROID
                    leaderboard1.TimeScope = LeaderboardTimeScope.AllTime;
#endif
                    leaderboard1.LoadPlayerCenteredScores((result, error) => {
                        if (error == null)
                        {
                            Debug.Log("Scores loaded : " + result.Scores);
                        }
                        else
                        {
                            Debug.Log("Failed loading top scores with error : " + error.Description);
                        }
                        IScore score2 = leaderboard1.LocalPlayerScore;
                        if(PlayerPrefs.GetInt("debugMode") == 1) balloon.ShowMsg("전체랭킹 : " + score2.Rank + "등!");
                        Debug.Log("" + score2.Rank + "등!");
                        rank.CheckForUpdate((int)score2.Rank);
                    });
                }
                else
                {
                    Debug.Log("Request to load leaderboards failed with error. Error: " + error);
                }
            });
        }
        yield return null;
    }
}
