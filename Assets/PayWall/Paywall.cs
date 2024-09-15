using System;
using MamboSdk.Purchasing;
using UnityEngine;
using UnityEngine.UI;

public class Paywall : MonoBehaviour
{
    public Action onPaywallClosed;
    public Action onPurchaseComplete;

    [SerializeField] private Button closePaywall, purchase;

    private const string subscriptionId = "com.amazon.mamboo.cube.island.subscription";
    
    private void OnEnable()
    {
        closePaywall.onClick.AddListener(() =>
        {
            onPaywallClosed?.Invoke();
            Destroy(gameObject);
        });
        
        purchase.onClick.AddListener(() =>
        {
            PurchaseController.instance.PurchaseProduct(subscriptionId);
        });
        
        PurchaseController.instance.OnPurchaseCompleted += OnSubscriptionComplete;
        PurchaseController.instance.OnSubscriptionExpired += OnSubscriptionExpired;
    }

    void OnSubscriptionComplete(string subsId)
    {
        if (subsId == subscriptionId)
        {
            PlayerPrefs.SetString("subscription", "true");
            Debug.Log("Todo some reward for player");
            onPurchaseComplete?.Invoke();
        }
    }
    
    void OnSubscriptionExpired(string subsId)
    {
        if (subsId == subscriptionId)
        {
            PlayerPrefs.SetString("subscription", "false");
        }
    }
}
