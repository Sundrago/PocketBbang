using UnityEngine;

public class NylonDialogue
{
    private GameManager gameManager;
    private PrompterController prompterController;

    public NylonDialogue(GameManager gameManager)
    {
        this.gameManager = gameManager;
        prompterController = gameManager.PrompterController;
    }

    public void Nylon(string ID)
    {
        MonoBehaviour.print("EVENT ID Nylon : " + ID);
        prompterController.Reset();
        prompterController.imageMode = true;

        switch (ID)
        {
            case "ID_F_NOMONEY_BEFORECOIN":
                prompterController.AddString("나이롱마스크", "오우~ 당신~ 빈털털이였네요우~");
                prompterController.AddString("나이롱마스크", "돈이 없으면 투자할 수 없어요우~ 돈 벌어와요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_NOMONEY_BEFORECOIN_END");
                break;

            case "ID_F_NOMONEY_BEFORECOIN_END":
                gameManager.storeOutAction = "(돈을 벌어서 빵에 투자해 보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_NOMONEY_AFTERECOIN_BBANGLAST":
                prompterController.AddString("나이롱마스크", "오우~ 손님~ 돈이 부족하네요우~");
                prompterController.AddString("나이롱마스크", "세상엔 돈을 버는 방법이 많이 있어요우~ 행운을 빌어요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_NOMONEY_AFTERECOIN_BBANGLAST_END");
                break;

            case "ID_F_NOMONEY_AFTERECOIN_BBANGLAST_END":
                gameManager.storeOutAction = "(돈이 부족해서 투자를 못하다니. 어서 돈을 벌어오자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_FIRST":
                prompterController.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
                prompterController.AddOption("포켓볼 빵 있어요?", "Nylon", "ID_F_BBANG");
                prompterController.AddOption("치킨 한 마리 주세요", "Nylon", "ID_F_CHICKEN");
                prompterController.AddOption("여기선 뭐 팔아요?", "Nylon", "ID_F_WHAT");
                break;

            case "ID_F_STOREOUT":
                prompterController.AddString("나이롱마스크", "나중에 또 오세요우~");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_BBANG":
                prompterController.AddString("나이롱마스크", "오우~ 포켓볼 뽱~ 그냥은 안 파라요우~");
                prompterController.AddString("훈이", "그러면 어떻게 하면 팔아요?");
                prompterController.AddString("나이롱마스크", "사지 말고 투자하세요우~");
                prompterController.AddString("나이롱마스크", "지금 만 냥을 내면 나중에 포켓볼 뽱으로 돌려줄게요우~");
                prompterController.AddString("훈이", "몇 개 줄 건데요?");
                prompterController.AddString("나이롱마스크", "그때그때 달라요우~");
                prompterController.AddString("훈이", "(왠지 사기꾼 같은데...)");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("네, 만 냥 투자할게요.", "ID_F_BBANG_INVEST", "ID_F_NOMONEY_BEFORECOIN");
                prompterController.AddOption("아니요, 괜찮아요.", "Nylon", "ID_F_BBANG_NOTINVEST");
                break;

            case "ID_F_BBANG_INVEST":
                prompterController.AddString("나이롱마스크", "감솨합니다~! 제가 뽱 많이 가져올게요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_INVEST_END");
                break;

            case "ID_F_BBANG_INVEST_END":
                gameManager.storeOutAction = "(이상한 가게지만 일단 투자를 했으니 다시 와야겠다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_BBANG_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 아쉽네요우~ 저한테 투자하시면 한번에 포켓볼 뽱 세 개까지도 얻을 수 있는데~");
                prompterController.AddString("나이롱마스크", "물론 하나도 못 얻을 수도 있지만...");
                prompterController.AddString("훈이", "네? 뭐라고요?");
                prompterController.AddString("나이롱마스크", "손님 참 운이 좋아보인다고요우~");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("정말요? 그럼 당장 투자할게요.", "ID_F_BBANG_NOTINVEST_INVEST", "ID_F_NOMONEY_BEFORECOIN");
                prompterController.AddOption("괜찮다니까요.", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST");
                break;

            case "ID_F_BBANG_NOTINVEST_INVEST":
                prompterController.AddString("나이롱마스크", "정말 현명한 선택이에요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddString("나이롱마스크", "뽱 많이 가져올 테니까 다음에 봐요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_BBANG_NOTINVEST_INVEST_END");
                break;

            case "ID_F_BBANG_NOTINVEST_INVEST_END":
                gameManager.storeOutAction = "(빵을 세 개나 얻을 수 있다니 너무 기대된다. 그런데 투자가 망하면 어떡하지?)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_BBANG_NOTINVEST_NOTINVEST":
                prompterController.AddString("나이롱마스크", "그렇다면 어쩔 수 없죠우~");
                prompterController.AddOption("그냥 치킨 한 마리 주세요", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN");
                prompterController.AddOption("안녕히 계세요.", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE");
                break;

            case "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN":
                prompterController.AddString("나이롱마스크", "재료가 다 떨어졌어요우~");
                prompterController.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN_END");
                break;

            case "ID_F_BBANG_NOTINVEST_NOTINVEST_CHICKEN_END":
                gameManager.storeOutAction = "(이 시간에 재료가 다 떨어졌다? 역시 뭔가 수상한 가게다. 치킨을 팔긴 하는 걸까? 그래도 빵을 세 개나 얻을 수 있는 투자는 흥미롭다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE":
                prompterController.AddString("나이롱마스크", "또 오세요우~");
                prompterController.AddOption("편의점을 나간다", "Nylon", "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE_END");
                break;

            case "ID_F_BBANG_NOTINVEST_NOTINVEST_BYE_END":
                gameManager.storeOutAction = "(수상한 가게다. 그런데 빵을 세 개나 얻을 수 있다고 하니 욕심이 난다. 여유가 생기면 다시 와보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_CHICKEN":
                prompterController.AddString("나이롱마스크", "오우~ 취킨~ 재료가 다 떨어졌어요우~ 안 팔아요우~");
                prompterController.AddOption("그럼 뭐 팔아요?", "Nylon", "ID_F_CHICKEN_WHAT");
                prompterController.AddOption("그럼 다음에 올게요.", "Nylon", "ID_F_CHICKEN_NOTINVEST");
                break;

            case "ID_F_CHICKEN_WHAT":
                prompterController.AddString("나이롱마스크", "취킨 사지 말고 저한테 투자하세요우~");
                prompterController.AddString("나이롱마스크", "지금 만 을 내면 나중에 포켓볼 뽱으로 돌려줄게요우~");
                prompterController.AddString("훈이", "몇 개 줄 건데요?");
                prompterController.AddString("나이롱마스크", "그때그때 달라요우~");
                prompterController.AddString("훈이", "(왠지 사기꾼 같은데...)");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("네, 만 냥 투자할게요.", "ID_F_CHICKEN_WHAT_INVEST", "ID_F_NOMONEY_BEFORECOIN");
                prompterController.AddOption("아니요, 괜찮아요.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST");
                break;

            case "ID_F_CHICKEN_WHAT_INVEST":
                prompterController.AddString("나이롱마스크", "오우~ 잘 선택하셨어요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddString("나이롱마스크", "제가 뽱 잔뜩 가져올게요우~ 최대 3개까지 가져올 수 있어요우~");
                prompterController.AddString("나이롱마스크", "물론 하나도 못 가져올 수도 있지만...");
                prompterController.AddString("훈이", "네? 뭐라고요?");
                prompterController.AddString("나이롱마스크", "아무것도 아니에요우~");
                prompterController.AddString("나이롱마스크", "다음에 봐요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_INVEST_END");
                break;

            case "ID_F_CHICKEN_WHAT_INVEST_END":
                gameManager.storeOutAction = "(뭔가 하나도 못 가져올 수도 있다는 말을 들은 것 같은데... 잘못 들은 거겠지? 투자가 잘 되길 기도하자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_CHICKEN_WHAT_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 아쉽네요우~");
                prompterController.AddString("나이롱마스크", "저한테 투자하시면 한번에 포켓볼 뽱 세 개까지도 얻을 수 있는데~");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("정말요? 그럼 당장 투자할게요.", "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST",
                    "ID_F_NOMONEY_BEFORECOIN");
                prompterController.AddOption("괜찮다니까요.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                break;

            case "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST":
                prompterController.AddString("나이롱마스크", "좋아요우~ 제가 뽱 많이 가져올게요우~");
                prompterController.AddString("나이롱마스크", "세 개까지 가져올 수 있어요우~");
                prompterController.AddString("나이롱마스크", "하나도 못 가져올 수도 있지만...");
                prompterController.AddString("훈이", "네?");
                prompterController.AddString("나이롱마스크", "아무말도 안 했어요우~ 다음에 봐요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST_END");
                break;

            case "ID_F_CHICKEN_WHAT_NOTINVEST_INVEST_END":
                gameManager.storeOutAction = "(빵을 한번에 세 개나 얻을 수 있다니! 그런데 하나도 못 가져올 수도 있다는 건... 잘못 들은 거겠지?)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST":
                prompterController.AddString("나이롱마스크", "어쩔 수 없죠우~ 그래도 잘 생각해 봐요우~");
                prompterController.AddString("훈이", "포켓볼 빵을 팔지는 않으시나요?");
                prompterController.AddString("나이롱마스크", "안 팔아요우~!");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST_END");
                break;

            case "ID_F_CHICKEN_WHAT_NOTINVEST_NOTINVEST_END":
                gameManager.storeOutAction = "(수상한 가게에 무턱대고 돈을 맡길 수는 없다. 그래도 빵을 세 개나 얻을 수 있다고 하니 더 고민해보고 방문하자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_CHICKEN_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 잠깐 기다려봐요우~");
                prompterController.AddString("나이롱마스크", "저는 사실 투자자에요우~");
                prompterController.AddString("나이롱마스크", "저한테 만 냥을 투자하면 포켓볼 뽱을 세 개까지 가져올 수 있어요우~");
                prompterController.AddOption("진짜요? 그럼 당장 투자할게요.", "Nylon", "ID_F_CHICKEN_NOTINVEST_INVEST");
                prompterController.AddOption("필요 없어요.", "Nylon", "ID_F_CHICKEN_NOTINVEST_NOTINVEST");
                break;

            case "ID_F_CHICKEN_NOTINVEST_INVEST":
                prompterController.AddString("나이롱마스크", "탁월한 선택이에요우~ 최선을 다할게요우~");
                prompterController.AddString("나이롱마스크", "물론 투자 실패 가능성도 있지만...");
                prompterController.AddString("훈이", "투자 실패요?");
                prompterController.AddString("나이롱마스크", "저는 그런 말 안 했어요우~ 다음에 또 와요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_NOTINVEST_INVEST_END");
                break;

            case "ID_F_CHICKEN_NOTINVEST_INVEST_END":
                gameManager.storeOutAction = "(빵을 세 개나 한번에 얻을 수 있다니 기대된다. 그런데 투자에 실패하면 어떻게 되는 거지?)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_CHICKEN_NOTINVEST_NOTINVEST":
                prompterController.AddString("나이롱마스크", "그렇다면 어쩔 수 없죠우~");
                prompterController.AddString("훈이", "그냥 포켓볼 빵을 팔지는 않아요?");
                prompterController.AddString("나이롱마스크", "안 팔아요우~! 투자하고 싶을 때 다시 오세요우~!");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_CHICKEN_NOTINVEST_NOTINVEST_END");
                break;

            case "ID_F_CHICKEN_NOTINVEST_NOTINVEST_END":
                gameManager.storeOutAction = "(아저씨가 치킨 파는 데는 관심이 없나보다. 투자를 하고 싶어지면 다시 오자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_WHAT":
                prompterController.AddString("나이롱마스크", "아무것도 안 팔아요우~ 대신 투자해요우~");
                prompterController.AddString("훈이", "투자요?");
                prompterController.AddString("나이롱마스크", "포켓볼 뽱 투자에요우~ 만 냥 투자로 최대 세 개까지 얻을 수 있어요우~");
                prompterController.AddString("나이롱마스크", "아무것도 못 얻을 수도 있지만...");
                prompterController.AddString("훈이", "뭐라고요?");
                prompterController.AddString("나이롱마스크", "아무것도 아니에요우~ 투자 하실래요우~? 뽱 많이 가져올 수 있어요우~");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("좋아요. 만 냥 투자할게요.", "ID_F_WHAT_INVEST", "ID_F_NOMONEY_BEFORECOIN");
                prompterController.AddOption("아니요. 괜찮아요.", "Nylon", "ID_F_WHAT_NOTINVEST");
                break;

            case "ID_F_WHAT_INVEST":
                prompterController.AddString("나이롱마스크", "완죤 좋은 선택이에요우~ 열심히 투자해 볼게요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddString("나이롱마스크", "다음에 또 봐요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_WHAT_INVEST_END");
                break;

            case "ID_F_WHAT_INVEST_END":
                gameManager.storeOutAction = "(빵을 세 개나 얻을 수 있다니 기대된다. 그런데 투자에 실패해서 아무것도 못 돌려받는 건 아니겠지?)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_WHAT_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 겁쟁이군요우~ 투자할 생각이 생기면 다시 와요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_WHAT_NOTINVEST_END");
                break;

            case "ID_F_WHAT_NOTINVEST_END":
                gameManager.storeOutAction = "(겁쟁이라니. 기분은 별로 안 좋지만 왠지 오기가 생긴다. 다음번엔 투자해 봐야겠다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED":
                prompterController.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
                prompterController.AddOption("투자는 잘 됐나요?", "Nylon", "ID_F_SECOND_IFINVESTED_BREADS_???");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_STOREOUT");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_???":
                gameManager.DomiTradeManager.Nylon_DrawBbang("ID_F_SECOND_IFINVESTED_BREADS_0", 20,
                    "ID_F_SECOND_IFINVESTED_BREADS_1", 45,
                    "ID_F_SECOND_IFINVESTED_BREADS_2", 30,
                    "ID_F_SECOND_IFINVESTED_BREADS_3", 15);
                PlayerPrefs.SetInt("NylonInvested", 0);
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_3":
                prompterController.AddString("나이롱마스크", "오우~ 예~ 완죤 잘 됐어요우~ 마치 투자의 신이 된 기분이에요우~");
                gameManager.AddBBangType(3, "나이롱");
                prompterController.AddString("훈이", "와 3개씩이나!");
                prompterController.AddString("나이롱마스크", "이 정돈 기본이에요우~! 또 투자해 볼래요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("당연하죠! 당장 투자할래요.", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST",
                    "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
                prompterController.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "똑똑한 선택이네요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 대박의 기운이 가득한 이몸만 믿으세요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_END":
                gameManager.storeOutAction = "(투자 한 번에 빵을 세 개나 얻다니, 완전 개이득이다. 다음 투자 결과도 무척 기대된다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "지금 투자의 신을 못 믿는 건가요우~? 어리석기 짝이 없군요우~ 아마 오늘 투자하지 않은 걸 후회할 거에요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_END":
                gameManager.storeOutAction = "(투자 한 번에 빵을 세 개나 얻다니, 완전 개이득이다. 그래도 두 번 연속 세 개를 얻긴 힘들겠지. 투자는 다음에 하자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_2":
                prompterController.AddString("나이롱마스크", "오우~ 꽤 잘 됐어요우~ 저는 투자 고수거든요우~");
                gameManager.AddBBangType(2, "나이롱");
                prompterController.AddString("훈이", "오 2개나!");
                prompterController.AddString("나이롱마스크", "나름 만족스럽죠우~? 또 투자해보는 건 어때요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("좋아요!", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST",
                    "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
                prompterController.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "투자의 묘미를 알아주는 분이군요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 제가 알아서 잘 투자해드릴테니 다음에 봐요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_END":
                gameManager.storeOutAction = "(투자도 빵을 얻는 데 좋은 방법인 것 같다. 다음 투자 결과도 기대된다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "이번 투자 결과가 마음에 안 들었나요우~? 이 정도면 정말 잘 한 거라고요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_END":
                gameManager.storeOutAction = "(투자 한 번에 빵 두 개 정도면 만족스럽다. 욕심을 너무 많이 부리는 건 좋지 않겠지. 투자는 다음에 하자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_1":
                prompterController.AddString("나이롱마스크", "오우~ 손해는 안 봤어요우~");
                gameManager.AddBBangType(1, "나이롱");
                prompterController.AddString("훈이", "1개네요.");
                prompterController.AddString("나이롱마스크", "맞아요우~ 이번에는 좀 아쉬운 결과지만 다음 번엔 더 잘할 수 있어요우~ 다시 한번 투자해 볼래요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("한번만 더 믿어볼게요.", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST",
                    "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
                prompterController.AddOption("아니요. 안 할래요.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST":
                prompterController.AddString("나이롱마스크", "한 번 더 기회를 줘서 고마워요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 이번엔 더 잘해 볼게요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_END":
                gameManager.storeOutAction = "(만 냥이나 투자해서 빵을 한 개밖에 못 얻다니. 조금 아쉽지만 다음 결과를 기다려보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST":
                prompterController.AddString("나이롱마스크", "정말 매정한 사람이네요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_END":
                gameManager.storeOutAction = "(끼워 파는 포켓볼 빵이랑 가격이 같다니 너무 비싸다. 조금 여유가 생기면 다시 투자해 보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_0":
                prompterController.AddString("나이롱마스크", "오우~ 이번에 상황이 좀 좋지 않았어요우~");
                prompterController.AddString("훈이", "한 개도 못 얻었다고요?");
                prompterController.AddString("나이롱마스크", "투자라는 게 원래 그런 거 아니겠어요우~? 맑은 날이 있으면 흐린 날이 있는 거죠우~");
                prompterController.AddString("나이롱마스크", "이번에는 정말 느낌이 좋은데 한번 더 투자해 볼래요우?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("정말이죠...? 그럼 딱 한번만 더 투자해 볼게요.", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST",
                    "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
                prompterController.AddOption("미쳤어요?", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "물론 정말이죠우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 이번엔 진짜 잘 될 거니까 저만 믿으세요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_END":
                gameManager.storeOutAction = "(만 냥이나 투자해서 빵을 하나도 못 얻다니 이거 완전 손해잖아? 다음 투자는 잘 되기를 기도한다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 그렇게 험한 말을~! 원숭이도 나무에서 떨어질 때가 있는 법이라고요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_END");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_END":
                gameManager.storeOutAction = "(완전 사기꾼이잖아? 분하다. 하지만 뭔가 또 투자하고 싶어서 손이 근질근질하다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFNOTINVESTED":
                prompterController.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~");
                prompterController.AddString("나이롱마스크", "포켓볼 뽱에 투자해볼 생각이 생기셨나요우~? 만 냥이면 포켓볼 뽱을 최대 3개 까지 얻을 수 있어요우~");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("네, 투자할게요.", "ID_F_SECOND_IFNOTINVESTED_INVEST",
                    "ID_F_NOMONEY_AFTERECOIN_BBANGLAST");
                prompterController.AddOption("아니요, 투자 안 할 건데요?", "Nylon", "ID_F_SECOND_IFNOTINVESTED_NOTINVEST");
                break;

            case "ID_F_SECOND_IFNOTINVESTED_INVEST":
                prompterController.AddString("나이롱마스크", "역시 투자할줄 있었어요우~ 저만 믿으세요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFNOTINVESTED_INVEST_END");
                break;

            case "ID_F_SECOND_IFNOTINVESTED_INVEST_END":
                gameManager.storeOutAction = "(결국 투자했다. 왠지 포켓볼 빵을 잔뜩 얻을 수 있을 것 같은 기분이 든다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_SECOND_IFNOTINVESTED_NOTINVEST":
                prompterController.AddString("나이롱마스크", "그럼 뭐하러 온 거에요우~ 투자할 생각 생기면 다시 와요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon", "ID_F_SECOND_IFNOTINVESTED_NOTINVEST_END");
                break;

            case "ID_F_SECOND_IFNOTINVESTED_NOTINVEST_END":
                gameManager.storeOutAction = "(역시 투자는 위험하다. 빵을 세 개나 얻을 수 있다는 건 매력적이지만 역시 돈이 아깝다. 좀 더 고민해본 뒤 투자하자.)";
                prompterController.AddNextAction("main", "store_out");
                break;
        }
    }

    public void Nylon_f(string ID)
    {
        MonoBehaviour.print("EVENT ID Nylon_f : " + ID);
        prompterController.Reset();
        prompterController.imageMode = true;

        switch (ID)
        {
            case "ID_F_FRIEND_HELLO":
                prompterController.AddString("나이롱마스크", "아뇽하세요우~ 파랑새 치킨임니다~ ");
                prompterController.AddString("나이롱마스크", "단골 손님~ 오늘도 이렇게 방문해 주셔서 캄사합니돠~");
                if (PlayerPrefs.GetInt("NylonInvested") == 0)
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_BREADS_???");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_???":
                gameManager.DomiTradeManager.Nylon_DrawBbang_f("ID_F_SECOND_IFINVESTED_BREADS_0", 20,
                    "ID_F_SECOND_IFINVESTED_BREADS_1", 50,
                    "ID_F_SECOND_IFINVESTED_BREADS_2", 20,
                    "ID_F_SECOND_IFINVESTED_BREADS_3", 10);
                PlayerPrefs.SetInt("NylonInvested", 0);
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_3":
                prompterController.AddString("훈이", "투자는 잘 됐나요?");
                prompterController.AddString("나이롱마스크", "오우~ 예~ 완죤 잘 됐어요우~ 마치 투자의 신이 된 기분이에요우~");
                gameManager.AddBBangType(3, "나이롱");
                prompterController.AddString("훈이", "와 3개씩이나!");
                prompterController.AddString("나이롱마스크", "이 정돈 기본이에요우~! 또 투자해 볼래요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("당연하죠! 당장 투자할래요.", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST", "ID_F_NOMONEY_BREAD",
                    "Nylon_f");
                prompterController.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "똑똑한 선택이네요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 이번에도 투자 잘해서 대봑 한번 터뜨려 볼게요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_INVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "지금 투자의 신을 못 믿는 건가요우~? 어리석기 짝이 없군요우~ 오늘 투자하지 않으면 후회할 거에요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_3BREADS_NOTINVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_2":
                prompterController.AddString("훈이", "투자는 잘 됐나요?");
                prompterController.AddString("나이롱마스크", "오우~ 꽤 잘 됐어요우~ 저는 투자 고수거든요우~");
                gameManager.AddBBangType(2, "나이롱");
                prompterController.AddString("훈이", "오 2개나!");
                prompterController.AddString("나이롱마스크", "나름 만족스럽죠우~? 또 투자해보는 건 어때요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("좋아요!", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST", "ID_F_NOMONEY_BREAD", "Nylon_f");
                prompterController.AddOption("죄송하지만 이번에는 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "투자의 묘미를 알아주는 분이군요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 다음번엔 뽱을 더 많이 가져오도록 노력할게요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_INVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "이번 투자 결과가 마음에 안 들었나요우~? 이 정도면 정말 잘 한 거라고요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_2BREADS_NOTINVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_1":
                prompterController.AddString("훈이", "투자는 잘 됐나요?");
                prompterController.AddString("나이롱마스크", "오우~ 손해는 안 봤어요우~");
                gameManager.AddBBangType(1, "나이롱");
                prompterController.AddString("훈이", "1개네요.");
                prompterController.AddString("나이롱마스크", "맞아요우~ 이번에는 좀 아쉬운 결과지만 다음 번엔 더 잘 할 수 있어요우~ 다시 한번 투자해 볼래요우~?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("한번만 더 믿어볼게요.", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST", "ID_F_NOMONEY_BREAD",
                    "Nylon_f");
                prompterController.AddOption("아니요. 안 할래요.", "Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST":
                prompterController.AddString("나이롱마스크", "한번 더 기회를 줘서 고마워요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 다음번엔 확실히 뽱을 더 많이 가져올 수 있을 거에요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_INVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST":
                prompterController.AddString("나이롱마스크", "정말 매정한 사람이네요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_1BREAD_NOTINVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_SECOND_IFINVESTED_BREADS_0":
                // No command found on line 89 : 
                prompterController.AddString("나이롱마스크", "오우~ 이번에 상황이 좀 좋지 않았어요우~");
                prompterController.AddString("훈이", "한 개 도 못 얻었다고요?");
                prompterController.AddString("나이롱마스크", "투자라는 게 원래 그런 거 아니겠어요우~? 맑은 날이 있으면 흐린 날이 있는 거죠우~");
                prompterController.AddString("나이롱마스크", "이번에는 정말 느낌이 좋은데 한번 더 투자해 볼래요우?");
                gameManager.DomiTradeManager.Nylon_TryInvestBbang("정말이죠...? 그럼 딱 한번만 더 투자해 볼게요.", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST",
                    "ID_F_NOMONEY_BREAD", "Nylon_f");
                prompterController.AddOption("미쳤어요?", "Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST":
                prompterController.AddString("나이롱마스크", "물론 정말이죠우~ I am 신뢰에요우~");
                prompterController.AddString("나이롱마스크", "만 냥 잘 받았어요우~ 다음 번엔 절대 투자 실패 같은 거 안 할 거에요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_INVEST_2":
                prompterController.AddString("", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 그렇게 험한 말을~! 원숭이도 나무에서 떨어질 때가 있는 법이라고요우~");
                if (PlayerPrefs.HasKey("NylonDomiTutorial"))
                    prompterController.AddNextAction("Nylon_f", "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2");
                else
                    prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN");
                break;

            case "ID_F_SECOND_IFINVESTED_0BREADS_NOTINVEST_2":
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN":
                prompterController.AddString("나이롱마스크", "무엇에 투자하시겠어요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_INTERSECTION":
                if (PlayerPrefs.GetInt("NylonInvested") == 0) gameManager.DomiTradeManager.Nylon_TryInvestBbang("빵에 투자하기", "ID_F_FRIEND_MAIN_BREAD", "ID_F_NOMONEY_BREAD", "Nylon_f");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddOption("코인 거래하기", "Nylon_f", "ID_F_FRIEND_MAIN_COIN");
                prompterController.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_MAIN_OUT");
                break;

            case "ID_F_FRIEND_MAIN_BREAD":
                prompterController.AddString("나이롱마스크", "포켓볼 뽱~에 투자하고 싶으시군요우~ 만 냥이면 포켓볼 뽱을 최대 3개 까지 얻을 수 있어요우~");
                prompterController.AddString("나이롱마스크", "투자를 확정하시겠어요우~?");
                prompterController.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_BREAD_YES");
                prompterController.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_BREAD_NO");
                break;

            case "ID_F_FRIEND_MAIN_BREAD_YES":
                prompterController.AddString("나이롱마스크", "역시 단골손님이에요우~ 화끈하네요우~");
                gameManager.PlayerHealthManager.UpdateMoney(-10000);
                PlayerPrefs.SetInt("NylonInvested", 1);
                prompterController.AddString("나이롱마스크", "다음에 들를 때 투자 결과를 알려드릴게요우~");
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_BREAD_NO":
                prompterController.AddString("나이롱마스크", "그럼 뭐하러 온 거에요우~");
                prompterController.AddString("나이롱마스크", "다른 투자에 관심이 있는 건가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_COIN":
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(true);
                var balancePercent = gameManager.DoMiCoinManager.GetBalancePercent();
                if (gameManager.DoMiCoinManager.GetAmount() > 0)
                {
                    if (balancePercent > 70)
                        prompterController.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 엄청나요우~ 이러다 정말 화성 가겠어요우~");
                    else if (balancePercent > 30)
                        prompterController.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 완전 떡상했네요우~");
                    else if (balancePercent > -30)
                        prompterController.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~");
                    else if (balancePercent > -70)
                        prompterController.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 떡락도 이런 떡락이 없네요우~");
                    else
                        prompterController.AddString("나이롱마스크", "현재 수익률은 " + balancePercent + "%에요우~ 오우~ 투자 정말 개못하시네요우~");
                }

                prompterController.AddOption("도미 코인을 사고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY");
                if (gameManager.DoMiCoinManager.GetAmount() > 0) prompterController.AddOption("도미 코인을 팔고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL");
                prompterController.AddOption("다른 거래를 하고 싶어요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_ANOTHER");
                break;

            case "ID_F_FRIEND_MAIN_COIN_ANOTHER":
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY":
                prompterController.AddString("나이롱마스크", "지금 코인 가격은 " + gameManager.DoMiCoinManager.GetPrice() + "냥 이에요우~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(true);
                if (gameManager.DoMiCoinManager.diff < 0.6)
                {
                    prompterController.AddString("나이롱마스크", "지금이니~? 평생에 한번 있을까말까한 기회에요우~ 저라면 몰빵했어요우~");
                    prompterController.AddString("나이롱마스크", "차가 있다면 차를 팔고 집이 있다면 집을 팔아서 도미 코인을 사야해요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 0.7)
                {
                    prompterController.AddString("나이롱마스크", "오늘이 블랙프라이데이인가요우~? 도미 코인이 미친듯한 할인을 하고 있네요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 0.85)
                {
                    prompterController.AddString("나이롱마스크", "바닥을 다지는 중이네요우~ 저라면 추매했어요우~ 물을 탈 때에요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1)
                {
                    prompterController.AddString("나이롱마스크", "도미 코인이 손님을 낚아먹으려고 하는군요우~ 물론 오를수도 있고 내릴 수도 있죠우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.15)
                {
                    prompterController.AddString("나이롱마스크", "이건 오른 것도 아니에요우~ 상승세 한번 타면 오늘이 제일 싸다는 걸 알게 될 거에요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.4)
                {
                    prompterController.AddString("나이롱마스크", "꽤 올랐네요우~ 하지만 아직 늦지 않았어요우~ 이 가격보다 오르기만 하면 이득이에요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.8)
                {
                    prompterController.AddString("나이롱마스크", "이미 버스 떠나버렸네요우~ 다음 버스를 기다리는 게 어떨까요우~? 물론 다음 버스가 온다면 말이죠우~");
                }
                else
                {
                    prompterController.AddString("나이롱마스크", "오우~ 은하수에 살던 도미가 안드로메다로 떠나갔군요우~ 배 아파해도 소용 없어요우~");
                    prompterController.AddString("나이롱마스크", "진작에 샀어야죠우~");
                }

                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_TRADE");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY_TRADE":
                gameManager.DomiTradeManager.Nylon_TradeCoin(GameManager.TradeType.Buy, "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER", "ID_F_FRIEND_MAIN_COIN_BUY_NOTBUY",
                    "ID_F_NOMONEY_COIN");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER":
                prompterController.AddString("나이롱마스크",
                    "총 " + gameManager.DoMiCoinManager.BuyAmount * gameManager.DoMiCoinManager.GetPrice() + "냥이에요우~ 구매를 확정하시겠어요우~?");
                prompterController.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_YES");
                prompterController.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_NO");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_YES":
                gameManager.DoMiCoinManager.BuyCoin(gameManager.DoMiCoinManager.BuyAmount);
                prompterController.AddString("나이롱마스크", "여기 도미 코인 " + gameManager.DoMiCoinManager.BuyAmount + "개에요우~ ");
                prompterController.AddString("나이롱마스크", "이제 도미 코인을 " + gameManager.DoMiCoinManager.GetAmount() + "개 가지고 계시네요우~");
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY_NUMBER_NO":
                prompterController.AddString("나이롱마스크", "변덕이 심하시군요우~ 몇 개 살지 확실히 정하세요우~");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_BUY_TRADE");
                break;

            case "ID_F_FRIEND_MAIN_COIN_BUY_NOTBUY":
                prompterController.AddString("나이롱마스크", "신중한 성격이시군요우~ 하지만 때론 과감할 필요도 있어요우~ 화성 갈끄니까~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL":
                prompterController.AddString("나이롱마스크", "지금 코인 가격은 " + gameManager.DoMiCoinManager.GetPrice() + " 이에요우~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(true);
                if (gameManager.DoMiCoinManager.diff < 0.6)
                {
                    prompterController.AddString("나이롱마스크", "오우~ 쥐저스~! 대공황이에요우~ 제 돈 아니라서 다행이에요우~");
                    prompterController.AddString("나이롱마스크", "만약 제가 지금 코인을 들고 있었다면 정신차리려고 세수하다가 화가나서 세면대를 부수고 말았을 거에요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 0.7)
                {
                    prompterController.AddString("나이롱마스크", "바닥 밑에 지하실이 있었네요우~ 당신이 선택한 코인이에요우~ 악으로 깡으로 버티세요우~!");
                }
                else if (gameManager.DoMiCoinManager.diff < 0.85)
                {
                    prompterController.AddString("나이롱마스크", "마음이 아프네요우~ 하지만 아프니까 청춘이죠우~ 단골 손님은 이팔청춘이네요우~ 너무 아프니까요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1)
                {
                    prompterController.AddString("나이롱마스크", "약간의 손실이 있네요우~ 하지만 이정도로 엄살 부리는 찌질이는 없겠죠우~?");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.15)
                {
                    prompterController.AddString("나이롱마스크", "오늘 장은 심심하네요우~ 차라리 적금을 드는 게 낫겠어요우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.4)
                {
                    prompterController.AddString("나이롱마스크", "나쁘지 않은 수익이에요우~ 이 정도는 돼야 코인할 맛이 나죠우~");
                }
                else if (gameManager.DoMiCoinManager.diff < 1.8)
                {
                    prompterController.AddString("나이롱마스크", "가즈아~ 불꽃 같은 상승에 제 마음이 뜨거워지는군요우~");
                }
                else
                {
                    prompterController.AddString("나이롱마스크", "오우~ 마취 로켓이 발사되는 것 같은 급등세로군요우~ 축하해요우~");
                    prompterController.AddString("나이롱마스크", "이정도면 하늘나라 천사 똥침도 찌를 수 있겠어요우~ ");
                }

                prompterController.AddString("나이롱마스크",
                    "지금 도미 코인을 " + gameManager.DoMiCoinManager.GetAmount() + "개 가지고 있으시네요우~ 몇 개 파시겠어요우~? ");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_TRADE");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL_TRADE":
                gameManager.DomiTradeManager.Nylon_TradeCoin(GameManager.TradeType.Sell, "ID_F_FRIEND_MAIN_COIN_SELL_NUMBER",
                    "ID_F_FRIEND_MAIN_COIN_SELL_NOTSELL", "");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL_NUMBER":
                prompterController.AddString("나이롱마스크",
                    "총 " + gameManager.DoMiCoinManager.SellAmount * gameManager.DoMiCoinManager.GetPrice() + "냥을 얻을 수 있어요우~ 판매를 확정하시겠어요우~?");
                prompterController.AddOption("네.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_YES");
                prompterController.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_NO");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL_YES":
                gameManager.DoMiCoinManager.SellCoin(gameManager.DoMiCoinManager.SellAmount);
                prompterController.AddString("나이롱마스크", "도미 코인을 " + gameManager.DoMiCoinManager.SellAmount + "개 팔았어요우~ ");
                prompterController.AddString("나이롱마스크", "이제 도미 코인이 " + gameManager.DoMiCoinManager.GetAmount() + "개 남았네요우~");
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL_NO":
                prompterController.AddString("나이롱마스크", "완전 변덕쟁이군요우~ 몇 개 팔지 확실히 정하세요우~");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_COIN_SELL_TRADE");
                break;

            case "ID_F_FRIEND_MAIN_COIN_SELL_NOTSELL":
                prompterController.AddString("나이롱마스크", "지금 팔긴 좀 아깝긴 하죠우~ 하지만 언제나 떨어질 수 있다는 걸 명심하세요우~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddString("나이롱마스크", "더 하고 싶은 일이 있으신가요우~?");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_MAIN_OUT":
                gameManager.storeOutAction = "(투자는 개인의 선택이다. 나는 오늘의 선택에 후회하지 않는다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_NOMONEY_BREAD":
                prompterController.AddString("나이롱마스크", "오우~ 손님~ 돈이 부족하네요우~");
                prompterController.AddString("나이롱마스크", "돈을 벌어서 포켓볼 뽱에 투자해 보세요우~");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_NOMONEY_COIN":
                prompterController.AddString("나이롱마스크", "오우~ 돈이 없잖아요우~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddString("나이롱마스크", "티끌 모아 태산이라는 말이 있죠우~");
                prompterController.AddString("나이롱마스크", "티끌이라도 있어야 태산이 된다는 뜻이에요우~");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_MAIN_INTERSECTION");
                break;

            case "ID_F_FRIEND_COINEXPLAIN":
                prompterController.AddString("나이롱마스크", "그나저나 우리가 투자로 인연을 쌓은지도 꽤 오래됐군요우~ 당신은 단골 손님이에요우~");
                //become friend
                PlayerPrefs.SetInt("NylonDomiTutorial", 1);
                prompterController.AddString("나이롱마스크", "그래서 제가 새로운 투자 상품을 준비해 봤어요우~ 한번 들어볼래요우~?");
                prompterController.AddString("훈이", "오! 어떤 거죠?");
                prompterController.AddString("나이롱마스크", "바로 도미 코인이에요우~");
                prompterController.AddString("훈이", "도미 코인이요?");
                prompterController.AddString("나이롱마스크", "비트코인 같은 거에요우~ 도미 코인을 사면 실시간으로 가격이 바뀌어요우~ 쌀 때 사서 비쌀 때 사면 되요우~");
                prompterController.AddString("훈이", "이것도 만 냥씩 투자하면 되나요?");
                prompterController.AddString("나이롱마스크", "아니요우~ 원하는 금액만큼 자유롭게 투자할 수 있어요우~ 한번 질러 볼래요우~? 벼락 부자 될 수 있어요우~");
                prompterController.AddOption("좋아요. 한번 해볼게요.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST");
                prompterController.AddOption("아니요. 돈은 알바해서 정직하게 벌어야죠.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_NOTINVEST");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST":
                prompterController.AddString("나이롱마스크", "역시 손님이라면 코인의 가치를 알아봐 줄줄 알았어요우~");
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_2":
                prompterController.AddString("나이롱마스크", "지금 코인 가격은 " + gameManager.DoMiCoinManager.GetPrice() + "냥 이에요우~ 코인을 몇 개 구매할래요우~?");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(true);
                prompterController.AddNextAction("Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2_TRADE");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_2_TRADE":
                gameManager.DomiTradeManager.Nylon_TradeCoin(GameManager.TradeType.Buy, "ID_F_FRIEND_COINEXPLAIN_INVEST_NUMBER",
                    "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST", "ID_F_NOMONEY_COIN");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_NUMBER":
                prompterController.AddString("나이롱마스크",
                    "총 " + gameManager.DoMiCoinManager.GetPrice() * gameManager.DoMiCoinManager.BuyAmount + "냥이에요우~ 구매를 확정하시겠어요우~?");
                prompterController.AddOption("네.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_YES");
                prompterController.AddOption("아니요.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_2");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_YES":
                prompterController.AddString("나이롱마스크",
                    "여기 도미 코인 " + gameManager.DoMiCoinManager.BuyAmount + "개에요우~ 역시 손님은 도전 정신이 투철하시군요우~ 도전하는 사람은 아름다워요우~");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddString("훈이", "가격이 이것보다 떨어지진 않겠죠?");
                prompterController.AddString("나이롱마스크", "걱정하지 마요우~ 떨어지면 또 사면 되죠우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_YES_OUT");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_YES_OUT":
                gameManager.storeOutAction = "(도미 코인을 샀다. 올랐으면 좋겠다.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_NOTINVEST":
                prompterController.AddString("나이롱마스크", "오우~ 쫄보가 따로 없네요우~ 다음에 왔을 땐 도미 코인 가격 따따블 돼 있을 거에요우~ 그땐 후회해도 소용 없어요우~");
                prompterController.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_NOTINVEST_OUT");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_NOTINVEST_OUT":
                gameManager.storeOutAction = "(도미 코인에 투자할 수 있게 됐다. 다음에 한번 투자해 보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST":
                prompterController.AddString("나이롱마스크", "답답하네요우~! 그런 푼돈 벌어서 언제 부자 될래요우~?");
                gameManager.DomiTradeManager.Nylon_OpenTradePanel(false);
                prompterController.AddOption("편의점을 나간다.", "Nylon_f", "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST_OUT");
                break;

            case "ID_F_FRIEND_COINEXPLAIN_INVEST_NOTINVEST_OUT":
                gameManager.storeOutAction = "(코인이라니. 열심히 번 돈을 하루 아침에 날려 버릴 수는 없다. 그래도 인생은 한방이긴 한데... 다음에 투자해 보자.)";
                prompterController.AddNextAction("main", "store_out");
                break;
        }
    }
}