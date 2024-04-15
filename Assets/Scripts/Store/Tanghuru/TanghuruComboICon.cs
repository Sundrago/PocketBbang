using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TanghuruComboICon : MonoBehaviour
{
    [SerializeField] private Text title_text, combo_text;

    public void Init(Vector3 pos, int comboCount)
    {
        gameObject.transform.position = pos;
        combo_text.text = comboCount.ToString();

        title_text.transform.localScale = Vector3.one * 0.5f;
        combo_text.transform.localScale = Vector3.one * 0.5f;
        title_text.color = new Color(title_text.color.r, title_text.color.g, title_text.color.b,0);
        combo_text.color = new Color(combo_text.color.r, combo_text.color.g, combo_text.color.b,0);
        
        combo_text.transform.DOScale(1.2f, 1.5f).SetEase(Ease.OutExpo);
        title_text.transform.DOScale(1.2f, 1.5f).SetEase(Ease.OutExpo);
        combo_text.DOFade(0.8f, 0.75f).SetEase(Ease.OutExpo);
        title_text.DOFade(1f, 0.75f).SetEase(Ease.OutExpo);
        combo_text.DOFade(0, 0.75f).SetDelay(0.75f).SetEase(Ease.OutExpo);
        title_text.DOFade(0, 0.75f).SetDelay(0.75f).SetEase(Ease.OutExpo)
            .OnComplete(()=>Destroy(gameObject));
        gameObject.SetActive(true);
    }
    
    public void InitFail(Vector3 pos)
    {
        gameObject.transform.position = pos;

        title_text.transform.localScale = Vector3.one * 0.5f;
        title_text.color = new Color(title_text.color.r, title_text.color.g, title_text.color.b,0);

        title_text.transform.DOPunchPosition(new Vector3(5, 5, 1), 1.5f);
        title_text.transform.DOScale(1.2f, 2f).SetEase(Ease.OutExpo);
        title_text.DOFade(1f, 1f).SetEase(Ease.OutExpo);
        title_text.DOFade(0, 1f).SetDelay(1f).SetEase(Ease.OutExpo)
            .OnComplete(()=>Destroy(gameObject));
        gameObject.SetActive(true);
    }
}
