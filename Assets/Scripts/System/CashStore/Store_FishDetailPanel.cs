using DG.Tweening;
using MyUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_FishDetailPanel : MonoBehaviour
{
    [SerializeField] private Text title_ui, descr_ui, count_ui;
    [SerializeField] private TextMeshProUGUI price_ui;
    [SerializeField] private Image bg, item_ui;
    [SerializeField] private Button btnUp, btnDown;
    [SerializeField] private Transform panel;
    [SerializeField] private Store_FishManager manager;

    private int count;
    private Store_FishManager.StoreFishData data;

    public void Init(Store_FishManager.StoreFishData _data)
    {
        if (gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;

        data = _data;
        title_ui.text = data.name;
        descr_ui.text = Converter.KoreanParticle(data.name + "을/를 구매할까요?");
        item_ui.sprite = data.Sprite;
        count = 1;
        UpdatePrice();

        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.localPosition = new Vector3(0, 50, 0);
        panel.transform.eulerAngles = Vector3.zero;

        panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.6f, 0.2f);

        gameObject.SetActive(true);
    }

    public void AdjustCountBtnClicked(int amt)
    {
        count += amt;
        UpdatePrice();
    }

    public void ItemBtnClicked()
    {
        ItemInfoUIPanel.Instance.OpenPanel(data.itemCode);
    }

    private void UpdatePrice()
    {
        count_ui.text = count.ToString();
        price_ui.text = (count * data.price).ToString();

        btnDown.interactable = count > 1;
        btnUp.interactable =
            (count + 1) * data.price <= PlayerHealthManager.Instance.GetAmount(PlayerHealthManager.MoneyType.Diamond)
            && count + 1 <= 10;
    }

    public void ConfirmBtnClicked()
    {
        manager.PurchaseItem(data, count);
        CloseBtnClicked();
    }

    public void CloseBtnClicked()
    {
        if (!gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;

        bg.DOFade(0f, 0.3f);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic)
            .OnComplete(() => { gameObject.SetActive(false); });
    }
}