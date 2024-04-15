using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DomiTradePanel : MonoBehaviour
{
    [SerializeField] private DoMiCoinManager domiCoin;

    private int amount;
    private string confirmID, cancelId;
    [SerializeField] private Text amt_ui, price_ui;
    [SerializeField] private Button Upbtn, DownBtn;

    [SerializeField] private GameObject namePlate, dialoguePlate;

    private Main_control.TradeType type;


    public void OpenPanel(Main_control.TradeType _type, string _confirmID, string _cancelID, string _noMoney=null)
    {
        type = _type;
        
        amount = 0;
        UpdateUI();
        confirmID = _confirmID;
        cancelId = _cancelID;
        
        if(_type == Main_control.TradeType.buy && Heart_Control.Instance.GetBalance() < domiCoin.GetPrice())
            Main_control.Instance.Nylon(_noMoney);
        
        namePlate.SetActive(false);
        dialoguePlate.SetActive(false);
        gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        amt_ui.text = amount + "개";
        price_ui.text = amount * domiCoin.GetPrice() + "냥";

        if(type == Main_control.TradeType.buy) Upbtn.interactable = (Heart_Control.Instance.GetBalance() >= (amount + 1) * domiCoin.GetPrice());
        else Upbtn.interactable = (domiCoin.GetAmount() >= amount + 1);
        DownBtn.interactable = (amount - 1 >= 0);
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
        if(!gameObject.activeSelf) return;

        HidePanel();
        
        if(amount <= 0) Main_control.Instance.Nylon_f(cancelId);
        else
        {
            domiCoin.BuyAmount = amount;
            domiCoin.SellAmount = amount;
            
            Main_control.Instance.Nylon_f(confirmID);
        }
        
    }

    public void ExitBtnClicked()
    {
        if(!gameObject.activeSelf) return;

        HidePanel();
        Main_control.Instance.Nylon_f(cancelId);
    }

    public void HidePanel()
    {
        namePlate.SetActive(true);
        dialoguePlate.SetActive(true);
        gameObject.SetActive(false);
    }
}
