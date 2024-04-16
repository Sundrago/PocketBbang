using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private Image glow, item;
    [SerializeField] private Text amount_text;
    [SerializeField] private ItemDataManager itemData;
    [SerializeField] private ItemData data;
    [SerializeField] private RewardItemDescrUI rewardItemDescr;
    [SerializeField] private GameObject rareFx;

    private int cardIdx = -1;
    private int itemIdx;

    [Button]
    public void Init(int _itemIdx, int amount, string tag)
    {
        itemIdx = _itemIdx;
        glow.color = Color.white;
        if (itemIdx == 1002) amount_text.text = string.Format("{0:#,###0}", amount) + "원";
        else amount_text.text = string.Format("{0:#,###0}", amount) + "개";
        gameObject.SetActive(true);
        rareFx.SetActive(false);

        StartCoroutine(itemData.GetItemDataAsync(itemIdx, amount, tag, _itemData =>
        {
            if (amount > 0) AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.winItem);
            if (_itemData != null)
            {
                data = _itemData;
                itemIdx = data.idx;
                item.sprite = data.sprite;
                glow.DOFade(0, 0.3f);
                gameObject.transform.DOPunchScale(Vector3.one * 1f, 0.5f, 5);
                rareFx.SetActive(data.isRare);
                gameObject.transform.localScale = data.isRare ? Vector3.one * 1.1f : Vector3.one;
                cardIdx = data.cardIdx;

                //UpdateAmt if changed
                if (amount != data.amount)
                {
                    amount = data.amount;
                    if (itemIdx == 1002) amount_text.text = string.Format("{0:#,###0}", amount) + "원";
                    else amount_text.text = string.Format("{0:#,###0}", amount) + "개";
                }

                //if card print card name instead of amount
                if (cardIdx > 0)
                {
                    amount_text.text = data.name;
                    if (data.isRare) CollectionManager.Instance.OpenCard(data.cardIdx, data.isRare);
                }

                amount_text.color = data.isRare ? new Color(0.5f, 0f, 0.5f, 1) : Color.black;
            }
        }));
    }

    public void OnBtnClick()
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);

        //if is card
        if (cardIdx > 0)
        {
            CollectionManager.Instance.OpenCard(data.cardIdx, data.isRare);
            return;
        }

        ItemInfoUIPanel.Instance.OpenPanel(itemIdx);
        // rewardItemDescr.Init(data.name, data.descr, gameObject.transform);
    }
}