using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DoMiCoinManager : MonoBehaviour
{
    [SerializeField] private Text totalBalance_ui,
        totalBalancePercentage_ui,
        amount_ui,
        value_ui,
        bought_ui,
        average_ui,
        current_ui;

    public int BuyAmount;
    public int SellAmount;
    public float diff;

    private int currentPrice;
    private int domi_amount;
    private float domi_average;

    [Button]
    public void UpdateCurrentPrice()
    {
        currentPrice = PlayerPrefs.GetInt("DomiCoin_CurrentPrice", 1000);
        domi_amount = PlayerPrefs.GetInt("DomiCoin_Amount", 0);
        domi_average = PlayerPrefs.GetFloat("DomiCoin_Average", 0);

        if (currentPrice < 333) diff = GetRandomDiff(0.4f, 3);
        else if (currentPrice < 3000) diff = GetRandomDiff(0.5f, 0);
        else if (currentPrice < 10000) diff = GetRandomDiff(0.4f, -2);
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
            var middle = 50 + adj / 2;
            var up = Random.Range(0, 100) < middle;

            var uniformRandom = Random.value;
            var skewedRandom = Mathf.Sqrt(uniformRandom);
            maxPercent *= skewedRandom;

            if (up) return 1 + maxPercent * 2f;
            return 1 - maxPercent;
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
        var totalBalace = Mathf.RoundToInt(domi_amount * currentPrice - domi_average * domi_amount);

        if (totalBalace > 0)
        {
            totalBalance_ui.color = new Color(185, 0, 0);
            totalBalancePercentage_ui.color = new Color(185, 0, 0);
            totalBalance_ui.text = "+" + totalBalace;
            totalBalancePercentage_ui.text =
                "" + Mathf.RoundToInt(totalBalace * 10000 / (domi_average * domi_amount) / 100) + "%";
        }
        else
        {
            totalBalance_ui.color = new Color(0, 0, 185);
            totalBalancePercentage_ui.color = new Color(0, 0, 185);
            totalBalance_ui.text = "" + totalBalace;
            totalBalancePercentage_ui.text =
                "" + Mathf.RoundToInt(totalBalace * 10000 / (domi_average * domi_amount) / 100) + "%";
        }

        current_ui.text = Mathf.Round(currentPrice).ToString();
        amount_ui.text = domi_amount + "개";
        value_ui.text = "" + domi_amount * currentPrice;
        bought_ui.text = "" + domi_average * domi_amount;
        average_ui.text = "" + domi_average;
        current_ui.text = "" + currentPrice;
    }

    [Button]
    public void BuyCoin(int amount)
    {
        PlayerHealthManager.Instance.UpdateMoney(-currentPrice * amount);
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
        PlayerHealthManager.Instance.UpdateMoney(currentPrice * amount);
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
        var totalBalace = Mathf.RoundToInt(domi_amount * currentPrice - domi_average * domi_amount);
        return Mathf.RoundToInt(totalBalace * 10000 / (currentPrice * domi_amount) / 100);
    }
}