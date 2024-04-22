using UnityEngine;
using UnityEngine.UI;

public class Tanghuru_Amt_Selector : MonoBehaviour
{
    private const int tanghuru_price = 3000;

    [SerializeField] private Text amt_ui, price_ui;
    [SerializeField] private Button Upbtn, DownBtn;
    [SerializeField] private GameObject namePlate, dialoguePlate;
    
    private int amount;
    private string confirmID, cancelId;

    public void OpenPanel(string _confirmID, string _cancelID)
    {
        amount = 0;
        UpdateUI();
        confirmID = _confirmID;
        cancelId = _cancelID;

        namePlate.SetActive(false);
        dialoguePlate.SetActive(false);
        gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        amt_ui.text = amount + "개";
        price_ui.text = amount * tanghuru_price + "냥";

        Upbtn.interactable = PlayerStatusManager.Instance.GetBalance() >= (amount + 1) * tanghuru_price;
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

        if (amount <= 0) GameManager.Instance.NylonDialogue.Nylon_f(cancelId);
        else
            GameManager.Instance.NylonDialogue.Nylon_f(confirmID);
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