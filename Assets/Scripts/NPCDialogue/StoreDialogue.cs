using UnityEngine;

public class StoreDialogue
{
    private GameManager gameManager;
    private PrompterController prompterController;
    
    public StoreDialogue(GameManager gameManager)
    {
        this.gameManager = gameManager;
        prompterController = gameManager.PrompterController;
    }

    public void StoreEvent(string code)
    {
        MonoBehaviour.print("STORE EVENT : " + code);

        switch (code)
        {
            case "p_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박종업원", "분식집에서 빵을 왜 찾아유.");
                prompterController.AddString("박종업원", "그런거 없어유.");
                prompterController.AddString("훈이", "아 네..");
                prompterController.AddOption("그러면 뭐 팔아요?", "store", "p_what");
                prompterController.AddOption("편의점을 나간다", "store", "p_out");
                break;

            case "p_what":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박종업원", "분식집하면 떡볶이 아니겠슈.");
                prompterController.AddString("박종업원", "떡볶이 레시피를 개발중인데\n한번 시식 해보실래유?");
                prompterController.AddString("훈이", "(음.. 배가 고프긴 한 것 같은데..)");
                prompterController.AddOption("네. 먹어볼게요.", "store", "p_yes");
                prompterController.AddOption("아니요. 괜찮아요.", "store", "p_no");
                break;

            case "p_yes":
                gameManager.DdukMinigameManager.StartDduk();
                break;

            case "p_again":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박종업원", "아이구. 제료가 방금 막 다 떨어졌어유.");
                prompterController.AddString("훈이", "어쩔수 없죠. 다음에 또 들르면 들를게요.");
                prompterController.AddString("박종업원", "다음에 언제든 또 오세유.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "p_good":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박종업원", "맛있게 먹었다니 기분이 참 좋군유.");
                prompterController.AddString("박종업원", "다음에 언제든 또 오세유.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "p_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "괜찮아요. 다음에 또 들를게요.");
                prompterController.AddString("박종업원", "섭섭하네유.");
                prompterController.AddString("박종업원", "알았어유. 다음에 또 오세유.");
                gameManager.storeOutAction = "다음에는 떡볶이를 먹어봐야겠다..";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "p_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박종업원", "다음에 또 오세유.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "girlf_none":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "이상하다..\n알림 설정이 꺼져있는거 아니야?");
                prompterController.AddString("미소녀", "핸드폰 설정에서 알림이 꺼저있는 건 아닌지 확인해봐!");
                prompterController.AddNextAction("store", "notiSetting");
                break;

            case "notiSetting":
#if UNITY_IPHONE
                gameManager.PhoneMessageController.SetMsg("아이폰 설정으로 이동할까요?\n[알림] 탭에서 [포켓볼빵 더게임] 알림 설정을 확인해주세요.", 2,
                    "SendtoSetting");
#endif
                prompterController.AddString("훈이", "고마워!");
                prompterController.AddString("미소녀", "후훗 천만에 말씀.");
                prompterController.AddOption("또 문자 보내줄 수 있어?", "store", "girlf_yes");
                prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                break;


            case "hostf_ball":
                if (!gameManager.MatdongsanBuy)
                {
                    prompterController.Reset();
                    prompterController.imageMode = true;
                    prompterController.AddString("점주", "그럼. 자네를 위해서라면 당연히 있지!");
                    prompterController.AddString("점주", "하지만 알지? 맛동산 한 박스를 같이 사야해!");
                    prompterController.AddString("훈이", "그래서 얼마라구요?");
                    prompterController.AddString("점주", "맛동산은 만냥이야. 어떻게 할 텐가?");
                    prompterController.AddOption("산다.", "store", "hostf_buy_check");
                    prompterController.AddOption("안 산다.", "store", "hostf_notbuy");
                }
                else
                {
                    prompterController.Reset();
                    prompterController.imageMode = true;
                    prompterController.AddString("점주", "자네 조금 아까 샀잖아?");
                    prompterController.AddString("점주", "다음에 또 구해둘 테니 언제든 들르라구!");
                    prompterController.AddString("훈이", "네 알겠어요.");
                    prompterController.AddOption("알바를 한다.", "store", "hostf_alba");
                    prompterController.AddOption("편의점을 나간다.", "store", "hostf_out");
                }

                break;

            case "hostf_buy_check":
                gameManager.PhoneMessageController.SetMsg("맛동산 1상자와 포켓볼빵을 10,000냥에 구매합니다.", 2, "matdongsanf");
                break;

            case "hostf_notbuy":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "안 살래요..");
                prompterController.AddString("점주", "그래. 난 강요하지 않았어!");
                gameManager.storeOutAction = "끼워팔기라니 너무한걸..";
                prompterController.AddOption("알바자리 있어요?", "store", "hostf_alba");
                prompterController.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "hostf_buy_noMoney":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "뭐야! 자네 돈이 부족하잖아?");
                prompterController.AddString("점주", "용돈이 필요하면 알바를 하라구!");
                prompterController.AddString("훈이", "네 그럴게요..");
                gameManager.storeOutAction = "요즘 물가가 많이 올랐다..";
                prompterController.AddOption("지금 알바 할게요!", "store", "hostf_alba");
                prompterController.AddOption("편의점을 나간다.", "store", "hostf_out");
                break;

            case "hostf_buy":
                gameManager.gameObject.GetComponent<PlayerStatusManager>().UpdateMoney(-10000);
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "뭔가 손해보는 것 같지만..");
                prompterController.AddString("점주", "자 여기있어.");
                prompterController.AddString("맛동산", "맛동산 한박스를 겟했다.");
                prompterController.AddString("훈이", "감사합니다..");
                gameManager.storeOutAction = "손해보는 것 같지만 기분은 좋다.";
                gameManager.MatdongsanBuy = true;

                if (!gameManager.PlayerStatusManager.IsHeartEmpty())
                {
                    prompterController.AddOption("편의점을 나간다.", "store", "hostf_out");
                    prompterController.AddOption("알바를 한다.", "store", "hostf_alba");
                }
                else
                {
                    prompterController.AddNextAction("store", "hostf_out");
                }

                gameManager.AddBBangType(1, "점주");
                PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
                gameManager.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_mat, 1);
                break;

            case "hostf_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "다음에 또 보자구!");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "hostf_alba":
                if (gameManager.PlayerStatusManager.IsHeartEmpty())
                {
                    prompterController.Reset();
                    prompterController.imageMode = true;
                    prompterController.AddString("훈이", "지금은 좀 피곤한데..");
                    prompterController.AddString("훈이", "알바는 다음에 하는게 좋겠다.");

                    if (!gameManager.MatdongsanBuy & (PlayerPrefs.GetInt("money") >= 10000))
                    {
                        prompterController.AddOption("포켓볼빵 살게요! 아직 있죠?", "store", "hostf_ball");
                        prompterController.AddOption("편의점을 나간다.", "store", "hostf_out");
                    }
                    else
                    {
                        prompterController.AddNextAction("store", "hostf_out");
                    }

                    return;
                }

                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "잘 됐다! 일손이 많이 부족했었는데!");
                prompterController.AddOption("바로 시작할게요.", "store", "albaf_start");
                prompterController.AddOption("어떻게 하면 되나요?", "store", "albaf_how");
                break;

            case "albaf_start":
                gameManager.PlayerStatusManager.ConsumeSingleHeart();
                gameManager.AlbaMinigameManager.StartAlba(true);
                break;

            case "albaf_end":
                prompterController.Reset();
                prompterController.imageMode = true;

                gameManager.storeOutAction = "휴.. 편의점 알바도 쉬운게 아니구나..";
                prompterController.AddOption("알바 더할게요.", "store", "hostf_alba");
                if (!gameManager.MatdongsanBuy & (PlayerPrefs.GetInt("money") >= 10000)) prompterController.AddOption("이제 포켓볼빵 살게요! 아직 있죠?", "store", "hostf_ball");
                prompterController.AddOption("편의점을 나간다.", "store", "hostf_out");
                break;

            case "albaf_how":
                prompterController.Reset();
                prompterController.imageMode = true;

                prompterController.AddString("점주", "손님한테 포켓볼빵 없다고만 얘기하면 돼!");
                prompterController.AddString("점주", "만약 언제 들어오는지 묻는다면 모른다고 답하면 돼!");
                prompterController.AddString("훈이", "참 별거 없네요.");
                prompterController.AddString("점주", "허허 그래도 상당한 어려운 일이라구!");
                prompterController.AddString("점주", "준비 됐나 이제?");
                prompterController.AddOption("네 시작할게요.", "store", "albaf_start");
                prompterController.AddOption("다시 설명해주세요!", "store", "albaf_how");
                break;

            case "note":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("박법학박사", "포켓볼빵은 품절되었단다!\n자 다음 편의점으로 당장 이동하렴!");
                prompterController.AddString("훈이", "품절인가보다..");
                prompterController.AddNextAction("store", "bbobgiStore");
                break;

            case "bbobgi":
                if (gameManager.currentLocation == "store")
                {
                    gameManager.currentLocation = "bbobgi";
                    gameManager.newStore.GetComponent<StoreControl>().StartBbobgi();
                }

                break;

            case "bbobgiStore":
                prompterController.Reset();
                prompterController.AddOption("뽑기 기계를 확인한다.", "store", "bbobgi");
                prompterController.AddOption("다른 편의점으로 간다.", "main", "store_next");
                prompterController.AddOption("집으로 돌아간다.", "main", "go_home");
                break;

            case "alba_start":
                gameManager.AlbaMinigameManager.StartAlba();
                break;

            case "alba_how":
                prompterController.Reset();
                prompterController.imageMode = true;

                prompterController.AddString("점주", "별거 없어! 요새 사람들이 다들 포켓볼빵만 찾으니까");
                prompterController.AddString("점주", "포켓볼빵 없다고만 얘기하면 돼!");
                prompterController.AddString("훈이", "그렇게만 얘기하면 되나요?");
                prompterController.AddString("점주", "그래 그게 다야!");
                prompterController.AddString("점주", "만약 언제 들어오는지 묻는다면 모른다고 답하면 돼!");
                prompterController.AddString("점주", "별거 없네요.");
                prompterController.AddString("점주", "허허 그래도 상당한 일이라구!");
                prompterController.AddString("점주", "준비 됐나 이제?");
                prompterController.AddOption("네 시작할게요.", "store", "alba_start");
                prompterController.AddOption("다시 설명해주세요!", "store", "alba_how");
                break;

            case "y_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "요즘 그 핫한 포켓볼빵..");
                prompterController.AddString("열정맨", "포켓볼빵에 대해 얼마나 간절하십니까?");

                prompterController.AddOption("엄청 간절해요!", "store", "y_yes");
                prompterController.AddOption("간절해요!", "store", "y_no");
                prompterController.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "그래요 그렇겠죠.");
                prompterController.AddString("열정맨", "포켓볼빵을.. 왜 드시고 싶습니까?");

                prompterController.AddOption("유행이라고 하니까요!", "store", "y_no");
                prompterController.AddOption("추억의 맛이 생각나서요!", "store", "y_yes2");
                prompterController.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "옛날에 먹던 그 추억의 맛이..");
                prompterController.AddString("훈이", "너무도 그리워서 계속 찾게 되더라구요..");

                prompterController.AddString("열정맨", "마지막으로 하고싶은 말 없습니까?");

                prompterController.AddString("훈이", "그리고 스티커를 모으는 건 마치..");

                prompterController.AddOption("추억을 모으는 것 같아요.", "store", "y_yes3");
                prompterController.AddOption("중독적이에요.", "store", "y_no");
                prompterController.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_yes3":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "스티커를 모으는 건 마치..");
                prompterController.AddString("훈이", "제 어릴적 추억을 모으는 것 같아요.");
                prompterController.AddString("훈이",
                    "하나 둘 스티커를 모으다                                                                                  린 아이였던 제 모습이 떠올라요.");
                prompterController.AddString("훈이", "그래서 포켓볼빵을 다시 맛보고 싶어요.");

                prompterController.AddString("열정맨", "...");
                prompterController.AddString("열정맨", "합격!!");
                prompterController.AddString("열정맨", "간절함을 잘 느껴졌어요. 빵을 드릴게요!!");
                prompterController.AddString("열정맨", "하지만 우리 매장에는 메이풀빵만 들어와요.");

                prompterController.AddString("훈이", "메이풀빵이요..?");
                prompterController.AddString("열정맨", "네 추억의 과자 풀빵이죠.");

                gameManager.AddBBangType(1, "열정맨", "maple");
                gameManager.storeOutAction = "면접을 합격한 기분이다.";
                PlayerPrefs.SetInt("yull_friend", 1);
                PlayerPrefs.Save();

                prompterController.AddString("열정맨", "다음에도 지나갈때 또 들러요.");
                prompterController.AddString("훈이", "네. 감사합니다!");


                prompterController.AddOption("편의점을 나간다.", "store", "yf_out");
                break;

            case "y_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "간절함이 없네요!!");
                prompterController.AddString("열정맨", "당신에게 판매할 빵은 없어요.");

                gameManager.storeOutAction = "아니. 빵을 사는데 면접을 본다고..?!";

                prompterController.AddOption("편의점을 나간다.", "store", "y_out");
                break;

            case "y_ask":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "저희 가게에는 포켓볼빵이 들어오지 않아요.");
                prompterController.AddString("훈이", "아.. 네..");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                prompterController.AddOption("편의점을 나간다", "store", "y_out");
                break;

            case "y_when":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "저희 가게에는 포켓볼빵이 들어오지 않아요.");
                prompterController.AddString("훈이", "아.. 네..");
                prompterController.AddOption("포켓볼빵 있나요?", "store", "y_ball");
                prompterController.AddOption("편의점을 나간다", "store", "y_out");
                break;

            case "y_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "안녕하가세요.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yf_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "간절한 손님이라면 언제든 또 오세요!");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yf_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "저희 가게에는 포켓볼빵이 안 들어와요.");
                prompterController.AddString("열정맨", "대신 풀빵이 있지요.");
                prompterController.AddOption("메이풀빵 있나요?", "store", "yf_maple");
                prompterController.AddOption("편의점을 나간다", "store", "yf_out");
                break;

            case "yf_maple":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("열정맨", "아, 추억의 풀빵이요! 재고 있나 확인해볼게요!");

                var rnd00 = Random.Range(0, 4);
                if (rnd00 == 0)
                {
                    prompterController.AddString("열정맨", "아이고.. 오늘은 빵이 다 떨어졌네요.");
                    prompterController.AddString("열정맨", "다음에 다시 와야겠어요.");
                    prompterController.AddString("훈이", "네, 다음에 다시 들를게요!");
                    prompterController.AddNextAction("store", "yf_out");
                }
                else
                {
                    prompterController.AddString("열정맨", "여기 하나 있네요!");
                    gameManager.AddBBangType(1, "열정맨", "maple");
                    gameManager.storeOutAction = "추억의 메이풀빵을 얻었다!";
                    prompterController.AddString("훈이", "네! 감사합니다!");
                    prompterController.AddNextAction("store", "yf_out");
                }

                break;

            case "host_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "그럼. 당연히 있지!");
                prompterController.AddString("점주", "하지만 맛동산 한 박스를 같이 사야해!");
                prompterController.AddString("훈이", "네? 맛동산을 사야 포켓볼빵을 살 수 있다고요?");
                prompterController.AddString("훈이", "얼마인데요?");
                prompterController.AddString("점주", "맛동산은 만냥이야. 어떻게 할 텐가?");
                prompterController.AddOption("산다.", "store", "host_buy_check");
                prompterController.AddOption("안 산다.", "store", "host_notbuy");
                break;

            case "host_buy_check":
                gameManager.PhoneMessageController.SetMsg("맛동산 1상자와 포켓볼빵을 10,000에 구매합니다.", 2, "matdongsan");
                break;

            case "host_buy_noMoney":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "뭐야! 자네 돈이 부족하잖아?");
                prompterController.AddString("점주", "용돈이 필요하면 알바를 하러 오라구!");
                prompterController.AddString("훈이", "네 그럴게요..");
                gameManager.storeOutAction = "요즘 물가가 많이 올랐다..";
                prompterController.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "host_buy":
                gameManager.gameObject.GetComponent<PlayerStatusManager>().UpdateMoney(-10000);
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "뭔가 손해보는 것 같지만..");
                prompterController.AddString("점주", "자 여기있어.");
                prompterController.AddString("맛동산", "맛동산 한박스를 겟했다.");
                gameManager.AddBBangType(1, "점주");
                prompterController.AddString("훈이", "감사합니다..");
                gameManager.storeOutAction = "손해보는 것 같지만 기분은 좋다.";
                prompterController.AddOption("편의점을 나간다.", "store", "host_out");

                PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
                PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
                gameManager.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_mat, 1);
                break;

            case "host_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "언제든 또 오라구!");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "host_notbuy":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "안 살래요..");
                prompterController.AddString("점주", "그래. 난 강요하지 않았어!");
                gameManager.storeOutAction = "끼워팔기라니 너무한걸..";
                prompterController.AddOption("편의점을 나간다.", "store", "host_out");
                break;

            case "host_alba":
                prompterController.Reset();
                prompterController.imageMode = true;
                if (PlayerPrefs.GetInt("albaExp") > 0)
                {
                    prompterController.AddString("점주", "당연히 있지!");
                    prompterController.AddString("점주", "이렇게 불쑥 찾아오지 말고 앱으로 신청하고 오라구!");
                    prompterController.AddString("점주", "알밥천국 앱 알지?");
                    prompterController.AddOption("네 알아요.", "store", "host_alba_ok");
                    prompterController.AddOption("잘 모르는데요.", "store", "host_alba_dontknow");
                }
                else
                {
                    prompterController.AddString("점주", "자네 알바 자리 관심있나?");
                    prompterController.AddString("훈이", "네..");
                    prompterController.AddString("점주", "마침 일손이 필요했는데 잘됐어!");
                    prompterController.AddString("점주", "요새 포켓볼빵을 찾는 사람이 워낙 많아서 말이야!");
                    prompterController.AddString("점주", "핸드폰에 알밥천국 앱 있지?");
                    prompterController.AddOption("네 있어요.", "store", "host_alba_ok");
                    prompterController.AddOption("그게 뭔데요?", "store", "host_alba_dontknow");
                    PlayerPrefs.SetInt("albaExp", 1);
                }

                break;

            case "host_alba_ok":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "그래. 알밥천국. 그 앱으로 신청할 수도 있고 다음에 지나가다 들러도 되고!");
                prompterController.AddString("훈이", "네. 알겠어요.");
                prompterController.AddString("점주", "언제든 또 오라구!");
                gameManager.storeOutAction = "편의점 알바로 용돈을 좀 모아볼까..?";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "host_alba_dontknow":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("점주", "젊은 사람이 그것도 모르나!");
                prompterController.AddString("점주", "핸드폰을 보면 '알밥천국'이라는 앱이 있을거야.");
                prompterController.AddString("점주", "그 앱으로 알바를 신청하고 나한테 찾아오면 돼.");
                prompterController.AddString("점주", "그러면 알바를 시켜주지.");
                prompterController.AddString("훈이", "네. 알겠어요.");
                prompterController.AddString("점주", "그래 이해가 빠르구먼!");
                gameManager.storeOutAction = "편의점 알바로 용돈을 좀 모아볼까..?";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "girl_ball":
                prompterController.Reset();
                prompterController.imageMode = true;

                if (Random.Range(0, 10) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        prompterController.AddString("미소녀", "포켓볼빵 1개 있어요.");
                        gameManager.AddBBangType(1, "미소녀");
                        gameManager.storeOutAction = "기분이 너무 좋다.";
                        prompterController.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                    }
                    else
                    {
                        prompterController.AddString("미소녀", "포켓볼빵 2개 있어요.");
                        gameManager.AddBBangType(2, "미소녀");
                        gameManager.storeOutAction = "기분이 너무 너무 좋다.";
                        prompterController.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                    }
                }
                else
                {
                    prompterController.AddString("미소녀", "포켓볼빵 없어요.");
                    prompterController.AddOption("편의점을 나간다.", "store", "girl_noBbang");
                }

                break;
            case "girl_boy":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "남자친구 있어요.");
                prompterController.AddOption("편의점을 나간다.", "main", "store_out");
                gameManager.storeOutAction = "(난 괜찮아....)";
                break;
            case "girl_phone":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "네..? 왜요..?");
                prompterController.AddOption("포켓몬 빵 들어오면 알려주세요.", "store", "girl_call");
                prompterController.AddOption("제 이상형이에요.", "store", "girl_type");
                prompterController.AddOption("저랑 친구 할래요?", "store", "girl_friend");
                break;
            case "girl_call":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "제가 왜 그래야 하죠?");
                prompterController.AddString("미소녀", "귀찮게 하지말고 나가세요.");
                gameManager.storeOutAction = "(포켓볼 빵을 구하는 것은 어렵구나.)";
                prompterController.AddOption("편의점을 나간다.", "main", "store_out");
                break;
            case "girl_type":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "미안한데 제 스타일 아니에요.");
                prompterController.AddString("미소녀", "볼 일 다 보셨으면 나가주세요.");
                gameManager.storeOutAction = "(눈에서 땀이 흐른다.)";
                prompterController.AddOption("편의점을 나간다.", "main", "store_out");
                break;
            case "girl_friend":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "응 그래.");
                prompterController.AddString("훈이", "(소녀와 친구가 되었다.)");
                PlayerPrefs.SetInt("girl_friend", 1);
                PlayerPrefs.Save();
                prompterController.AddOption("편의점을 나간다.", "store", "girl_out");
                break;
            case "girl_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "또 와.");
                gameManager.storeOutAction = "다음에 만나면 빵이 언제 들어오나 물어봐야 겠다.";
                prompterController.AddNextAction("main", "store_out");
                break;
            case "girl_noBbang":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "또 오세요.");
                prompterController.AddNextAction("main", "store_out");
                break;
            case "girlf_ball":
                prompterController.Reset();
                prompterController.imageMode = true;

                if (Random.Range(0, 6) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        prompterController.AddString("미소녀", "응, 포켓볼빵 1개 있어.");
                        gameManager.AddBBangType(1, "미소녀");
                        gameManager.storeOutAction = "기분이 너무 좋다.";
                        prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                    }
                    else
                    {
                        prompterController.AddString("미소녀", "응, 포켓볼빵 2개 있어.");
                        gameManager.AddBBangType(2, "미소녀");
                        gameManager.storeOutAction = "기분이 너무 너무 좋다.";
                        prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                    }
                }
                else
                {
                    prompterController.AddString("미소녀", "포켓볼빵 없어.");
                    prompterController.AddOption("언제 들어와?.", "store", "girlf_when");
                    prompterController.AddOption("거짓말.", "store", "girlf_lie");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }

                break;
            case "girlf_noBbang":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "다음에 또 와.");
                prompterController.AddNextAction("main", "store_out");
                gameManager.storeOutAction = "포켓볼빵은 참 구하기 어렵다.";
                break;
            case "girlf_when":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "잘 모르겠는데.");
                prompterController.AddOption("그러면 들어올 때 알려줄래?", "store", "girlf_when2");
                prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                break;
            case "girlf_when2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "그래. 들어오면 문자 할까?");
                prompterController.AddOption("응 부탁해.", "store", "girlf_yes");
                prompterController.AddOption("아니 괜찮아.", "store", "girlf_no");
                break;
            case "girlf_yes":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "알겠어. 빵 들어오면 문자 할게.");
                prompterController.AddString("훈이", "정말 고마워!");
                prompterController.AddString("미소녀", "응. 다음에 또 봐.");
                gameManager.storeOutAction = "좋은 친구다.\n연락이 오면 바로 달려와야겠다.";
                prompterController.AddNextAction("main", "store_out");
                gameManager.AddAlarm();
                break;
            case "girlf_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("미소녀", "그래. 뭐 더 필요한 거 있어?");
                prompterController.AddString("훈이", "아니 없어.");
                prompterController.AddString("미소녀", "응. 다음에 또 봐.");
                prompterController.AddNextAction("main", "store_out");
                break;
            case "girlf_lie":
                prompterController.Reset();
                prompterController.imageMode = true;
                if (Random.Range(0, 2) == 0)
                {
                    prompterController.AddString("미소녀", "헤헷 들켰네. 자 여기.");
                    gameManager.AddBBangType(1, "미소녀");
                    prompterController.AddString("훈이", "고마워.");
                    prompterController.AddOption("언제 또 들어와?", "store", "girlf_when");
                    prompterController.AddOption("편의점을 나간다.", "store", "girlf_noBbang");
                }
                else
                {
                    prompterController.AddString("미소녀", "무슨 소리 하는거야?");
                    prompterController.AddString("미소녀", "귀찮게 하지말고 빨리 나가.");
                    prompterController.AddNextAction("main", "store_out");
                }

                break;

            case "yang_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "거기 없으면 없는거예요.");
                prompterController.AddOption("편의점을 둘러본다.", "store", "yang_find_1");
                prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_exit":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "안녕히가세요.");
                gameManager.storeOutAction = "무슨 게임을 하는지 궁금하다.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yang_exit2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "안녕히가세요.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yang_find_1":
                prompterController.Reset();
                prompterController.imageMode = true;
                if (Random.Range(0, 10) == 0)
                {
                    prompterController.AddString("훈이", "포켓볼빵 1개가 있다!");
                    gameManager.AddBBangType(1, "양아치");
                    gameManager.storeOutAction = "기분이 너무 좋다.";
                    prompterController.AddOption("편의점을 나간다.", "store", "yang_exit2");
                    prompterController.AddOption("빵을 더 찾아본다.", "store", "yang_find_2");
                }
                else
                {
                    prompterController.AddString("훈이", "빵을 찾아봤지만 보이지 않는다.");
                    prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                    prompterController.AddOption("빵을 더 찾아본다.", "store", "yang_find_2");
                }

                break;

            case "yang_find_2":
                prompterController.Reset();
                prompterController.imageMode = true;
                if (Random.Range(0, 10) == 0)
                {
                    prompterController.AddString("훈이", "구석진 곳까지 잘 찾아봐야겠다.");
                    prompterController.AddString("훈이", "구석진 곳에서 포켓볼빵 1개를 발견했다!");
                    gameManager.AddBBangType(1, "양아치");
                    gameManager.storeOutAction = "좋았어! 운이 정말 좋구나!";
                    prompterController.AddOption("편의점을 나간다.", "store", "yang_exit2");
                }
                else
                {
                    prompterController.AddString("훈이", "구석진 곳까지 잘 찾아봐야겠다.");
                    prompterController.AddString("훈이", "열심히 찾아봤지만 보이지 않는다.");
                    prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                    gameManager.storeOutAction = "부지런한 사람들이 많나보다..";
                }

                break;

            case "yang_game":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "[정자 키우기]라고 요즘 핫한 게임 있어요.");
                prompterController.AddOption("들어보지 못 한 게임이네요.", "store", "yang_game_no");
                prompterController.AddOption("정자를 키우는 게임이에요?", "store", "yang_game_jungja");
                prompterController.AddOption("요즘 완전 핫한 게임이잖아요!", "store", "yang_game_hot");
                break;

            case "yang_game_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "무슨 그런 게임이 다 있어요?");
                prompterController.AddString("양아치", "알지도 못하면서 왜 물어봐요?");
                prompterController.AddString("양아치", "귀찮게 하지 말고 나가세요.");
                prompterController.AddString("양아치", "[정자 키우기] 해야돼요.");
                prompterController.AddOption("밖으로 나간다.", "store", "yang_exit");
                break;

            case "yang_game_jungja":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "헐 맞아요! 어떻게 아세요?");
                prompterController.AddString("양아치", "4억마리 정자와 펼치는 레이싱 게임이죠.");
                prompterController.AddString("양아치", "1등으로 난자를 만나야 승리하는 게임이에요.");
                prompterController.AddOption("재미있어 보이네요.", "store", "yang_game_fun");
                prompterController.AddOption("이상한 게임 아니에요?", "store", "yang_game_weird");
                break;

            case "yang_game_fun":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "맞아요 굉장히 재미있는 게임이죠.");
                prompterController.AddOption("이상한 게임 아니에요?", "store", "yang_game_fun2");
                break;

            case "yang_game_fun2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "이상해 보일지 몰라도 교육용 컨텐츠랍니다.");
                prompterController.AddOption("꼭 한번 플레이해볼게요.", "store", "yang_game_play");
                break;

            case "yang_game_weird":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "이상한 게임이라뇨!");
                prompterController.AddString("양아치", "성에 대한 다양한 상식을 담은 잡학사전도 있다구요.");
                prompterController.AddString("양아치", "이래보여도 교육용 컨텐츠라구요!");
                prompterController.AddOption("재미있을 것 같아 보이네요.", "store", "yang_game_weird2");
                break;

            case "yang_game_weird2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "맞아요 굉장히 재미있는 게임이죠.");
                prompterController.AddOption("꼭 한번 플레이해볼게요.", "store", "yang_game_play");
                break;

            case "yang_game_play":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "말이 생각보다 잘 통하시는 분이네요.");
                prompterController.AddString("양아치", "우리 친구 할까요?");
                prompterController.AddOption("좋아요.", "store", "yang_friend_yes");
                prompterController.AddOption("아니요.", "store", "yang_friend_no");
                break;

            case "yang_friend_yes":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "그래 반가워.");
                prompterController.AddOption("혹시 포켓볼빵은 없을까?", "store", "yang_friend_yes2");
                prompterController.AddOption("반말은 좀 그런것 같은데.", "store", "yang_friend_no");
                break;

            case "yang_friend_yes2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "아쉽지만 오늘은 포켓볼빵이 없어.");
                prompterController.AddString("양아치", "다음에 들어오면 내가 몇 개 챙겨둘게.");
                prompterController.AddOption("고마워.", "store", "yang_friend_thanks");
                prompterController.AddOption("필요 없어.", "store", "yang_friend_no");
                break;

            case "yang_friend_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "그래요.");
                prompterController.AddString("양아치", "볼 일 다 봤으면 나가주세요.");
                prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_friend_thanks":
                prompterController.Reset();
                prompterController.imageMode = true;
                PlayerPrefs.SetInt("yang_friend", 1);
                PlayerPrefs.Save();
                prompterController.AddString("훈이", "정말 고마워!");
                prompterController.AddString("양아치", "그래. 더 볼일 있어?");
                prompterController.AddOption("다음에 다시 올게.", "store", "yang_friend_exit");
                prompterController.AddOption("아니 없어.", "store", "yang_friend_exit");
                break;

            case "yang_friend_exit":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "그래 조심히 가고 또 와.");
                gameManager.storeOutAction = "좋은 친구가 생겼다.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yang_friend_ask":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "누구신데요. 저 아세요?");
                prompterController.AddOption("알아요.", "store", "yang_friend_ask_yes");
                prompterController.AddOption("잘 몰라요.", "store", "yang_friend_ask_no");
                break;

            case "yang_friend_ask_yes":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "누구신데요? 난 모르겠는데.");
                prompterController.AddString("양아치", "공통의 관심사도 없는데 어떻게 친구를 해요.");
                prompterController.AddString("양아치", "귀찮게 하지 말고 나가세요.");

                prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_friend_ask_no":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "서로 아는게 없는데 어떻게 친구를 해요.");
                prompterController.AddString("양아치", "귀찮게 하지 말고 나가세요.");

                prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yang_game_hot":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("훈이", "요즘 완전 인기 많은 게임이잖아요?");
                prompterController.AddString("양아치", "하는 사람 아무도 없는 망겜이에요.");
                prompterController.AddString("양아치", "아는 척 하지 마세요.");
                prompterController.AddString("훈이", "...");

                prompterController.AddOption("편의점을 나간다.", "store", "yang_exit");
                break;

            case "yangf_ball":
                prompterController.Reset();
                prompterController.imageMode = true;
                if (Random.Range(0, 2) == 0)
                {
                    prompterController.AddString("양아치", "아 응. 빵 하나 내가 빼뒀어.");
                    gameManager.AddBBangType(1, "양아치");
                    prompterController.AddString("훈이", "고맙다 친구야!");
                    prompterController.AddString("양아치", "뭐 이런걸 갖고.");
                    gameManager.storeOutAction = "좋은 친구를 두니 좋다.";
                    prompterController.AddOption("편의점을 나간다.", "store", "yangf_out");
                }
                else
                {
                    prompterController.AddString("양아치", "아직 안 들어왔어.");
                    prompterController.AddString("양아치", "들어오면 몇개 빼둘게.");
                    prompterController.AddString("훈이", "그래 고맙다!");
                    gameManager.storeOutAction = "다음에 또 들러야 겠다.";
                    prompterController.AddOption("편의점을 나간다.", "store", "yangf_out");
                }

                break;

            case "yangf_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "또 와.");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "yangf_game":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "지금 열심히 정자를 키우고있어.");
                prompterController.AddString("양아치", "너도 해봐. 앱스토어에 있어.");
                prompterController.AddOption("게임 이름이 뭐였지?", "store", "yangf_game2");
                prompterController.AddOption("나도 해봤어 그 게임", "store", "yangf_game3");
                break;

            case "yangf_game2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "게임 이름은 [정자키우기]야.");
                prompterController.AddString("훈이", "응. 꼭 해볼게.");
                prompterController.AddOption("혹시 빵 들어왔어?", "store", "yangf_ball");
                break;

            case "yangf_game3":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("양아치", "[정자키우기] 너무 재밌지 않아?");
                prompterController.AddString("양아치", "엔딩이 굉장히 감동적이라던데.");
                prompterController.AddString("훈이", "응. 중독성이 있는 게임이더라.");
                prompterController.AddString("양아치", "맞아. 이런 게임 처음이야.");
                prompterController.AddOption("그건 그렇고.. 혹시 빵 들어왔어?", "store", "yangf_ball");
                break;


            // 비실이
            case "bi_ball":
                if (Random.Range(0, 2) == 0)
                {
                    prompterController.Reset();
                    prompterController.imageMode = true;
                    prompterController.AddString("비실이", "오늘 들어온 빵은 다 나갔어요!");
                    prompterController.AddString("훈이", "그렇군요. 아쉽네요.");
                    prompterController.AddString("훈이", "그러면 다음에 또 올게요.");
                    prompterController.AddString("비실이", "다음에 또 오세요우~");
                    gameManager.storeOutAction = "말투가 참 이상하다. 친해지면 재밌을것 같다.";
                    prompterController.AddNextAction("main", "store_out");
                }
                else
                {
                    prompterController.Reset();
                    prompterController.imageMode = true;
                    prompterController.AddString("비실이", "저랑 가위바위보 해서 이기시면 빵 하나를 드릴게요오~");
                    prompterController.AddString("훈이", "아 진짜요?");
                    prompterController.AddString("비실이", "그럼 진짜죠우~");
                    prompterController.AddString("비실이", "준비 됐나요우~");
                    prompterController.AddOption("가위를 낸다.", "store", "bi_game");
                    prompterController.AddOption("바위를 낸다.", "store", "bi_game");
                    prompterController.AddOption("보를 낸다.", "store", "bi_game");
                }

                break;
            case "bi_game":
                prompterController.Reset();
                prompterController.imageMode = true;
                var rnd = Random.Range(0, 3);
                switch (rnd)
                {
                    case 0:
                        prompterController.AddString("비실이", "앗 비겼다아!!");
                        prompterController.AddString("훈이", "한판 더!!");
                        prompterController.AddString("비실이", "준비 됐나요우~");
                        prompterController.AddOption("가위를 낸다.", "store", "bi_game");
                        prompterController.AddOption("바위를 낸다.", "store", "bi_game");
                        prompterController.AddOption("보를 낸다.", "store", "bi_game");
                        break;
                    case 1:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", "이겼다!!!!");
                        prompterController.AddString("비실이", "좋아요우 졌으니까 약속대로 포켓볼빵 하나 드릴게요우.");
                        gameManager.AddBBangType(1, "비실이");
                        prompterController.AddString("훈이", "오예 신난다!");
                        gameManager.storeOutAction = "신난다!! 저 알바와 친해지면 재밌을것 같다.";
                        prompterController.AddNextAction("store", "bi_out");
                        break;
                    default:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", "졌다......");
                        prompterController.AddString("비실이", "좋은 승부였어요우.");
                        prompterController.AddString("훈이", "분하다.. 다음에 한판 더 해요.");
                        gameManager.storeOutAction = "정말 이상한 알바다... 그치만 친해지면 재밌을 것 같다.";
                        prompterController.AddNextAction("store", "bi_out");
                        break;
                }

                break;

            case "bi_out":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "다음에 또 오세요우!");
                prompterController.AddNextAction("main", "store_out");
                break;

            case "bi_fried":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "...!!");
                prompterController.AddString("비실이", "뭐라고 했나요우 방금?");
                prompterController.AddString("훈이", "친구 하자구요.");
                prompterController.AddString("비실이", "...!!!!");
                prompterController.AddString("비실이", "정말인가요오오!!!!");
                prompterController.AddOption("네!", "store", "bi_yes");
                prompterController.AddOption("농담이에요.", "store", "bi_out");
                gameManager.storeOutAction = "참 이상한 알바다....";
                break;

            case "bi_yes":
                prompterController.Reset();
                prompterController.imageMode = true;
                gameManager.storeOutAction = "재미있는 친구가 생겼다!";
                prompterController.AddString("비실이", "너무 죠와.");
                prompterController.AddString("비실이", "기념으로 빵 하나 줄겡.");
                gameManager.AddBBangType(1, "비실이");
                prompterController.AddString("훈이", "고마워 친구야!");
                prompterController.AddString("비실이", "언제든 또 놀러왕.");
                prompterController.AddNextAction("main", "store_out");
                PlayerPrefs.SetInt("bi_friend", 1);
                PlayerPrefs.Save();
                break;

            case "bi_ball_2":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "자. 게임을 시작해볼까?");
                prompterController.AddString("비실이", "지금 포켓볼빵 1개를 갖고 가거나");
                prompterController.AddString("비실이", "가위바위보에서 이기면 포켓볼빵 2개를 줄게.");
                prompterController.AddString("비실이", "대신 지면 아무것도 못 가져가는거얌!");
                prompterController.AddString("비실이", "어떻게 할래?");
                prompterController.AddOption("좋아. 게임을 시작하자.", "store", "bif_game");
                prompterController.AddOption("하나만 받고 끝낼래.", "store", "bif_nogame");
                break;

            case "bif_game":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "좋아. 준비되면 시작해!");
                prompterController.AddOption("가위를 낸다.", "store", "bif_game_2");
                prompterController.AddOption("바위를 낸다.", "store", "bif_game_2");
                prompterController.AddOption("보를 낸다.", "store", "bif_game_2");
                break;

            case "bif_nogame":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "칫 시시하긴.");
                prompterController.AddString("비실이", "언제든 또 오라구!");
                gameManager.AddBBangType(1, "비실이");
                gameManager.storeOutAction = "도박은 역시 좋지 않아.";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "bif_game_2":
                prompterController.Reset();
                prompterController.imageMode = true;
                var rnd2 = Random.Range(0, 3);
                switch (rnd2)
                {
                    case 0:
                        prompterController.AddString("비실이", "앗 비겼다!!");
                        prompterController.AddString("훈이", "한판 더!!");
                        prompterController.AddOption("가위를 낸다.", "store", "bif_game_2");
                        prompterController.AddOption("바위를 낸다.", "store", "bif_game_2");
                        prompterController.AddOption("보를 낸다.", "store", "bif_game_2");
                        break;
                    case 1:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", "이겼다!!!!");
                        prompterController.AddString("비실이", "좋아 그러면 두번째 게임을 시작할까?");
                        prompterController.AddString("비실이", "가위바위보를 한번 더 이기면 빵 4개를 줄게.");
                        prompterController.AddString("비실이", "대신 지게되면 다 꽝이야.");
                        prompterController.AddString("훈이", "흠.. 어떻게 하는 게 좋으려나..");

                        prompterController.AddOption("두 번째 게임을 시작한다.", "store", "bif_game_3_start");
                        prompterController.AddOption("빵 2개를 받고 끝낸다.", "store", "bif_finish_game");

                        gameManager.score = 2;
                        break;
                    default:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", "졌다......");
                        prompterController.AddString("비실이", "좋은 승부였어.");
                        prompterController.AddString("훈이", "분하다!! 다음에 한판 더 해!");
                        prompterController.AddString("비실이", "좋아. 언제든 또 오라구!");
                        gameManager.storeOutAction = "졌지만 좋은 승부였다..";
                        prompterController.AddNextAction("main", "store_out");
                        break;
                }

                break;

            case "bif_endgame":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "언제든 또 오라구!");
                gameManager.storeOutAction = "도박은 역시 좋지 않아.";
                break;

            case "bif_finish_game":
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "좋은 승부였어!");
                gameManager.AddBBangType(gameManager.score, "비실이");
                prompterController.AddString("훈이", "오예 신난다!");
                prompterController.AddString("비실이", "언제든 또 오라구!");
                gameManager.storeOutAction = "빵을 많이 얻었다! 역시 친구가 좋다!";
                prompterController.AddNextAction("main", "store_out");
                break;

            case "bif_game_3_start":
                gameManager.score += 2;
                prompterController.Reset();
                prompterController.imageMode = true;
                prompterController.AddString("비실이", "우왕 긴장 되는걸!!");
                prompterController.AddOption("가위를 낸다.", "store", "bif_game_3");
                prompterController.AddOption("바위를 낸다.", "store", "bif_game_3");
                prompterController.AddOption("보를 낸다.", "store", "bif_game_3");
                break;

            case "bif_game_3":
                prompterController.Reset();
                prompterController.imageMode = true;
                var rnd3 = Random.Range(0, 3);
                switch (rnd3)
                {
                    case 0:
                        prompterController.AddString("비실이", "앗 비겼다!!");
                        prompterController.AddString("훈이", "한판 더!!");
                        prompterController.AddOption("가위를 낸다.", "store", "bif_game_3");
                        prompterController.AddOption("바위를 낸다.", "store", "bif_game_3");
                        prompterController.AddOption("보를 낸다.", "store", "bif_game_3");
                        break;
                    case 1:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", ".....!!!");
                        prompterController.AddString("훈이", "이겼다!!!!!!!");
                        prompterController.AddString("비실이", "좋아 그러면 다음 게임을 시작할까?");
                        prompterController.AddString("비실이", "가위바위보를 한번 더 이기면 빵 " + (gameManager.score + 2) + "개를 줄게.");
                        prompterController.AddString("비실이", "지면 어떻게 되는 지 알지?");
                        prompterController.AddString("훈이", "우와.. 어떻게 하는 게 좋으려나..");

                        prompterController.AddOption("빵 " + gameManager.score + "개를 받고 끝낸다.", "store", "bif_finish_game");
                        prompterController.AddOption("다음 게임을 시작한다.", "store", "bif_game_3_start");
                        break;

                    default:
                        prompterController.AddString("훈이", "...!!");
                        prompterController.AddString("훈이", ".....!!!");
                        prompterController.AddString("훈이", "졌다.........");
                        prompterController.AddString("비실이", "좋은 승부였어.");
                        prompterController.AddString("훈이", "분하다!! 다음에 한판 더 해!");
                        prompterController.AddString("비실이", "좋아. 언제든 또 오라구!");
                        gameManager.storeOutAction = "졌지만 좋은 승부였다..";
                        prompterController.AddNextAction("main", "store_out");
                        break;
                }

                break;
        }
    }
}