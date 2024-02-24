using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_DiamondDetailInfo : MonoBehaviour
{
    [SerializeField] private Text descr_ui;
    [SerializeField] private TextMeshProUGUI price_ui, amt0, amt1;
    [SerializeField] private Image bg;
    [SerializeField] private Transform panel;
    [SerializeField] private Store_DiamondManager manager;
    [SerializeField] private GameObject bonusItem, plusIcon;
    [SerializeField] private Transform shineFX;
    
    private JsonData.StorePackageData data;
    private int idx;
    
    [Button]
    public void Init(int _idx, int amount, int price, bool isBonus)
    {
        if(DOTween.IsTweening(panel)) return;

        idx = _idx;
        price_ui.text = "\u20a9" + MyUtility.Converter.IntToCommaSeporatedString(price);
        descr_ui.text = "다이아몬드\n" +  MyUtility.Converter.IntToCommaSeporatedString(amount) + "개를 구매할까요?";
        
        amt0.text = MyUtility.Converter.IntToCommaSeporatedString(amount) + "개";
        amt1.text = MyUtility.Converter.IntToCommaSeporatedString(amount) + "개";
        
        gameObject.SetActive(true);
        plusIcon.SetActive(false);
        bonusItem.SetActive(false);
        
        if(isBonus) StartCoroutine(LoadRewardItems(amount));

        
        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.localPosition = new Vector3(0, 50, 0);
        panel.transform.eulerAngles = Vector3.zero;
        
        panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.6f, 0.2f);
        
    }
    
    private IEnumerator LoadRewardItems(int amount)
    {
        plusIcon.SetActive(true);
        bonusItem.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(0.2f);
        bonusItem.gameObject.SetActive(true);
        bonusItem.transform.DOPunchScale(Vector3.one*0.1f, 0.5f, 5);
        shineFX.localPosition = new Vector3(-300, 0, 0);
        shineFX.DOLocalMoveX(350, 1f).SetEase(Ease.OutQuint);
        DOVirtual.Int(amount, amount * 2, 2f, (x) =>
        {
            descr_ui.text = "다이아몬드\n" +  MyUtility.Converter.IntToCommaSeporatedString(x) + "개를 구매할까요?";
        }).SetEase(Ease.OutExpo);
    }
    
    public void ConfirmBtnClicked()
    {
        manager.RequestPurchase(idx);
        CloseBtnClicked();
    }
    public void CloseBtnClicked()
    {
        if(!gameObject.activeSelf) return;
        if(DOTween.IsTweening(panel)) return;

        bg.DOFade(0f, 0.3f);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
