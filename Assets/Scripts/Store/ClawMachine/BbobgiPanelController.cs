using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BbobgiPanelController : MonoBehaviour
{
    [Header("Managers and Controllers")] 
    [SerializeField] public BbobgiController bbobgiController;
    [SerializeField] private BalloonUIManager balloonUIManager;
    [SerializeField] private ADManager adManager;
    [SerializeField] private GameManager gameManager;
    [FormerlySerializedAs("playerHealthManager")] [SerializeField] private PlayerStatusManager playerStatusManager;
    [SerializeField] private AudioController audioController;

    [Header("UI Components")] 
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private Text playCountText;
    [SerializeField] private Button insertMoneyButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text continueText;
    
    private int totalCount, currentCount;

    public bool Reload { get; set; }


    public void OpenGame()
    {
        //LoadBbobgi();
        gameObject.SetActive(true);
        mainPlayer.SetActive(false);
        gameManager.currentLocation = "bbobgiController";
        gameManager.HideBtn();
        gameManager.prompter.GetComponent<Animator>().SetTrigger("hide");

        continueText.text = "돈을 넣어주세요";

        if (Reload)
        {
            bbobgiController.StartGame();
            Reload = false;
            bbobgiController.playing = false;
            bbobgiController.SetBtnActive(false);
        }

        UpdateContinue();
        UpdateBtnSetting();
    }

    public void CloseGame()
    {
        gameObject.SetActive(false);
        mainPlayer.SetActive(true);
        gameManager.currentLocation = "store";
        audioController.PlayMusic(1);

        gameManager.StoreDialogue.StoreEvent("bbobgiStore");
        gameManager.prompter.SetActive(true);
        gameManager.prompter.GetComponent<Animator>().SetTrigger("show");
    }

    public void LoadBbobgi()
    {
        bbobgiController.StartGame();
        Reload = false;
        bbobgiController.playing = false;
        bbobgiController.SetBtnActive(false);
    }

    public void InsertMoney()
    {
        if (PlayerPrefs.GetInt("money") < 1000)
        {
            balloonUIManager.ShowMsg("돈이 부족하다..");
            return;
        }

        if (bbobgiController.playing == false)
        {
            playerStatusManager.UpdateMoney(-1000);
            totalCount = 1;
            currentCount = 1;
            bbobgiController.playing = true;
            bbobgiController.OepnClaw();
            bbobgiController.SetBtnActive(true);
        }

        UpdateBtnSetting();
        UpdateContinue();
    }

    public void GameFinished()
    {
        if (currentCount >= totalCount)
        {
            bbobgiController.playing = false;
        }
        else
        {
            currentCount += 1;
            bbobgiController.playing = true;
            bbobgiController.OepnClaw();
            bbobgiController.SetBtnActive(true);
        }

        UpdateBtnSetting();
        UpdateContinue();
    }

    public void UpdateContinue()
    {
        if (bbobgiController.playing)
        {
            continuePanel.SetActive(false);
            playCountText.GetComponent<Text>().text = "" + currentCount + "/" + totalCount;
        }
        else
        {
            continuePanel.SetActive(true);
            playCountText.GetComponent<Text>().text = "";
        }
    }

    public void UpdateBtnSetting()
    {
        print(bbobgiController.bbangCount);
        if (bbobgiController.bbangCount == 0)
        {
            continueText.text = "포켓볼빵을 모두 뽑았습니다.";
            exitButton.image.color = Color.yellow;
            insertMoneyButton.image.color = Color.white;
            watchAdButton.image.color = Color.white;
            return;
        }

        exitButton.image.color = Color.white;

        if (bbobgiController.playing)
        {
            insertMoneyButton.image.color = Color.white;
            watchAdButton.image.color = Color.white;
            insertMoneyButton.interactable = false;
            watchAdButton.interactable = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("money") < 1000)
            {
                insertMoneyButton.image.color = Color.white;
                watchAdButton.image.color = Color.yellow;
            }
            else
            {
                insertMoneyButton.image.color = Color.yellow;
                watchAdButton.image.color = Color.white;
            }

            insertMoneyButton.interactable = true;
            watchAdButton.interactable = true;
        }

        AudioCtrl();
    }

    public void AudioCtrl()
    {
        if (bbobgiController.playing)
            audioController.PlayMusic(6);
        else
            audioController.PlayMusic(7);
    }

    public void WatchAds()
    {
        adManager.PlayAds(ADManager.AdsType.bbogi);
        audioController.StopMusic();
    }

    public void WathcedAds()
    {
        audioController.PlayMusic(6);
        totalCount = 3;
        currentCount = 1;
        bbobgiController.playing = true;
        bbobgiController.OepnClaw();
        bbobgiController.SetBtnActive(true);
        UpdateBtnSetting();
        UpdateContinue();
    }
}