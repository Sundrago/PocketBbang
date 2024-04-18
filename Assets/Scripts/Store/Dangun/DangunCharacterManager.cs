using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DangunCharacterManager : MonoBehaviour
{
    [Header("Managers and Controllers")] 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DanunMessageCountManager danunMessageManager;
    [SerializeField] private PrompterController prompterController;
    [SerializeField] private PhoneMessageController phoneMessageController;
    [SerializeField] private BalloonUIManager balloonUIManager;
    [SerializeField] private BbangShowroomManager bbangShowroomManager;
    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private DangunManager dangunManager;
    [SerializeField] private CollectionPanelManager collectionPanelManager;
    [SerializeField] private CollectionManager myCollection;

    [Header("UI Elements")] 
    [SerializeField] private Image dangunChaImage;
    [SerializeField] private Animator dangunMsgAnimator;
    [SerializeField] private Text debugText;
    [SerializeField] private Sprite[] characterSprites = new Sprite[5];

    private int bbangBoyIdx;
    private int playerCharacterIndex, maxCount;
    private int returnCount;

    public List<int> CardSelectIdx { get; set; } = new();
    public bool EndDangun { get; set; }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PlayDangunCharacterAnimation(int idx)
    {
        gameObject.SetActive(true);
        UpdateUIElements();
        playerCharacterIndex = idx;
        dangunChaImage.sprite = characterSprites[idx];
        gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    private void UpdateUIElements()
    {
        gameManager.dangunIng = true;
        gameManager.currentLocation = "dangun";
        gameManager.lowerUIPanel.GetComponent<Animator>().SetTrigger("hide");
        gameManager.phone_ui.GetComponent<Animator>().SetTrigger("hide");
        gameManager.mainPlayerAnimator.GetComponent<Animator>().SetTrigger("idle");
    }

    public void DangunChaIn()
    {
        print("DangunChaIn IDX : " + playerCharacterIndex);
        EndDangun = false;
        switch (playerCharacterIndex)
        {
            case 1:
                maxCount = PlayerPrefs.GetInt("bbang_mat");
                if (maxCount > 10) maxCount = 10;
                danunMessageManager.Start();
                danunMessageManager.SetMsg(
                    "맛동산을 몇개 판매할까요?\n최대 10개까지 판매할 수 있습니다.\n(내 맛동산 : " + PlayerPrefs.GetInt("bbang_mat") + "개)",
                    maxCount, "DangunChaMsgCallBack");
                break;
            case 2:
                maxCount = Mathf.FloorToInt(PlayerPrefs.GetInt("money") / 20000f);
                if (maxCount > 3) maxCount = 3;
                danunMessageManager.Start();
                danunMessageManager.SetMsg(
                    "핫소스 빵을 몇개 구매할까요?\n3개까지 구매할 수 있습니다.\n (빵 1개당 2만냥)\n(내가 가진 돈 : " + PlayerPrefs.GetInt("money") +
                    "냥)", maxCount, "DangunChaMsgCallBack");
                break;
            case 3:
                phoneMessageController.Init();
                phoneMessageController.SetMsg("스티커를 선택해주세요!\n같은 A급 스티커 4장을 판매합니다.\n최대 3 종류를 선택할 수 있습니다.\n(스티커당 바캉스 1병)",
                    1, "OpenCardSelection");
                break;
            case 4:
                phoneMessageController.Init();
                phoneMessageController.SetMsg(
                    "스티커를 선택해주세요!\n같은 B급 스티커 4장을 판매합니다.\n최대 3 종류를 선택할 수 있습니다.\n(스티커당 야쿠르트 1병)", 1, "OpenCardSelection");
                break;
            case 5:
                phoneMessageController.Init();
                EndDangun = false;
                phoneMessageController.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name +
                                              "] 스티커를" +
                                              "\n" + PlayerPrefs.GetInt("sellPrice" + 0) + "냥을 내고 구매합니다.\n" +
                                              "\n(나의 보유 슈량 : " +
                                              myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].count + "개)", 2,
                    "DangunChaMsgCallBack");
                break;
            case 6:
                phoneMessageController.Init();
                EndDangun = false;
                phoneMessageController.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name +
                                              "] 스티커를" +
                                              "\n" + PlayerPrefs.GetInt("sellPrice" + 1) + "냥에 판매합니다.\n" +
                                              "\n(나의 보유 슈량 : " +
                                              myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)", 2,
                    "DangunChaMsgCallBack");
                break;
        }
    }

    public void OpenCardSelection()
    {
        collectionPanelManager.ShowSelectionPanel();
    }

    public void CardSelectFinishBtn()
    {
        CardSelectIdx = collectionPanelManager.ClosePanelAndReturnSelectionIdx();
        if (CardSelectIdx.Count == 0)
        {
            phoneMessageController.SetMsg("아무 스티커도 선택하지 않으셨습니다.\n다시 선택할까요?\n(아니요를 누르면 거래가 취소됩니다.)\n", 2, "SelectAgain");
        }
        else
        {
            EndDangun = false;
            DangunChaMsgCallBack(0);
        }
    }

    public void SelectAgain(bool again)
    {
        if (again)
        {
            if (playerCharacterIndex == 3) collectionPanelManager.UpdateAndReturnSelectionAvailability(5, 'A');
            else collectionPanelManager.UpdateAndReturnSelectionAvailability(5, 'B');
            collectionPanelManager.ShowSelectionPanel();
        }
        else
        {
            EndDangun = true;
            DangunChaMsgCallBack(-1);
        }
    }

    public void DangunChaMsgCallBack(int myReturnCount)
    {
        returnCount = myReturnCount;
        gameObject.GetComponent<Animator>().SetTrigger("hide");

        switch (playerCharacterIndex)
        {
            case 1:
                bbangShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_mat, -returnCount);
                playerHealthManager.UpdateMoney(returnCount * 2000);
                break;
            case 2:
                gameManager.AddBBangType(returnCount, "단군_빵구매", "hot");
                playerHealthManager.UpdateMoney(-returnCount * 20000);
                break;
            case 3:
                if (EndDangun)
                {
                    //
                }
                else
                {
                    for (var i = 0; i < CardSelectIdx.Count; i++)
                    {
                        PlayerPrefs.SetInt("card_" + CardSelectIdx[i],
                            PlayerPrefs.GetInt("card_" + CardSelectIdx[i]) - 4);
                        if (PlayerPrefs.GetInt("card_" + CardSelectIdx[i]) < 0)
                            PlayerPrefs.SetInt("card_" + CardSelectIdx[i], 1);
                    }

                    gameManager.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_vacance,
                        CardSelectIdx.Count);
                }

                break;
            case 4:
                if (EndDangun)
                {
                    //
                }
                else
                {
                    for (var i = 0; i < CardSelectIdx.Count; i++)
                    {
                        PlayerPrefs.SetInt("card_" + CardSelectIdx[i],
                            PlayerPrefs.GetInt("card_" + CardSelectIdx[i]) - 4);
                        if (PlayerPrefs.GetInt("card_" + CardSelectIdx[i]) < 0)
                            PlayerPrefs.SetInt("card_" + CardSelectIdx[i], 1);
                    }

                    gameManager.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_yogurt,
                        CardSelectIdx.Count);
                }

                break;
            case 5:
                if (EndDangun)
                {
                    //
                }
                else
                {
                    myCollection.AddData(PlayerPrefs.GetInt("sellIdx" + 0));
                    playerHealthManager.UpdateMoney(-PlayerPrefs.GetInt("sellPrice" + 0));
                }

                PlayerPrefs.SetInt("sellUpdated" + 0, 0);
                PlayerPrefs.Save();
                break;
            case 6:
                if (EndDangun)
                {
                    //
                }
                else
                {
                    PlayerPrefs.SetInt("card_" + PlayerPrefs.GetInt("sellIdx" + 1),
                        PlayerPrefs.GetInt("card_" + PlayerPrefs.GetInt("sellIdx" + 1)) - 1);
                    myCollection.LoadData();
                    playerHealthManager.UpdateMoney(PlayerPrefs.GetInt("sellPrice" + 1));
                }

                PlayerPrefs.SetInt("sellUpdated" + 1, 0);
                PlayerPrefs.Save();
                break;
        }

        dangunManager.SetDateData(playerCharacterIndex);
    }

    public void DangunMsg01()
    {
        switch (playerCharacterIndex)
        {
            case 1:
                Msg("안녕하세요~");
                break;
            case 2:
                Msg("혹시.. 단군..?!");
                break;
            case 3:
                Msg("안녕하세요!");
                break;
            case 4:
                Msg("실례합니다~");
                break;
            case 5:
                Msg("안녕하세욧~!");
                break;
            case 6:
                Msg("안녕하세요!!");
                break;
        }
    }

    public void DangunMsg02()
    {
        switch (playerCharacterIndex)
        {
            case 1:
                Msg("혹시.. 단군~?");
                break;
            case 2:
                Msg("한개당 2만냥이에요!");
                break;
            case 3:
                Msg("혹시 단군..?!");
                break;
            case 4:
                Msg("야쿠르트 아줌마에요!");
                break;
            case 5:
                Msg("스티커 팔러 왔어요~!");
                break;
            case 6:
                Msg("스티커 사러 왔어요!!");
                break;
        }
    }

    public void DangunMsg03()
    {
        switch (playerCharacterIndex)
        {
            case 1:
                Msg("고맙구려~");
                break;
            case 2:
                Msg("감사합니다!!");
                break;
            case 3:
                if (EndDangun) Msg("아이고 아쉬워라!");
                else Msg("감사해요!");
                break;
            case 4:
                if (EndDangun) Msg("에고고 아쉽다!");
                else Msg("고마워요!");
                break;
            case 5:
                if (EndDangun) Msg("괜찮아요!");
                else Msg("고맙습니다!");
                break;
            case 6:
                if (EndDangun) Msg("에고고 아쉽다!");
                else Msg("감사합니다!!");
                break;
        }
    }

    public void DangunMsg04()
    {
        switch (playerCharacterIndex)
        {
            case 1:
                Msg("제일 좋아하는 과자야!");
                break;
            case 2:
                Msg("좋은 하루 되세요~");
                break;
            case 3:
                if (EndDangun) Msg("다음에 또 봐요!");
                else Msg("애들이 너무 좋아해요!");
                break;
            case 4:
                if (EndDangun) Msg("다음에 또 봐요!");
                else Msg("맛있게 드세요!");
                break;
            case 5:
                if (EndDangun) Msg("조금 더 고민해보세요!");
                else Msg("좋은 거래였어요!");
                break;
            case 6:
                if (EndDangun) Msg("다음에 또 봐요!");
                else Msg("거의 다 모아간다!!");
                break;
        }
    }

    public void Msg(string output)
    {
        debugText.GetComponent<Text>().text = output;
        dangunMsgAnimator.gameObject.SetActive(true);
        dangunMsgAnimator.SetTrigger("bbangShowroomManager");
    }

    public void DangunEnd()
    {
        gameObject.SetActive(false);

        prompterController.gameObject.SetActive(true);
        prompterController.Start();
        prompterController.Reset();
        prompterController.imageMode = true;
        switch (playerCharacterIndex)
        {
            case 1:
                prompterController.AddString("용돈", "맛동산 " + returnCount + "개를 팔고\n" + returnCount * 2000 + "냥을 받았다!");
                prompterController.AddString("훈이", "좋은 거래였다!");
                prompterController.AddNextAction("dangunCha", "BackToHome");
                break;
            case 2:
                prompterController.AddString("핫소스빵", returnCount * 20000 + "냥을 내고\n핫소스빵 " + returnCount + "개를 겟했다!");
                prompterController.AddString("훈이", "가성비가 좋은지는 모르겠다.");
                prompterController.AddNextAction("dangunCha", "BackToHome");
                break;
            case 3:
                if (EndDangun)
                {
                    prompterController.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    prompterController.AddString("훈이",
                        "같은 카드 종류 " + CardSelectIdx.Count + "개를\n팔고 바캉스 " + CardSelectIdx.Count + "병을 얻었다!");
                    prompterController.AddString("바캉스", "힘이 솟아오를 것 같다!");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }

                break;
            case 4:
                if (EndDangun)
                {
                    prompterController.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    prompterController.AddString("훈이",
                        "같은 카드 종류 " + CardSelectIdx.Count + "개를\n팔고 야쿠르트 " + CardSelectIdx.Count + "병을 얻었다!");
                    prompterController.AddString("야쿠르트", "마시면 조금 힘이날 것 같다!");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }

                break;
            case 5:
                if (EndDangun)
                {
                    prompterController.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    prompterController.AddString("훈이", PlayerPrefs.GetInt("sellPrice" + 0) + "냥을 내고 \n["
                        + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "] 스티커를 겟했다!");
                    prompterController.AddNextAction("OpenCard", PlayerPrefs.GetInt("sellIdx" + 0) + "");
                }

                break;
            case 6:
                if (EndDangun)
                {
                    prompterController.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    prompterController.AddString("훈이",
                        "[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + "] 스티커를 팔았다!\n(남은 수량 : " +
                        myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)");
                    prompterController.AddString("용돈", "보상으로 " + PlayerPrefs.GetInt("sellPrice" + 1) + "냥을 받았다!");
                    prompterController.AddNextAction("dangunCha", "BackToHome");
                }

                break;
        }

        dangunManager.UpdateState();
        prompterController.gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    public void BackToHome()
    {
        prompterController.Reset();
        prompterController.GetComponent<Animator>().SetTrigger("hide");
        if (!EndDangun) balloonUIManager.ShowMsg("좋은 거래였다..!");
        gameManager.dangunIng = false;
        gameManager.currentLocation = "home";
        gameManager.lowerUIPanel.GetComponent<Animator>().SetTrigger("bbangShowroomManager");
    }
}