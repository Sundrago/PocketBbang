using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoreEnterDialogue
{
    private GameManager gameManager;
    private PrompterController prompterController;
    
    public StoreEnterDialogue(GameManager gameManager)
    {
        this.gameManager = gameManager;
        prompterController = gameManager.PrompterController;

    }

    public void EnterStore()
    {
        prompterController.Reset();
        prompterController.imageMode = true;
        gameManager.storeOutAction = "";
        gameManager.currentLocation = "store_in";
        //int chaIdx = Random.Range(0, 3);
        var chaIdx = gameManager.MyStoreType;

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
                prompterController.AddString("미소녀", "어서오세요. (포도)씨유입니다.");

                prompterController.AddOption("포켓볼빵 있어요?", "store", "girl_ball");
                prompterController.AddOption("남자친구 있어요?", "store", "girl_boy");
                prompterController.AddOption("전화번호좀 주세요.", "store", "girl_phone");
            }
            else
            {
                //timer not yet
                if (gameManager.DangunNotificationManager.TimeGap() <= 0)
                {
                    prompterController.AddString("미소녀", "어서와.");
                    prompterController.AddOption("포켓볼빵 있어?", "store", "girlf_ball");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (gameManager.DangunNotificationManager.TimeGap() <= 5)
                {
                    //2-3개
                    prompterController.AddString("미소녀", "우와 바로 왔구나!");
                    prompterController.AddString("훈이", "아직 안팔렸지?");
                    var rnd = Random.Range(2, 4);
                    prompterController.AddString("미소녀", "응 포켓볼빵 " + rnd + "개 있어!");
                    gameManager.AddBBangType(rnd, "미소녀");
                    prompterController.AddString("훈이", "대박이다! 정말 고마워!");
                    prompterController.AddString("미소녀", "후훗 천만에 말씀.");
                    prompterController.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (gameManager.DangunNotificationManager.TimeGap() <= 30)
                {
                    //1-2개
                    prompterController.AddString("미소녀", "왔구나!!");
                    prompterController.AddString("훈이", "포켓볼빵 아직 있지?!");
                    var rnd = Random.Range(1, 3);
                    prompterController.AddString("미소녀", "조금만 더 일찍 오지. \n 포켓볼빵 " + rnd + "개 남았어.");
                    gameManager.AddBBangType(rnd, "미소녀");
                    prompterController.AddString("훈이", "우와 !정말 고마워!");
                    prompterController.AddString("미소녀", "친구라면 이정도 쯤이야!");
                    prompterController.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else if (gameManager.DangunNotificationManager.TimeGap() <= 60)
                {
                    //1개
                    prompterController.AddString("미소녀", "조금만 더 일찍 오지!");
                    prompterController.AddString("훈이", "허걱 벌써 다 팔렸어?");
                    prompterController.AddString("미소녀", "아니 딱 한개 남았어.");
                    gameManager.AddBBangType(1, "미소녀");
                    prompterController.AddString("훈이", "그래도 정말 고마워!");
                    prompterController.AddString("미소녀", "다음에는 조금 더 일찍와.");
                    prompterController.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else
                {
                    //late
                    prompterController.AddString("미소녀", "어 왔구나!");
                    prompterController.AddString("훈이", "포켓볼빵 아직 있어?");
                    prompterController.AddString("미소녀", "한시간도 안 돼서 다 팔렸어..");
                    prompterController.AddString("훈이", "에고 그렇구나..\n그래도 문자해줘서 고마워.");
                    prompterController.AddString("미소녀", "다음에는 문자하면 바로와!");
                    prompterController.AddString("훈이", "응. 고마워.");
                    prompterController.AddOption("알림이 안왔어!", "store", "girlf_none");
                    prompterController.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
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
                prompterController.AddString("양아치", "어 왔네.");

                prompterController.AddOption("포켓볼빵 들어왔어?", "store", "yangf_ball");
                prompterController.AddOption("게임은 어떻게 되어가?", "store", "yangf_game");
                prompterController.AddOption("편의점을 나간다", "store", "yangf_out");
            }
            else
            {
                prompterController.AddString("양아치", "어서오세요. 잼미니스톱입니다.");
                prompterController.AddOption("포켓볼빵 있어요?", "store", "yang_ball");
                prompterController.AddOption("무슨 게임해요?", "store", "yang_game");
                prompterController.AddOption("저랑 친구 할래요?", "store", "yang_friend_ask");
            }
        }

        //점주
        else if (chaIdx == 2)
        {
            if (PlayerPrefs.GetInt("albaExp") == 0)
            {
                prompterController.AddString("점주", "어서오세요! 쌈마트24입니다!");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "host_ball");
                prompterController.AddOption("알바 자리 있어요?", "store", "host_alba");
                prompterController.AddOption("편의점을 나간다", "store", "host_out");
            }
            else
            {
                gameManager.MatdongsanBuy = false;
                prompterController.AddString("점주", "오 자네가 왔구만! 그래 무슨일인가?");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "hostf_ball");
                prompterController.AddOption("알바하러 왔어요!", "store", "hostf_alba");
                prompterController.AddOption("편의점을 나간다", "store", "hostf_out");
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
                prompterController.AddString("비실이", "어서오세요오~! 나인투식스입니다아~!");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "bi_ball");
                prompterController.AddOption("나랑 친구할래?", "store", "bi_fried");
                prompterController.AddOption("편의점을 나간다", "store", "bi_out");
            }
            else
            {
                prompterController.AddString("비실이", "어서와아!");
                prompterController.AddOption("포켓볼빵 있어?", "store", "bi_ball_2");
                prompterController.AddOption("편의점을 나간다", "store", "bi_out");
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
                prompterController.AddString("열정맨", "어서오세요! 진상25입니다!");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                prompterController.AddOption("포켓볼빵 언제 들어와요?", "store", "y_when");
                prompterController.AddOption("편의점을 나간다", "store", "y_out");
            }
            else
            {
                prompterController.AddString("열정맨", "어서오세요! 진상25입니다!");
                prompterController.AddString("열정맨", "간절한 손님이 왔네요!");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "yf_ball");
                prompterController.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                prompterController.AddOption("편의점을 나간다", "store", "yf_out");
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
                prompterController.AddString("박종업원", "어서오세유. 머박분식집이에유.");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "p_ball");
                prompterController.AddOption("뭐 팔아요?", "store", "p_what");
                prompterController.AddOption("편의점을 나간다", "store", "p_out");
            }
            else
            {
                //PrompterController.AddString("박종업원", "어서오세요! 진상25입니다!");
                //PrompterController.AddString("박종업원", "간절한 손님이 왔네요!");
                //PrompterController.AddOption("포켓볼빵 있나요?", "store", "yf_ball");
                //PrompterController.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                prompterController.AddOption("편의점을 나간다", "store", "yf_out");
            }
        }

        //나이롱
        else if (chaIdx == 7)
        {
            gameManager.DoMiCoinManager.UpdateCurrentPrice();

            if (PlayerPrefs.GetInt("NylonDrawBbangCount") < 4)
            {
                if (!PlayerPrefs.HasKey("NylonInvested"))
                {
                    gameManager.NylonDialogue.Nylon("ID_F_FIRST");
                }
                else
                {
                    if (PlayerPrefs.GetInt("NylonInvested") == 1)
                        gameManager.NylonDialogue.Nylon("ID_F_SECOND_IFINVESTED");
                    else
                        gameManager.NylonDialogue.Nylon("ID_F_SECOND_IFNOTINVESTED");
                }
            }
            else
            {
                gameManager.NylonDialogue.Nylon_f("ID_F_FRIEND_HELLO");
            }
        }

        //왕형 탕후루
        else if (chaIdx == 8)
        {
            gameManager.tanghuruState = "";
            // Tanghuru("ID_T_OPEN_FRIEND"); //[진입점 2] <친구 이벤트> 친구가 되기 전 오후 12시에서 오후 6시 사이에 방문하면 여기로 진입됨.
            if (PlayerPrefs.GetInt("tanghuru_friend", 0) == 1)
            {
                gameManager.TanghuruDialogue.Tanghuru("ID_T_OPEN_AFTERFRIEND"); //[진입점 3] <친구가 된 후> 아무때나 가도 여기로 진입됨.
            }
            else
            {
                var hr = DateTime.Now.Hour;
                if (hr >= 12 && hr < 18)
                    gameManager.TanghuruDialogue.Tanghuru("ID_T_OPEN_FRIEND"); //[진입점 2] <친구 이벤트> 친구가 되기 전 오후 12시에서 오후 6시 사이에 방문하면 여기로 진입됨.
                else
                    gameManager.TanghuruDialogue.Tanghuru("ID_T_OPEN_STORE"); //[진입점 1] <친구 되기 전> 처음 만났을 때부터 친구가 되기 전까지. -> [연결점 1], [연결점 3]으로 연결.
            }
        }

        //장왕 보쌈
        else if (chaIdx == 9)
        {
            // [진입점 2] 친구가 된 후
            if (PlayerPrefs.GetInt("bossam_friend", 0) == 1)
                gameManager.JangJokBalDialogue.JangJokBal("ID_J_FRIEND");
            else
                // [진입점 1] 친구가 되기 전
                gameManager.JangJokBalDialogue.JangJokBal("ID_J_OPEN");
        }
    }
}