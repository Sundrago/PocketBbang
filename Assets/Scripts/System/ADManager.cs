using UnityEngine;
//using GoogleMobileAds.Api;

public class ADManager : MonoBehaviour
{
    public enum AdsType
    {
        heart,
        dangun,
        bbogi,
        dduk,
        tanghuru,
        bossam,
        diamondStore,
        bbangShuttle
    }

    public static ADManager Instance;
    [SerializeField] private GameManager main;
    [SerializeField] private BalloonUIManager balloon;
    [SerializeField] private PlayerHealthManager heart;
    [SerializeField] private AudioController audioC;
    [SerializeField] private DangunManager dangun;

    [SerializeField] private IronSourceAd iron;
    [SerializeField] private DdukMinigameManager dduck;
    [SerializeField] private TanghuruGameManager tanguru;
    [SerializeField] private Bossam_GameManager bossam;
    [SerializeField] private StoreDiamondWatchAdsPanel diamondWatchAdsPanel;

    private AdsType adsType;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (!PlayerPrefs.HasKey("adCount")) PlayerPrefs.SetInt("adCount", 0);

#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("Ads", "adCount", PlayerPrefs.GetInt("adCount",0));
#endif
    }

    public void PlayAds(AdsType type)
    {
        if (!IronSource.Agent.isRewardedVideoAvailable())
        {
            if (PlayerPrefs.GetInt("debugMode") == 1)
            {
                adsType = type;
                IronAdsWatched();
                return;
            }

            balloon.ShowMsg("광고를 로드하지 못했습니다..");
            return;
        }

        audioC.PauseMusic();
        adsType = type;
        iron.ShowIronAds();
    }

    public void AdFailed()
    {
        balloon.ShowMsg("오류가 발생했습니다.");
        audioC.ResumeMusic();
    }

    public void IronAdsWatched()
    {
        switch (adsType)
        {
            case AdsType.heart:
                heart.HeartFull();
                break;
            case AdsType.dangun:
                dangun.ReSetDateTimeData();
                break;
            case AdsType.bbogi:
                main.newStore.GetComponent<StoreControl>().bbobgiPanel.GetComponent<BbobgiPanelController>()
                    .WathcedAds();
                break;
            case AdsType.dduk:
                dduck.OpenPanel();
                break;
            case AdsType.tanghuru:
                tanguru.WathcedAds();
                break;
            case AdsType.bossam:
                bossam.WathcedAds();
                break;
            case AdsType.diamondStore:
                PopupTextManager.Instance.ShowOKPopup("보상이 도착했어요!", () => { diamondWatchAdsPanel.WatchedAd(); },
                    "보상 받기");
                break;
            case AdsType.bbangShuttle:
                PlayerHealthManager.Instance.ReduceTime();
                break;
        }
        //
        // if (bbobgiMode)
        //     main.newStore.GetComponent<StoreControl>().bbobgiPanelController.GetComponent<BbobgiPanelController>().WathcedAds();
        // else if (dangunMode)
        //     dangun.ReSetDateTimeData();
        // else if (ddukMode)
        //     dduck.OpenPanel();
        // else
        //     heart.HeartFull();
        //this.CreateAndLoadRewardedAd();

        audioC.ResumeMusic();

#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("Ads", "WatchAds", adsType.ToString());
#endif
        PlayerPrefs.SetInt("adCount", PlayerPrefs.GetInt("adCount") + 1);
        PlayerPrefs.Save();
    }

    // public void UserChoseToWatchAd()
    // {
    //     dangunMode = false;
    //     bbobgiMode = false;
    //
    //     iron.ShowIronAds();
    // }
    //
    // public void PlayDangunAds()
    // {
    //     dangunMode = true;
    //     bbobgiMode = false;
    //     iron.ShowIronAds();
    // }
    //
    // public void BbobgiAds()
    // {
    //     bbobgiMode = true;
    //     iron.ShowIronAds();
    // }
    //
    // public void DdukAds()
    // {
    //     ddukMode = true;
    //     iron.ShowIronAds();
    // }
}