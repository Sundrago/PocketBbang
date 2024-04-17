using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Store_TabUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] tabs;
    [SerializeField] private GameObject[] panels;

    [Button]
    public void BtnClicked(int idx)
    {
        for (var i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == idx);
            SelectTab(tabs[i], i == idx);
        }
    }

    private void SelectTab(RectTransform tab, bool isSelected)
    {
        if (isSelected)
        {
            tab.DOAnchorPosY(-110, 0.4f).SetEase(Ease.OutExpo);
            tab.gameObject.GetComponent<Image>().DOColor(new Color(1f, 0.5f, 0.45f), 0.4f);
        }
        else
        {
            tab.DOAnchorPosY(-140, 0.3f).SetEase(Ease.OutCirc);
            tab.gameObject.GetComponent<Image>().DOColor(new Color(0.6f, 0.45f, 0.4f), 0.3f);
        }
    }
}