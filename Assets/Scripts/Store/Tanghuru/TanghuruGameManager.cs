using System.Collections.Generic;
using DG.Tweening;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TanghuruGameManager : MonoBehaviour
{
    public static TanghuruGameManager Instance { get; private set; }
    [SerializeField] private TanghuruStick tanghuruStick;
    [FormerlySerializedAs("endScoreUI")] [FormerlySerializedAs("EndScore")] [SerializeField]
    private MiniGameEndScoreUIManager miniGameEndScoreUIManager;

    [SerializeField] public List<Sprite> fruits_sprite = new();
    [SerializeField] private List<GameObject> fruits_btn = new();
    [SerializeField] private List<TanghuruRequestObj> requests = new();
    [SerializeField] private List<Tanghuru_cutomer> customers = new();
    [SerializeField] private List<GameObject> GameObjSetActiveFalse = new();
    
    [SerializeField] private Text scoreText, timerText;
    [SerializeField] private Image fruitFX;
    [SerializeField] private Transform fxHolder, posA, posB, posC;
    [SerializeField] private TanghuruComboICon combo_prefab, fail_prefab;
    [SerializeField] private Transform trash_btn;
    [SerializeField] private Transform stick_holder_mask;
    [SerializeField] private GameObject characterAnimationA, characterAnimationB, tutorialPanel;
    [SerializeField] private int twinObjCount;

    private List<GameObject> actives;
    private int beforeAudioIdx;

    private int comboCount;
    private int levelCount;
    private int score;
    private float startTime;
    private gameStatus status;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (status == gameStatus.tutorial || status == gameStatus.score) return;
        var timePast = Time.time - startTime;
        if (timePast > 30f)
        {
            if (status != gameStatus.waitForTwinToEnd) AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.whislte_start);
            status = gameStatus.waitForTwinToEnd;
            if (twinObjCount == 0 || timePast > 35f) EndGame();
            return;
        }

        timerText.text = Mathf.Round((30f - timePast) * 10) / 10f + "초";
    }

    private void UpdateCharacterAnim()
    {
        if (characterAnimationA.gameObject.activeSelf)
        {
            characterAnimationA.SetActive(false);
            characterAnimationB.SetActive(true);
        }
        else
        {
            characterAnimationA.SetActive(true);
            characterAnimationB.SetActive(false);
        }
    }

    public void FruitBtnClicked(int idx)
    {
        UpdateCharacterAnim();
        if (status != gameStatus.playing) return;
        var fruitCount = tanghuruStick.GetFruitCount();
        if (fruitCount > 3) return;

        var targetObj = tanghuruStick.GetFruitsTargetTransform();
        if (targetObj == null) return;

        var fruit = Instantiate(fruitFX, fxHolder);
        fruit.transform.position = fruits_btn[idx].transform.position;
        fruit.gameObject.SetActive(true);
        fruit.sprite = fruits_sprite[idx];

        targetObj.transform.localScale = Vector3.zero;

        fruit.transform.DOMove(targetObj.transform.position, 0.2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                DOTween.Kill(tanghuruStick.gameObject.transform);
                targetObj.transform.localScale = Vector3.one;
                targetObj.gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
                if (fruitCount == 3) SugarClicked();
                Destroy(fruit);
            });
        tanghuruStick.AddFruits(fruits_sprite[idx], idx);
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
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
        miniGameEndScoreUIManager.gameObject.SetActive(false);

        PlauMusic();

        void PlauMusic()
        {
            beforeAudioIdx = AudioController.Instance.currentPlaying;
            AudioController.Instance.PlayMusic(8);
        }

        PlayerPrefs.SetInt("Minigame_adCount", 0);
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

        if (GameManager.Instance.albaMode)
        {
            if (PlayerPrefs.GetString("tanghuru_rank") == "F")
            {
                GameManager.Instance.PrompterController.AddString("왕형", "구태여 찾아와 일거리를 만드는구려.");
                GameManager.Instance.PrompterController.AddString("왕형", "다음번에는 조금 더 일을 잘 해주었으면 좋겠소.");
                GameManager.Instance.storeOutAction = "일을 너무 못했다. 다음번에는 탕후루를 더 열심히 만들어보자...";
                GameManager.Instance.PrompterController.AddNextAction("main", "alba_end");
            }
            else
            {
                GameManager.Instance.PrompterController.AddString("왕형", "바쁜 와중에 찾아와주어 고마웠소.");
                GameManager.Instance.PrompterController.AddString("왕형", "우리 가게는 언제나 그대를 기다리고 있으니 또 찾아와 주시오.");
                GameManager.Instance.storeOutAction = "탕후루는 맛도 달달하고 알바비도 달달하다. 다음에 또 알바하러 오자.";
                GameManager.Instance.PrompterController.AddNextAction("main", "alba_end");
            }
        }
        else
        {
            GameManager.Instance.TanghuruDialogue.Tanghuru("ID_T_GAME_OVER_LINK");
        }
    }

    public void StartGame()
    {
        Application.targetFrameRate = 60;
        score = 0;
        comboCount = 0;
        levelCount = 0;
        UpdateScore(0);
        twinObjCount = 0;

        UpdateRequestStick();
        tanghuruStick.Init();
        startTime = Time.time;
        status = gameStatus.playing;
        miniGameEndScoreUIManager.HideScore();
        AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.whislte_start);
        tutorialPanel.SetActive(false);
    }

    public void EndGame()
    {
        status = gameStatus.score;
        miniGameEndScoreUIManager.ShowScore(score);
    }

    private void UpdateScore(int amt)
    {
        score += amt;
        scoreText.text = "" + score;
    }

    private void UpdateRequestStick()
    {
        foreach (var customer in customers) customer.SetAnimIn();
        foreach (var requestStick in requests) requestStick.MakeNewOrder(levelCount);
    }

    // private void GetNewRandomStick(TanghuruStick tanghuruStick)
    // {
    //     tanghuruStick.Init();
    //     for (int i = 0; i < 4; i++)
    //     {
    //         int rnd = Random.Range(0, fruits_sprite.Count);
    //         tanghuruStick.AddFruits(fruits_sprite[rnd], rnd);
    //     }
    // }

    public void SugarClicked()
    {
        if (tanghuruStick.GetFruitCount() == 0) return;
        var id = tanghuruStick.GetIds();

        var retimeFactor = Mathf.Lerp(0.9f, 0.25f, (comboCount + 1) / 40f);

        var finished = Instantiate(tanghuruStick, gameObject.transform);
        twinObjCount += 1;
        finished.transform.position = tanghuruStick.transform.position;
        finished.ids = tanghuruStick.ids;

        finished.transform.DOMove(posA.position, 0.3f * retimeFactor);
        finished.transform.DORotateQuaternion(posA.rotation, 0.3f * retimeFactor)
            .OnComplete(() => { finished.transform.SetParent(stick_holder_mask, true); });
        finished.transform.DOMove(posB.position, 0.3f * retimeFactor)
            .SetDelay(0.3f * retimeFactor)
            .OnComplete(() => { finished.Icing(); });
        finished.transform.DOMove(posC.position, 0.3f * retimeFactor)
            .SetDelay(0.6f * retimeFactor);
        finished.transform.DORotateQuaternion(posC.rotation, 0.5f * retimeFactor)
            .SetDelay(0.6f * retimeFactor)
            .OnComplete(() =>
            {
                foreach (var requestStick in requests)
                {
                    if (requestStick.finished) continue;
                    if (id == requestStick.GetIds())
                    {
                        finished.transform.DOMove(requestStick.transform.position, 0.5f * retimeFactor)
                            .SetEase(Ease.InOutExpo);
                        finished.transform.DORotateQuaternion(requestStick.transform.rotation, 0.5f * retimeFactor)
                            .SetEase(Ease.InOutExpo)
                            .OnComplete(() =>
                            {
                                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.win);
                                comboCount += 1;
                                var combo = Instantiate(combo_prefab, gameObject.transform);
                                combo.Init(customers[requests.IndexOf(requestStick)].transform.position, comboCount);
                                customers[requests.IndexOf(requestStick)].SetAnimOut();

                                DOTween.Kill(requestStick.gameObject.transform);
                                requestStick.transform.localScale = Vector3.one;
                                requestStick.gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f * retimeFactor);
                                requestStick.CompleteOrder();
                                UpdateScore(100 + comboCount * 10);
                                RequestFinishCheck();
                                Destroy(finished.gameObject);
                                twinObjCount -= 1;
                                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                            });
                        return;
                    }
                }

                finished.transform.DOPunchScale(Vector3.one * 0.5f, 0.75f * retimeFactor)
                    .OnComplete(() =>
                    {
                        finished.transform.DOScale(Vector3.zero, 0.3f * retimeFactor)
                            .OnComplete(() =>
                            {
                                Destroy(finished.gameObject);
                                twinObjCount -= 1;
                            });
                    });
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                comboCount = 0;
                var combo = Instantiate(fail_prefab, gameObject.transform);
                combo.InitFail(finished.transform.position);
                AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.loose);
                UpdateScore(-100);
            });


        tanghuruStick.Init();
    }

    public void RequestFinishCheck()
    {
        var flag = true;
        foreach (var requestStick in requests)
            if (requestStick.finished == false)
            {
                flag = false;
                break;
            }

        if (flag)
        {
            levelCount += 1;
            UpdateRequestStick();
        }
    }

    public void TrashBtnClikced()
    {
        if (tanghuruStick.GetFruitCount() == 0) return;

        var retimeFactor = Mathf.Lerp(1, 0.25f, (comboCount + 1) / 50f);

        var finished = Instantiate(tanghuruStick, gameObject.transform);
        twinObjCount += 1;
        finished.transform.position = tanghuruStick.transform.position;
        finished.ids = tanghuruStick.ids;

        finished.transform.DOMove(trash_btn.position, 0.5f * retimeFactor).SetEase(Ease.OutExpo);
        finished.transform.DORotateQuaternion(posA.rotation, 0.5f * retimeFactor).SetEase(Ease.OutExpo);
        finished.transform.DOScale(0, 0.75f * retimeFactor)
            .OnComplete(() =>
            {
                Destroy(finished);
                twinObjCount -= 1;
            }).SetEase(Ease.InOutCirc);

        tanghuruStick.Init();
    }

    public void WatchAdsAndRestartBtnClicked()
    {
        PlayerPrefs.SetInt("Minigame_adCount", PlayerPrefs.GetInt("Minigame_adCount", 0) + 1);
        ADManager.Instance.PlayAds(ADManager.AdsType.tanghuru);
    }

    public void WathcedAds()
    {
        miniGameEndScoreUIManager.HideScore();
        tutorialPanel.SetActive(true);
    }

    private enum gameStatus
    {
        tutorial,
        playing,
        waitForTwinToEnd,
        score
    }
}