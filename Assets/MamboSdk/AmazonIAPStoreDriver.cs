#if mamboo_iap
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using com.amazon.device.iap.cpt;
using MamboSdk.Analytics;
using MamboSdk.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace MamboSdk
{
    public class AmazonIAPStoreDriver : IStore, IAmazonStoreExtensions
    {
        public static Action<bool> PurchasesRestoredCallback;
        public Dictionary<string, string> PurchaseRequests { get; set; } = new Dictionary<string, string>();
        private IAmazonIapV2 iapService = AmazonIapV2Impl.Instance;
        private IStoreCallback callback;
        private List<ProductDescription> retrievedProducts;

        public void Initialize(IStoreCallback callback)
        {
            this.callback = callback;
            iapService.AddGetProductDataResponseListener(Retrived);
            iapService.AddPurchaseResponseListener(Purchased);
            iapService.AddGetPurchaseUpdatesResponseListener(PurchasesRestored);
        }
        
        public void RestoreTransactions(Action<bool> callback)
        {
            PurchasesRestoredCallback = callback;
            var response = iapService.GetPurchaseUpdates(new ResetInput { Reset = true });
        }

        public void PurchasesRestored(GetPurchaseUpdatesResponse args)
        {
            var requestId = args.RequestId;
            var receipts = args.Receipts;
            var skuReceiptDict = receipts
                .GroupBy(receipt => receipt.Sku)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.OrderByDescending(receipt => receipt.PurchaseDate).FirstOrDefault());
            
            foreach (var group in skuReceiptDict)
            {
                var sku = group.Key;
                var receipt = group.Value;
                
                var unixTimeNow = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
                if (receipt.CancelDate != 0 && receipt.CancelDate < unixTimeNow)
                {
                    Debug.LogWarning($"[Amazon IAP Driver] purchase product canceled by CancelDate");
                    PurchaseController.instance.OnSubscriptionExpired?.Invoke(sku);
                    continue;
                }
                
                Purchased(new PurchaseResponse
                {
                    Status = "SUCCESSFUL",
                    PurchaseReceipt = receipt,
                    AmazonUserData = args.AmazonUserData,
                    RequestId = requestId,
                    isRestore = true
                });
            }

            PurchasesRestoredCallback?.Invoke(args.Status == "SUCCESSFUL");
        }

        public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            var request = new SkusInput { Skus = products.Select(x => x.id).ToList() };

            // Call synchronous operation with input object
            var response = iapService.GetProductData(request);
        }
    
        private void Retrived(GetProductDataResponse args)
        {
            retrievedProducts = new List<ProductDescription>();
            Debug.LogWarning($"[Amazon IAP Driver] GetProductDataResponse:{args.Status}");
            foreach (var productDataKeyValue in args.ProductDataMap)
            {
                var key = productDataKeyValue.Key;
                var data = productDataKeyValue.Value;
                Debug.LogWarning($"[Amazon IAP Driver] product:{data.ToJson()}");
                
                var decimalPrice = ParsePrice(data.Price);
                
                var priceString = string.IsNullOrEmpty(data.Price)
                    ? decimalPrice.ToString(CultureInfo.InvariantCulture)
                    : data.Price.Trim();
                
                Debug.LogWarning($"[Amazon IAP Driver] parsed price:{priceString}");
                var metadata = new ProductMetadata(priceString, data.Title, data.Description, "", decimalPrice);
                retrievedProducts.Add(new ProductDescription(data.Sku, metadata));
            }
        
            foreach (var unavailableSku in args.UnavailableSkus)
            {
                Debug.LogWarning($"[Amazon IAP Driver] UnavailableSku product:{unavailableSku}");
            }
            callback.OnProductsRetrieved(retrievedProducts);
        }

        public void Purchase(ProductDefinition product, string developerPayload)
        {
            // Call synchronous operation with input object
            var response = iapService.Purchase(new SkuInput { Sku = product.id });
            PurchaseRequests.Add(response.RequestId, product.id);
        }

        public void Purchased(PurchaseResponse args)
        {
            Debug.LogWarning($"[Amazon IAP Driver] Purchased product:{args.ToJson()}");
            var productId = GetProductId(args);
            
            if (args.Status != "SUCCESSFUL")
            {
                callback.OnPurchaseFailed(new PurchaseFailureDescription(productId, PurchaseFailureReason.Unknown, args.ToJson()));
                return;
            }
            
            if (!args.isRestore)
            {
                var purchasedProduct = retrievedProducts.First(x => x.storeSpecificId == productId);
                
                var metadata = new PurchaseMetadata
                {
                    ProductId = productId,
                    Platform = PurchasePlatform.ANDROID,
                    CurrencyCode = purchasedProduct.metadata.isoCurrencyCode,
                    UnitPrice = decimal.ToDouble(purchasedProduct.metadata.localizedPrice),
                    TransactionId = args.RequestId,
                    Receipt = purchasedProduct.receipt,
                    PurchaseTime = args.PurchaseReceipt.PurchaseDate.ToString(),
                };
                MambooAnalyticsWrapper.Instance.TrackPurchase(metadata);
            }
        
            callback.OnPurchaseSucceeded(productId, args.PurchaseReceipt?.ToJson() , args.PurchaseReceipt?.ReceiptId);
        }
        
        private decimal ParsePrice(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 9.99M;
            }

            var digits = Regex.Replace(input, @"[^\d.,]", "");
            digits = digits.Replace(" ", "").Replace(",", ".");

            if (decimal.TryParse(digits, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
            {
                return price;
            }

            throw new Exception($"Cannot parse price: {input}");
        }

        private string GetProductId(PurchaseResponse args)
        {
            var requestId = args.RequestId;
            var isRequestFound = PurchaseRequests.TryGetValue(requestId, out var value);
            if (isRequestFound)
            {
                return value;
            }
            
            if (args.PurchaseReceipt != null)
            {
                return args.PurchaseReceipt.Sku;
            }

            return "unknown";
        }

        public void FinishTransaction(ProductDefinition product, string transactionId)
        {
        }
    }
}
#endif