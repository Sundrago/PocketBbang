using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class DoMiCoinManager : MonoBehaviour
{
    [SerializeField]
    private Text totalBalance_ui, totalBalancePercentage_ui, amount_ui, value_ui, bought_ui, average_ui, current_ui;

    private int currentPrice;
    private int domi_amount;
    private float domi_average;

    public int BuyAmount = 0;
    public int SellAmount = 0;
    public float diff;
    
    [Button]
    public void UpdateCurrentPrice()
    {
        currentPrice = PlayerPrefs.GetInt("DomiCoin_CurrentPrice", 1000);
        domi_amount = PlayerPrefs.GetInt("DomiCoin_Amount", 0);
        domi_average = PlayerPrefs.GetFloat("DomiCoin_Average", 0);

        if (currentPrice < 333) diff = GetRandomDiff(0.4f, 3);
        else if(currentPrice < 3000) diff = GetRandomDiff(0.5f, 0);
        else if(currentPrice < 10000) diff = GetRandomDiff(0.4f, -2);
        else diff = GetRandomDiff(0.4f, -3);
        
        currentPrice = Mathf.RoundToInt(currentPrice * diff);
        PlayerPrefs.SetInt("DomiCoin_CurrentPrice", currentPrice);
        UpdateUI();
#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("DomiCoin", "currentPrice", currentPrice);
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("DomiCoin", "domi_amount", domi_amount);
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("DomiCoin", "balancePercent", GetBalancePercent());
#endif

        float GetRandomDiff(float maxPercent, float adj)
        {
            float middle = 50 + adj/2;
            bool up = Random.Range(0, 100) < middle;
            
            float uniformRandom = Random.value;
            float skewedRandom = Mathf.Sqrt(uniformRandom);
            maxPercent *= skewedRandom;
            
            if (up) return 1 + maxPercent * 2f;
            else return 1 - maxPercent;
        }
        
        // float BiasedRandom(float min, float max, int count)
        // {
        //     float sum = 0;
        //
        //     for (int i = 0; i < count; i++)
        //     {
        //         sum += UnityEngine.Random.Range(min, max);
        //     }
        //     return sum / count;
        // }
    }

    private void UpdateUI()
    {
        int totalBalace = Mathf.RoundToInt((domi_amount * currentPrice) - (domi_average * domi_amount));

        if (totalBalace > 0)
        {
            totalBalance_ui.color = new Color(185, 0, 0);
            totalBalancePercentage_ui.color = new Color(185, 0, 0);
            totalBalance_ui.text = "+" + totalBalace;
            totalBalancePercentage_ui.text = "" + Mathf.RoundToInt(totalBalace * 10000 / (domi_average * domi_amount)/100) + "%";
        }
        else
        {
            totalBalance_ui.color = new Color(0, 0, 185);
            totalBalancePercentage_ui.color = new Color(0, 0, 185);
            totalBalance_ui.text = "" + totalBalace;
            totalBalancePercentage_ui.text = "" + Mathf.RoundToInt(totalBalace * 10000 / (domi_average * domi_amount)/100) + "%";
        }
        
        current_ui.text = Mathf.Round(currentPrice).ToString();
        amount_ui.text = domi_amount + "개";
        value_ui.text = "" + (domi_amount * currentPrice);
        bought_ui.text = "" + (domi_average * domi_amount);
        average_ui.text = "" + domi_average;
        current_ui.text = "" + currentPrice;
    }

    [Button]
    public void BuyCoin(int amount)
    {
        Heart_Control.Instance.UpdateMoney(-currentPrice * amount);
        domi_average = (domi_amount * domi_average + amount * currentPrice) / (domi_amount + amount);
        domi_average = Mathf.Round(domi_average * 100) / 100f;
        domi_amount += amount;
        
        PlayerPrefs.SetInt("DomiCoin_Amount", domi_amount);
        PlayerPrefs.SetFloat("DomiCoin_Average", domi_average);
        UpdateUI();
    }

    [Button]
    public void SellCoin(int amount)
    {
        Heart_Control.Instance.UpdateMoney(currentPrice * amount);
        domi_amount -= amount;
        PlayerPrefs.SetInt("DomiCoin_Amount", domi_amount);
        UpdateUI();
    }

    public int GetPrice()
    {
        return currentPrice;
    }

    public int GetAmount()
    {
        return domi_amount;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public int GetBalancePercent()
    {
        int totalBalace = Mathf.RoundToInt((domi_amount * currentPrice) - (domi_average * domi_amount));
        return Mathf.RoundToInt(totalBalace * 10000 / (currentPrice * domi_amount)/100);
    }
    
}
