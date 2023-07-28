using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BbangBoyContol : MonoBehaviour
{
    public GameObject boy_ui;
    public Bbang_showroom bbang_Showroom;

    public GameObject bbangboyBalloon_ui, bbangboyBalloon;
    bool showText = false;
    int showTextIdx = 0;
    float oldTime = 0;
    public bool stopEat = false;

    public PrompterControl pmtComtrol;
    public Main_control main;
    public GameObject prompter;


    // Start is called before the first frame update
    void Update()
    {
        if(Time.frameCount % 30 == 0 & showText)
        {
            switch(showTextIdx)
            {
                case 0:
                    if(oldTime < Time.time - 1f)
                    {
                        Msg("안녕하세오 당근이에오!");
                        oldTime = Time.time;
                        showTextIdx = 1;
                    }
                    break;
                case 1:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Msg("빵 먹고 갈게오!");
                        oldTime = Time.time;
                        showTextIdx = 2;
                    }
                    break;
                case 2:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Boy_Eat();
                        Msg("냠냠 맛있다!");
                        showTextIdx = 3;
                    }
                    break;
                case 3:
                    if (stopEat)
                    {
                        Msg("감사함미다!");
                        Boy_Wait();
                        showTextIdx = 4;
                        oldTime = Time.time;
                    }
                    break;
                case 4:
                    if (oldTime < Time.time - 1f)
                    {
                        oldTime = Time.time;
                        showTextIdx = 5;
                        pmtComtrol.Reset();
                        main.AddBBang(1, "choco");
                        pmtComtrol.AddString("훈이", "집이 좀 깨끗해진 것 같아서 좋다.");
                        pmtComtrol.AddNextAction("bbangBoy", "bbangBoyOut");
                        prompter.SetActive(true);
                        prompter.GetComponent<Animator>().SetTrigger("show");
                    }
                    break;
                case 5:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Boy_Out();
                        showText = false;
                    }
                    break;

            }
        }
    }

    public void Boy_Wait()
    {
        boy_ui.GetComponent<Animator>().SetTrigger("wait");
    }

    public void Boy_Eat()
    {
        boy_ui.GetComponent<Animator>().SetTrigger("eat");
        bbang_Showroom.removeBbangMode = true;
        stopEat = false;
    }

    public void Boy_In()
    {
        pmtComtrol.CallAction("bbangBoy", "bbangBoyIn");
        boy_ui.GetComponent<Animator>().SetTrigger("in");
        boy_ui.SetActive(true);
        showText = true;
        showTextIdx = 0;
        oldTime = Time.time;
    }

    public void Boy_Out()
    {
        boy_ui.GetComponent<Animator>().SetTrigger("out");
    }

    public void StopEating()
    {
        Boy_Out();
    }

    public void Msg(string output)
    {
        bbangboyBalloon_ui.GetComponent<Text>().text = output;
        bbangboyBalloon.SetActive(true);
        bbangboyBalloon.GetComponent<Animator>().SetTrigger("show");
    }

    public void SetBbang80()
    {
        PlayerPrefs.SetInt("bbang_choco", 80);
        bbang_Showroom.UpdateBbangShow();
    }

    public void Debug_mat()
    {
        PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
        PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") + 1);
        bbang_Showroom.UpdateBbangShow();
    }
}
