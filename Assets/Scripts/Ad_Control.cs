using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;
using System;

public class Ad_Control : MonoBehaviour
{
    //private RewardedAd rewardedAd;
    [SerializeField] Main_control main;
    [SerializeField] BalloonControl balloon;
    [SerializeField] Heart_Control heart;
    [SerializeField] AudioControl audioC;
    [SerializeField] DangunControl dangun;

    [SerializeField] IronSourceAd iron;
    [SerializeField] DdukCtrl dduck;

    public bool dangunMode = false;
    public bool bbobgiMode = false;
    public bool ddukMode = false;

    public void Start()
    {
        if(!PlayerPrefs.HasKey("adCount"))
        {
            PlayerPrefs.SetInt("adCount", 0);
        }
    }

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
    }

    public void PlayDangunAds()
    {
        dangunMode = true;
        bbobgiMode = false;
        iron.ShowIronAds();
    }

    public void BbobgiAds()
    {
        bbobgiMode = true;
        iron.ShowIronAds();
    }

    public void DdukAds()
    {
        ddukMode = true;
        iron.ShowIronAds();
    }
}
