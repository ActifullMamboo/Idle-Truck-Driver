#if mamboo_iap
using System;
using com.amazon.device.iap.cpt;

namespace MamboSdk
{
    public class AmazonStoreExtension : IAmazonStoreExtensions
    {
        // Obtain object used to interact with plugin
        private IAmazonIapV2 iapService = AmazonIapV2Impl.Instance;
        
        public void RestoreTransactions(Action<bool> callback)
        {
            AmazonIAPStoreDriver.PurchasesRestoredCallback = callback;
            // Call synchronous operation with input object
            var response = iapService.GetPurchaseUpdates(new ResetInput
            {
                Reset = true
            });

            // Get return value
            var requestIdString = response.RequestId;
        }
    }
}
#endif