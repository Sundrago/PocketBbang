using System.Collections;
using DG.Tweening;
using MyUtility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorePackageDetailPanel : MonoBehaviour
{
    [SerializeField] private Text title_ui, descr_ui;
    [SerializeField] private TextMeshProUGUI price_ui;
    [SerializeField] private Image bg;
    [SerializeField] private Transform panel;
    [SerializeField] private Store_PackageManager manager;
    [SerializeField] private StoreRewardItemUI[] rewardItemUis;
    [SerializeField] private GameObject diamondIcon_ui;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [SerializeField] private GameObject[] hardworkingImg, heartPlusImg;

    private DeserializeJsonData.StorePackageData data;
    private int idx;

    [Button]
    public void Init(int _idx)
    {
        if (DOTween.IsTweening(panel)) return;

        idx = _idx;


        data = DeserializeJsonData.Instance.StorePackageDatas[idx];

        title_ui.text = data.name;
        descr_ui.text = data.descr.Replace("\\n", "\n");
        ;

        //Init Price
        if (data.NeedItemType == DeserializeJsonData.NeedItemType.KRW)
        {
            price_ui.text = "\u20a9" + Converter.IntToCommaSeparatedString(data.needAmt);
            diamondIcon_ui.SetActive(false);
        }
        else
        {
            price_ui.text = "    " + data.needAmt;
            diamondIcon_ui.SetActive(true);
        }

        foreach (var obj in heartPlusImg) obj.gameObject.SetActive(idx == 1);

        foreach (var obj in hardworkingImg) obj.gameObject.SetActive(idx == 2);
        if (idx == 2) descr_ui.text = "";

        gameObject.SetActive(true);

        //Init Reward Items
        if (data.itemCode.Length <= 3)
            gridLayoutGroup.cellSize = new Vector2(250, 300);
        else
            gridLayoutGroup.cellSize = new Vector2(200, 250);
        StartCoroutine(LoadRewardItems(data));

        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.localPosition = new Vector3(0, 50, 0);
        panel.transform.eulerAngles = Vector3.zero;

        panel.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        bg.color = new Color(1, 1, 1, 0);
        bg.DOFade(0.6f, 0.2f);
    }

    private IEnumerator LoadRewardItems(DeserializeJsonData.StorePackageData data)
    {
        foreach (var itemUi in rewardItemUis)
        {
            itemUi.gameObject.SetActive(false);
            itemUi.transform.localScale = Vector3.one;
        }

        for (var i = 0; i < data.itemCode.Length; i++)
        {
            rewardItemUis[i].InitItem(data.itemCode[i], data.itemAmt[i]);
            rewardItemUis[i].transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5);
            yield return new WaitForSeconds(0.1f);
        }

        for (var i = data.itemCode.Length; i < rewardItemUis.Length; i++)
        {
            rewardItemUis[i].InitItem(-1, -1);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    public void ConfirmBtnClicked()
    {
        manager.RequestPurchaseItem(idx);
        CloseBtnClicked();
    }

    public void CloseBtnClicked()
    {
        if (!gameObject.activeSelf) return;
        if (DOTween.IsTweening(panel)) return;

        bg.DOFade(0f, 0.3f);
        panel.DOLocalMoveY(-2500, 0.3f).SetEase(Ease.InCubic);
        panel.DORotate(new Vector3(0, 0, -60), 0.3f).SetEase(Ease.InCubic)
            .OnComplete(() => { gameObject.SetActive(false); });
    }
}