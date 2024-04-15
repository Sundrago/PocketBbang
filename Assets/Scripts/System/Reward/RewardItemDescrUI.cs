using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemDescrUI : MonoBehaviour
{
    [SerializeField] private Text title, descr;
    [SerializeField] private Image bg;
    [SerializeField] private float offset;
    
    public void Init(string _title, string _descr, Transform targetPos)
    {
        title.text = _title;
        descr.text = _descr;

        title.color = new Color(1, 1, 1, 0);
        descr.color = new Color(1, 1, 1, 0);
        bg.color = new Color(0, 0, 0, 0);

        DOTween.Kill(title);
        DOTween.Kill(title.transform);
        DOTween.Kill(descr);
        DOTween.Kill(bg);
        
        gameObject.transform.position =
            new Vector3(targetPos.position.x, targetPos.position.y + offset, gameObject.transform.position.z);

        title.DOFade(1, 0.5f);
        descr.DOFade(1, 0.5f);
        bg.DOFade(1, 0.5f);
        title.transform.DOScale(Vector3.one, 3f).OnComplete(() =>
        {
            title.DOFade(0, 0.5f);
            descr.DOFade(0, 0.5f);
            bg.DOFade(0, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
        
        gameObject.SetActive(true);
    }
}
