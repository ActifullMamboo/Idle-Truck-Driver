using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MamboSdk.Ads;
using MamboSdk.Settings;
using UnityEngine;

namespace MamboSdk.Analytics
{
    public class MambooAnalyticsWrapper
    {
        public static MambooAnalyticsWrapper Instance => instance ?? (instance = new MambooAnalyticsWrapper());
        private static MambooAnalyticsWrapper instance;
        private List<IMambooAnalyticsService> analyticsServices;
        private Queue<EventModel> eventQueue;
        private Queue<AdRevenueEventModel> adRevenueEventQueue;
        private Queue<PurchaseMetadata> purchaseQueue;
        private bool initialized;

        public MambooAnalyticsWrapper()
        {
            eventQueue = new Queue<EventModel>();
            adRevenueEventQueue = new Queue<AdRevenueEventModel>();
            purchaseQueue = new Queue<PurchaseMetadata>();
            analyticsServices = new List<IMambooAnalyticsService>
            {
#if mamboo_firebase
                new FirebaseAnalyticsProvider(),
#endif
#if mamboo_appmetrica
                new AppmetricaAnalyticsProvider(),
#endif
                new AdjustProvider()
            };
        }

        public Task Init(MambooSdkSettings settings)
        {
            var completionSource = new TaskCompletionSource<bool>();
            
            var servicesToInitialize = analyticsServices.Count;
            if (servicesToInitialize == 0)
            {
                completionSource.SetResult(true);
                return completionSource.Task;
            }

            void InitializationCompleted()
            {
                servicesToInitialize--;
                if (servicesToInitialize == 0)
                {
                    completionSource.SetResult(true);
                }
            }

            foreach (var mambooAnalyticsService in analyticsServices)
            {
                mambooAnalyticsService.Initialize(settings, () =>
                {
                    Debug.Log($"{mambooAnalyticsService.GetType()} is finished");
                    InitializationCompleted();
                });
            }
            
            return completionSource.Task;
        }

        public bool IsInitialized()
        {
#if UNITY_EDITOR
            return true;
#endif
            
            if (initialized)
                return true;

            var allInitialized = analyticsServices.All(x => x.isInitialized());
            if (!allInitialized) return false;
            
            initialized = true;
            return true;

        }

        public void TrackPurchase(PurchaseMetadata metadata)
        {
            purchaseQueue.Enqueue(metadata);
        }

        public void TrackEvent(string eventName, Dictionary<string, object> values)
        {
            eventQueue.Enqueue(new EventModel
            {
                EventName = eventName,
                Values = values
            });
        }

        public void TrackAdRevenue(double value, string currency, MambooAdFormat adFormat, string adSource)
        {
            adRevenueEventQueue.Enqueue(new AdRevenueEventModel
            {
                Value = value,
                Currency = currency,
                AdFormat = adFormat,
                AdSource = adSource
            });
        }

        public void HandlePurchases()
        {
            if (!initialized)
                return;
            
            try
            {
                while (purchaseQueue.Count > 0)
                {
                        Debug.Log($"[Mamboo] Sending purchase to analytics...");
                        var purchase = purchaseQueue.Dequeue();
                    
                        foreach (var mambooAnalyticsService in analyticsServices)
                        {
                            mambooAnalyticsService.TrackPurchase(purchase);
                        }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Mamboo] Error on sending purchase to analytics...");
                Debug.LogError($"[Mamboo] {e.Message}");
            }
        }

        public void HandleEvents()
        {
            if (!initialized)
                return;
            
            try
            {
                while (eventQueue.Count > 0)
                {
                    var eventModel = eventQueue.Dequeue();
                
                    Debug.Log($"[Mamboo] Sending event {eventModel.EventName} to analytics...");
                    foreach (var mambooAnalyticsService in analyticsServices)
                    {
                        mambooAnalyticsService.TrackEvent(eventModel.EventName, eventModel.Values);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Mamboo] Error on sending event to analytics...");
                Debug.LogError($"[Mamboo] {e.Message}");
            }
        }

        public void HandleAdRevenueEvents()
        {
            if (!initialized)
                return;
            
            try
            {
                while (adRevenueEventQueue.Count > 0)
                {
                    var adRevenueEventModel = adRevenueEventQueue.Dequeue();
                
                    Debug.Log($"[Mamboo] Sending ad revenue event to analytics...");
                    foreach (var mambooAnalyticsService in analyticsServices)
                    {
                        mambooAnalyticsService.TrackAdRevenue(adRevenueEventModel.Value,adRevenueEventModel.Currency, 
                            adRevenueEventModel.AdFormat, adRevenueEventModel.AdSource);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Mamboo] Error on sending ad revenue event to analytics...");
                Debug.LogError($"[Mamboo] {e.Message}");
            }
        }

        public void PauseSession()
        {
            foreach (var mambooAnalyticsService in analyticsServices)
            {
                mambooAnalyticsService.PauseSession();
            }
        }

        public void ResumeSession()
        {
            foreach (var mambooAnalyticsService in analyticsServices)
            {
                mambooAnalyticsService.ResumeSession();
            }
        }
        
        private class EventModel
        {
            public string EventName { get; set; }
            public Dictionary<string, object> Values { get; set; }
        }
        
        private class AdRevenueEventModel
        {
            public double Value { get; set; }
            public string Currency { get; set; } 
            public MambooAdFormat AdFormat { get; set; } 
            public string AdSource { get; set; } 
        }
    }

}