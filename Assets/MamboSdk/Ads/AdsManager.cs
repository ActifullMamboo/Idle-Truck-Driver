using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MamboSdk.Analytics;
using MamboSdk.Core;
using MamboSdk.Settings;
using Newtonsoft.Json;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace MamboSdk.Ads
{
    public class AdsManager
    {
        private const string TAG = "MamboSdk-AdsManager";
        
        public bool IsInitialized = false;
        public static AdsManager Instance => instance ?? (instance = new AdsManager());
        private static AdsManager instance;

        private Action<bool> rewardOnCompleteAction;
        private Action rewardOnHiddenAction;
        
        private Action interstitialOnHiddenAction;

        public event Action<ImpressionData> OnRewardShownEvent;
        public event Action<ImpressionData> OnInterstitialShownEvent;
        
        public event Action OnRewardAvailable;

        public Dictionary<MambooAdFormat, AdProvider> AdFormatProvidersDict => mediationManager.GetAdFormatMediation();
        public Dictionary<AdProvider, IAdsService> AdProvidersDict;
        public bool IsRewardAvailable => AdProvidersDict?.Any(pair => pair.Value.HasReward()) ?? false;
        public bool IsIntersitialAvailable => AdProvidersDict?.Any(pair => pair.Value.HasReward()) ?? false;
        private MediationManager mediationManager;
        private bool bannerIsShowing;

        public void Initialize(MambooSdkSettings mambooSettings, Action onInitialized)
        {
            mediationManager = new MediationManager();
            mediationManager.Initialize();
            mediationManager.OnRefreshBanner += RefreshBanner;

            AdProvidersDict = new Dictionary<AdProvider, IAdsService>
            {
#if mamboo_appodeal_mediation
                { AdProvider.Appodeal, new AppodealAdsManager() },
#endif
#if mamboo_is_mediation
                { AdProvider.IronSource, new IronSourceAdsManager() },
#endif
#if UNITY_EDITOR
                { AdProvider.Dummy, new DummyAdsManager() }
#endif
            };
            InitializeAdServices(mambooSettings, onInitialized);
        }
    

        public void ShowInterstitial(string placement, Action onClose = null)
        {
            MambooLog.LogDebug(TAG, "Try Show Interstitial");
            interstitialOnHiddenAction = onClose;
            
            if (IsIntersitialAvailable)
            {
                var adService = GetAdService(MambooAdFormat.Interstitial);
                
                if (adService?.HasInterstitial() ?? false)
                {
                    MambooLog.LogDebug(TAG, "Show Interstitial");
                    adService?.ShowInterstitial(placement);
                }
                else
                {
                    MambooLog.LogDebug(TAG, "Show Interstitial IS");
                    var ironSourceAdsProvider = AdProvidersDict[AdProvider.IronSource];
                    if (ironSourceAdsProvider.HasInterstitial() == false)
                    {
                        ThreadSaveInvoke(() => onClose?.Invoke());
                        return;
                    }

                    ironSourceAdsProvider?.ShowInterstitial(placement);
                }
            }
            else
            {
                MambooLog.LogDebug(TAG,"End Show Interstitial because there's no advertising available ");
                ThreadSaveInvoke(() => interstitialOnHiddenAction?.Invoke());
            }
        }

        public void ShowReward(string placement, Action<bool> onComplete, Action onClose = null)
        {
            MambooLog.LogDebug(TAG,"Try Show Reward");
            rewardOnCompleteAction = onComplete;
            rewardOnHiddenAction = onClose;

            if (IsRewardAvailable)
            {
                var adService = GetAdService(MambooAdFormat.Reward);
                if (adService?.HasReward() ?? false)
                {
                    MambooLog.LogDebug(TAG,"Show Reward");
                    adService?.ShowReward(placement);
                }
                else
                {
                    MambooLog.LogDebug(TAG, "Show Reward IS");
                    var ironSourceAdsProvider = AdProvidersDict[AdProvider.IronSource];
                    if (ironSourceAdsProvider.HasReward() == false)
                    {
                        ThreadSaveInvoke(() => onClose?.Invoke());
                        return;
                    }

                    ironSourceAdsProvider?.ShowReward(placement);
                }
            }
            else
            {
                MambooLog.LogDebug(TAG,"End Show Reward because there's no advertising available");
                ThreadSaveInvoke(() =>
                {
                    onComplete?.Invoke(false);
                    onClose?.Invoke();
                });
            }
        }

        public void ShowBanner()
        {
            if (bannerIsShowing)
            {
                return;
            }
            
            var adService = GetAdService(MambooAdFormat.Banner);
            MambooLog.LogDebug(TAG,"Show Banner");
            adService?.ShowBanner();
            
            bannerIsShowing = true;
        }

        public void HideBanner()
        {
            var adService = GetAdService(MambooAdFormat.Banner);
            MambooLog.LogDebug(TAG,"Hide Banner");
            adService?.HideBanner();
            
            bannerIsShowing = false;
        }

        public void DestroyBanner()
        {
            var adService = GetAdService(MambooAdFormat.Banner);
            MambooLog.LogDebug(TAG,"Hide Banner");
            adService?.DestroyBanner();

            bannerIsShowing = false;
        }

        public void RefreshBanner(AdProvider oldAdProviderType, AdProvider newAdProviderType)
        {
            AdProvidersDict.TryGetValue(oldAdProviderType, out var oldAdProvider);
            AdProvidersDict.TryGetValue(newAdProviderType, out var newAdProvider);

            oldAdProvider?.DestroyBanner();
            if (bannerIsShowing) newAdProvider?.ShowBanner();
        }

        private void InitializeAdServices(MambooSdkSettings mambooSettings, Action onInitialized)
        {
            foreach (var adsServicePair in AdProvidersDict)
            {
                var adsService = adsServicePair.Value;
                adsService.Initialize(mambooSettings, () =>
                {
                    MambooLog.LogDebug(TAG,$"{adsService.PlatformName()} is initialized");
                    if (!AdProvidersDict.All(x => x.Value.IsInitialized()))
                    {
                        return;
                    };
                    IsInitialized = true;
                    
                    MambooLog.LogDebug(TAG,$"all is initialized");
                    onInitialized.Invoke();
                });

                AssignInterstitialCallbacks(adsService);
                AssignRewardCallbacks(adsService);
            }
        }

        private void AssignRewardCallbacks(IAdsService adsService)
        {
            adsService.OnRewardReceived += () =>
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardReceived");
                ThreadSaveInvoke(() => rewardOnCompleteAction?.Invoke(true));
            };
            adsService.OnRewardHidden += () =>
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardHidden");
                ThreadSaveInvoke(() => rewardOnHiddenAction?.Invoke());
            };
            adsService.OnRewardDisplayFailed += (format, info) =>
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardDisplayFailed");
                ThreadSaveInvoke(() => rewardOnCompleteAction?.Invoke(false));
                OnRewardLoadFail(format, info);
            };
            adsService.OnRewardAvailable += () =>
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardAvailable");
                OnRewardLoaded();
            };
            adsService.OnRewardShow += data =>
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardShow");
                OnRewardShown(data);
            };
            adsService.OnRewardFailed += (format, info) => 
            {
                MambooLog.LogDebug(TAG,$"{adsService.GetType()} OnRewardFailed");
                OnRewardLoadFail(format, info);
            };
        }

        private void AssignInterstitialCallbacks(IAdsService adsService)
        {
            adsService.OnInterstitialAvailable += OnInterstitialLoaded;
            adsService.OnInterstitialShow += OnInterstitialShown;
            adsService.OnInterstitialHidden += () =>
            {
                ThreadSaveInvoke(() => interstitialOnHiddenAction?.Invoke());
                adsService.LoadInterstitial();
            };
            adsService.OnInterstitialFailed += OnInterstitialLoadFail;
            adsService.OnInterstitialDisplayFailed += (format, info) => 
            {
                ThreadSaveInvoke(() => interstitialOnHiddenAction?.Invoke());
                OnInterstitialLoadFail(format, info);
            };
        }

        private IAdsService GetAdService(MambooAdFormat adFormat)
        {
            if (AdFormatProvidersDict == null || !AdFormatProvidersDict.TryGetValue(adFormat, out var adServiceType)) return null;
            if (AdProvidersDict != null && AdProvidersDict.TryGetValue(adServiceType, out var adService)) return adService;
            
            MambooLog.LogDebug(TAG,$"Cannot get AdService {adServiceType}");
            return null;
        }

        private int rewardRetryAttempt;
        private int interRetryAttempt;

        private void OnInterstitialLoadFail(string adFormat, AdsFailInfo failInfo)
        {
            interRetryAttempt++;
            var adService = GetAdService(MambooAdFormat.Interstitial);
            TrackAndRetryFail(interRetryAttempt, ()=> adService?.LoadInterstitial(), adFormat, failInfo);
        }
        
        private void OnRewardLoadFail(string adFormat, AdsFailInfo failInfo)
        {
            rewardRetryAttempt++;
            var adService = GetAdService(MambooAdFormat.Interstitial);
            TrackAndRetryFail(rewardRetryAttempt, ()=> adService?.LoadReward(), adFormat, failInfo);
        }

        private void TrackAndRetryFail(int retryAttempt, Action retryAction, string adFormat, AdsFailInfo failInfo)
        {
            MambooAnalyticsWrapper.Instance.TrackEvent("ad_load_fail", new Dictionary<string, object>
            {
                {"adFormat", adFormat},
                {"message", failInfo.Message},
                {"code", failInfo.Code},
                {"additionalInfo", JsonConvert.SerializeObject(failInfo.AdditionalInfo)}
            });
            Task.Run(async () =>{ await WaitDelay(retryAttempt); })
                .ContinueWith(task => { retryAction?.Invoke(); }, TaskScheduler.FromCurrentSynchronizationContext() );
        }
        
        private void OnInterstitialShown(ImpressionData impressionData)
        {
            var adService = GetAdService(MambooAdFormat.Interstitial);
            adService?.LoadInterstitial();
            OnInterstitialShownEvent?.Invoke(impressionData);
        }
        
        private void OnRewardShown(ImpressionData impressionData)
        {
            var adService = GetAdService(MambooAdFormat.Reward);
            adService?.LoadReward();
            OnRewardShownEvent?.Invoke(impressionData);
        }
        
        private void OnInterstitialLoaded()
        {
            interRetryAttempt = 0;
        }
        
        private void OnRewardLoaded()
        {
            rewardRetryAttempt = 0;
            OnRewardAvailable?.Invoke();
        }
        
        private static void ThreadSaveInvoke(Action action){
            UnityMainThreadDispatcher.Instance().Enqueue(() => action?.Invoke());
        }
        
        private static async Task WaitDelay(int retryAttempt)
        {
            var retryDelay = Math.Pow(2, Math.Min(8, retryAttempt));
            await Task.Delay((int)retryDelay * 1000);
        }
    }

    public enum AdProvider
    {
        Appodeal,
        IronSource,
        Dummy
    }
}