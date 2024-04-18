using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class IAPManager : MonoBehaviour
{
    public enum Product
    {
        diamonds0,
        diamonds1,
        diamonds2,
        diamonds3,
        diamonds4,
        maxheartplus,
        itemBundle,
        hardworking
    }

    public static IAPManager Instance;
    [SerializeField] private HardWorkManager hardWorkManager;

    private IBillingProduct product_maxheartplus, product_itemBundle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BillingServices.InitializeStore();
        hardWorkManager.UpdateData();
    }

    private void OnEnable()
    {
        BillingServices.OnInitializeStoreComplete += OnInitializeStoreComplete;
        BillingServices.OnTransactionStateChange += OnTransactionStateChange;
        BillingServices.OnRestorePurchasesComplete += OnRestorePurchasesComplete;
    }

    private void OnDisable()
    {
        BillingServices.OnInitializeStoreComplete -= OnInitializeStoreComplete;
        BillingServices.OnTransactionStateChange -= OnTransactionStateChange;
        BillingServices.OnRestorePurchasesComplete -= OnRestorePurchasesComplete;
    }
    
    private void OnInitializeStoreComplete(BillingServicesInitializeStoreResult result, Error error)
    {
        if (error == null)
        {
            var products = result.Products;
            for (var iter = 0; iter < products.Length; iter++)
            {
                var product = products[iter];
                Debug.Log(string.Format("[{0}]: {1}", iter, product));
                if (product.Id == Product.maxheartplus.ToString())
                    product_maxheartplus = product;
                else if (product.Id == Product.itemBundle.ToString()) product_itemBundle = product;
            }
        }
        else
        {
            Debug.Log("Store initialization failed with error. Error: " + error);
        }

        var invalidIds = result.InvalidProductIds;
        if (invalidIds.Length > 0)
            for (var iter = 0; iter < invalidIds.Length; iter++)
                Debug.Log(string.Format("[{0}]: {1}", iter, invalidIds[iter]));

        UpdatePurchasedData();
    }

    private void OnTransactionStateChange(BillingServicesTransactionStateChangeResult result)
    {
        var transactions = result.Transactions;
        for (var iter = 0; iter < transactions.Length; iter++)
        {
            var transaction = transactions[iter];
            switch (transaction.TransactionState)
            {
                case BillingTransactionState.Purchased:
                    BalloonUIManager.Instance.ShowMsg("구매 성공..!");
                    Debug.Log(string.Format("Buy product with id:{0} finished successfully.",
                        transaction.Payment.ProductId));
                    ItemPurchased((Product)Enum.Parse(typeof(Product), transaction.Payment.ProductId));
                    UpdatePurchasedData();
#if !UNITY_EDITOR
                    FirebaseAnalytics.LogEvent("IAP", "ItemPurchased", transaction.Payment.ProductId);
#endif
                    break;

                case BillingTransactionState.Failed:
                    BalloonUIManager.Instance.ShowMsg("구매 실패..");
                    Debug.Log(string.Format("Buy product with id:{0} failed with error. Error: {1}",
                        transaction.Payment.ProductId, transaction.Error));
                    StoreManager.Instance.HideLoadingBar();
                    break;
            }
        }
    }

    private void OnRestorePurchasesComplete(BillingServicesRestorePurchasesResult result, Error error)
    {
        if (error == null)
        {
            var transactions = result.Transactions;
            Debug.Log("Request to restore purchases finished successfully.");
            Debug.Log("Total restored products: " + transactions.Length);
            for (var iter = 0; iter < transactions.Length; iter++)
            {
                var transaction = transactions[iter];
                Debug.Log(string.Format("[{0}]: {1}", iter, transaction.Payment.ProductId));
            }

            UpdatePurchasedData();
            BalloonUIManager.Instance.ShowMsg("구매 내역 복원 성공!");
            StoreManager.Instance.HideLoadingBar();
        }
        else
        {
            BalloonUIManager.Instance.ShowMsg("구매 내역 복원 실패..");
            StoreManager.Instance.HideLoadingBar();
            Debug.Log("Request to restore purchases failed with error. Error: " + error);
        }
    }

    public void RestorePurchases()
    {
        if (!BillingServices.CanMakePayments())
        {
            BillingServices.InitializeStore();
            return;
        }

        BillingServices.RestorePurchases();
    }

    private void UpdatePurchasedData()
    {
        PlayerPrefs.SetInt("product_maxheartplus", BillingServices.IsProductPurchased(product_maxheartplus) ? 1 : 0);
        PlayerPrefs.SetInt("product_itemBundle", BillingServices.IsProductPurchased(product_itemBundle) ? 1 : 0);
        PlayerPrefs.Save();
        StoreManager.Instance.RestorePurchaseRetrieved();
    }

    public void BuyProduct(Product product)
    {
        if (!BillingServices.CanMakePayments())
        {
            BillingServices.InitializeStore();
            return;
        }
        BillingServices.BuyProduct(product.ToString());
    }

    private void ItemPurchased(Product product)
    {
        switch (product)
        {
            case Product.diamonds0:
                StoreManager.Instance.DiamondManager.DiamondPurchased(1);
                break;
            case Product.diamonds1:
                StoreManager.Instance.DiamondManager.DiamondPurchased(2);
                break;
            case Product.diamonds2:
                StoreManager.Instance.DiamondManager.DiamondPurchased(3);
                break;
            case Product.diamonds3:
                StoreManager.Instance.DiamondManager.DiamondPurchased(4);
                break;
            case Product.diamonds4:
                StoreManager.Instance.DiamondManager.DiamondPurchased(5);
                break;
            case Product.maxheartplus:
                PlayerPrefs.SetInt("product_maxheartplus", 1);
                StoreManager.Instance.PackageManager.ItemPurchased(1);
                UpdatePurchasedData();
                break;
            case Product.itemBundle:
                PlayerPrefs.SetInt("product_itemBundle", 1);
                StoreManager.Instance.PackageManager.ItemPurchased(0);
                UpdatePurchasedData();
                break;
            case Product.hardworking:
                StoreManager.Instance.PackageManager.ItemPurchased(2);
                break;
        }
    }
}