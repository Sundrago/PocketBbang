using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private BalloonUIManager balloonUIManager;
    [SerializeField] private PhoneMessageController phoneMessageController;
    [SerializeField] private CollectionPanelManager collectionPanelManager; 
    [SerializeField] private RankManager rankManager;

    private void Start()
    {
        Autheticate();
    }

    private void OnEnable()
    {
        GameServices.OnAuthStatusChange += OnAuthStatusChange;
    }

    private void OnDisable()
    {
        GameServices.OnAuthStatusChange -= OnAuthStatusChange;
    }

    private void OnAuthStatusChange(GameServicesAuthStatusChangeResult result, Error error)
    {
        if (error == null)
        {
            Debug.Log("Auth status: " + result.AuthStatus);

            if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
            {
#if UNITY_IPHONE
                // BalloonUIManager.ShowMsg("애플 게임센터 로그인 성공!");
#elif UNITY_ANDROID
                // BalloonUIManager.ShowMsg("플레이 게임 서비스 로그인 성공!");
#endif
                UpdateScore();
                Debug.Log("Local player: " + result.LocalPlayer);
            }
        }
        else
        {
#if UNITY_IPHONE
            balloonUIManager.ShowMsg("애플 게임센터 로그인 실패 : " + error);
#elif UNITY_ANDROID
                BalloonUIManager.ShowMsg("플레이 게임 서비스 로그인 실패 : " + error);
#endif
            phoneMessageController.SetMsg("Failed login with error : " + error, 1);
            Debug.LogError("Failed login with error : " + error);
        }
    }

    public void Autheticate()
    {
        if (GameServices.IsAvailable())
        {
            if (!GameServices.IsAuthenticated)
                GameServices.Authenticate();
            ReceiveRank();
        }
        else
        {
#if UNITY_IPHONE
            phoneMessageController.SetMsg("아이폰 설정에서 Game Center가 켜져 있는지 확인해주세요!", 1);
#elif UNITY_ANDROID
                msg.SetMsg("Play Games Services에 로그인이 되어 있는지 확인해주세요!", 1);
#endif
            phoneMessageController.SetMsg("GameServices.IsAvailable false", 1);
            Debug.LogError("!IsAvailable");
            if (!GameServices.IsAuthenticated)
                GameServices.Authenticate();
        }
    }

    public void ShowLeaderBoard()
    {
        if (GameServices.IsAuthenticated)
        {
            UpdateScore();
            GameServices.ShowLeaderboards(callback: (result, error) => { Debug.Log("Leaderboards UI closed"); });
        }
        else
        {
            Autheticate();
            GameServices.ShowLeaderboards(callback: (result, error) => { Debug.Log("Leaderboards UI closed"); });
        }
    }

    public void UpdateScore()
    {
        int typeScore, totalScore, storeScore;
        collectionPanelManager.GetCount();

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

        GameServices.ReportScore("LeaderBoard", typeScore, error =>
        {
            if (error != null) Debug.Log("Request to submit score failed with error: " + error.Description);
        });
        GameServices.ReportScore("LeaderBoardTotal", totalScore, error =>
        {
            if (error != null) Debug.Log("Request to submit score failed with error: " + error.Description);
        });
        GameServices.ReportScore("LeaderBoardStore", storeScore, error =>
        {
            if (error != null) Debug.Log("Request to submit score failed with error: " + error.Description);
        });
    }

    public void UpdateAchievement(string code)
    {
        string achievementId = null; //This is the Id set in Setup for each achievement
        double percentageCompleted = 100; // This is in the range [0 - 100]
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

        GameServices.ReportAchievementProgress(achievementId, percentageCompleted, error =>
        {
            if (error == null)
                Debug.Log("Request to submit progress finished successfully.");
            else
                Debug.Log("Request to submit progress failed with error. Error: " + error);
        });
    }

    public void ShowAchievement()
    {
        if (GameServices.IsAuthenticated)
        {
            GameServices.ShowAchievements((result, error) => { Debug.Log("Achievements view closed"); });
        }
        else
        {
            Autheticate();
            GameServices.ShowAchievements((result, error) => { Debug.Log("Achievements view closed"); });
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
                    var leaderboards = result.Leaderboards;
                    ILeaderboard leaderboard1;
                    for (var iter = 0; iter < leaderboards.Length; iter++)
                    {
                        leaderboard1 = leaderboards[iter];
                        Debug.Log(string.Format("[{0}]: {1}", iter, leaderboard1));
                    }

                    leaderboard1 = leaderboards[0];

#if UNITY_ANDROID
                    leaderboard1.TimeScope = LeaderboardTimeScope.AllTime;
#endif
                    leaderboard1.LoadPlayerCenteredScores((result, error) =>
                    {
                        if (error == null)
                            Debug.Log("Scores loaded : " + result.Scores);
                        else
                            Debug.Log("Failed loading top scores with error : " + error.Description);
                        var score2 = leaderboard1.LocalPlayerScore;
                        if (PlayerPrefs.GetInt("debugMode") == 1)
                            balloonUIManager.ShowMsg("전체랭킹 : " + score2.Rank + "등!");
                        Debug.Log("" + score2.Rank + "등!");
                        rankManager.CheckForUpdate((int)score2.Rank);
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

    public void UpdateAchievementStatus(GameManager gameManager)
    {
        if ((gameManager.MyStoreType1 == 0) & (PlayerPrefs.GetInt("girl_friend") != 0))
            this.UpdateAchievement("미소녀");
        else if ((gameManager.MyStoreType1 == 1) & (PlayerPrefs.GetInt("yang_friend") != 0))
            this.UpdateAchievement("양아치");
        else if ((gameManager.MyStoreType1 == 2) & (PlayerPrefs.GetInt("albaExp") != 0))
            this.UpdateAchievement("맛동석");
        else if ((gameManager.MyStoreType1 == 3) & (PlayerPrefs.GetInt("bi_friend") != 0))
            this.UpdateAchievement("비실이");
        else if ((gameManager.MyStoreType1 == 4) & (PlayerPrefs.GetInt("yull_friend") != 0))
            this.UpdateAchievement("열정맨");
        else if ((gameManager.MyStoreType1 == 7) & (PlayerPrefs.GetInt("NylonDomiTutorial") == 1))
            this.UpdateAchievement("나이롱마스크");
        else if (gameManager.MyStoreType1 == 8 && PlayerPrefs.GetInt("tanghuru_friend", 0) == 1)
            this.UpdateAchievement("왕형");
        else if (gameManager.MyStoreType1 == 9 && PlayerPrefs.GetInt("bossam_friend", 0) == 1)
            this.UpdateAchievement("장왕");
    }
}