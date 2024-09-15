using System;
using System.Collections.Generic;
using com.adjust.sdk;
using MamboSdk.Ads;
using MamboSdk.Settings;
using Newtonsoft.Json;
using UnityEngine;

namespace MamboSdk.Analytics
{
    public class AdjustProvider : IMambooAnalyticsService
    {
        private Dictionary<string, string> eventsTokens = new Dictionary<string, string>();
        private bool initialized;
        
        public bool isInitialized() => initialized;
        public void Initialize(MambooSdkSettings settings, Action onEnd)
        {
            try
            {
                eventsTokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.adjustSettings.eventTokensJson);
                this.InitAdjust(settings);
            
                onEnd.Invoke();
                initialized = true;
                Debug.Log($"[Mamboo SDK] Adjust is initialized");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Mamboo SDK] Adjust is NOT initialized. Exception: {e.Message}");
            }
        }


        private void InitAdjust(MambooSdkSettings settings)
        {
            var config = new AdjustConfig(settings.adjustSettings.appToken, settings.adjustSettings.environment, true);
            config.setLogLevel(AdjustLogLevel.Suppress);
            config.setPreinstallTrackingEnabled(true);
            config.setExternalDeviceId(settings.playerSettings.UserId);            
            config.setAttributionChangedDelegate(attr =>
            {
                settings.onAttributionChanged.Invoke(attr.network, attr.campaign, attr.creative);
            });
            Adjust.start(config);
        }
        
        public void TrackEvent(string eventName, Dictionary<string, object> values)
        {
            try
            {            
                if (eventsTokens.TryGetValue(eventName, out var token))
                    Adjust.trackEvent(new AdjustEvent(token));
                else
                    Debug.LogWarning($"[Mamboo] Cannot get token for {eventName} event. Adjust");
            }
            catch
            {
                Debug.LogError($"[Mamboo] Cannot send event {eventName} to Adjust");
            }
        }

        public void TrackPurchase(PurchaseMetadata metadata)
        {
            var transationId = metadata.TransactionId;

            if (eventsTokens.TryGetValue("purchase", out var token))
            {
                var adjustEvent = new AdjustEvent(token);
                adjustEvent.setRevenue(metadata.UnitPrice, metadata.CurrencyCode);
                adjustEvent.setTransactionId(transationId);
                Adjust.trackEvent(adjustEvent);
            }
            else
                Debug.LogError($"[Mamboo] Cannot get token for purchase event. Adjust");
        }

        public void TrackAdRevenue(double value, string currency, MambooAdFormat adFormat, string adSource)
        {
        }

        public void PauseSession()
        {
#if UNITY_ANDROID && UNITY_EDITOR == false
            AdjustAndroid.OnPause();
#endif
        }

        public void ResumeSession()
        {
#if UNITY_ANDROID && UNITY_EDITOR == false
            AdjustAndroid.OnResume();
#endif
        }
    }
}