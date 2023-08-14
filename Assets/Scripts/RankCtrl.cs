using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class RankCtrl : MonoBehaviour
{
    [SerializeField] AchievementCtrl achievement;
    [SerializeField] GameObject rankup_ui, text01_ui, text02_ui, closeBtn_ui, rankSlider, sliderText_ui, rankPos, showRankBtn_ui, sharBtn_ui;
    [SerializeField] GameObject rankingChart, rankingChartText, showChartBtn, closeChartBtn;
    [SerializeField] GameObject small_rank_ui;

    public enum State { iron, bronze, silver, gold, platinum, diamond, master, grandMaster };
    public GameObject[] ranks;
    public Sprite[] ranks_small;

    int currnetRank = 0;
    int currnetScore;
    bool initial = false;
    bool started = false;

#if UNITY_IPHONE
                const int totalCount = 40000;
#elif UNITY_ANDROID
                const int totalCount = 3500;
#endif

    int debug = 0;

    // Start is called before the first frame update
    public void Start()
    {
        if (started) return;
        started = true;

        if (!PlayerPrefs.HasKey("oldScore")) //!PlayerPrefs.HasKey("oldScore"))
        {
            PlayerPrefs.SetInt("oldScore", totalCount);
            PlayerPrefs.Save();
        }

        rankingChart.SetActive(false);
        if(PlayerPrefs.GetInt("oldScore") == totalCount)
        {
            small_rank_ui.SetActive(false);
            initial = true;
            achievement.UpdateDataAndReceiveRank();
        } else
        {
            small_rank_ui.SetActive(true);
            currnetRank = (int)SetState(PlayerPrefs.GetInt("oldScore"));
            small_rank_ui.GetComponent<Image>().sprite = ranks_small[currnetRank];
        }

    }

    public void CheckForUpdate(int score)
    {
        print(score);
        PlayerPrefs.SetInt("currnetRankScore", score);
        PlayerPrefs.Save();

        if(initial)
        {
            small_rank_ui.GetComponent<Image>().sprite = ranks_small[0];
            small_rank_ui.SetActive(true);

            if((int)SetState(score) == 0)
            {
                PlayerPrefs.SetInt("oldScore", score);
                PlayerPrefs.Save();
            }
            initial = false;
        }

        int oldScore = PlayerPrefs.GetInt("oldScore");

        if(SetState(oldScore) != SetState(score))
        {
            currnetRank = (int)SetState(oldScore);
            SetRankObjActive();
            RankUp();
            currnetRank = (int)SetState(score);
            currnetScore = score;
        }
    }

    public State SetState(int _score)
    {
        float rank = (1-_score / (float)totalCount) * 100f;
        State myState;
        if (rank > 99) myState = State.grandMaster;
        else if (rank > 97) myState = State.master;
        else if (rank > 90) myState = State.diamond;
        else if (rank > 80) myState = State.platinum;
        else if (rank > 70) myState = State.gold;
        else if (rank > 50) myState = State.silver;
        else if (rank > 30) myState = State.bronze;
        else myState = State.iron;

        return myState;
    }

    public void RankUp()
    {
        print(currnetRank);
        ranks[currnetRank].GetComponent<RankObj>().RankUp();
    }

    private void SetRankObjActive()
    {
        for (int i = 0; i < ranks.Length; i++)
        {
            ranks[i].transform.position = new Vector3(0, 0, 0);
            if (i == currnetRank)
            {
                ranks[i].SetActive(true);
            }
            else ranks[i].SetActive(false);
        }

        //RESET
        bool show = false;
        rankup_ui.SetActive(show);
        text01_ui.SetActive(show);
        text02_ui.SetActive(show);
        closeBtn_ui.SetActive(show);
        rankSlider.SetActive(show);
        showRankBtn_ui.SetActive(show);
        sharBtn_ui.SetActive(show);
        rankingChart.SetActive(show);
    }

    public void Ready()
    {
        ranks[currnetRank].GetComponent<RankObj>().Appear();
        small_rank_ui.GetComponent<Image>().sprite = ranks_small[currnetRank];
    }

    public void AnimFinished()
    {
        //RESET
        bool show = true;
        rankup_ui.SetActive(show);
        text01_ui.SetActive(show);
        text02_ui.SetActive(show);
        closeBtn_ui.SetActive(show);
        rankSlider.SetActive(show);
        showRankBtn_ui.SetActive(show);
        sharBtn_ui.SetActive(show);

        ranks[currnetRank].transform.DOMove(rankPos.transform.position, 1f).SetEase(Ease.OutCirc);
        rankup_ui.transform.DOMove(new Vector3(0,0,0), 1f).SetEase(Ease.OutCirc).From();
        rankup_ui.GetComponent<Text>().DOFade(0, 1f).From();

        rankSlider.GetComponent<Slider>().value = (1 - PlayerPrefs.GetInt("oldScore") / (float)totalCount);
        ChnageSliderValue();


        rankSlider.transform.DOScale(new Vector3(0,0,0) , 0.5f)
            .SetDelay(0.3f).From();
        rankSlider.GetComponent<Slider>().DOValue(1-currnetScore/(float)totalCount, 3f)
            .OnUpdate(ChnageSliderValue);

        text01_ui.GetComponent<Text>().text = "#수집한 스티커 " + PlayerPrefs.GetInt("myTotalCards") + "/" + PlayerPrefs.GetInt("TotalCards");
        text02_ui.GetComponent<Text>().text = "#방문한 편의점 " + PlayerPrefs.GetInt("storeCount") + "군데";
        text01_ui.GetComponent<Text>().DOText("", 2f, true, ScrambleMode.All).SetDelay(1.5f).From();
        text02_ui.GetComponent<Text>().DOText("", 2f, true, ScrambleMode.All).SetDelay(2f).From();


        showRankBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(4f).From();
        closeBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).SetDelay(5f).From();
        sharBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).SetDelay(5.2f).From();

        PlayerPrefs.SetInt("oldScore", currnetScore);
        PlayerPrefs.Save();
    }

    public void ChnageSliderValue()
    {
        sliderText_ui.GetComponent<Text>().text = "상위 " + Mathf.Round(1000 - rankSlider.GetComponent<Slider>().value * 1000) / 10f + "%";
    }

    public void ShareBtnClicked()
    {
        StartCoroutine(RecordFrame());
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        string myText = "#" + sliderText_ui.GetComponent<Text>().text + "\n" + text01_ui.GetComponent<Text>().text + "\n";
        myText += text02_ui.GetComponent<Text>().text + "\n";
        myText += "#포켓볼빵_더게임";

        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(myText);
        shareSheet.AddImage(texture);
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        // cleanup
        Object.Destroy(texture);
    }

    public void CloseBtnClicked()
    {
        achievement.UpdateDataAndReceiveRank();
        gameObject.SetActive(false);
    }

    public void DebugInt()
    {
        //gameObject.SetActive(true);
        //currnetRank = debug;
        //SetRankObjActive();
        //RankUp();
        //debug += 1;
        //currnetRank = debug;
        //currnetScore = debug;

        //ShowScore(debug);
        //debug += 1;
    }

    public void ShowChart()
    {
        if (rankingChart.activeSelf) return;
        rankingChart.GetComponent<Image>().color = new Color(0,0,0,0);
        rankingChartText.GetComponent<Text>().color = new Color(1, 1, 1, 0);

        rankingChart.GetComponent<Image>().DOFade(0.75f, 0.5f);
        rankingChartText.GetComponent<Text>().DOFade(1, 0.5f);
        closeChartBtn.transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetDelay(0.5f);

        rankingChart.SetActive(true);
    }

    public void CloseChart()
    {
        rankingChart.GetComponent<Image>().DOFade(0, 0.3f);
        rankingChartText.GetComponent<Text>().DOFade(0, 0.3f).OnComplete(SetChartActiveFalse);
        closeChartBtn.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
    }

    public void SetChartActiveFalse()
    {
        rankingChart.SetActive(false);
    }

    public void ShowScore()
    {
        gameObject.SetActive(true);

        int oldScore = PlayerPrefs.GetInt("oldScore");
        currnetRank = (int)SetState(oldScore);
        SetRankObjActive();
        currnetScore = PlayerPrefs.GetInt("currnetRankScore");

        //RESET
        bool show = true;
        rankup_ui.SetActive(false);
        text01_ui.SetActive(show);
        text02_ui.SetActive(show);
        closeBtn_ui.SetActive(show);
        rankSlider.SetActive(show);
        showRankBtn_ui.SetActive(show);
        sharBtn_ui.SetActive(show);

        ranks[currnetRank].transform.DOMove(rankPos.transform.position, 0.5f).SetEase(Ease.OutCirc);

        rankSlider.GetComponent<Slider>().value = 0;
        ChnageSliderValue();

        rankSlider.transform.DOScale(new Vector3(0, 0, 0), 0.3f)
            .SetDelay(0.2f).From();
        rankSlider.GetComponent<Slider>().DOValue(1 - currnetScore / (float)totalCount, 0.5f)
            .OnUpdate(ChnageSliderValue).SetDelay(0.3f);
        showChartBtn.transform.DOScale(0, 0.3f).SetDelay(0.8f).From();

        text01_ui.GetComponent<Text>().text = "#수집한 스티커 " + PlayerPrefs.GetInt("myTotalCards") + "/" + PlayerPrefs.GetInt("TotalCards");
        text02_ui.GetComponent<Text>().text = "#방문한 편의점 " + PlayerPrefs.GetInt("storeCount") + "군데";
        text01_ui.GetComponent<Text>().DOText("", 0.5f, true, ScrambleMode.All).SetDelay(0.5f).From();
        text02_ui.GetComponent<Text>().DOText("", 0.5f, true, ScrambleMode.All).SetDelay(0.5f).From();

        showRankBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(1f).From();
        closeBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).SetDelay(1.1f).From();
        sharBtn_ui.transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce).SetDelay(1.2f).From();
    }
}
