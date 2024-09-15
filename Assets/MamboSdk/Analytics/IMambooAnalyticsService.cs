using System;
using System.Collections.Generic;
using MamboSdk.Ads;
using MamboSdk.Settings;
namespace MamboSdk.Analytics
{
    public interface IMambooAnalyticsService
    {
        void Initialize(MambooSdkSettings settings, Action onEnd);
        
        bool isInitialized();

        void TrackEvent(string eventName, Dictionary<string, object> values);
        void TrackPurchase(PurchaseMetadata purchaseMetadata);
        void TrackAdRevenue(double value, string currency, MambooAdFormat adFormat, string adSource);


        void PauseSession();

        void ResumeSession();
    }

    public class PurchaseMetadata
    {
        public string ProductId { get; set; }
        public PurchasePlatform Platform { get; set; }
        public string CurrencyCode { get; set; }
        public double UnitPrice { get; set; }
        public string TransactionId { get; set; }
        public string Receipt { get; set; }
        public string PurchaseTime  { get; set; }
    }

    public enum PurchasePlatform
    {
        ANDROID,
        IOS
    }
}