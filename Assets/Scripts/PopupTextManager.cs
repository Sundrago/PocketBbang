using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupTextManager : MonoBehaviour
{
    public static PopupTextManager Instance;

    public delegate void Callback();
    private Callback callbackOK = null;
    private Callback callbackYES = null;
    private Callback callbackNO = null;

    [SerializeField] private Button okay_btn, yes_btn, no_btn;
    [SerializeField] private Text okay_text, yes_text, no_text, msg_text;

    [SerializeField] private Image bgImage;
    [SerializeField] private Transform panel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowYesNoPopup(string msg, Callback yesFunction = null, Callback noFunction = null, string yesText = "네!", string noText = "아니요")
    {
        yes_text.text = yesText;
        no_text.text = noText;
        msg_text.text = msg;
        callbackYES = yesFunction;
        callbackNO = noFunction;
        
        okay_btn.gameObject.SetActive(false);
        yes_btn.gameObject.SetActive(true);
        no_btn.gameObject.SetActive(true);
        
        ShowPanel();
    }
    public void ShowOKPopup(string msg, Callback okFunction=null, string okayText = "확인")
    {
        okay_text.text = okayText;
        msg_text.text = msg;
        callbackOK = okFunction;
        
        okay_btn.gameObject.SetActive(true);
        yes_btn.gameObject.SetActive(false);
        no_btn.gameObject.SetActive(false);
        
        ShowPanel();
    }

    public void HidePanel()
    {
        if (DOTween.IsTweening(panel)) return;
        
        // AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.UI_CLOSE);
        bgImage.DOFade(0, 0.25f);
        panel.DOScale(0.8f, 0.25f).SetEase(Ease.OutExpo);
        panel.DOShakePosition(0.3f, new Vector3(10, 10, 0)).SetEase(Ease.OutQuad);
        panel.DOLocalMoveY(3000, 0.7f)
            .SetDelay(0.15f)
            .SetEase(Ease.InOutExpo)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    public void ShowPanel()
    {
        if (DOTween.IsTweening(panel)) DOTween.Kill(panel);
        if (DOTween.IsTweening(bgImage)) DOTween.Kill(bgImage);

        panel.localScale = new Vector3(0.7f, 0.7f, 1);
        panel.localPosition = Vector3.zero;
        panel.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic);
        
        gameObject.SetActive(true);
        bgImage.DOFade(0.3f, 0.5f);
    }

    public void BtnClicked(int idx)
    {
        if (DOTween.IsTweening(panel)) return;
        // AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.UI_SELECT);
        
        okay_btn.gameObject.SetActive(false);
        yes_btn.gameObject.SetActive(false);
        no_btn.gameObject.SetActive(false);
        
        switch (idx)
        {
            case 0:
                callbackOK?.Invoke();
                break;
            case 1:
                callbackYES?.Invoke();
                break;
            case 2:
                callbackNO?.Invoke();
                break;
        }
        HidePanel();
    }
}
