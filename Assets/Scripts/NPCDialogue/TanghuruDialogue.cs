using UnityEngine;

public class TanghuruDialogue
{
    /// <summary>
    /// This script was automatically generated by using Google Apps Scripts
    /// https://docs.google.com/spreadsheets/d/1vnVZIWT2fYS4G4XNV8qeBGQ6GA3vfaEEbCJuwcgcqJs/edit#gid=428386786
    /// </summary>
    private GameManager gameManager;
    private PrompterController prompterController;

    public TanghuruDialogue(GameManager gameManager)
    {
        this.gameManager = gameManager;
        prompterController = gameManager.PrompterController;
    }

    public void Tanghuru(string ID)
    {
        MonoBehaviour.print("EVENT ID Tanghuru : " + ID);
        prompterController.Reset();
        prompterController.imageMode = true;

        switch (ID)
        {
            case "ID_T_OPEN_STORE":
                prompterController.AddString("왕형", "안녕하시오.");
                prompterController.AddString("왕형", "왕형 탕후루에 당도한 것을 환영하오, 낯선 이여.");
                prompterController.AddString("왕형", "나는 당이 떨어진 자들을 굽어살피는 달콤한 사업가, 왕형이오.");
                gameManager.tanghuruState = "ID_T_OPEN_STORE";
                prompterController.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU");
                prompterController.AddOption("포켓볼 빵 있어요?", "Tanghuru", "ID_T_OPEN_STORE_BREAD");
                prompterController.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_STORE_STOREOUT");
                break;

            case "ID_T_OPEN_STORE_STOREOUT":
                gameManager.storeOutAction = "";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_TANGHURU":
                prompterController.AddString("왕형", "탕후루는 당 함량이 높아 원기를 회복시켜 주는 효과가 있소.");
                prompterController.AddString("왕형", "먹으면 편의점 하나 정도 더 찾아갈 수 있는 힘이 날 것이오.");
                prompterController.AddNextAction("Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_2":
                prompterController.AddString("왕형", "탕후루는 하나에 3,000냥이오. 구매하겠소?");
                gameManager.DomiTradeManager.DialogueCheckMoney(3000, "ID_T_OPEN_STORE_TANGHURU_BUY_YES", "ID_T_OPEN_STORE_TANGHURU_BUY_NOMONEY",
                    "네 주세요.", "Tanghuru");
                prompterController.AddOption("그냥 다음에 살게요.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU_STOREOUT");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_BUY_YES":
                prompterController.AddString("왕형", "고맙소.");
                prompterController.AddString("왕형", "탕후루 여기 있소.");
                gameManager.PlayerStatusManager.UpdateMoney(-3000);
                gameManager.PlayerStatusManager.SetHeart(1);
                gameManager.PlayerStatusManager.SetHeart(1);
                prompterController.AddString("훈이", "(탕후루를 먹고 하트가 두 개 회복되었다!)");
                prompterController.AddString("왕형", "또 오시오.");
                if (gameManager.tanghuruState == "ID_T_OPEN_STORE") prompterController.AddString("왕형", "단, 오후 12시부터 오후 6시 사이는 손님이 많으니 되도록 피해 주시오.");

                gameManager.storeOutAction = "달고 맛있는 탕후루! 다음에 또 사러 와야겠다.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_BUY_NOMONEY":
                prompterController.AddString("왕형", "그대가 소지하고 있는 돈이 부족하오.");
                prompterController.AddNextAction("Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                break;

            case "ID_T_OPEN_STORE_TANGHURU_STOREOUT":
                prompterController.AddString("왕형", "그럼 그렇게 하시오.");
                prompterController.AddString("왕형", "또 오시오.");
                if (gameManager.tanghuruState == "ID_T_OPEN_STORE") prompterController.AddString("왕형", "단, 오후 12시부터 오후 6시 사이는 손님이 많으니 되도록 피해 주시오.");

                gameManager.storeOutAction = "달달한 탕후루가 먹고 싶긴 하지만... 다음에 사먹도록 하자.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_STORE_BREAD":
                prompterController.AddString("훈이", "포켓볼 빵 있어요?");
                prompterController.AddString("왕형", "어허. 말세로다. 어찌 탕후루 가게에서 빵 따위를 팔겠는가?");
                prompterController.AddString("훈이", "혹시 몰라서 여쭤봤어요...");
                prompterController.AddString("왕형", "어림없는 소리!");
                prompterController.AddString("왕형", "사대부의 나라가 어찌 오랑케의 음식을 팔겠는가. 당치도 않다!");
                gameManager.storeOutAction = "괜히 호통만 들었다... 다음에는 탕후루를 사러 오자.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_FRIEND":
                prompterController.AddString("왕형", "또 손님이 들이닥치다니, 난세로다!");
                prompterController.AddString("왕형", "이제 나의 워라벨은 누가 지켜준단 말인가! 암흑의 시대가 도래하였구나!");
                gameManager.tanghuruState = "ID_T_OPEN_FRIEND";
                prompterController.AddNextAction("Tanghuru", "ID_T_OPEN");
                break;

            case "ID_T_OPEN_AFTERFRIEND":
                prompterController.AddString("왕형", "어서오시오. 환영하오.");
                prompterController.AddString("왕형", "그대가 다시 찾아 주니 기쁘구려.");
                // No command found on line 57 : 
                prompterController.AddNextAction("Tanghuru", "ID_T_OPEN");
                break;

            case "ID_T_OPEN":
                prompterController.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_ORDER");
                prompterController.AddOption("포켓볼 빵 있어요?", "Tanghuru", "ID_T_OPEN_BBANG");
                prompterController.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_ORDER":
                prompterController.AddString("왕형", "아아. 지금은 주문이 폭주해서 당장 드릴 수가 없소.");
                prompterController.AddString("왕형", "혹시 탕후루 만드는 것을 좀 도와주겠소? 도와준다면 내 넉넉하게 사례하겠소.");
                prompterController.AddOption("네, 도와 드릴게요!", "Tanghuru", "ID_T_OPEN_ORDER_HELP");
                prompterController.AddOption("다음에 다시 올게요.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_ORDER_HELP":
                gameManager.TanghuruGameManager.EnterGame();
                break;

            case "ID_T_OPEN_BBANG":
                prompterController.AddString("왕형", "어찌 탕후루 가게에서 빵 따위를 팔겠는가? 내 지금 바쁘니 당치 않은 이야기는 삼가 주시게.");
                prompterController.AddOption("탕후루 주세요.", "Tanghuru", "ID_T_OPEN_ORDER");
                prompterController.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_OPEN_END");
                break;

            case "ID_T_OPEN_END":
                prompterController.AddString("왕형", "미안하오. 다음에 다시 와 주시오.");
                gameManager.storeOutAction = "괜히 호통만 들었다... 다음에는 탕후루를 사러 오자.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_OPEN_ORDER_HELP_NOHEART":
                break;

            case "ID_T_OPEN_ORDER_HELP_NOHEART_STOREOUT":
                break;

            case "ID_T_GAME_OVER_LINK":
                //앞으로는 알밥 천국을 통해서도 왕형 탕후루에 일을 도와주러 올 수 있소.
                if (PlayerPrefs.GetInt("tanghuru_friend", 0) == 0)
                {
                    if (PlayerPrefs.GetString("tanghuru_rank") == "F")
                    {
                        prompterController.AddString("왕형", "내가 괜한 부탁을 한 모양이오.");
                        prompterController.AddString("왕형", "돈이 궁할 때 와서 알바를 하든지 말든지 알아서 하시오.");
                    }
                    else
                    {
                        prompterController.AddString("왕형", "고맙소.. 자네 덕분에 살았소.");
                        prompterController.AddString("왕형", "앞으로도 용돈이 필요할 때 가끔씩 들러서 알바를 해주시오.");
                    }

                    PlayerPrefs.SetInt("tanghuru_friend", 1);
                }
                else
                {
                    if (PlayerPrefs.GetString("tanghuru_rank") == "F")
                        prompterController.AddString("왕형", "내가 괜한 부탁을 한 모양이오.");
                    else
                        prompterController.AddString("왕형", "고생많았소.");
                }

                prompterController.AddString("왕형", "이제 좀 한숨 돌릴 여유가 생긴 듯 한데 탕후루를 구매하겠소?");
                prompterController.AddOption("탕후루를 산다.", "Tanghuru", "ID_T_OPEN_STORE_TANGHURU_2");
                prompterController.AddOption("편의점을 나간다.", "Tanghuru", "ID_T_GAME_CLOSE_END");
                break;

            case "ID_T_GAME_CLOSE_END":
                prompterController.AddString("왕형", "또 오시오.");
                gameManager.storeOutAction = "";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_T_ALBAB":
                prompterController.AddString("왕형", "어서오시오. 환영하오.");
                prompterController.AddString("왕형", "그대가 일부러 찾아 주니 기쁘구려.");
                prompterController.AddString("왕형", "탕후루 만드는 것을 좀 도와주겠소?");
                prompterController.AddOption("네, 도와 드릴게요!", "Tanghuru", "ID_T_ALBAB_HELP");
                prompterController.AddOption("다음에 다시 올게요.", "Tanghuru", "ID_T_ALBAB_STOREOUT");
                break;

            case "ID_T_ALBAB_HELP":
                gameManager.TanghuruGameManager.EnterGame();
                break;

            case "ID_T_ALBAB_STOREOUT":
                prompterController.AddString("왕형", "다음엔 꼭 도와주길 바라오.");
                gameManager.storeOutAction = "";
                prompterController.AddNextAction("main", "store_out");
                break;
        }
    }
}