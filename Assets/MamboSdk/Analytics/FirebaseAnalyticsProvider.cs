#if mamboo_firebase
using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Firebase.Crashlytics;
using MamboSdk.Ads;
using MamboSdk.Settings;
using UnityEngine;

namespace MamboSdk.Analytics
{
    public class FirebaseAnalyticsProvider: IMambooAnalyticsService
    {
        internal static Firebase.FirebaseApp app;            
        private bool initialized;
            
        public bool isInitialized() => initialized;

        public void Initialize(MambooSdkSettings settings, Action onEnd)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(t =>
            {
                var dependencyStatus = t.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                    app = Firebase.FirebaseApp.DefaultInstance;
                }
                else
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }).ContinueWith(q => 
            { 
                onEnd.Invoke();
                initialized = true;
            });
        }

        public void TrackEvent(string eventName, Dictionary<string, object> values)
        {
            try
            {
                Debug.Log($"[Mamboo] Sending event {eventName} to Firebase...");
                var parameters = new List<Parameter>();
                foreach (var eventValue in values)
                {
                    try
                    {
                        switch (eventValue.Value)
                        {
                            case double d:
                                parameters.Add(new Parameter(eventValue.Key, d));
                                break;
                            case long l:
                                parameters.Add(new Parameter(eventValue.Key, l));
                                break;
                            case string s:
                                parameters.Add(new Parameter(eventValue.Key, s));
                                break;
                            case int i:
                                parameters.Add(new Parameter(eventValue.Key, i));
                                break;
                            default:
                                parameters.Add(new Parameter(eventValue.Key, Convert.ToString(eventValue.Value)));
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError($"[Mamboo] Cannot add parameter {eventValue.Key} to Firebase. Exception: {ex.Message}");
                    }
                }
                FirebaseAnalytics.LogEvent(eventName, parameters.ToArray());
            }
            catch(Exception ex)
            {
                Debug.LogError($"[Mamboo] Cannot send event {eventName} to Firebase. Exception: {ex.Message}");
            }
        }

        public void TrackPurchase(PurchaseMetadata purchaseMetadata)
        {
        }

        public void TrackAdRevenue(double value, string currency, MambooAdFormat adFormat, string adSource)
        {
        }

        public void PauseSession()
        {
        }

        public void ResumeSession()
        {
        }
    }
}
#endif