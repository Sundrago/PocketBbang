using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangunChaCtrl : MonoBehaviour
{
    [SerializeField] Sprite[] chaImgs = new Sprite[5];

    [SerializeField] GameObject dangunChaImg_ui, dangunMsg_ui, dangunMsgText_ui;
    [SerializeField] MsgCountCtrl MsgCtrl;
    [SerializeField] Main_control main;
    [SerializeField] PrompterControl pmtCtrl;
    [SerializeField] PhoneMsgCtrl SimpleMsg;

    [SerializeField] BalloonControl balloon;
    [SerializeField] Heart_Control heart;
    [SerializeField] Bbang_showroom show;
    [SerializeField] DangunControl dangunControl;
    [SerializeField] Collection_Panel_Control collection;
    [SerializeField] CollectionControl myCollection;

    int chaIdx, maxCount;
    int returnCount;
    int bbangBoyIdx;

    public List<int> cardSelectIdx = new List<int>();
    public bool endDangun = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PlayDangunCha(int idx)
    {
        print(idx);
        //Start Dangun
        gameObject.SetActive(true);
        main.dangunIng = true;
        main.currentLocation = "dangun";
        main.lower_bar.GetComponent<Animator>().SetTrigger("hide");
        main.phone_ui.GetComponent<Animator>().SetTrigger("hide");
        main.frontCha.GetComponent<Animator>().SetTrigger("idle");
        chaIdx = idx;
        //if (idx == 4) idx = 3;
        dangunChaImg_ui.GetComponent<Image>().sprite = chaImgs[idx];
        gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    public void DangunChaIn()
    {
        print("DangunChaIn IDX : " + chaIdx);
        endDangun = false;
        switch (chaIdx)
        {
            case 1:
                maxCount = PlayerPrefs.GetInt("bbang_mat");
                if (maxCount > 10) maxCount = 10;
                MsgCtrl.Start();
                MsgCtrl.SetMsg("맛동산을 몇개 판매할까요?\n최대 10개까지 판매할 수 있습니다.\n(내 맛동산 : " + PlayerPrefs.GetInt("bbang_mat") + "개)", maxCount, "DangunChaMsgCallBack");
                break;
            case 2:
                maxCount = Mathf.FloorToInt(PlayerPrefs.GetInt("money") / 20000f);
                if (maxCount > 3) maxCount = 3;
                MsgCtrl.Start();
                MsgCtrl.SetMsg("핫소스 빵을 몇개 구매할까요?\n3개까지 구매할 수 있습니다.\n (빵 1개당 2만냥)\n(내가 가진 돈 : " + PlayerPrefs.GetInt("money")+"냥)", maxCount, "DangunChaMsgCallBack");
                break;
            case 3:
                SimpleMsg.Start();
                SimpleMsg.SetMsg("스티커를 선택해주세요!\n같은 A급 스티커 4장을 판매합니다.\n최대 3 종류를 선택할 수 있습니다.\n(스티커당 바캉스 1병)", 1, "OpenCardSelection");
                break;
            case 4:
                SimpleMsg.Start();
                SimpleMsg.SetMsg("스티커를 선택해주세요!\n같은 B급 스티커 4장을 판매합니다.\n최대 3 종류를 선택할 수 있습니다.\n(스티커당 야쿠르트 1병)", 1, "OpenCardSelection");
                break;
            case 5:
                SimpleMsg.Start();
                endDangun = false;
                SimpleMsg.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "] 스티커를" +
                    "\n" + PlayerPrefs.GetInt("sellPrice" + 0) + "냥을 내고 구매합니다.\n" +
                    "\n(나의 보유 슈량 : " + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].count + "개)", 2, "DangunChaMsgCallBack");
                break;
            case 6:
                SimpleMsg.Start();
                endDangun = false;
                SimpleMsg.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + "] 스티커를" +
                    "\n" + PlayerPrefs.GetInt("sellPrice" + 1) + "냥에 판매합니다.\n" +
                    "\n(나의 보유 슈량 : " + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)", 2, "DangunChaMsgCallBack");
                break;
        }
    }

    public void OpenCardSelection()
    {
        collection.ShowSelectionPanel();
    }

    public void CardSelectFinishBtn()
    {
        cardSelectIdx = collection.ClosePanelAndReturnSelectionIdx();
        if(cardSelectIdx.Count == 0)
        {
            SimpleMsg.SetMsg("아무 스티커도 선택하지 않으셨습니다.\n다시 선택할까요?\n(아니요를 누르면 거래가 취소됩니다.)\n", 2, "SelectAgain");
        } else
        {
            endDangun = false;
            DangunChaMsgCallBack(0);
        }
    }

    public void SelectAgain(bool again)
    {
        if(again)
        {
            if(chaIdx == 3) collection.UpdateAndReturnSelectionAvailability(5, 'A');
            else collection.UpdateAndReturnSelectionAvailability(5, 'B');
            collection.ShowSelectionPanel();
        } else
        {
            endDangun = true;
            DangunChaMsgCallBack(-1);
        }
    }

    public void DangunChaMsgCallBack(int myReturnCount)
    {
        returnCount = myReturnCount;
        gameObject.GetComponent<Animator>().SetTrigger("hide");

        switch (chaIdx)
        {
            case 1:
                //RemoveMatdongSagn : returnCount
                PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") - returnCount);
                show.UpdateBbangShow();
                //Add Money and Update UI
                heart.UpdateMoney(returnCount * 2000);
                break;
            case 2:
                main.AddBBangType(returnCount,"단군_빵구매", "hot");
                //Add Money and Update UI
                heart.UpdateMoney(-returnCount * 20000);
                break;
            case 3:
                if (endDangun)
                {
                    //
                }
                else
                {
                    for(int i = 0; i<cardSelectIdx.Count; i++)
                    {
                        PlayerPrefs.SetInt("card_" + cardSelectIdx[i], PlayerPrefs.GetInt("card_" + cardSelectIdx[i]) - 4);
                        if (PlayerPrefs.GetInt("card_" + cardSelectIdx[i]) < 0) PlayerPrefs.SetInt("card_" + cardSelectIdx[i], 1);
                    }
                    PlayerPrefs.SetInt("bbang_vacance", PlayerPrefs.GetInt("bbang_vacance") + cardSelectIdx.Count);
                    PlayerPrefs.Save();
                    main.showroom.UpdateBbangShow();
                }
                break;
            case 4:
                if (endDangun)
                {
                    //
                }
                else
                {
                    for (int i = 0; i < cardSelectIdx.Count; i++)
                    {
                        PlayerPrefs.SetInt("card_" + cardSelectIdx[i], PlayerPrefs.GetInt("card_" + cardSelectIdx[i]) - 4);
                        if (PlayerPrefs.GetInt("card_" + cardSelectIdx[i]) < 0) PlayerPrefs.SetInt("card_" + cardSelectIdx[i], 1);
                    }
                    PlayerPrefs.SetInt("bbang_yogurt", PlayerPrefs.GetInt("bbang_yogurt") + cardSelectIdx.Count);
                    PlayerPrefs.Save();
                    main.showroom.UpdateBbangShow();
                }
                break;
            case 5:
                /*
                 * "[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "] 스티커를" +
                    "\n" + PlayerPrefs.GetInt("sellPrice" + 0) + "냥에 구매합니다.\n" +
                    PlayerPrefs.GetInt("card_" + i)
                 */
                if (endDangun)
                {
                    //
                }
                else
                {
                    myCollection.AddData(PlayerPrefs.GetInt("sellIdx" + 0));
                    heart.UpdateMoney(-PlayerPrefs.GetInt("sellPrice" + 0));
                }
                PlayerPrefs.SetInt("sellUpdated" + 0, 0);
                PlayerPrefs.Save();
                break;
            case 6:
                if (endDangun)
                {
                    //
                }
                else
                {
                    PlayerPrefs.SetInt("card_" + PlayerPrefs.GetInt("sellIdx" + 1), PlayerPrefs.GetInt("card_" + PlayerPrefs.GetInt("sellIdx" + 1)) - 1);
                    myCollection.LoadData();
                    heart.UpdateMoney(PlayerPrefs.GetInt("sellPrice" + 1));
                }
                PlayerPrefs.SetInt("sellUpdated" + 1, 0);
                PlayerPrefs.Save();
                break;
        }

        dangunControl.SetDateData(chaIdx);
    }

    public void DangunMsg01()
    {
        switch (chaIdx)
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
        switch (chaIdx)
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
        switch (chaIdx)
        {
            case 1:
                Msg("고맙구려~");
                break;
            case 2:
                Msg("감사합니다!!");
                break;
            case 3:
                if(endDangun) Msg("아이고 아쉬워라!");
                else Msg("감사해요!");
                break;
            case 4:
                if (endDangun) Msg("에고고 아쉽다!");
                else Msg("고마워요!");
                break;
            case 5:
                if (endDangun) Msg("괜찮아요!");
                else Msg("고맙습니다!");
                break;
            case 6:
                if (endDangun) Msg("에고고 아쉽다!");
                else Msg("감사합니다!!");
                break;
        }
    }

    public void DangunMsg04()
    {
        switch (chaIdx)
        {
            case 1:
                Msg("제일 좋아하는 과자야!");
                break;
            case 2:
                Msg("좋은 하루 되세요~");
                break;
            case 3:
                if (endDangun) Msg("다음에 또 봐요!");
                else Msg("애들이 너무 좋아해요!");
                break;
            case 4:
                if (endDangun) Msg("다음에 또 봐요!");
                else Msg("맛있게 드세요!");
                break;
            case 5:
                if (endDangun) Msg("조금 더 고민해보세요!");
                else Msg("좋은 거래였어요!");
                break;
            case 6:
                if (endDangun) Msg("다음에 또 봐요!");
                else Msg("거의 다 모아간다!!");
                break;
        }
    }

    public void Msg(string output)
    {
        dangunMsgText_ui.GetComponent<Text>().text = output;
        dangunMsg_ui.SetActive(true);
        dangunMsg_ui.GetComponent<Animator>().SetTrigger("show");
    }

    //Finish Dangun
    public void DangunEnd()
    {
        gameObject.SetActive(false);

        pmtCtrl.gameObject.SetActive(true);
        pmtCtrl.Start();
        pmtCtrl.Reset();
        pmtCtrl.imageMode = true;
        switch (chaIdx)
        {
            case 1:
                pmtCtrl.AddString("용돈", "맛동산 " + returnCount + "개를 팔고\n" + (returnCount * 2000) + "냥을 받았다!");
                pmtCtrl.AddString("훈이", "좋은 거래였다!");
                pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                break;
            case 2:
                pmtCtrl.AddString("핫소스빵", returnCount * 20000 + "냥을 내고\n핫소스빵 " + returnCount + "개를 겟했다!");
                pmtCtrl.AddString("훈이", "가성비가 좋은지는 모르겠다.");
                pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                break;
            case 3:
                if(endDangun)
                {
                    pmtCtrl.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                } else
                {
                    pmtCtrl.AddString("훈이", "같은 카드 종류 " + cardSelectIdx.Count + "개를\n팔고 바캉스 " + cardSelectIdx.Count + "병을 얻었다!");
                    pmtCtrl.AddString("바캉스", "힘이 솟아오를 것 같다!");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                break;
            case 4:
                if (endDangun)
                {
                    pmtCtrl.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    pmtCtrl.AddString("훈이", "같은 카드 종류 " + cardSelectIdx.Count + "개를\n팔고 야쿠르트 " + cardSelectIdx.Count + "병을 얻었다!");
                    pmtCtrl.AddString("야쿠르트", "마시면 조금 힘이날 것 같다!");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                break;
            case 5:
                if (endDangun)
                {
                    pmtCtrl.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    /*SimpleMsg.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "] 스티커를" +
                    "\n" + PlayerPrefs.GetInt("sellPrice" + 0) + "냥에 구매합니다.\n" +
                    "\n(나의 보유 슈량 : " + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].count + "개)", 2, "DangunChaMsgCallBack");
                    */

                    pmtCtrl.AddString("훈이", PlayerPrefs.GetInt("sellPrice" + 0) + "냥을 내고 \n["
                        + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 0)].name + "] 스티커를 겟했다!");
                    pmtCtrl.AddNextAction("OpenCard", PlayerPrefs.GetInt("sellIdx" + 0) + "");
                }
                break;
            case 6:
                if (endDangun)
                {
                    pmtCtrl.AddString("훈이", "아쉽다. 다음에 또 거래하는 게 좋겠어.");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                else
                {
                    /*
                     * SimpleMsg.SetMsg("[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + "] 스티커를" +
                    "\n" + PlayerPrefs.GetInt("sellPrice" + 1) + "냥에 판매합니다.\n" +
                    "\n(나의 보유 슈량 : " + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)", 2, "DangunChaMsgCallBack");
                    */
                    pmtCtrl.AddString("훈이", "[" + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].name + "] 스티커를 팔았다!\n(남은 수량 : " + myCollection.myCard[PlayerPrefs.GetInt("sellIdx" + 1)].count + "개)");
                    pmtCtrl.AddString("용돈", "보상으로 " + PlayerPrefs.GetInt("sellPrice" + 1) + "냥을 받았다!");
                    pmtCtrl.AddNextAction("dangunCha", "BackToHome");
                }
                break;
        }
        dangunControl.UpdateState();
        pmtCtrl.gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    public void BackToHome()
    {
        pmtCtrl.Reset();
        pmtCtrl.GetComponent<Animator>().SetTrigger("hide");
        if(!endDangun) balloon.ShowMsg("좋은 거래였다..!");
        main.dangunIng = false;
        main.currentLocation = "home";
        main.lower_bar.GetComponent<Animator>().SetTrigger("show");
    }
}
