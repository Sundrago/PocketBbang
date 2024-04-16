using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_fish_item : MonoBehaviour
{
    [SerializeField] private Store_FishManager manager;
    [SerializeField] private TextMeshProUGUI title_ui, descr_ui, amount_ui;
    [SerializeField] private Image mainImage_ui;
    [SerializeField] private int idx;


    [Button]
    public void InitBtn(int _idx, string title, string descr, int amt, Sprite mainImage)
    {
        idx = _idx;
        title_ui.text = title;
        descr_ui.text = descr;
        amount_ui.text = amt.ToString();
        mainImage_ui.sprite = mainImage;
    }

    public void StoreItemClicked()
    {
        manager.ItemClicked(idx);
    }
}