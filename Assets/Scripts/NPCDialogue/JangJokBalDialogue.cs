using UnityEngine;

public class JangJokBalDialogue
{
    private GameManager gameManager;
    private PrompterController prompterController;

    public JangJokBalDialogue(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void JangJokBal(string ID)
    {
        MonoBehaviour.print("EVENT ID JangJokBal : " + ID);
        gameManager.PrompterController.Reset();
        gameManager.PrompterController.imageMode = true;

        switch (ID)
        {
            case "ID_J_OPEN":
                gameManager.PrompterController.AddString("장왕", "어서오세요오!!");
                gameManager.PrompterController.AddString("장왕", "왕충동 장 족발 보싸암!!");
                gameManager.PrompterController.AddOption("포켓볼 빵 있나요?", "JangJokBal", "ID_J_OPEN_BREAD");
                gameManager.PrompterController.AddOption("족발 보쌈 한 세트 주세요.", "JangJokBal", "ID_J_OPEN_JOKBAL");
                gameManager.PrompterController.AddOption("편의점을 나간다.", "JangJokBal", "ID_J_OPEN_STOREOUT");
                break;

            case "ID_J_OPEN_STOREOUT":
                gameManager.storeOutAction = "";
                gameManager.PrompterController.AddNextAction("main", "store_out");
                break;

            case "ID_J_OPEN_BREAD":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "족발 보쌈 집에 포켓볼 빵이 있겠냐고요오!!");
                gameManager.PrompterController.AddString("장왕", "족발 뼈로 맞고 싶어요오?? 어??");
                gameManager.PrompterController.AddString("장왕", "이렇게?? 이렇게?! 이렇게!!");
                gameManager.PrompterController.AddString("훈이", "죄송합니다...");
                gameManager.PrompterController.AddString("장왕", "됐고. 온 김에 일이나 좀 도와주세요.");
                gameManager.PrompterController.AddString("훈이", "갑자기요...?");
                gameManager.PrompterController.AddString("장왕", "그냥 좀 도와주세요오!!");
                gameManager.PrompterController.AddOption("네...", "JangJokBal", "ID_J_GAME");
                gameManager.PrompterController.AddOption("네!!", "JangJokBal", "ID_J_GAME");
                break;

            case "ID_J_OPEN_JOKBAL":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "바빠 죽겠는데 주문을 받게 생겼냐고요오!!");
                gameManager.PrompterController.AddString("장왕", "족발 뼈로 맞고 싶어요오?? 어??");
                gameManager.PrompterController.AddString("장왕", "이렇게?? 이렇게?! 이렇게!!");
                gameManager.PrompterController.AddString("훈이", "죄송합니다...");
                gameManager.PrompterController.AddString("장왕", "됐고. 온 김에 일이나 좀 도와주세요.");
                gameManager.PrompterController.AddString("훈이", "갑자기요...?");
                gameManager.PrompterController.AddString("장왕", "그냥 좀 도와주세요오!!");
                gameManager.PrompterController.AddOption("네...", "JangJokBal", "ID_J_GAME");
                gameManager.PrompterController.AddOption("네!!", "JangJokBal", "ID_J_GAME");
                break;

            case "ID_J_GAME":
                gameManager.PrompterController.AddString("장왕", "쌈을 만들 때는 족발 두 개, 겨자 이만큼, 마늘 두 개, 청양고추 하나.");
                gameManager.PrompterController.AddString("장왕", "그리고 너 각오해, 딱 기다려. 하는 마음으로 손님 입에 던지면 돼요.");
                gameManager.PrompterController.AddOption("네!!", "JangJokBal", "ID_J_GAME_START");
                gameManager.PrompterController.AddOption("조금만 더 자세히 알려주시면 안 될까요?", "JangJokBal", "ID_J_GAME_EXPLAIN");
                gameManager.PrompterController.AddOption("그냥 도망간다.", "JangJokBal", "ID_J_GAME_RUN");
                break;

            case "ID_J_GAME_RUN":
                gameManager.PrompterController.AddString("훈이", "겨우 도망쳤다.");
                gameManager.storeOutAction = "역시 사장님이 조금 무서운 것 같다. 마음의 준비가 되면 다시 오자.";
                gameManager.PrompterController.AddNextAction("main", "store_out");
                break;

            case "ID_J_GAME_EXPLAIN":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "한번 할 때 제대로 하려는 그 자세!!");
                gameManager.PrompterController.AddString("장왕", "저는 지금 굉장히 기분이 좋습니다. 궁금한 게 있으시면 몇 번이고 다시 물어보셔도 돼요.");
                gameManager.PrompterController.AddString("장왕", "쌈은 제가 싸드릴 거니까 걱정하실 필요 없고요.");
                gameManager.PrompterController.AddString("장왕", "일 시작하면 손님이 아기새마냥 입을 쩍 벌리고 계실 건데, 그 안에 제가 싸둔 쌈을 던져 넣으시면 됩니다.");
                gameManager.PrompterController.AddString("장왕", "손님은 한 분만 오실 때도 있고요, 단체로 오실 때도 있어요.");
                // No command found on line 57 : 
                gameManager.PrompterController.AddString("장왕", "그런데 손님 입에 못 넣으면 점수 까버릴 거에요.");
                gameManager.PrompterController.AddString("장왕", "대신 연속으로 성공하면 콤보 점수를 드릴게요.");
                gameManager.PrompterController.AddString("장왕", "아시겠어요오!!");
                gameManager.PrompterController.AddOption("네!!", "JangJokBal", "ID_J_GAME_START");
                gameManager.PrompterController.AddOption("다시 한번만 알려주시면 안 될까요?", "JangJokBal", "ID_J_GAME_EXPLAIN");
                break;

            case "ID_J_GAME_START":
                gameManager.PrompterController.AddString("장왕", "그럼 시작할게요오!!");
                gameManager.BossamGameManager.EnterGame();
                break;

            case "ID_J_GAME_OVER":
                break;

            case "ID_J_GAME_OVER_GOOD":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "아주 잘해주셨어요.");
                gameManager.PrompterController.AddString("장왕", "저는 지금 굉장히 감동 받았습니다.");
                gameManager.PrompterController.AddString("장왕", "여기 오늘 일당 받으시고. 또 오세요.");
                if (PlayerPrefs.GetString("bossam_rank") == "A" && PlayerPrefs.GetInt("bossam_friend", 0) == 0)
                {
                    gameManager.PrompterController.AddString("장왕", "알밥 천국에도 등록해 드렸으니까, 이제부터는 원할 때 언제든지 오실 수  있어요.");
                    PlayerPrefs.SetInt("bossam_friend", 1);
                }

                gameManager.PrompterController.AddString("장왕", "왕충동 장 족발보쌈!!");
                gameManager.PrompterController.AddOption("편의점을 나간다.", "JangJokBal", "ID_J_GAME_OVER_GOOD_STOREOUT");
                break;

            case "ID_J_GAME_OVER_GOOD_STOREOUT":
                gameManager.storeOutAction = "뭔가 정신 없는 족발 보쌈집이다. 그래도 뭔가 재밌으니 다음에 또 알바하러 와야겠다.";
                gameManager.PrompterController.AddNextAction("main", "store_out");
                break;

            case "ID_J_GAME_OVER_BAD":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "일을 정말 못하시네요.");
                gameManager.PrompterController.AddString("장왕", "저는 지금 굉장히 실망했습니다.");
                gameManager.PrompterController.AddString("장왕", "오늘 일당은 없고요. 두번 다시 오지 마세요.");
                gameManager.PrompterController.AddString("장왕", "왕충동 장 족발보쌈!!");
                gameManager.PrompterController.AddOption("편의점을 나간다.", "JangJokBal", "ID_J_GAME_OVER_BAD_STOREOUT");
                break;

            case "ID_J_GAME_OVER_BAD_STOREOUT":
                gameManager.storeOutAction = "정말 정신 없는 족발 보쌈집이다. 오지 말라고 했지만 재밌으니까 다음에 또 와야겠다.";
                gameManager.PrompterController.AddNextAction("main", "store_out");
                break;

            case "ID_J_FRIEND":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "기다리고 있었잖아요.");
                gameManager.PrompterController.AddString("장왕", "왕충동 장 족발 보쌈!!");
                gameManager.PrompterController.AddOption("알바 하러 왔어요.", "JangJokBal", "ID_J_FRIEND_GAME");
                gameManager.PrompterController.AddOption("족발 보쌈 한 세트 주세요.", "JangJokBal", "ID_J_OPEN_JOKBAL");
                gameManager.PrompterController.AddOption("편의점을 나간다.", "JangJokBal", "ID_J_OPEN_STOREOUT");
                break;

            case "ID_J_FRIEND_GAME":
                gameManager.PrompterController.AddString("장왕", "그럴 줄 알았어요오!!");
                gameManager.PrompterController.AddString("장왕", "기억하고 있죠??");
                gameManager.PrompterController.AddNextAction("JangJokBal", "ID_J_GAME");
                break;

            case "ID_J_ALBA":
                gameManager.PrompterController.AddString("장왕", "이거보세요오!!");
                gameManager.PrompterController.AddString("장왕", "기다리고 있었잖아요.");
                gameManager.PrompterController.AddString("장왕", "왕충동 장 족발 보쌈!!");
                gameManager.PrompterController.AddOption("알바 하러 왔어요.", "JangJokBal", "ID_J_FRIEND_GAME");
                gameManager.PrompterController.AddOption("편의점을 나간다.", "JangJokBal", "ID_J_OPEN_STOREOUT");
                break;
        }
    }
}