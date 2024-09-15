#if mamboo_is_mediation
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonAds;
using Mamboo.Ads;
using MamboSdk.Ads;
using MamboSdk.Analytics;
using MamboSdk.Settings;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

public class IronSourceAdsManager : IAdsService
{
    public string PlatformName() => "IronSource";
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
    private AmazonAdsAdapter _amazonAdsAdapter = new AmazonAdsAdapter();

    private bool bannerIsShowing = false;

    public void Initialize(MambooSdkSettings settings, Action onInitialized)
    {
#if UNITY_EDITOR
        Initialized = true;
        onInitialized?.Invoke();
        // return;
#endif
        _amazonAdsAdapter.Initialize(settings);

        IronSource.Agent.setManualLoadRewardedVideo(true);
        IronSource.Agent.init(settings.adsSettings.IronSourceAppKey);
        IronSource.Agent.shouldTrackNetworkState(true);


        IronSourceEvents.onSdkInitializationCompletedEvent += () =>
        {
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
            
#if mamboo_qa_build
        IronSource.Agent.validateIntegration();
#endif
#if DEV_BUILD
        IronSource.Agent.launchTestSuite();
#endif
            InitializeBannerAds();
            InitializeRewardedAds();
            InitializeInterstitialAds();
            Initialized = true;
            onInitialized?.Invoke();
        };
    }

    public bool IsInitialized()
    {
        return Initialized;
    }

    private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log($"IronSourceImpressionData: {impressionData}");
        if (impressionData.adUnit == "banner")
        {
            _amazonAdsAdapter.RequestAmazonBanner(
                onSuccess: response =>
                {
                    IronSource.Agent.setNetworkData(APSMediationUtils.APS_IRON_SOURCE_NETWORK_KEY, response);
                },
                onFailed: () => { });
        }
    }

    private void InitializeBannerAds()
    {
        IronSourceBannerEvents.onAdLoadedEvent += IronSourceBannerAdLoaded;
        IronSourceBannerEvents.onAdScreenDismissedEvent += IronSourceBannerClosed;
        IronSourceBannerEvents.onAdClickedEvent += BannerClickedEvent;
        
        LoadBanner();
    }

    private void IronSourceBannerClosed(IronSourceAdInfo obj)
    {
        LoadBanner();
    }

    private void LoadBanner()
    {
        _amazonAdsAdapter.RequestAmazonBanner(
            onSuccess: response =>
            {
                IronSource.Agent.setNetworkData(APSMediationUtils.APS_IRON_SOURCE_NETWORK_KEY, response);
                IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            },
            onFailed: () =>
            {
                IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            });
    }

    private void IronSourceBannerAdLoaded(IronSourceAdInfo adInfo)
    {
        if (bannerIsShowing == false) HideBanner();
    }
    
    void BannerClickedEvent(IronSourceAdInfo obj)
    {
        MambooAnalyticsWrapper.Instance.TrackEvent("iron_source_ads_clicked", new Dictionary<string, object>
        {
            { "format", "banner" }
        });
    }

    public bool HasReward()
    {
        Debug.Log($"[IronSource] isRewardedVideoAvailable = {IronSource.Agent.isRewardedVideoAvailable()}");
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    public bool HasInterstitial()
    {
        return IronSource.Agent.isInterstitialReady();
    }

    public void ShowBanner()
    {
        IronSource.Agent.displayBanner();
        bannerIsShowing = true;
    }

    public void HideBanner()
    {
        IronSource.Agent.hideBanner();
        bannerIsShowing = false;
    }

    public void DestroyBanner()
    {
        IronSource.Agent.destroyBanner();
        bannerIsShowing = false;
    }

    public void ShowInterstitial(string placement)
    {
        if (IronSource.Agent.isInterstitialReady())
            IronSource.Agent.showInterstitial(placement);
    }

    public void ShowReward(string placement)
    {
        Debug.Log($"[IronSource] isRewardedVideoAvailable = {IronSource.Agent.isRewardedVideoAvailable()}");
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo(placement);  
        }
    }

    private void InitializeInterstitialAds()
    {
        IronSourceInterstitialEvents.onAdReadyEvent += (adInfo) => OnInterstitialAvailable?.Invoke();
        IronSourceInterstitialEvents.onAdLoadFailedEvent += (ironSourceError) => OnInterstitialFailed?.Invoke("inter",
            new AdsFailInfo
            {
                Code = (ironSourceError.getErrorCode()).ToString(),
                Message = ironSourceError.getDescription(),
                Platform = PlatformName(),
                AdditionalInfo = null
            });
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialFailedToDisplayEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += OnInterstitialDismissedEvent;
        IronSourceInterstitialEvents.onAdOpenedEvent += (adInfo) =>
        {
            OnInterstitialShow?.Invoke(new ImpressionData
            {
                Revenue = adInfo.revenue ?? 0,
                AdFormat = MambooAdFormat.Interstitial,
                NetworkName = adInfo.adNetwork,
                AdUnitIdentifier = adInfo.adUnit
            });
        };
        LoadInterstitial();
    }

    private void InterstitialAdClickedEvent(IronSourceAdInfo obj)
    {
        MambooAnalyticsWrapper.Instance.TrackEvent("iron_source_ads_clicked", new Dictionary<string, object>
        {
            { "format", "interstitial" },
        });
    }

    private void InterstitialFailedToDisplayEvent(IronSourceError ironSourceError, IronSourceAdInfo ironSourceAdInfo)
    {
        OnInterstitialDisplayFailed?.Invoke("inter", new AdsFailInfo()
        {
            Code = (ironSourceError.getErrorCode()).ToString(),
            Message = ironSourceError.getDescription(),
            Platform = PlatformName(),
            AdditionalInfo = null
        });
        
        MambooAnalyticsWrapper.Instance.TrackEvent("iron_source_ads_shown", new Dictionary<string, object>
        {
            { "format", "interstitial" },
        });
    }

    private void OnInterstitialDismissedEvent(IronSourceAdInfo ironSourceAdInfo)
    {
        OnInterstitialHidden?.Invoke();
    }

    public void LoadInterstitial()
    {
        _amazonAdsAdapter.RequestAmazonInterstitialVideo(
            onSuccess: response =>
            {
                IronSource.Agent.setNetworkData(APSMediationUtils.APS_IRON_SOURCE_NETWORK_KEY, response);
                IronSource.Agent.loadInterstitial();
            }, 
            onFailed: () =>
            {
                IronSource.Agent.loadInterstitial();
            });

        IronSource.Agent.loadInterstitial();
    }

    private int _rewardRetryAttempt = 0;
    private void InitializeRewardedAds()
    {
        IronSourceRewardedVideoEvents.onAdReadyEvent += OnRewardedAdLoadedEvent;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += async (error, info) => await OnRewardedAdFailedToDisplayEvent(error, info);
        IronSourceRewardedVideoEvents.onAdClosedEvent += OnRewardedAdDismissedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += OnRewardedAdReceivedRewardEvent;
        IronSourceRewardedVideoEvents.onAdOpenedEvent += OnRewardedAdDisplayEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += OnRewardedAdClickedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += OnRewardAvailableEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent += async error => await OnAdLoadFailedEvent(error);
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += async () => await OnAdUnavailableEvent();
        
        LoadReward();
    }

    private void OnRewardedAdClickedEvent(IronSourcePlacement arg1, IronSourceAdInfo arg2)
    {
        MambooAnalyticsWrapper.Instance.TrackEvent("iron_source_ads_clicked", new Dictionary<string, object>
        {
            { "format", "Reward" },
        });
    }

    private async Task OnAdUnavailableEvent()
    {
        Debug.Log("[IronSource] Reward Unavailable");
        _rewardRetryAttempt++;
        var retryDelay = Math.Pow(2, Math.Min(6, _rewardRetryAttempt));

        await Task.Delay(TimeSpan.FromSeconds(retryDelay));
        LoadReward();
    }

    private async Task OnAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log($"[IronSource] Reward load failed {error.getDescription()}");
        _rewardRetryAttempt++;
        var retryDelay = Math.Pow(2, Math.Min(6, _rewardRetryAttempt));

        await Task.Delay(TimeSpan.FromSeconds(retryDelay));
        LoadReward();
    }

    public void LoadReward()
    {
        _amazonAdsAdapter.RequestAmazonRewardedVideo(
            onSuccess: response => {
            IronSource.Agent.setNetworkData(APSMediationUtils.APS_IRON_SOURCE_NETWORK_KEY, response);
            IronSource.Agent.loadRewardedVideo();}, 
            onFailed: () => IronSource.Agent.loadRewardedVideo());
    }

    private void OnRewardedAdLoadedEvent(IronSourceAdInfo ironSourceAdInfo)
    {
        Debug.Log($"[IronSource] OnRewardedAdLoadedEvent {ironSourceAdInfo}");
        _rewardRetryAttempt = 0;
    }

    private void OnRewardAvailableEvent(IronSourceAdInfo ironSourceAdInfo)
    {
        _rewardRetryAttempt = 0;
        Debug.Log("[IronSource] Reward loaded");
        OnRewardAvailable?.Invoke();
    }

    private void OnRewardedAdDisplayEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log($"[IronSource] OnRewardedAdDisplayEvent");
        OnRewardShow?.Invoke(new ImpressionData
        {
            Revenue = adInfo.revenue ?? 0,
            AdFormat = MambooAdFormat.Reward,
            NetworkName = adInfo.adNetwork,
            AdUnitIdentifier = adInfo.adUnit
        });
        
        MambooAnalyticsWrapper.Instance.TrackEvent("iron_source_ads_shown", new Dictionary<string, object>
        {
            { "format", "Reward" },
        });
    }

    private async Task OnRewardedAdFailedToDisplayEvent(IronSourceError ironSourceError, IronSourceAdInfo ironSourceAdInfo)
    {
        Debug.Log($"[IronSource] OnRewardedAdFailedToDisplayEvent");
        OnRewardDisplayFailed?.Invoke("reward", new AdsFailInfo()
        {
            Code = (ironSourceError.getErrorCode()).ToString(),
            Message = ironSourceError.getDescription(),
            Platform = PlatformName(),
            AdditionalInfo = null
        });
        
        _rewardRetryAttempt++;
        var retryDelay = Math.Pow(2, Math.Min(6, _rewardRetryAttempt));

        await Task.Delay(TimeSpan.FromSeconds(retryDelay));
        LoadReward();
    }

    private void OnRewardedAdDismissedEvent(IronSourceAdInfo ironSourceAdInfo)
    {
        Debug.Log($"[IronSource] OnRewardedAdDismissedEvent");
        // Rewarded ad is hidden. Pre-load the next ad
        OnRewardHidden?.Invoke();
        LoadReward();
    }

    private void OnRewardedAdReceivedRewardEvent(IronSourcePlacement ironSourcePlacement,
        IronSourceAdInfo ironSourceAdInfo)
    {
        Debug.Log($"[IronSource] OnRewardedAdReceivedRewardEvent");
        OnRewardReceived?.Invoke();
        LoadReward();
    }

}

#region Facebook

namespace AudienceNetwork
{
    public static class AdSettings
    {
        public static void SetDataProcessingOptions(string[] dataProcessingOptions)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass adSettings = new AndroidJavaClass("com.facebook.ads.AdSettings");
            adSettings.CallStatic("setDataProcessingOptions", (object)dataProcessingOptions);
#endif

#if UNITY_IOS && !UNITY_EDITOR
            FBAdSettingsBridgeSetDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length);
#endif
        }

        public static void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass adSettings = new AndroidJavaClass("com.facebook.ads.AdSettings");
            adSettings.CallStatic("setDataProcessingOptions", (object)dataProcessingOptions, country, state);
#endif

#if UNITY_IOS && !UNITY_EDITOR
            FBAdSettingsBridgeSetDetailedDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length,
                country, state);
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDataProcessingOptions(string[] dataProcessingOptions,
            int length);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDetailedDataProcessingOptions(string[] dataProcessingOptions,
            int length, int country, int state);
#endif

        public static void SetAdTracking()
        {
#if UNITY_IOS && !UNITY_EDITOR
            bool advertiserTrackingEnabled =
                IDFATracking.IDFATracking.GetCurrentStatus() == IDFATracking.Status.Authorized;

            // ATE FB Audience network
            SetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
#endif
        }
#if UNITY_IOS && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        private static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
#endif
    }
}

#endregion

#endif
