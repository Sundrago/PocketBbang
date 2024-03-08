using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SimpleWatchAds : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Transform phone;
    public static SimpleWatchAds Instance;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPhone()
    {
        if(gameObject.activeSelf) return;
        if(DOTween.IsTweening(phone.transform)) return;
        
        phone.transform.localPosition = new Vector3(0, -2200, 0);
        bg.DOFade(0.3f, 1f);
        phone.DOLocalMove(new Vector3(0, -300, 0), 1f).SetEase(Ease.OutExpo);
        gameObject.SetActive(true);
    }

    public void ClosePhone()
    {
        if(!gameObject.activeSelf) return;
        if(DOTween.IsTweening(phone.transform)) return;
        
        bg.DOFade(0f, 1f);
        phone.DOLocalMove(new Vector3(0, -2200, 0), 1f).SetEase(Ease.InOutExpo)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
    
    public void WatchAds()
    {
        if(Heart_Control.Instance.IsHeartFull()) BalloonControl.Instance.ShowMsg("하트가 가득 차있다!");
        ClosePhone();
        Ad_Control.Instance.PlayAds(Ad_Control.AdsType.heart);
    }
}
