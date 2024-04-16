using System;
using Unity.Advertisement.IosSupport;
using UnityEngine;

public class IronSourceAd : MonoBehaviour
{
    [SerializeField] private PhoneMessageController msg;
    [SerializeField] private ADManager ads;

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
    }

    private void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public event Action sentTrackingAuthorizationRequest;

    private void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
    }


    public void ShowIronAds()
    {
#if UNITY_IOS
        Debug.Log(ATTrackingStatusBinding.GetAuthorizationTrackingStatus());
        if ((ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
             ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) & !PlayerPrefs.HasKey("ATT"))
        {
            msg.SetMsg("맞춤형 광고를 설정하시면 관심있을 광고를 위주로 보여줍니다.", 1, "RequestAuthorizationTracking");
            return;
        }
#endif

        Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - ?");
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - True");
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }
    }

    public void RequestAuthorizationTracking()
    {
#if UNITY_IOS
        Debug.Log("RequestAuthorizationTracking");
        IronSourceEvents.onConsentViewDidAcceptEvent += onConsentViewDidAcceptEvent;
        IronSource.Agent.loadConsentViewWithType("pre");
        IronSource.Agent.showConsentViewWithType("pre");
        ATTrackingStatusBinding.RequestAuthorizationTracking();
        sentTrackingAuthorizationRequest?.Invoke();
        PlayerPrefs.SetInt("ATT", 1);
#endif
    }

    // The user pressed the Settings or Next buttons
    private void onConsentViewDidAcceptEvent(string consentViewType)
    {
        ShowIronAds();
    }

    #region AdInfo Rewarded Video

    private void ReardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdOpenedEvent With AdInfo " + adInfo);
    }

    private void ReardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdClosedEvent With AdInfo " + adInfo);
    }

    private void ReardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdAvailable With AdInfo " + adInfo);
    }

    private void ReardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdUnavailable");
    }

    private void ReardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Debug.Log(
            "unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError + "And AdInfo " + adInfo);
    }

    private void ReardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement +
                  "And AdInfo " + adInfo);
    }

    private void ReardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement +
                  "And AdInfo " + adInfo);
    }

    #endregion


    #region RewardedAd callback handlers

    private void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    private void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    //USER WATCHED ADS
    private void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " +
                  ssp.getRewardName());
        ads.IronAdsWatched();
    }

    private void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
    }

    private void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    private void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    private void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() +
                  ", description : " + error.getDescription());
    }

    private void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    #endregion
}