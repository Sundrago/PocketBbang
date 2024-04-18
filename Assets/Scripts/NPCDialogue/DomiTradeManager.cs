using UnityEngine;

public class DomiTradeManager
{
    private GameManager gameManager;
    public DomiTradeManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Nylon_TryInvestBbang(string inputString, string id_y, string id_n, string parent_id = "Nylon")
    {
        var price = 10000;
        if (PlayerPrefs.GetInt("money") >= price)
            gameManager.PrompterController.AddOption(inputString, parent_id, id_y);
        else
            gameManager.PrompterController.AddOption(inputString, parent_id, id_n);
    }

    public void Nylon_DrawBbang(string id0, float weight0, string id1, float weight1, string id2, float weight2,
        string id3, float weight3)
    {
        var count = PlayerPrefs.GetInt("NylonDrawBbangCount", 0);
        count++;
        PlayerPrefs.SetInt("NylonDrawBbangCount", count);

        var totalWeight = weight0 + weight1 + weight2 + weight3;
        var rnd = Random.Range(0, totalWeight);

        if (rnd < weight0)
            gameManager.PrompterController.AddNextAction("Nylon", id0);
        else if (rnd < weight0 + weight1)
            gameManager.PrompterController.AddNextAction("Nylon", id1);
        else if (rnd < weight0 + weight1 + weight2)
            gameManager.PrompterController.AddNextAction("Nylon", id2);
        else
            gameManager.PrompterController.AddNextAction("Nylon", id3);
    }

    public void Nylon_DrawBbang_f(string id0, float weight0, string id1, float weight1, string id2, float weight2,
        string id3, float weight3)
    {
        var count = PlayerPrefs.GetInt("NylonDrawBbangCount", 0);
        count++;
        PlayerPrefs.SetInt("NylonDrawBbangCount", count);

        var totalWeight = weight0 + weight1 + weight2 + weight3;
        var rnd = Random.Range(0, totalWeight);

        if (rnd < weight0)
            gameManager.PrompterController.AddNextAction("Nylon_f", id0);
        else if (rnd < weight0 + weight1)
            gameManager.PrompterController.AddNextAction("Nylon_f", id1);
        else if (rnd < weight0 + weight1 + weight2)
            gameManager.PrompterController.AddNextAction("Nylon_f", id2);
        else
            gameManager.PrompterController.AddNextAction("Nylon_f", id3);
    }

    public void Nylon_TradeCoin(GameManager.TradeType type, string confirmID, string cancelID, string noMoney = null)
    {
        gameManager.DomiTradePanelController.OpenPanel(type, confirmID, cancelID, noMoney);
    }

    public void Nylon_OpenTradePanel(bool flag)
    {
        if (flag)
            gameManager.DoMiCoinManager.ShowPanel();
        else
            gameManager.DoMiCoinManager.HidePanel();
    }

    public void DialogueCheckMoney(int price, string id_y, string id_n, string inputString, string parent_id)
    {
        if (PlayerPrefs.GetInt("money") >= price)
            gameManager.PrompterController.AddOption(inputString, parent_id, id_y);
        else
            gameManager.PrompterController.AddOption(inputString, parent_id, id_n);
    }
}