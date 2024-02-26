using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class HardworkingItemUI : MonoBehaviour
{
    [SerializeField] private Image bg, icon, lowerBar;
    [SerializeField] private TextMeshProUGUI title_ui, dayCount_ui;
    [SerializeField, ReadOnly] private int day;
    [SerializeField] private GameObject clearIcon;
    private HardWorkManager.HardworkingStatus status;
    
    [Button]
    public void Init(int _day)
    {
        day = _day;
        if(_day == 0) dayCount_ui.text = "즉시 보상";
        else dayCount_ui.text = _day + "일차";
    }

    [Button]
    public void SetUIStatus(HardWorkManager.HardworkingStatus status)
    {
        switch (status)
        {
            case HardWorkManager.HardworkingStatus.NotReceived:
                bg.color = new Color(0.5f, 0.4f, 0.3f, 0.7f);
                icon.color = new Color(1, 1, 1, 0.2f);
                title_ui.color = new Color(1, 1, 1, 0.5f);
                lowerBar.color = Color.white;
                clearIcon.SetActive(false);
                break;
            case HardWorkManager.HardworkingStatus.Received:
                bg.color = new Color(0.7f, 0.6f, 0.5f);
                icon.color = new Color(1, 1, 1, 0.5f);
                title_ui.color = Color.white;
                lowerBar.color = Color.white;
                clearIcon.SetActive(true);
                break;
            case HardWorkManager.HardworkingStatus.Ready:
                bg.color = new Color(1f, 0.75f, 0.4f);
                icon.color = new Color(1, 1, 1, 1);
                title_ui.color = Color.white;
                lowerBar.color = new Color(1, 0.9f, 0.4f);
                clearIcon.SetActive(false);
                break;
        }
    }
}
