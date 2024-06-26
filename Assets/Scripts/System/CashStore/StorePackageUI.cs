using System.Collections;
using MyUtility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class StorePackageUI : MonoBehaviour
{
    [SerializeField] [ReadOnly] private int idx;
    [SerializeField] private TextMeshProUGUI title_text, price_text;
    [SerializeField] private GameObject soldOut_ui, diamondIcon_ui;
    [SerializeField] private StoreRewardItemUI[] rewardItemUis;
    [SerializeField] private Store_PackageManager manager;

    [Button]
    public void Init(DeserializeJsonData.StorePackageData data)
    {
        title_text.text = data.name;
        idx = DeserializeJsonData.Instance.StorePackageDatas.IndexOf(data);

        //Init Price
        if (data.NeedItemType == DeserializeJsonData.NeedItemType.KRW)
        {
            price_text.text = "\u20a9" + Converter.IntToCommaSeparatedString(data.needAmt);
            diamondIcon_ui.SetActive(false);
        }
        else
        {
            price_text.text = data.needAmt.ToString();
            diamondIcon_ui.SetActive(true);
        }

        //Init Reward Items
        StartCoroutine(LoadRewardItems(data));
    }

    private IEnumerator LoadRewardItems(DeserializeJsonData.StorePackageData data)
    {
        for (var i = 0; i < data.itemCode.Length; i++) rewardItemUis[i].InitItem(data.itemCode[i], data.itemAmt[i]);

        for (var i = data.itemCode.Length; i < rewardItemUis.Length; i++) rewardItemUis[i].InitItem(-1, -1);

        yield return null;
    }

    public void SetSoldOut(bool isSoldOut)
    {
        soldOut_ui.SetActive(isSoldOut);
    }

    public void BtnClicked()
    {
        manager.OpenDetailInfo(idx);
    }
}