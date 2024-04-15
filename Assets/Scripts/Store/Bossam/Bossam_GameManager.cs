using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Sirenix.OdinInspector;

public class Bossam_GameManager : MonoBehaviour
{
    [SerializeField] private List<Bossam_StageManager> bossamChar_prefab = new List<Bossam_StageManager>();
    public static Bossam_GameManager Instance;
    [SerializeField] private Bossam_StageManager bosamChar;
    [SerializeField] private TanghuruComboICon comboFX, failFX;
    [SerializeField] private Transform comboFxPos;
    [SerializeField] private Text score_text, time_text;
    [SerializeField] private GameObject tutorialPanel, scorePanel;
    [SerializeField] private MiniGameEndScoreManager EndScore;
    [SerializeField] private Bossam_ssam ssam;
    [SerializeField] private List<GameObject> GameObjSetActiveFalse = new List<GameObject>();
    [SerializeField] private Transform chaholder;
    [SerializeField] private Transform hand, handPos;
    
    private List<GameObject> actives;
    private int beforeAudioIdx;
    private enum gameStatus
    {
        tutorial, playing, score
    }

    private gameStatus status = gameStatus.score;
    private int comboConut = 0;
    private float startTime;

    private void Awake()
    {
        Instance = this;
    }

    private int score;
    
    public void StartGame()
    {
        SetGameStatus(gameStatus.playing);
    }

    private void Update()
    {
        if(status != gameStatus.playing) return;

        float timeLeft = 30f - (Time.time - startTime);
        timeLeft = MathF.Round(timeLeft * 10f) / 10f;
        time_text.text = timeLeft.ToString();

        if (timeLeft <= 0) SetGameStatus(gameStatus.score);
    }

    private void SetGameStatus(gameStatus _status)
    {
        if(status == _status) return;
        status = _status;

        switch (status)
        {
            case gameStatus.tutorial:
                score = 0;
                UpdateScoreUI();
                ssam.gameObject.SetActive(false);
                tutorialPanel.SetActive(true);
                scorePanel.SetActive(false);
                hand.gameObject.SetActive(false);
                ssam.Reset();
                break;
            case gameStatus.playing:
                ssam.gameObject.SetActive(true);
                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.whislte_start);
                tutorialPanel.SetActive(false);
                scorePanel.SetActive(false);
                hand.gameObject.SetActive(true);
                ShowHand();
                GetNewChar();
                comboConut = 0;
                score = 0;
                startTime = Time.time;
                break;
            case gameStatus.score:
                ssam.gameObject.SetActive(false);
                hand.gameObject.SetActive(false);
                EndScore.ShowScore(score);
                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.whistle_end);
                Destroy(bosamChar.gameObject);
                tutorialPanel.SetActive(true);
                break;
            
        }
    }

    private void UpdateScoreUI()
    {
        score_text.text = score + "점";
    }
    
    public void GotSssamPos(Vector3 pos)
    {
        if(bosamChar == null) return;
        
        for (int i = 0; i < bosamChar.chardata.Count; i++)
        {
            if(!bosamChar.chardata[i].mouthOpen) continue;
            float dist = Vector2.Distance(pos, bosamChar.chardata[i].character.center.position);
            if (dist < 1)
            {
                bosamChar.GotBossam(i);
                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.win);
                comboConut += 1;
                score += 100 * (10 + comboConut) / 10;
                UpdateScoreUI();
                TanghuruComboICon comboFXN = Instantiate(comboFX, gameObject.transform);
                comboFXN.Init(comboFxPos.position, comboConut);
                return;
            }
        }
        
        AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.loose);
        TanghuruComboICon comboFAILFX = Instantiate(failFX, gameObject.transform);
        comboFAILFX.InitFail(comboFxPos.position);
        comboConut = 0;
        score -= 20;
        UpdateScoreUI();

        // print(Vector2.Distance(gameObject.transform.position, bosamChar.center.gameObject.transform.position));
        // float dist = Vector2.Distance(pos, bosamChar.center.gameObject.transform.position);
        // print(pos);
        // if (bosamChar.ready && dist < 1)
        // {
        //     AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.win);
        //     comboConut += 1;
        //     TanghuruComboICon comboFXN = Instantiate(comboFX, gameObject.transform);
        //     comboFXN.Init(comboFxPos.position, comboConut);
        // }
        // else
        // {
        //     AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.loose);
        //     TanghuruComboICon comboFXN = Instantiate(failFX, gameObject.transform);
        //     comboFXN.InitFail(comboFxPos.position);
        //     comboConut = 0;
        // }
        //
        // bosamChar.AnimOut();
        // GetNewChar();

    }

    public void StageClear()
    {
        bosamChar.AnimOut();
        GetNewChar();
    }
    private void GetNewChar()
    {
        int rnd = UnityEngine.Random.Range(0, bossamChar_prefab.Count);
        bosamChar = Instantiate(bossamChar_prefab[rnd], chaholder);
        bosamChar.AnimIn();
        bosamChar.gameObject.SetActive(true);
    }
    
    public void WatchAdsAndRestartBtnClicked()
    {
        Ad_Control.Instance.PlayAds(Ad_Control.AdsType.bossam);
    }

    public void WathcedAds()
    {
        PlayerPrefs.SetInt("Minigame_adCount", (PlayerPrefs.GetInt("Minigame_adCount", 0) + 1));
        EndScore.HideScore();
        tutorialPanel.SetActive(true);
    }

    [Button]
    public void EnterGame()
    {
        actives = new List<GameObject>();
        foreach (GameObject obj in GameObjSetActiveFalse)
        {
            if (obj.activeSelf)
                actives.Add(obj);
        }

        foreach (var obj in actives)
        {
            obj.SetActive(false);
        }
        gameObject.SetActive(true);
        tutorialPanel.SetActive(true);
        EndScore.gameObject.SetActive(false);

        PlauMusic();
        void PlauMusic()
        {
            beforeAudioIdx = AudioControl.Instance.currentPlaying;
            AudioControl.Instance.PlayMusic(8);
        }
        PlayerPrefs.SetInt("Minigame_adCount", 0);
        SetGameStatus(gameStatus.tutorial);
        
    }

    [Button]
    public void ExitGame()
    {
        foreach (var obj in actives)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);

        StopMusic();
        void StopMusic()
        {
            AudioControl.Instance.PlayMusic(beforeAudioIdx);
        }
        
        Main_control.Instance.lower_bar.GetComponent<Animator>().SetTrigger("hide");
        Heart_Control.Instance.UpdateHeartUI();

        
        
        if (PlayerPrefs.GetString("bossam_rank") == "F")
        {
            Main_control.Instance.JangJokBal("ID_J_GAME_OVER_BAD");
        }
        else Main_control.Instance.JangJokBal("ID_J_GAME_OVER_GOOD");
    }

    public void ShowHand()
    {
        DOTween.Kill(hand);
        DOTween.Kill(ssam.gameObject.transform);
        ssam.gameObject.transform.SetParent(hand, true);
        ssam.gameObject.transform.localPosition = Vector3.zero;
        hand.DOMoveY(handPos.position.y, 0.5f);
    }

    public void HideHand()
    {
        hand.DOMoveY(handPos.position.y - 5f, 1f);
    }
}
