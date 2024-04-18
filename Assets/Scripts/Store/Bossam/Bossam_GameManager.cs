using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bossam_GameManager : MonoBehaviour
{
    public static Bossam_GameManager Instance { get; private set; }
    [SerializeField] private MiniGameEndScoreUIManager endScoreUI;
    [SerializeField] private Bossam_StageManager bosamChar;
    [SerializeField] private TanghuruComboICon comboFX, failFX;
    [SerializeField] private Transform comboFxPos;
    [SerializeField] private Text score_text, time_text;
    [SerializeField] private GameObject tutorialPanel, scorePanel;
    [SerializeField] private List<Bossam_StageManager> bossamChar_prefab = new();

    [SerializeField] private Bossam_ssam ssam;
    [SerializeField] private List<GameObject> GameObjSetActiveFalse = new();
    [SerializeField] private Transform chaholder;
    [SerializeField] private Transform hand, handPos;

    private List<GameObject> actives;
    private int beforeAudioIdx;
    private int comboConut;

    private int score;
    private float startTime;

    private gameStatus status = gameStatus.score;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (status != gameStatus.playing) return;

        var timeLeft = 30f - (Time.time - startTime);
        timeLeft = MathF.Round(timeLeft * 10f) / 10f;
        time_text.text = timeLeft.ToString();

        if (timeLeft <= 0) SetGameStatus(gameStatus.score);
    }

    public void StartGame()
    {
        SetGameStatus(gameStatus.playing);
    }

    private void SetGameStatus(gameStatus _status)
    {
        if (status == _status) return;
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
                endScoreUI.ShowScore(score);
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
        if (bosamChar == null) return;

        for (var i = 0; i < bosamChar.chardata.Count; i++)
        {
            if (!bosamChar.chardata[i].mouthOpen) continue;
            var dist = Vector2.Distance(pos, bosamChar.chardata[i].character.center.position);
            if (dist < 1)
            {
                bosamChar.GotBossam(i);
                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.win);
                comboConut += 1;
                score += 100 * (10 + comboConut) / 10;
                UpdateScoreUI();
                var comboFXN = Instantiate(comboFX, gameObject.transform);
                comboFXN.Init(comboFxPos.position, comboConut);
                return;
            }
        }

        AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.loose);
        var comboFAILFX = Instantiate(failFX, gameObject.transform);
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
        var rnd = Random.Range(0, bossamChar_prefab.Count);
        bosamChar = Instantiate(bossamChar_prefab[rnd], chaholder);
        bosamChar.AnimIn();
        bosamChar.gameObject.SetActive(true);
    }

    public void WatchAdsAndRestartBtnClicked()
    {
        ADManager.Instance.PlayAds(ADManager.AdsType.bossam);
    }

    public void WathcedAds()
    {
        PlayerPrefs.SetInt("Minigame_adCount", PlayerPrefs.GetInt("Minigame_adCount", 0) + 1);
        endScoreUI.HideScore();
        tutorialPanel.SetActive(true);
    }

    [Button]
    public void EnterGame()
    {
        actives = new List<GameObject>();
        foreach (var obj in GameObjSetActiveFalse)
            if (obj.activeSelf)
                actives.Add(obj);

        foreach (var obj in actives) obj.SetActive(false);
        gameObject.SetActive(true);
        tutorialPanel.SetActive(true);
        endScoreUI.gameObject.SetActive(false);

        PlauMusic();

        void PlauMusic()
        {
            beforeAudioIdx = AudioController.Instance.currentPlaying;
            AudioController.Instance.PlayMusic(8);
        }

        PlayerPrefs.SetInt("Minigame_adCount", 0);
        SetGameStatus(gameStatus.tutorial);
    }

    [Button]
    public void ExitGame()
    {
        foreach (var obj in actives) obj.SetActive(true);
        gameObject.SetActive(false);

        StopMusic();

        void StopMusic()
        {
            AudioController.Instance.PlayMusic(beforeAudioIdx);
        }

        GameManager.Instance.lowerUIPanel.GetComponent<Animator>().SetTrigger("hide");
        PlayerHealthManager.Instance.UpdateHeartUI();


        if (PlayerPrefs.GetString("bossam_rank") == "F")
            GameManager.Instance.JangJokBalDialogue.JangJokBal("ID_J_GAME_OVER_BAD");
        else GameManager.Instance.JangJokBalDialogue.JangJokBal("ID_J_GAME_OVER_GOOD");
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

    private enum gameStatus
    {
        tutorial,
        playing,
        score
    }
}