using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.adjust.sdk;
using MamboSdk.Analytics;
using MamboSdk.RemoteConfig;
using MamboSdk.Settings;
using Unity.Collections;
using UnityEngine;

namespace MamboSdk.Core
{
    [RequireComponent(typeof(PlayTimeManager))]
    public class MambooSdkCore : MonoBehaviour
    {
        private string TAG = "MambooSdkCore";
        public static bool Initialized;


        public static MambooSdkCore Instance;
        public RemoteConfigWrapper RemoteConfigWrapper;
        public PlayTimeManager PlayTimeManager { get; private set; }
        private static MambooSdkSettings mambooSdkSettings;
        private MambooAnalyticsWrapper analyticsWrapper = MambooAnalyticsWrapper.Instance;
        private bool _actualPauseStatus;
        
        private void Awake()
        {
            if (transform != transform.root)
                throw new Exception("[Mamboo SDK] Mamboo prefab must be at the Root level.");

            mambooSdkSettings = MambooSdkSettings.Load();
            if (mambooSdkSettings == null)
                throw new Exception("[Mamboo SDK] Cannot find Mamboo settings file.");

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(this);

            PlayTimeManager = GetComponent<PlayTimeManager>();
        }
        private void Start()
        {
            InitPlayerSettings();
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            var analyticsInitTask = analyticsWrapper.Init(mambooSdkSettings);
            yield return new WaitUntil(() => analyticsInitTask.IsCompleted);

            RemoteConfigWrapper = new RemoteConfigWrapper(mambooSdkSettings);
            yield return new WaitUntil(() => RemoteConfigWrapper.IsAllProviderInitialized());
            
            
            Ads.AdsManager.Instance.Initialize(mambooSdkSettings, () => { mambooSdkSettings.AdsInitialized = true; });
            
            Initialized = true;
        }

        private void Update()
        {
            if (!analyticsWrapper.IsInitialized())
            {
                MambooLog.LogWarning(TAG,"AnalyticsWrapper is NOT initialized");
                return;
            };
            
            analyticsWrapper.HandleEvents();
            analyticsWrapper.HandlePurchases();
        }
        
        private void InitPlayerSettings()
        {
            var guid = PlayerPrefs.GetString("mamboo_user_id", null);
            if (string.IsNullOrEmpty(guid))
            {
                guid = Adjust.getAmazonAdId()?.ToLowerInvariant() ?? Adjust.getAdid()?.ToLowerInvariant();
                Debug.Log($"guid = ${guid}");
                PlayerPrefs.SetString("mamboo_user_id", guid);
            }
            mambooSdkSettings.playerSettings.UserId = guid;
            mambooSdkSettings.onAttributionChanged = (network, campaign, creative) =>
            {
                MambooLog.LogDebug(TAG,$"Attribution: network={network} campaign={campaign} creative={creative}");
                analyticsWrapper.TrackEvent("attribution_info", new Dictionary<string, object>
                {
                    {"network", network},
                    {"campaign", campaign},
                    {"creative", creative}
                });
            };
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (_actualPauseStatus != pauseStatus)
            {
                _actualPauseStatus = pauseStatus;
                if (pauseStatus)
                {
                    analyticsWrapper.PauseSession();
                }
                else
                {
                    analyticsWrapper.ResumeSession();
                }
            }
        }
    }
}