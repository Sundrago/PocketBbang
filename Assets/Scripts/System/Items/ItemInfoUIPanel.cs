using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUIPanel : MonoBehaviour
{
    [SerializeField] private Image bg, itemImage;
    [SerializeField] private Text title, descr;
    [SerializeField] private Transform panel;
    public static ItemInfoUIPanel Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPanel(int idx)
    {
        if (!ItemDataManager.Instacne.ItemDatas.ContainsKey(idx)) return;
        if (DOTween.IsTweening(panel)) return;

        gameObject.SetActive(true);
        StartCoroutine(RetrieveData(idx));
    }

    public void ClosePanel()
    {
        if (DOTween.IsTweening(panel)) return;

        DOTween.Kill(panel);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic);

        DOTween.Kill(bg);
        bg.DOFade(0f, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
    }

    private IEnumerator RetrieveData(int idx)
    {
        itemImage.sprite = ItemDataManager.Instacne.ItemDatas[idx].sprite;
        title.text = ItemDataManager.Instacne.ItemDatas[idx].name;
        descr.text = ItemDataManager.Instacne.ItemDatas[idx].descr;

        DOTween.Kill(bg);
        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.65f, 0.3f);

        DOTween.Kill(panel);
        panel.localPosition = new Vector3(0, -2000);
        panel.eulerAngles = new Vector3(0, 0, 60);
        panel.DOLocalMoveY(30, 0.3f).SetEase(Ease.OutExpo);
        panel.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutBack);
        yield return null;
    }
}