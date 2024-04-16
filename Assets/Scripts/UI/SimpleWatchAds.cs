using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SimpleWatchAds : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Transform phone;
    public static SimpleWatchAds Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPhone()
    {
        if (gameObject.activeSelf) return;
        if (DOTween.IsTweening(phone.transform)) return;

        phone.transform.localPosition = new Vector3(0, -2200, 0);
        bg.DOFade(0.3f, 1f);
        phone.DOLocalMove(new Vector3(0, -300, 0), 1f).SetEase(Ease.OutExpo);
        gameObject.SetActive(true);
    }

    public void ClosePhone()
    {
        if (!gameObject.activeSelf) return;
        if (DOTween.IsTweening(phone.transform)) return;

        bg.DOFade(0f, 1f);
        phone.DOLocalMove(new Vector3(0, -2200, 0), 1f).SetEase(Ease.InOutExpo)
            .OnComplete(() => { gameObject.SetActive(false); });
    }

    public void WatchAds()
    {
        if (PlayerHealthManager.Instance.IsHeartFull()) BalloonUIManager.Instance.ShowMsg("하트가 가득 차있다!");
        ClosePhone();
        ADManager.Instance.PlayAds(ADManager.AdsType.heart);
    }
}