#if mamboo_appodeal_mediation
using System;
using System.Collections.Generic;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using com.adjust.sdk;
using MamboSdk.Analytics;
using MamboSdk.Settings;
using UnityEngine;

namespace MamboSdk.Ads
{
    public class AppodealAdsManager: IAdsService
    {
        public string PlatformName() => "Appodeal";
        public bool Initialized = false;
        public event Action OnBannerLoaded;
        public event Action OnRewardAvailable;
        public event Action<ImpressionData> OnRewardShow;
        public event Action OnRewardReceived;
        public event Action OnRewardHidden;
        public event AdsFailDelegate OnRewardFailed;
        public event AdsFailDelegate OnRewardDisplayFailed;
        public event Action OnInterstitialAvailable;
        public event Action<ImpressionData> OnInterstitialShow;
        public event Action OnInterstitialHidden;
        public event AdsFailDelegate OnInterstitialFailed;
        public event AdsFailDelegate OnInterstitialDisplayFailed;
        public void Initialize(MambooSdkSettings settings, Action onInitialized)
        {
            Appodeal.SetTabletBanners(true);
#if mamboo_qa_build
            Appodeal.SetLogLevel(AppodealLogLevel.Verbose);
            Appodeal.SetAutoCache(AppodealAdType.Interstitial, true);
            Appodeal.SetAutoCache(AppodealAdType.RewardedVideo, true);
#endif

            var adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner | AppodealAdType.RewardedVideo;
            AppodealCallbacks.Sdk.OnInitialized += (sender, args) =>
            {
                Debug.Log("[Mamboo] AppodealAdsManager Initialization Finished");
                InitializationInterstitialEvents();
                InitializationRewardEvents();
                InitializationBannerEvents();
                Initialized = true;
                onInitialized?.Invoke();
            };
            Appodeal.Initialize(settings.adsSettings.AppodealAppKey, adTypes);
        }

        public bool IsInitialized()
        {
#if UNITY_EDITOR
            return true;
#endif
            return Initialized;
        }

        #region Init Banner Events
        private void InitializationBannerEvents()
        {
            AppodealCallbacks.Banner.OnClicked += BannerOnClicked;
            AppodealCallbacks.Banner.OnLoaded += BannerIsLoaded;
        }

        private void BannerOnClicked(object sender, EventArgs e)
        {
            MambooAnalyticsWrapper.Instance.TrackEvent("crosspromo_clicked", new Dictionary<string, object>
            {
                { "format", "banner" }
            });
        }

        private void BannerIsLoaded(object sender, BannerLoadedEventArgs e)
        {
            OnBannerLoaded?.Invoke();
        }

        public void ShowBanner()
        {
            if (Appodeal.IsLoaded(AppodealAdType.Banner))
            {
                Appodeal.Show(AppodealShowStyle.BannerBottom);
            }
        }

        public void HideBanner()
        {
            Appodeal.Hide(AppodealAdType.Banner);
        }

        public void DestroyBanner()
        {
            Appodeal.Destroy(AppodealAdType.Banner);
        }

        #endregion


#region Init Interstitial Events
        private void InitializationInterstitialEvents()
        {
            AppodealCallbacks.Interstitial.OnShown += OnInterstitialDismissedEvent;
            AppodealCallbacks.Interstitial.OnClicked += OnInterstitialClicked;
            AppodealCallbacks.Interstitial.OnShowFailed += (sender, args) => 
                OnInterstitialDisplayFailed?.Invoke("inter", new AdsFailInfo());
        }

        private void OnInterstitialDismissedEvent(object sender, EventArgs eventArgs)
        {
            OnInterstitialHidden?.Invoke();
            MambooAnalyticsWrapper.Instance.TrackEvent("crosspromo_shown", new Dictionary<string, object>
            {
                { "format", "interstitial" },
            });
        }

        private void OnInterstitialClicked(object sender, EventArgs eventArgs)
        {
            MambooAnalyticsWrapper.Instance.TrackEvent("crosspromo_clicked", new Dictionary<string, object>
            {
                { "format", "interstitial" },
            });
        }
        
        public void LoadInterstitial()
        {
        }

        public bool HasInterstitial()
        {
            return Appodeal.IsLoaded(AppodealAdType.Interstitial);
        }

        public void ShowInterstitial(string placement)
        {
            if (!Appodeal.IsLoaded(AppodealAdType.Interstitial))
                return;

            Debug.Log("ShowInterstitialMethodCalled");
            Appodeal.Show(AppodealShowStyle.Interstitial);
        }
#endregion

#region Rewards
        private void InitializationRewardEvents()
        {
            AppodealCallbacks.RewardedVideo.OnFinished += (o, args) => ProcessingViewedRewardAds();
            AppodealCallbacks.RewardedVideo.OnClicked += OnRewardClicked;
            AppodealCallbacks.RewardedVideo.OnClosed += (sender, args) => OnRewardHidden?.Invoke();
            AppodealCallbacks.RewardedVideo.OnShowFailed += (sender, args) => OnRewardDisplayFailed?.Invoke("reward", new AdsFailInfo());
        }

        private void ProcessingViewedRewardAds()
        {
            MambooAnalyticsWrapper.Instance.TrackEvent("crosspromo_shown", new Dictionary<string, object>
            {
                { "format", "Reward" },
            });
            OnRewardReceived?.Invoke();
        }

        private void OnRewardClicked(object sender, EventArgs eventArgs)
        {
            MambooAnalyticsWrapper.Instance.TrackEvent("crosspromo_clicked", new Dictionary<string, object>
            {
                { "format", "Reward" },
            });
        }
        
        public void ShowReward(string placement)
        {
            if (!HasReward())
            {
                Debug.Log($"[Mamboo] HasReward -> false");
                return;
            }

            Appodeal.Show(AppodealShowStyle.RewardedVideo);
        }

        public void LoadReward()
        {
            throw new NotImplementedException();
        }

        public bool HasReward()
        {
            return Appodeal.IsLoaded(AppodealAdType.RewardedVideo);
        }
        
#endregion
        
    }
}
#endif