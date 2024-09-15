#if mamboo_iap
using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MamboSdk.Purchasing
{
    public class PurchaseController : MonoBehaviour, IStoreListener
    {
        public static PurchaseController instance;
        public Action<string> OnPurchaseCompleted;
        public Action<string> OnSubscriptionExpired;
        private IStoreController storeController;
        private IExtensionProvider extensionsController;

        private PurchaseConfig _purchaseConfig;
        private static bool purchaseCalled = false;

        public PurchaseConfig PurchaseConfig => _purchaseConfig;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else DestroyImmediate(this);
        }

        private void Start()
        {
            _purchaseConfig = PurchaseConfig.Load();

            var builder = ConfigurationBuilder.Instance(new AmazonCustomModule());
            foreach (var product in _purchaseConfig.Products)
            {
                Debug.Log($"[Mamboo] product:{product.ProductId}");
                builder.AddProduct(product.ProductId, product.ProductType);
            }

            if (builder.products.Count != 0)
            {
                Debug.Log($"[Mamboo] Start UnityPurchasing Initializing");
                UnityPurchasing.Initialize(this, builder);
            }
        }

        #region IStoreListener
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionsController = extensions;

            // TODO PURCHASE: uncomment this if you need automatic restore purchases on start
            // WARNING: enabling this can be the reason of build rejection because restore button will be pressed
            // when the restore has already happened and from UI it looks like it does not work

            extensions.GetExtension<IAmazonStoreExtensions>().RestoreTransactions(RestoreTransactions);
        }

        private void RestoreTransactions(bool result)
        {
            if (result)
            {
                Debug.Log("RestoreTransactions success");
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Store initialization failed! Error: {error.ToString()}");
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Debug.LogError($"{i.transactionID} failed because of {p.ToString()}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            try
            {
                OnPurchaseCompleted?.Invoke(e.purchasedProduct.definition.id);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }

            purchaseCalled = false;
            return PurchaseProcessingResult.Complete;
        }

        #endregion

        public void PurchaseProduct(string productId)
        {
            purchaseCalled = true;
            storeController?.InitiatePurchase(productId);
        }

        public void RestorePurchase()
        {
            extensionsController.GetExtension<IAmazonStoreExtensions>().RestoreTransactions(RestoreTransactions);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
#if DEV_BUILD
            Debug.Log($"[Mamboo] PurchaseController OnInitializeFailed: {error} - {message}");
#endif
        }
    }
}
#endif
