using UnityEngine;

public class DebugManager
{
    private GameManager gameManager;

    public DebugManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void OpenDebugPanel()
    {
        if (!gameManager.debugPanel.activeSelf)
        {
            PlayerPrefs.SetInt("debugMode", 1);
            PlayerPrefs.Save();
            gameManager.debugPanel.SetActive(true);
            gameManager.debugData.SetActive(true);
        }
        else
        {
            gameManager.debugPanel.SetActive(false);
            gameManager.debugData.SetActive(false);
        }

        gameManager.debugCount = 0;
    }

    public void CloseDebugPanel()
    {
        gameManager.debugPanel.SetActive(false);
        gameManager.collection.GetComponent<CollectionPanelManager>().debugMode = false;
        gameManager.debugCount = 0;
    }

    public void AddMaple()
    {
        gameManager.AddBBangType(1, "DEBUG", "maple");
    }

    public void AddPurin()
    {
        gameManager.AddBBangType(1, "DEBUG", "purin");
    }

    public void GotoStore(int idx)
    {
        if (gameManager.currentLocation == "bbobgi")
        {
            gameManager.DebugManager.RestartBbobgi(gameManager);
            return;
        }

        UpdateUIElements();
        gameManager.MyStoreType = idx;
        SetupNewStore();
    }

    private void SetupNewStore()
    {
        gameManager.newStore = GameObject.Instantiate((GameObject)gameManager.store_panel, gameManager.storePanelHolder.transform);
        gameManager.newStore.GetComponent<StoreControl>().UpdateStore(gameManager.MyStoreType);
        gameManager.newStore.SetActive(true);
        gameManager.newStore.GetComponent<Animator>().SetTrigger("show");
        gameManager.goBtnText_ui.text = "집으로 돌아가기";
    }

    private void UpdateUIElements()
    {
        gameManager.GetComponent<PlayerStatusManager>().SetHeart(-1);
        gameManager.AudioController.PlayMusic(1);
        gameManager.parkBtn.SetActive(false);
        gameManager.front_panel.SetActive(true);
        gameManager.mainPlayerAnimator.SetTrigger("walk");
        gameManager.mainUIPanel.SetTrigger("hide");
        gameManager.lowerUIPanel.SetTrigger("hide");
        gameManager.currentLocation = "toStore";
        gameManager.parkBtn.SetActive(false);
        gameManager.PrompterController.Reset();
        var str = "" + gameManager.storeCount() + "번째 편의점을 향해 걸어가고 있다.";
        gameManager.PrompterController.AddString("훈이", str);
        gameManager.prompter.GetComponent<Animator>().SetTrigger("show");
        gameManager.prompter.SetActive(true);
        gameManager.currentLocation = "toStore";
    }

    public void RestartBbobgi(GameManager gameManager)
    {
        gameManager.newStore.GetComponent<StoreControl>().bbobgiPanel.bbobgiController.StartGame();
    }

    public void RemoveMoney()
    {
        gameManager.PlayerStatusManager.UpdateMoney(-10000);
    }

    public void SetDebugMode(GameManager gameManager)
    {
        gameManager.PhoneMessageController.SetMsg("디버그 모드를 사용한 이력이 있어 랭킹 시스템에서 제외됩니다. 랭킹을 사용하려면 데이터를 초기화 해주세요.", 1);
        gameManager.debugPanlShowBtn.SetActive(true);
        gameManager.AchievementManager.Autheticate();
    }

    public void AddMoney()
    {
        gameManager.PlayerStatusManager.UpdateMoney(10000);
    }
}