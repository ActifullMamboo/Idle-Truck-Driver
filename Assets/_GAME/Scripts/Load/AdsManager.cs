using System;
using System.Collections;
using System.Globalization;
using _GAME.Scripts;
using _Game.Scripts.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _GAME
{
    public class AdsManager : MonoBehaviour
    {
        private static bool _isInitialized;
        private readonly string interUnit = "3e111abfe0f8bd91";
        private readonly string rewardUnit = "908ff3b2721dbcaa";
        private readonly string banerUnit = "02496df909190eca";
        private int retryAttempt;
        public int steps = 0;

        private static bool _canShowInter = false;
        private static bool _isTimerEnded = false;
        private static Action OnDone;
        private static Action OnRewardWatched;

        public static void ActionDone()
        {
            if (!_isTimerEnded)
            {
                return;
            } 
            if (_adBreaks)
            {
                return;
            }

            _canShowInter = true;

            OnDone?.Invoke();            
        }

        public void Initialize()
        {
            //MaxSdk.SetHasUserConsent(true);
            if (!_isInitialized)
            {
                _isInitialized = true;
               // MaxSdkCallbacks.OnSdkInitializedEvent += MaxSdkCallbacksOnOnSdkInitializedEvent;

               /* MaxSdk.SetSdkKey(
                    "6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR");
                MaxSdk.InitializeSdk();*/
                DontDestroyOnLoad(gameObject);
                StartCoroutine(nameof(CounterRoutine));
                OnDone += ShowInterstitial;
                OnRewardWatched += ResetTimer;

            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void ResetTimer()
        {
            StopCoroutine(nameof(CounterRoutine));
            StopCoroutine(nameof(SingleTimer));
            StartCoroutine(nameof(CounterRoutine));
            timer.Deactivate();

        }

        private void MaxSdkCallbacksOnOnSdkInitializedEvent(/*MaxSdkBase.SdkConfiguration obj*/)
        {
            //MaxSdkCallbacks.OnSdkInitializedEvent -= MaxSdkCallbacksOnOnSdkInitializedEvent;

            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
        }

        public void ShowDebug()
        {
           // MaxSdk.ShowMediationDebugger();
        }

        #region Inter

        private float time = 40;
        private int id = 0;
        private IEnumerator CounterRoutine()
        {
            Debug.Log("SS");
            _canShowInter = false;
            _isTimerEnded = false;

            time = 40;
            while (true)
            {
                time -= Time.deltaTime;
                yield return null;
                if (time <= 0)
                {
                    _isTimerEnded = true;
                    StartCoroutine(nameof(SingleTimer));
                    break;
                }
            }
        }

        public GameObject timer;
        public TextMeshProUGUI Text;
        private static bool _adBreaks = false;
        private IEnumerator SingleTimer()
        {

            time = 20;
            while (true)
            {
                if (time <= 5)
                {
                    _adBreaks = true;
                    time = (int)time;
                    timer.Activate();
                    Text.text = "Ad break in : " + time.ToString(CultureInfo.InvariantCulture);

                    yield return new WaitForSeconds(1f);
                    time -= 1;
                    if (time < 1)
                    {
                        _adBreaks = false;

                        ActionDone();

                        break;
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                    yield return null;
                }
                
                
            }
        }

        private void ShowInterstitial() 
        {
            // var isReady = MaxSdk.IsInterstitialReady(interUnit);
            // AppMetricaEvents.VideoAdStarted(false, "interstitial", isReady, true);
            //
            // if (isReady)
            //     MaxSdk.ShowInterstitial(interUnit);
            // else
            //     MaxSdk.LoadInterstitial(interUnit);

        }


        private void InitializeInterstitialAds()
        {
            // Attach callback
            // MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            // MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            // MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            // MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            // MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            // MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            LoadInterstitial();
            steps++;
        }

        private void LoadInterstitial()
        {
            // MaxSdk.LoadInterstitial(interUnit);
        }


        // private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     AppMetricaEvents.VideoAdAvailable(false, adInfo.Placement, true, true);
        //     retryAttempt = 0;
        // }
        //
        // private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        // {
        //     // Interstitial ad failed to load 
        //     // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
        //
        //     retryAttempt++;
        //     double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));
        //
        //     Invoke("LoadInterstitial", (float)retryDelay);
        // }
        //
        // private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     AppMetricaEvents.VideoAdWatched(false, adInfo.Placement, true, true);
        //     ResetTimer();
        //
        //     LoadInterstitial();
        // }
        //
        // private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        //     MaxSdkBase.AdInfo adInfo)
        // {
        //     AppMetricaEvents.VideoAdWatched(false, adInfo.Placement, false, true);
        //     LoadInterstitial();
        // }
        //
        // private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        // }
        //
        // private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     // Interstitial ad is hidden. Pre-load the next ad.
        //     LoadInterstitial();
        // }

        #endregion

        #region Reward

        private Action OnRewardRecived;
        private void InitializeRewardedAds()
        {
            // Attach callback
            // MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            // MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            // MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            // MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            // MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            // MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            // MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first rewarded ad
            LoadRewardedAd();
            steps++;
        }

        private static Action _callback;
        public void ShowRewardAd(string placementName, Action callback)
        {
            // var isReady = MaxSdk.IsRewardedAdReady(rewardUnit);
            // AppMetricaEvents.VideoAdStarted(true, placementName, isReady, true);
            //
            // if (isReady)
            // {
            //     OnRewardRecived += callback;
            //     _callback = callback;
            //     MaxSdk.ShowRewardedAd(rewardUnit);
            //     OnRewardWatched?.Invoke();
            //
            // }
            // else
            //     LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
           // MaxSdk.LoadRewardedAd(rewardUnit);
        }

        // private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     AppMetricaEvents.VideoAdAvailable(true, adInfo.Placement, true, true);
        //
        //     retryAttempt = 0;
        // }
        //
        // private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        // {
        //     // Rewarded ad failed to load 
        //     // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
        //
        //     retryAttempt++;
        //     var retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
        //
        //     Invoke("LoadRewardedAd", (float)retryDelay);
        // }
        //
        // private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        // }
        //
        // private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        //     MaxSdkBase.AdInfo adInfo)
        // {
        //     AppMetricaEvents.VideoAdWatched(true, adInfo.Placement, false, true);
        //     LoadRewardedAd();
        // }
        //
        // private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        // }
        //
        // private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     // Rewarded ad is hidden. Pre-load the next ad
        //     LoadRewardedAd();
        // }
        //
        // private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward,
        //     MaxSdkBase.AdInfo adInfo)
        // {
        //     OnRewardRecived?.Invoke();
        //     OnRewardRecived -= _callback;
        //     AppMetricaEvents.VideoAdWatched(true, adInfo.Placement, true, true);
        // }
        //
        // private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     // Ad revenue paid. Use this callback to track user revenue.
        // }

        #endregion

        #region  Banner

        private void InitializeBannerAds()
        {
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            // MaxSdk.CreateBanner(banerUnit, MaxSdkBase.BannerPosition.BottomCenter);
            //
            // // Set background or background color for banners to be fully functional
            // MaxSdk.SetBannerBackgroundColor(banerUnit, Color.blue);
            // MaxSdkCallbacks.Banner.OnAdLoadedEvent      += OnBannerAdLoadedEvent;
            // MaxSdkCallbacks.Banner.OnAdLoadFailedEvent  += OnBannerAdLoadFailedEvent;
            // MaxSdkCallbacks.Banner.OnAdClickedEvent     += OnBannerAdClickedEvent;
            // MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            // MaxSdkCallbacks.Banner.OnAdExpandedEvent    += OnBannerAdExpandedEvent;
            // MaxSdkCallbacks.Banner.OnAdCollapsedEvent   += OnBannerAdCollapsedEvent;
            //
            // MaxSdk.LoadBanner(banerUnit);
            steps++;

        }

        // private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     MaxSdk.ShowBanner(banerUnit);
        // }
        //
        // private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        // {
        //     MaxSdk.LoadBanner(banerUnit);
        // }
        //
        // private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}
        //
        // private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}
        //
        // private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)  {}
        //
        // private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

        #endregion
    }
}