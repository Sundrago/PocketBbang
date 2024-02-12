using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbaControl : MonoBehaviour
{
    [SerializeField] Sprite[] customers = new Sprite[11];
    [SerializeField] PrompterControl pmtControl;
    [SerializeField] AudioControl myAudio;
    [SerializeField] Heart_Control heart;
    [SerializeField] Bbang_showroom showroom;

    [SerializeField] GameObject main, balloon, circles;
    [SerializeField] GameObject[] circle = new GameObject[7];
    [SerializeField] Image customer_ui;
    [SerializeField] GameObject imgGroup;
    [SerializeField] Text q_ui, combo_ui, count_ui, timer_ui, ready_ui;
    [SerializeField] Slider slider_ui;

    public bool fMode = false;

    string answer = "";
    int comboCount = 0;
    int customerCount;
    int readyCount;
    int BeforeAudioIdx;
    float downSize = 0.08f;
    float reductionPoint = 0.2f;
    float startTime;
    bool circleShow;
    bool lowData = false;

    private void Start()
    {
#if UNITY_ANDROID
        downSize = 0.06f;
        reductionPoint = 0.15f;
#endif
    }
    // Start is called before the first frame update
    public void StartAlba(bool myFmode = false)
    {
        if (myFmode) fMode = true;
        else fMode = false;

        circleShow = false;
        CircleFadeOut();
        main.GetComponent<Main_control>().BtnClicked("close_phone");
        slider_ui.value = 0.6f;
        gameObject.SetActive(true);
        comboCount = 0;
        customerCount = 0;
        startTime = Time.time;
        readyCount = 0;
        imgGroup.SetActive(false);
        combo_ui.gameObject.SetActive(false);
        ready_ui.gameObject.SetActive(true);
        ready_ui.text = "30초 동안 최대한 많은 손님을 상대하세요!";
        timer_ui.text = "준비하세요!";
        count_ui.text = " ";
        PlauMusic();

        if (PlayerPrefs.GetInt("lowDataMode") == 1)
        {
            foreach(GameObject obj in circle)
            {
                obj.SetActive(false);
            }
            lowData = true;
        }
        else
        {
            foreach (GameObject obj in circle)
            {
                obj.SetActive(true);
            }
            lowData = false;
        }
        //UpdateQ();
    }

    void UpdateQ()
    {
        int rnd = Random.Range(0, 11);
        customerCount += 1;
        count_ui.text = (customerCount) + "번째 손님";
        customer_ui.sprite = customers[rnd];
        rnd = Random.Range(0, 25);


        PlayerPrefs.SetInt("albaCustomerCount", PlayerPrefs.GetInt("albaCustomerCount") + 1);
        PlayerPrefs.Save();

        switch (rnd)
        {
            case 0:
                q_ui.text = "포켓볼빵 있어요?";
                answer = "no";
                break;
            case 1:
                q_ui.text = "포켓볼빵 있나요?";
                answer = "no";
                break;
            case 2:
                q_ui.text = "혹시 포켓볼빵 있어요?";
                answer = "no";
                break;
            case 3:
                q_ui.text = "포켓볼빵 혹시 있나요?";
                answer = "no";
                break;
            case 4:
                q_ui.text = "포켓볼빵 있죠?";
                answer = "no";
                break;
            case 5:
                q_ui.text = "혹시 있어요 포켓볼빵?";
                answer = "no";
                break;
            case 6:
                q_ui.text = "포켓볼빵 남았나요?";
                answer = "no";
                break;
            case 7:
                q_ui.text = "포켓볼빵 들어왔어요?";
                answer = "no";
                break;
            case 8:
                q_ui.text = "저기요 포켓볼빵 있어요?";
                answer = "no";
                break;
            case 9:
                q_ui.text = "포켓볼빵 팔아요?";
                answer = "no";
                break;

            case 10:
                q_ui.text = "포켓볼빵 없죠?";
                answer = "yes";
                break;
            case 11:
                q_ui.text = "포켓볼빵 아직이죠?";
                answer = "yes";
                break;
            case 12:
                q_ui.text = "포켓볼빵 아직 안 들어왔죠?";
                answer = "yes";
                break;
            case 13:
                q_ui.text = "포켓볼빵 없나요?";
                answer = "yes";
                break;
            case 14:
                q_ui.text = "포켓볼빵 아직 없죠?";
                answer = "yes";
                break;
            case 15:
                q_ui.text = "포켓볼빵 안 들어왔죠?";
                answer = "yes";
                break;
            case 16:
                q_ui.text = "아직이죠 포켓볼빵?";
                answer = "yes";
                break;
            case 17:
                q_ui.text = "포켓볼빵 안 팔아요?";
                answer = "yes";
                break;
            case 18:
                q_ui.text = "혹시 포켓볼빵 없겠죠?";
                answer = "yes";
                break;

            case 19:
                q_ui.text = "포켓볼빵 언제 들어와요?";
                answer = "dontknow";
                break;
            case 20:
                q_ui.text = "포켓볼빵 언제 오나요?";
                answer = "dontknow";
                break;
            case 21:
                q_ui.text = "포켓볼빵 언제 살 수 있나요?";
                answer = "dontknow";
                break;
            case 22:
                q_ui.text = "포켓볼빵 언제오면 살 수 있나요?";
                answer = "dontknow";
                break;
            case 23:
                q_ui.text = "포켓볼빵 어디가면 살 수 있죠?";
                answer = "dontknow";
                break;
            case 24:
                q_ui.text = "포켓볼빵 어디에 있나요?";
                answer = "dontknow";
                break;
            case 25:
                q_ui.text = "왜 안팔아요?";
                answer = "dontknow";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyCount == 0 & Time.time - startTime >= 1)
        {
            ready_ui.text = "READY!";
            readyCount = 1;
            startTime = Time.time;
        }
        else if (readyCount == 1 & Time.time - startTime >= 1)
        {
            ready_ui.text = "GO!";
            readyCount = 2;
            startTime = Time.time;
        } else if (readyCount == 2 & Time.time - startTime >= 1)
        {
            imgGroup.SetActive(true);
            ready_ui.gameObject.SetActive(false);
            UpdateQ();
            readyCount = 3;
            startTime = Time.time;
        }
        else if(readyCount == 3) //main
        {
            timer_ui.text = Mathf.Floor((-Time.time + startTime + 30) * 10) / 10 + " 초";
            if (-Time.time + startTime + 30 < 0.1f) readyCount = 4;
            
            float dynamic = (slider_ui.value - 0.5f) * 0.07f;

            if (slider_ui.value - (downSize + dynamic) * Time.deltaTime < 0)
                slider_ui.value = 0;
            else
                slider_ui.value -= (downSize + dynamic) * Time.deltaTime;

            if (slider_ui.value < 0.1f) {
                readyCount = 6;
                FailAlba();
            }

            if (circleShow & slider_ui.value < 0.65f & !lowData)
            {
                CircleFadeOut();
                circleShow = false;
            }
        } else if(readyCount == 4)
        {
            ready_ui.gameObject.SetActive(true);
            imgGroup.SetActive(false);
            ready_ui.text = "종료!";
            timer_ui.text = "0초";
            readyCount = 5;
            startTime = Time.time;

        } else if(readyCount == 5 & Time.time - startTime >= 1)
        {
            readyCount = 6;
            EndAlba();
        }
    }

    public void AddPoint()
    {
        if(slider_ui.value + 0.15f > 1 ){
            slider_ui.value = 1;
        } else
        {
            slider_ui.value += 0.15f;
        }
        UpdateQ();
    }

    public void RemovePoint()
    {
        if (slider_ui.value - 0.2f < 0)
        {
            slider_ui.value = 0;
        }
        else
        {
            slider_ui.value -= reductionPoint;
        }
        UpdateQ();
    }

    public void AWbtnClicked(string aw)
    {
        if (aw == answer)
        {
            AddPoint();
            if (slider_ui.value > 0.1f)
            {
                comboCount += 1;
                combo_ui.color = Color.green;
                combo_ui.text = comboCount + " COMBO";
                if (!lowData)
                {
                    circles.GetComponent<Animator>().speed = 1f + comboCount * 0.1f;
                    if (!circleShow & slider_ui.value >= 0.65f)
                    {
                        CircleFadeIn();
                        circleShow = true;
                    }
                }
                combo_ui.gameObject.SetActive(true);
            }
        }
        else
        {
            RemovePoint();
            comboCount = 0;
            combo_ui.color = Color.red;
            combo_ui.text = "MISS!!";
            combo_ui.gameObject.SetActive(true);
        }
    }

    void EndAlba()
    {
        main.GetComponent<Main_control>().lower_bar.GetComponent<Animator>().SetTrigger("hide");
        StopMusic();
        gameObject.SetActive(false);
        pmtControl.gameObject.SetActive(true);
        pmtControl.gameObject.GetComponent<Animator>().SetTrigger("show");
        pmtControl.Reset();
        pmtControl.imageMode = true;
        pmtControl.AddString("점주", "수고 많았어!");
        pmtControl.AddString("점주", "총 " + customerCount + "명의 손님을 받았구나!");

        if(customerCount <= 10)
        {
            pmtControl.AddString("점주", "자네 일솜씨는 영 별로구만?");
        }
        else if (customerCount <= 20)
        {
            pmtControl.AddString("점주", "그럭저럭 나쁘지 않은 성적이군!");
        }
        else if (customerCount <= 30)
        {
            pmtControl.AddString("점주", "아쉽구만 조금 더 잘하면 보너스를 줄텐데.");
        }
        else if (customerCount > 30)
        {
            pmtControl.AddString("점주", "훌륭해! 보너스로 맛동산을 한 상자 줄게!");
            pmtControl.AddString("맛동산", "맛동산 한 상자를 받았다.");
            PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
            PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") + 1);
            showroom.UpdateBbangShow();
        }
        else if (customerCount > 50)
        {
            pmtControl.AddString("점주", "정말 대단해! 보너스로 맛동산을 두 상자 줄게!");
            pmtControl.AddString("맛동산", "맛동산 두 상자를 받았다.");
            PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 2);
            PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("bbang_mat") + 2);
            showroom.UpdateBbangShow();
        }

        pmtControl.AddString("점주", "1시간 일했으니까.."); //9,160
        pmtControl.AddString("점주", "자. 고생한만큼 두둑히 챙겨줬어.");
        pmtControl.AddString("용돈", "최저시급 9,860원을 받았다!");
        pmtControl.AddString("훈이", "감사합니다.");

        heart.UpdateMoney(9860);

        if(fMode)
            pmtControl.AddNextAction("store", "albaf_end");
        else
            pmtControl.AddNextAction("main", "alba_end");

        balloon.GetComponent<BalloonControl>().ShowMsg("뿌듯하다..");
        
    }

    void FailAlba()
    {
        main.GetComponent<Main_control>().lower_bar.GetComponent<Animator>().SetTrigger("hide");
        StopMusic();
        gameObject.SetActive(false);
        pmtControl.gameObject.SetActive(true);
        pmtControl.gameObject.GetComponent<Animator>().SetTrigger("show");
        pmtControl.Reset();
        pmtControl.imageMode = true;
        pmtControl.AddString("점주", "뭐야!");
        pmtControl.AddString("점주", "정말 도움이 안 되는걸!");
        pmtControl.AddString("점주", "이렇게 하면 알바비를 줄 수가 없지!");

        if (fMode)
            pmtControl.AddNextAction("store", "albaf_end");
        else
            pmtControl.AddNextAction("main", "alba_end");

        balloon.GetComponent<BalloonControl>().ShowMsg("너무 힘들다..");
    }

    void CircleFadeIn()
    {
        for(int i = 0; i<circle.Length; i++)
        {
            circle[i].SetActive(true);
            circle[i].GetComponent<Animator>().SetTrigger("show");
        }
    }

    void CircleFadeOut()
    {
        for (int i = 0; i < circle.Length; i++)
        {
            circle[i].GetComponent<Animator>().SetTrigger("hide");
        }
    }

    void PlauMusic()
    {
        BeforeAudioIdx = myAudio.currentPlaying;
        myAudio.PlayMusic(6);
    }

    void StopMusic()
    {
        myAudio.PlayMusic(BeforeAudioIdx);
    }
}
