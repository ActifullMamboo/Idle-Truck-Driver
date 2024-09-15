#if mamboo_appmetrica
using System;
using System.Collections.Generic;
using MamboSdk.Ads;
using MamboSdk.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MamboSdk.Analytics
{
    public class AppmetricaAnalyticsProvider: IMambooAnalyticsService
    {
        private static bool s_isInitialized;
        private static IYandexAppMetrica s_metrica;
        private static readonly object s_syncRoot = new Object();

        private const bool exceptionsReporting = true;
        private const uint sessionTimeoutSec = 120;
        private const bool locationTracking = false;
#if mamboo_qa_build
        private const bool logs = true;
#else
        private const bool logs = false;
#endif
        private const bool handleFirstActivationAsUpdate = false;
        private const bool statisticsSending = true;
        private readonly bool _actualPauseStatus;
        private bool initialized;
        
        public static IYandexAppMetrica Instance
        {
            get
            {
                if (s_metrica == null)
                {
                    lock (s_syncRoot)
                    {
    #if UNITY_IPHONE || UNITY_IOS
                        if (s_metrica == null && Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            s_metrica = new YandexAppMetricaIOS();
                        }
    #elif UNITY_ANDROID
                        if (s_metrica == null && Application.platform == RuntimePlatform.Android)
                        {
                            s_metrica = new YandexAppMetricaAndroid();
                        }
    #endif
                        if (s_metrica == null)
                        {
                            s_metrica = new YandexAppMetricaDummy();
                        }
                    }
                }

                return s_metrica;
            }
        }

        public bool isInitialized() => initialized;
        public void Initialize(MambooSdkSettings settings, Action onEnd)
        {
#if UNITY_ANDROID
            var apiKey = settings.appmetricaSettings.androidKey;
#elif UNITY_IOS
            var apiKey = settings.appmetricaSettings.iosKey;
#endif
            
            Instance.OnActivation += _ =>
            {
                onEnd.Invoke();
                initialized = true;
            };
            Instance.ActivateWithConfiguration(new YandexAppMetricaConfig(apiKey)
            {
                SessionTimeout = (int)sessionTimeoutSec,
                Logs = logs,
                HandleFirstActivationAsUpdate = handleFirstActivationAsUpdate,
                StatisticsSending = statisticsSending,
                LocationTracking = locationTracking,
                UserProfileID = settings.playerSettings.UserId,
            }); 
            
#if UNITY_EDITOR
            onEnd.Invoke();
#endif
        }

        public void TrackEvent(string eventName, Dictionary<string, object> values)
        {
            try
            {            
                Instance.ReportEvent(eventName, values);
            }
            catch
            {
                Debug.LogError($"[Mamboo] Cannot send event {eventName} to Appmetrica");
            }
        }
        
        public void TrackAdRevenue(double value, string currency, MambooAdFormat adFormat, string adSource)
        {
            try
            {
                AppMetrica.Instance.ReportAdRevenue(new YandexAppMetricaAdRevenue(value, currency)
                {
                    AdType = adFormat == MambooAdFormat.Interstitial 
                        ? YandexAppMetricaAdRevenue.AdTypeEnum.Interstitial 
                        : YandexAppMetricaAdRevenue.AdTypeEnum.Rewarded,
                    AdNetwork = adSource
                });
            }
            catch
            {
                Debug.LogError($"[Mamboo] Cannot send ad revenue event to Appmetrica");
            }
        }

        public void TrackPurchase(PurchaseMetadata purchaseMetadata)
        {
            try
            {            
                var appmetricaRevenue = new YandexAppMetricaRevenue(Convert.ToDecimal(purchaseMetadata.UnitPrice),
                    purchaseMetadata.CurrencyCode)
                {
                    ProductID = purchaseMetadata.ProductId,
                    Quantity = 1,
                    Receipt = new YandexAppMetricaReceipt
                    {
                        TransactionID = purchaseMetadata.TransactionId,
                        Data = purchaseMetadata.PurchaseTime,
                        Signature = purchaseMetadata.Receipt
                    }
                };
                AppMetrica.Instance.ReportRevenue(appmetricaRevenue);
            }
            catch
            {
                Debug.LogError($"[Mamboo] Cannot send purchase to Appmetrica");
            }
        }

        public void PauseSession()
        {
            Instance.PauseSession();
        }

        public void ResumeSession()
        {
            Instance.ResumeSession();
        }
    }
}
#endif