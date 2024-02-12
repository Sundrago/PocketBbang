using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DdukCtrl : MonoBehaviour
{
    [SerializeField] AudioControl myAudio;
    [SerializeField] Ad_Control ads;
    [SerializeField] PrompterControl pmtControl;
    [SerializeField] Heart_Control heart;
    [SerializeField] List<GameObject> dducks = new List<GameObject>();

    [SerializeField] GameObject[] ddukcPrefabs = new GameObject[3];
    [SerializeField] GameObject[] spiderPrefabs = new GameObject[2];
    [SerializeField] GameObject[] hearts = new GameObject[6];
    [SerializeField] GameObject[] heartsResult = new GameObject[6];
    [SerializeField] GameObject topLeft, topRight;
    [SerializeField] GameObject ddukHolder;

    [SerializeField] GameObject combo_holder, combo;
    [SerializeField] GameObject yellowBG;
    [SerializeField] GameObject btmPoint;
    [SerializeField] GameObject spawner_pos;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject heartHolder, heartHolderPos, resultHeartHolder;
    [SerializeField] GameObject restartBtn;
    [SerializeField] GameObject rullet, disk;

    [SerializeField] Text timer_ui, resultText;
    [SerializeField] GameObject cupA, cupB;
    [SerializeField] GameObject main;
    [SerializeField] GameObject restartBtn2, restartSlider;
    [SerializeField] GameObject frontCha;

    bool SpawnerMove;
    bool SpawnerLeft;
    bool bonus;

    int event2Count;
    int comboCount;
    int dduckCount;
    int spiderCount;
    int BeforeAudioIdx;
    int myScore, resultScore;

    float SpawnerMoveTime;
    float midPoint; 
    float startTime;

    enum state { ready, idle, rullet, bonus, result };
    state currentState = state.ready;


    public void OpenPanel()
    {
        timer_ui.text = "";
        gameObject.SetActive(true);
        UpdateHeart(0);
        YellowBG(0);
        tutorialPanel.SetActive(true);
        resultHeartHolder.SetActive(false);
        cupA.transform.position = new Vector2(0, cupA.transform.position.y);
        cupB.transform.position = cupA.transform.position;
        comboCount = 0;
    }

    void StartGame()
    {
        myScore = 0;
        UpdateHeart(0);
        YellowBG(0);
        ChanageState(state.idle);
        HeartHolderShowHide(true);
        resultHeartHolder.SetActive(false);
        comboCount = 0;
        startTime = Time.time;
        //resetCupPos
    }

    void Update()
    {
        if (currentState == state.ready) return;

        if(currentState == state.idle | currentState == state.bonus)
        {
            timer_ui.text = Mathf.Floor((-Time.time + startTime + 30) * 10) / 10 + " 초";
            if (-Time.time + startTime + 30 < 0.1f) ChanageState(state.result);

        }

        if (currentState == state.idle) {

            if (Time.frameCount % 20 == 0)
            {
                int rnd = Random.Range(0, 3);
                GameObject dduck = Instantiate(ddukcPrefabs[rnd], ddukHolder.transform);
                dduck.transform.position = new Vector2(Random.Range(topLeft.transform.position.x, topRight.transform.position.x), topLeft.transform.position.y);
                dduck.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                dduck.SetActive(true);
                dduck.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2 * myScore, 2 * myScore), 0));
                dducks.Add(dduck);

                //1/3
                if(Random.Range(0f,1f) <= myScore / 100f)
                {
                    dduck = Instantiate(spiderPrefabs[1], ddukHolder.transform);
                    dduck.transform.position = new Vector2(Random.Range(topLeft.transform.position.x, topRight.transform.position.x), topLeft.transform.position.y);
                    dduck.SetActive(true);
                    dduck.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2 * myScore, 2 * myScore), 0));
                    dducks.Add(dduck);
                }
            }

            if (Time.frameCount % 65 == 0)
            {
                GameObject dduck = Instantiate(spiderPrefabs[1], ddukHolder.transform);
                dduck.transform.position = new Vector2(Random.Range(topLeft.transform.position.x, topRight.transform.position.x), topLeft.transform.position.y);
                //dduck.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                dduck.SetActive(true);
                dduck.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2 * myScore, 2 * myScore), 0));
                dducks.Add(dduck);

                for(int i = dducks.Count-1; i>=0; i--)
                {
                    if(dducks[i].transform.position.y < cupA.transform.position.y - 10f)
                    {
                        Destroy(dducks[i]);
                        dducks.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void GotDduk (GameObject obj)
    {
        if (dducks.Contains(obj)) dducks.Remove(obj);
        Destroy(obj);

        if (currentState == state.result) return;

        myAudio.PlaySoundFx(3);
        comboCount += 1;
        if(comboCount<=10 & currentState == state.idle) YellowBG(comboCount);
        //ShowCombo(comboCount);

        myScore += 1;
        myScore = Mathf.Min(myScore, 60);
        UpdateHeart(myScore);
        dduckCount += 1;
    }

    public void LostDduck (GameObject obj)
    {

        if (dducks.Contains(obj)) dducks.Remove(obj);
        Destroy(obj);

        if (currentState == state.result) return;

        myAudio.PlaySoundFx(4);
        if (comboCount != 0 & currentState == state.idle) YellowBG(0);
        comboCount = 0;

        myScore -= 6;
        myScore = Mathf.Max(myScore, 0);
        UpdateHeart(myScore);
        spiderCount += 1;
    }

    private void SpawnerMoveToPoint(float normal, float time)
    {
        float x = Mathf.Lerp(topLeft.transform.position.x, topRight.transform.position.x, normal);
        spawner_pos.transform.DOMoveX(x, time)
            .OnComplete(SpawnerMoveComplete);
    }

    private void SpawnerMoveComplete()
    {
        if (!SpawnerMove) return;

        if(SpawnerLeft)
        {
            SpawnerMoveToPoint(0.9f, SpawnerMoveTime);
            SpawnerLeft = false;
        } else
        {
            SpawnerMoveToPoint(0.1f, SpawnerMoveTime);
            SpawnerLeft = true;
        }
    }

    private void UpdateHeart(int score)
    {
        float c = score / 10f;
        int quotient = (int)System.Math.Truncate(c); // 몫
        int remainder = score % 10;  // 나머지

        for(int i = 0; i<6; i++)
        {
            if (i < quotient) hearts[i].GetComponent<HeartIconCtrl>().SetHeartStatus(10);
            else if (i == quotient) hearts[i].GetComponent<HeartIconCtrl>().SetHeartStatus(remainder);
            else hearts[i].GetComponent<HeartIconCtrl>().SetHeartStatus(0);
        }
    }

    IEnumerator Event1(int count)
    {
        SpawnerMove = true;
        SpawnerMoveTime = 0.65f;
        SpawnerMoveComplete();

        for (int i = 0; i<count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            AddDduck();
        }
        SpawnerMove = false;

        if (currentState != state.result) ChanageState(state.idle);
    }

    IEnumerator Event2(int count) 
    {
        event2Count = count;

        AddSpiderOnNormal(0.1f);
        AddSpiderOnNormal(0.9f);
        yield return new WaitForSeconds(0.5f);

        AddSpiderOnNormal(0.1f);
        AddSpiderOnNormal(0.9f);
        yield return new WaitForSeconds(0.4f);

        AddSpiderOnNormal(0.1f);
        AddSpiderOnNormal(0.9f);
        yield return new WaitForSeconds(0.3f);

        AddSpiderOnNormal(0.1f);
        AddSpiderOnNormal(0.9f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.15f);
        AddSpiderOnNormal(0.85f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.2f);
        AddSpiderOnNormal(0.8f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.25f);
        AddSpiderOnNormal(0.75f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.3f);
        AddSpiderOnNormal(0.7f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.2f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.1f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.1f);

        AddSpiderOnNormal(0.35f);
        AddSpiderOnNormal(0.65f);
        yield return new WaitForSeconds(0.1f);

        midPoint = 0.5f;
        float randomPos = Random.Range(0.15f, 0.85f);
        float randomTime = Random.Range(0.5f, 2f);
        DOTween.To(() => midPoint, x => midPoint = x, randomPos, randomTime)
            .OnComplete(Event1TweenComplete);

        for(int i = 0; i<100; i++)
        {
            if (event2Count <= 0) break;
            AddSpiderOnNormal(midPoint - 0.15f);
            AddSpiderOnNormal(midPoint + 0.15f);
            yield return new WaitForSeconds(0.1f);
        }

        if (currentState != state.result) ChanageState(state.idle);
    }

    public void Event1TweenComplete() {
        event2Count -= 1;
        if (event2Count <= 0) return;

        float randomPos = Random.Range(0.15f, 0.85f);
        float randomTime = Random.Range(0.3f, 1f);
        DOTween.To(() => midPoint, x => midPoint = x, randomPos, randomTime)
            .OnComplete(Event1TweenComplete);
    }

    IEnumerator Event3(int count)
    {
        float normalPos = Random.Range(0.1f, 0.9f);
        float x = Mathf.Lerp(topLeft.transform.position.x, topRight.transform.position.x, normalPos);
        float y = topRight.transform.position.y;
        spawner_pos.transform.position = new Vector2(x, y);

        bool isSpider = false;

        AddDduck();
        yield return new WaitForSeconds(0.5f);
        AddDduck();
        yield return new WaitForSeconds(0.4f);
        AddDduck();
        yield return new WaitForSeconds(0.3f);
        AddDduck();
        yield return new WaitForSeconds(0.2f);

        SpawnerMove = true;
        SpawnerMoveTime = 1.2f;
        SpawnerMoveComplete();

        for (int i = 0; i<count; i++)
        {
            isSpider = toggleSpider(isSpider);
            if (isSpider) AddSpider();
            else AddDduck();
            yield return new WaitForSeconds(0.1f);
        }

        SpawnerMove = false;
        if (currentState != state.result) ChanageState(state.idle);
    }

    bool toggleSpider(bool isSpider)
    {
        if(isSpider)
        {
            if (Random.Range(0, 4) < 1) return false;
            else return true;
        } else
        {
            if (Random.Range(0, 6) < 1) return true;
            else return false;
        }
    }

    IEnumerator Event4(int count)
    {
        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(1f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.5f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.5f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.5f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.4f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.3f);

        CreateSpiderExcept(Random.Range(0.1f, 0.9f));
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i<count; i++)
        {
            CreateSpiderExcept(Random.Range(0.1f, 0.9f));
            if (count > 20) yield return new WaitForSeconds(0.25f);
            else if(count > 10)  yield return new WaitForSeconds(0.2f);
            else if (count > 5) yield return new WaitForSeconds(0.15f);
            else yield return new WaitForSeconds(0.1f);
        }

        if (currentState != state.result) ChanageState(state.idle);
    }

    private void CreateSpiderExcept(float normal)
    {
        for(int i = 0; i<10; i++)
        {
            float normalX = i / 10f;
            if (Mathf.Abs(normalX - normal) < 0.15f) continue;

            AddSpiderOnNormal(normalX);
        }
    }


    private void AddDduck(int rnd = -1)
    {
        if (currentState == state.result) return;
        if(rnd == -1) rnd = Random.Range(0, 3);
        GameObject dduck = Instantiate(ddukcPrefabs[rnd], ddukHolder.transform);
        dduck.transform.position = spawner_pos.transform.position;
        dduck.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        dduck.SetActive(true);
        dducks.Add(dduck);
    }

    private void AddSpider()
    {
        if (currentState == state.result) return;
        GameObject dduck = Instantiate(spiderPrefabs[1], ddukHolder.transform);
        dduck.transform.position = spawner_pos.transform.position;
        dduck.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        dduck.SetActive(true);
        dducks.Add(dduck);
    }

    private void AddSpiderOnNormal(float normalPosition)
    {
        if (currentState == state.result) return;
        float x = Mathf.Lerp(topLeft.transform.position.x, topRight.transform.position.x, normalPosition);
        GameObject dduck = Instantiate(spiderPrefabs[1], ddukHolder.transform);
        dduck.transform.position = new Vector2(x, spawner_pos.transform.position.y);
        dduck.SetActive(true);

        dducks.Add(dduck);
    }

    public void Debug_Play_Evnet(int idx)
    {
        spiderCount = 0;
        dduckCount = 0;

        switch(idx)
        {
            case 1:
                ChanageState(state.rullet);
                //ShowRullet();
                //StartCoroutine(Event1(50));
                break;
            case 2:
                StartCoroutine(Event2(3));
                break;
            case 3:
                StartCoroutine(Event3(50));
                break;
            case 4:
                StartCoroutine(Event4(10));
                break;
        }
    }

    public void YellowBG(int idx)
    {
        if (idx == 10) ChanageState(state.rullet);

        if (DOTween.IsTweening(yellowBG.transform)) DOTween.Kill(yellowBG.transform);
        float normal = idx / 10f;
        float y = Mathf.Lerp(btmPoint.transform.position.y, topLeft.transform.position.y, normal);
        print(idx + " " + y);
        yellowBG.transform.DOMoveY(y,0.2f);
    }


    private void ShowRullet()
    {
        if (DOTween.IsTweening(rullet.transform)) DOTween.Kill(rullet.transform);

        Vector2 pos = rullet.transform.position;
        pos.x = topLeft.transform.position.x * 1.5f;

        rullet.transform.position = pos;
        rullet.transform.DOMoveX(0f, 1f)
            .SetEase(Ease.OutExpo)
            .OnComplete(SpinRullet);

        rullet.SetActive(true);
    }

    private void SpinRullet()
    {
        if (DOTween.IsTweening(disk.transform)) DOTween.Kill(disk.transform);
        disk.transform.DORotate(new Vector3(0, 0, 30), 0.3f)
            .SetDelay(0.3f);

        int targetAngle;
        if (bonus) targetAngle = -720;
        else targetAngle = -810;

        disk.transform.DORotate(new Vector3(0, 0, targetAngle), 2.5f, RotateMode.FastBeyond360)
            .SetDelay(0.65f)
            .SetEase(Ease.OutExpo)
            .OnComplete(RulletSpinFinished);
    }

    private void RulletSpinFinished()
    {
        float x = topLeft.transform.position.x * 1.5f;

        rullet.transform.DOMoveX(x, 0.5f)
            .SetDelay(0.23f)
            .SetEase(Ease.InExpo)
            .OnComplete(RulletHide);

        ChanageState(state.bonus);
        startTime += 3.15f;
    }

    private void RulletHide()
    {
        rullet.gameObject.SetActive(false);
    }

    private void ChanageState(state toState)
    {
        currentState = toState;

        if (toState == state.ready)
        {
            tutorialPanel.SetActive(true);
            HeartHolderShowHide(false);
            myAudio.PlayMusic(7);
        }
        else if(toState == state.idle)
        {
            tutorialPanel.SetActive(false);
            myAudio.PlayMusic(7);
        } else if (toState == state.rullet)
        {
            bonus = Random.Range(0, 2) == 0;
            myAudio.PlayMusic(6);
            ShowRullet();
        } else if (toState == state.bonus)
        {
            if(bonus)
            {
                myAudio.PlaySoundFx(1);
                int rnd = Random.Range(0, 2);
                if (rnd == 0) StartCoroutine(Event1(40));
                else StartCoroutine(Event3(50));
            } else
            {
                YellowBG(0);
                myAudio.PlaySoundFx(2);
                myAudio.PlayMusic(7);
                int rnd = Random.Range(0, 2);
                if(rnd == 0) StartCoroutine(Event2(4));
                else StartCoroutine(Event4(10));
            }
        } else if (toState == state.result)
        {
            YellowBG(0);
            restartBtn.SetActive(false);
            restartBtn2.SetActive(false);
            resultHeartHolder.SetActive(true);
            restartSlider.SetActive(false);
            timer_ui.text = "";
            myAudio.PlayMusic(7);
            HeartHolderShowHide(false);
            resultScore = 0;
            DOTween.To(() => resultScore, x => resultScore = x, myScore, myScore / 20f)
                .OnUpdate(HeartUpdateResult)
                .OnComplete(GameFinished);
        }
    }

    private void HeartUpdateResult()
    {
        int score = resultScore;
        float c = score / 10f;
        int quotient = (int)System.Math.Truncate(c); // 몫
        int remainder = score % 10;  // 나머지

        for (int i = 0; i < 6; i++)
        {
            if (i < quotient) heartsResult[i].GetComponent<HeartIconCtrl>().SetHeartStatus(10, false);
            else if (i == quotient) heartsResult[i].GetComponent<HeartIconCtrl>().SetHeartStatus(remainder);
            else heartsResult[i].GetComponent<HeartIconCtrl>().SetHeartStatus(0, false);
        }

        resultText.text = "하트 " + quotient + "개 획득!";
    }

    private void GameFinished()
    {
#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("GameScore", "Dduk", resultScore);
#endif
        
        if (resultScore >= 10) myAudio.PlaySoundFx(1); //bgm
        else myAudio.PlaySoundFx(2); //failBGM

        restartBtn.transform.DOScale(0, 0.5f)
            .From();
        restartBtn.SetActive(true);
        restartBtn2.SetActive(false);
        restartBtn2.transform.DOScale(0, 0.5f)
            .SetDelay(0.5f)
            .OnComplete(RestartTimerTick)
            .OnStart(ShowrestartBtn2)
            .From();
    }

    void ShowrestartBtn2()
    {
        restartBtn2.SetActive(true);
    }

    private void RestartTimerTick()
    {
        restartBtn2.GetComponent<Button>().interactable = true;
        restartSlider.SetActive(true);
        restartSlider.GetComponent<Slider>().value = 1;
        restartSlider.GetComponent<Slider>().DOValue(0, 5f)
            .SetEase(Ease.Linear)
            .OnComplete(SliderFinished);
    }

    private void SliderFinished()
    {
        restartBtn2.GetComponent<Button>().interactable = false;
    }

    private void HeartHolderShowHide(bool show)
    {
        if (DOTween.IsTweening(heartHolder)) DOTween.Kill(heartHolder.transform);

        if(show) heartHolder.transform.DOMoveY(heartHolderPos.transform.position.y, 1f);
        else heartHolder.transform.DOMoveY(topLeft.transform.position.y, 1f);
    }

    public void EndDduk()
    {
        int score = resultScore;
        float c = score / 10f;
        int quotient = (int)System.Math.Truncate(c); // 몫


        main.GetComponent<Main_control>().lower_bar.GetComponent<Animator>().SetTrigger("hide");
        gameObject.SetActive(false);
        myAudio.PlayMusic(BeforeAudioIdx);

        pmtControl.gameObject.SetActive(true);
        pmtControl.gameObject.GetComponent<Animator>().SetTrigger("show");
        pmtControl.Reset();
        pmtControl.imageMode = true;
        frontCha.SetActive(true);

        if (quotient == 0)
        {
            pmtControl.AddString("훈이", "으.. 속이 좀 안 좋은 것 같다..");
            pmtControl.AddString("훈이", "체력을 하나도 회복하지 못 했다.");
            pmtControl.AddString("박종업원", "...");
            pmtControl.AddString("박종업원", "......");
            pmtControl.AddString("박종업원", "표정이 썩 좋지 않구먼유..");
            pmtControl.AddString("훈이", "맛이 없는건 아닌데..");
            pmtControl.AddString("박종업원", "휴.. 속상하구먼유.");
            pmtControl.AddString("박종업원", "다음에 또 오세유.");
            pmtControl.AddString("훈이", "네. 다음에 또 들를게요.");
            pmtControl.AddNextAction("main", "store_out");
            main.GetComponent<Main_control>().storeOutAction = "다음에도 떡볶이를 먹으러 와야겠다.";
        } else
        {
            if(heart.heartCount + quotient >= 6)
            {
                pmtControl.AddString("훈이", "든든하다..!");
                pmtControl.AddString("훈이", "체력이 가득 찼다!!");
                pmtControl.AddString("박종업원", "맛있게 먹으니 뿌듯하구먼유.");
                pmtControl.AddString("훈이", "감사합니다 박종업원 아저씨. 다음에 또 올게요!");
                pmtControl.AddString("박종업원", "언제든 들르슈!");
                pmtControl.AddNextAction("main", "store_out");
                main.GetComponent<Main_control>().storeOutAction = "떡볶이로 속이 든든해졌다! 다음에 또 와야겠다.";
            }
            else
            {
                pmtControl.AddString("훈이", "맛있었다..!");
                pmtControl.AddString("훈이", "체력이 + " + quotient + " 칸 회복됐다!!");
                pmtControl.AddString("박종업원", "맛이 어때유?");
                pmtControl.AddString("훈이", "음.. 맛있어요!");
                pmtControl.AddString("박종업원", "그것 참 다행이구먼유.");

                pmtControl.AddOption("잘 먹었습니다. 다음에 또 올게요!", "store", "p_good");
                pmtControl.AddOption("더 먹어도 되나요?", "store", "p_again");
                
                pmtControl.AddString("훈이", "다음에 또 들러도 되죠?");
                pmtControl.AddString("박종업원", "그럼유. 언제든 또 오세유.");
                pmtControl.AddNextAction("main", "store_out");
            }

            for (int i = 0; i<quotient; i++)
            {
                heart.SetHeart(1);
            }
        }
    }

    public void StartDduk()
    {
        myAudio.PlayMusic(7);
        main.GetComponent<Main_control>().BtnClicked("close_phone");
        BeforeAudioIdx = myAudio.currentPlaying;
        gameObject.SetActive(true);
        OpenPanel();
        frontCha.SetActive(false);
    }

    public void RestartBtnClicked()
    {
        ads.PlayAds(Ad_Control.AdsType.dduk);
    }
}
