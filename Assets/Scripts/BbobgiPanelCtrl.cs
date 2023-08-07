using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BbobgiPanelCtrl : MonoBehaviour
{
    [SerializeField] public BbobgiCtrl bbobgi;
    [SerializeField] BalloonControl balloon;
    [SerializeField] Ad_Control ads;
    [SerializeField] Main_control main;

    [SerializeField] Heart_Control heart;
    [SerializeField] AudioControl myAydio;

    [SerializeField] GameObject frontCha;
    [SerializeField] GameObject continuePanel, playCount;

    [SerializeField] Button insertMoneyBtn, watchAdBtn, exitBtn;
    [SerializeField] Text contineueText;

    public bool reload = false;
    int totalCount, currentCount;


    public void OpenGame()
    {
        //LoadBbobgi();
        gameObject.SetActive(true);
        frontCha.SetActive(false);
        main.currentLocation = "bbobgi";
        main.HideBtn();
        main.prompter.GetComponent<Animator>().SetTrigger("hide");

        contineueText.text = "돈을 넣어주세요";

        if (reload)
        {
            bbobgi.StartGame();
            reload = false;
            bbobgi.playing = false;
            bbobgi.SetBtnActive(false);
        }
        UpdateContinue();
        UpdateBtnSetting();
    }

    public void CloseGame()
    {
        gameObject.SetActive(false);
        frontCha.SetActive(true);
        main.currentLocation = "store";
        myAydio.PlayMusic(1);

        main.StoreEvent("bbobgiStore");
        main.prompter.SetActive(true);
        main.prompter.GetComponent<Animator>().SetTrigger("show");
    }

    public void LoadBbobgi()
    {
        bbobgi.StartGame();
        reload = false;
        bbobgi.playing = false;
        bbobgi.SetBtnActive(false);
    }

    public void InsertMoney()
    {
        if (PlayerPrefs.GetInt("money") < 1000)
        {
            balloon.ShowMsg("돈이 부족하다..");
            return;
        }
        if(bbobgi.playing == false)
        {
            heart.UpdateMoney(-1000);
            totalCount = 1;
            currentCount = 1;
            bbobgi.playing = true;
            bbobgi.OepnClaw();
            bbobgi.SetBtnActive(true);
        }
        UpdateBtnSetting();
        UpdateContinue();
    }

    public void GameFinished()
    {
        if(currentCount >= totalCount)
        {
            bbobgi.playing = false;
        } else
        {
            currentCount += 1;
            bbobgi.playing = true;
            bbobgi.OepnClaw();
            bbobgi.SetBtnActive(true);
        }
        UpdateBtnSetting();
        UpdateContinue();
    }

    public void UpdateContinue()
    {
        if(bbobgi.playing)
        {
            continuePanel.SetActive(false);
            playCount.GetComponent<Text>().text = "" + currentCount + "/" + totalCount;
        } else
        {
            continuePanel.SetActive(true);
            playCount.GetComponent<Text>().text = "";
        }        
    }

    public void UpdateBtnSetting()
    {
        print(bbobgi.bbangCount);
        if(bbobgi.bbangCount == 0)
        {
            contineueText.text = "포켓볼빵을 모두 뽑았습니다.";
            exitBtn.image.color = Color.yellow;
            insertMoneyBtn.image.color = Color.white;
            watchAdBtn.image.color = Color.white;
            return;
        } else
        {
            exitBtn.image.color = Color.white;
        }

        if(bbobgi.playing == true)
        {
            insertMoneyBtn.image.color = Color.white;
            watchAdBtn.image.color = Color.white;
            insertMoneyBtn.interactable = false;
            watchAdBtn.interactable = false;
        } else
        {
            if(PlayerPrefs.GetInt("money") < 1000)
            {
                insertMoneyBtn.image.color = Color.white;
                watchAdBtn.image.color = Color.yellow;
            } else
            {
                insertMoneyBtn.image.color = Color.yellow;
                watchAdBtn.image.color = Color.white;
            }

            insertMoneyBtn.interactable = true;
            watchAdBtn.interactable = true;
        }
        AudioCtrl();
    }

    public void AudioCtrl()
    {
        if(bbobgi.playing == true)
        {
            myAydio.PlayMusic(6);
        } else
        {
            myAydio.PlayMusic(7);
        }
    }

    public void WatchAds()
    {
        ads.BbobgiAds();
        myAydio.StopMusic();
    }

    public void WathcedAds()
    {
        myAydio.PlayMusic(6);
        totalCount = 3;
        currentCount = 1;
        bbobgi.playing = true;
        bbobgi.OepnClaw();
        bbobgi.SetBtnActive(true);
        UpdateBtnSetting();
        UpdateContinue();
    }
    
}
