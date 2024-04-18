using UnityEngine;

public class PlayerPreferenceManager
{
    private GameManager gameManager;

    public PlayerPreferenceManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void InitPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("albaCustomerCount"))
        {
            PlayerPrefs.SetInt("albaCustomerCount", 0);
            PlayerPrefs.SetInt("albaTime", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("ChaName") | !PlayerPrefs.HasKey("ChaIdx"))
        {
            if (!PlayerPrefs.HasKey("ChaName")) PlayerPrefs.SetString("ChaName", "훈이");
            if (!PlayerPrefs.HasKey("ChaIdx")) PlayerPrefs.SetInt("ChaName", 0);
            PlayerPrefs.Save();
            gameManager.chaSelectPanel.GetComponent<CharacterSelector>().OpenChaSelect();
        }

        //DataUpdate MatDongSan
        if (!PlayerPrefs.HasKey("Matdongsan"))
        {
            PlayerPrefs.SetInt("Matdongsan", Mathf.RoundToInt(PlayerPrefs.GetInt("storeCount") / 6.5f));
            PlayerPrefs.Save();
        }

        //DataUpdate ShuttleCount
        if (!PlayerPrefs.HasKey("ShuttleCount"))
        {
            PlayerPrefs.SetInt("ShuttleCount", Mathf.RoundToInt(PlayerPrefs.GetInt("totalBbangCount") / 11f));
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("new_choco")) PlayerPrefs.SetInt("new_choco", PlayerPrefs.GetInt("bbang"));
        if (!PlayerPrefs.HasKey("new_strawberry")) PlayerPrefs.SetInt("new_strawberry", 0);
        if (!PlayerPrefs.HasKey("new_hot")) PlayerPrefs.SetInt("new_hot", 0);
        if (!PlayerPrefs.HasKey("albaExp")) PlayerPrefs.SetInt("albaExp", 0);
        if (!PlayerPrefs.HasKey("debugMode")) PlayerPrefs.SetInt("debugMode", 0);
        
        if (!PlayerPrefs.HasKey("myRealTotalCard"))
        {
            gameManager.CollectionPanelManager.GetCount();
            PlayerPrefs.SetInt("myRealTotalCard", PlayerPrefs.GetInt("myTotalCards"));
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetInt("myTotalCards") > PlayerPrefs.GetInt("myRealTotalCard"))
        {
            PlayerPrefs.SetInt("myRealTotalCard", PlayerPrefs.GetInt("myTotalCards"));
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbobgiCount")) PlayerPrefs.SetInt("bbobgiCount", 0);
        if (!PlayerPrefs.HasKey("lowDataMode"))
        {
#if UNITY_IPHONE
            PlayerPrefs.SetInt("lowDataMode", 0);
#elif UNITY_ANDROID
            PlayerPrefs.SetInt("lowDataMode", 1);
#endif
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("muteAudio"))
        {
            PlayerPrefs.SetInt("muteAudio", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("yull_friend"))
        {
            PlayerPrefs.SetInt("yull_friend", 0);
            PlayerPrefs.Save();
        }
    }
}