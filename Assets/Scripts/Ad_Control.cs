using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;
using System;

public class Ad_Control : MonoBehaviour
{
    //private RewardedAd rewardedAd;
    public Heart_Control heart;
    public AudioControl audioC;
    public bool dangunMode = false;
    public bool bbobgiMode = false;
    public bool ddukMode = false;
    public DangunControl dangun;
    public Main_control main;
    public BalloonControl balloon;

    public IronSourceAd iron;
    public DdukCtrl dduck;

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        // MobileAds.Initialize(initStatus => { });
        //CreateAndLoadRewardedAd();

        if(!PlayerPrefs.HasKey("adCount"))
        {
            PlayerPrefs.SetInt("adCount", 0);
        }
    }

    /*
    public void CreateAndLoadRewardedAd()
    {
        string adUnitId;
#if UNITY_ANDROID
                adUnitId = "ca-app-pub-2796864814295819/2077600860";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-2796864814295819/6998383922";
        #else
                adUnitId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        this.rewardedAd = new RewardedAd(adUnitId);
        
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args);
        //heart.HeartFull();
        balloon.ShowMsg("광고보기실패 : " + args);
        this.CreateAndLoadRewardedAd();
        audioC.ResumeMusic();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("InterstitialFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
        balloon.ShowMsg("광고로드실패 : " + args);
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        if (bbobgiMode) main.newStore.GetComponent<StoreControl>().bbobgiPanel.GetComponent<BbobgiPanelCtrl>().WathcedAds();
        else if (dangunMode) dangun.ReSetDateTimeData();
        else heart.HeartFull();

        this.CreateAndLoadRewardedAd();
        audioC.ResumeMusic();

        PlayerPrefs.SetInt("adCount", PlayerPrefs.GetInt("adCount") + 1);
        PlayerPrefs.Save();
    }
    */

    public void IronAdsWatched()
    {
        if (bbobgiMode) main.newStore.GetComponent<StoreControl>().bbobgiPanel.GetComponent<BbobgiPanelCtrl>().WathcedAds();
        else if (dangunMode) dangun.ReSetDateTimeData();
        else if (ddukMode) dduck.OpenPanel();
        else heart.HeartFull();

        //this.CreateAndLoadRewardedAd();
        audioC.ResumeMusic();

        PlayerPrefs.SetInt("adCount", PlayerPrefs.GetInt("adCount") + 1);
        PlayerPrefs.Save();
    }

    public void UserChoseToWatchAd()
    {
        dangunMode = false;
        bbobgiMode = false;

        iron.ShowIronAds();
        /*
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            audioC.PauseMusic();
        } else
        {
            this.CreateAndLoadRewardedAd();
            UserChoseToWatchAd();
        }
        */
        //print("is Ad Loaded : " + this.rewardedAd.IsLoaded());
    }

    public void PlayDangunAds()
    {
        dangunMode = true;
        bbobgiMode = false;
        iron.ShowIronAds();
        /*
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            audioC.PauseMusic();
        } */
        //print("is Ad Loaded : " + this.rewardedAd.IsLoaded());
    }

    public void BbobgiAds()
    {
        bbobgiMode = true;
        iron.ShowIronAds();
        /*
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            audioC.PauseMusic();
        }
        */
        //print("is Ad Loaded : " + this.rewardedAd.IsLoaded());
    }

    public void DdukAds()
    {
        ddukMode = true;
        iron.ShowIronAds();
    }
}

