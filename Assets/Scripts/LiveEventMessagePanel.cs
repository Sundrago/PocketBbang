using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class LiveEventMessagePanel : MonoBehaviour
{
    public static LiveEventMessagePanel Instance;
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    [Button]
    public void ShowEventPanel()
    {
        pages[0].SetActive(true);
        gameObject.transform.localPosition = new Vector3(-1500, -2000);
        gameObject.transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutExpo);
        gameObject.SetActive(true);
    }
    
    [Button]
    public void HideEventPanel()
    {
        gameObject.transform.DOLocalMove(new Vector3(1500, -2000, 0), 1f).SetEase(Ease.OutExpo)
            .OnComplete(()=>gameObject.SetActive(false));
    }
}
