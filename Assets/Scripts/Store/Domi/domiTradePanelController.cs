using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class domiTradePanelController : MonoBehaviour
{
    [FormerlySerializedAs("domiconManager")] [FormerlySerializedAs("domiCoin")] [SerializeField] private DoMiCoinManager domicoinManager;
    [SerializeField] private Text amt_ui, price_ui;
    [SerializeField] private Button Upbtn, DownBtn;

    [SerializeField] private GameObject namePlate, dialoguePlate;

    private int amount;
    private string confirmID, cancelId;

    private GameManager.TradeType type;


    public void OpenPanel(GameManager.TradeType _type, string _confirmID, string _cancelID, string _noMoney = null)
    {
        type = _type;

        amount = 0;
        UpdateUI();
        confirmID = _confirmID;
        cancelId = _cancelID;

        if (_type == GameManager.TradeType.Buy && PlayerHealthManager.Instance.GetBalance() < domicoinManager.GetPrice())
            GameManager.Instance.NylonDialogue.Nylon(_noMoney);

        namePlate.SetActive(false);
        dialoguePlate.SetActive(false);
        gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        amt_ui.text = amount + "개";
        price_ui.text = amount * domicoinManager.GetPrice() + "냥";

        if (type == GameManager.TradeType.Buy)
            Upbtn.interactable = PlayerHealthManager.Instance.GetBalance() >= (amount + 1) * domicoinManager.GetPrice();
        else Upbtn.interactable = domicoinManager.GetAmount() >= amount + 1;
        DownBtn.interactable = amount - 1 >= 0;
    }

    public void UpbtnClicked()
    {
        if (Upbtn.interactable) amount++;
        UpdateUI();
    }

    public void DownBtnClicked()
    {
        if (DownBtn.interactable) amount--;
        UpdateUI();
    }

    public void ConfirmBtnClicked()
    {
        if (!gameObject.activeSelf) return;

        HidePanel();

        if (amount <= 0)
        {
            GameManager.Instance.NylonDialogue.Nylon_f(cancelId);
        }
        else
        {
            domicoinManager.BuyAmount = amount;
            domicoinManager.SellAmount = amount;

            GameManager.Instance.NylonDialogue.Nylon_f(confirmID);
        }
    }

    public void ExitBtnClicked()
    {
        if (!gameObject.activeSelf) return;

        HidePanel();
        GameManager.Instance.NylonDialogue.Nylon_f(cancelId);
    }

    public void HidePanel()
    {
        namePlate.SetActive(true);
        dialoguePlate.SetActive(true);
        gameObject.SetActive(false);
    }
}