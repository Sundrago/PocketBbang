using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeartIconObject : MonoBehaviour
{
    [SerializeField] private Image frame, fill;

    private bool isEnabled;
    private bool isFull;

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        fill.color = enabled ? Color.white : new Color(0.5f, 0.5f, 0.5f);
    }

    public void Show()
    {
        if (!isEnabled) return;
        fill.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        fill.DOFade(1, 0.3f);
        isFull = true;
    }

    public void Hide()
    {
        if (!isEnabled) return;
        fill.transform.DOScale(Vector3.zero, 0.5f);
        fill.DOFade(0, 0.5f);
        isFull = false;
    }

    public void HeartClicked()
    {
        if (!isEnabled)
        {
            StoreManager.Instance.OpenStoreAt("maxHeart");
            return;
        }

        if (!isFull) SimpleWatchAds.Instance.OpenPhone();
    }

    public void DoFishAnim()
    {
        fill.color = new Color(1, 1, 0);
        gameObject.transform.DOScale(1.25f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
        Show();
    }

    public void EndFishAnim()
    {
        DOTween.Kill(gameObject.transform);
        gameObject.transform.localScale = Vector3.one;
        fill.color = Color.white;
    }
}