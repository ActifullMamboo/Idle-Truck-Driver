using System;
using MamboSdk.Settings;

namespace MamboSdk.Ads
{
    public class DummyAdsManager : IAdsService
    {
#pragma warning disable 0067
        public string PlatformName() => "Dummy";
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
#pragma warning restore 0067
        
        public void Initialize(MambooSdkSettings settings, Action onInitialized)
        {
            onInitialized.Invoke();
        }

        public bool IsInitialized()
        {
            return true;
        }

        public void ShowBanner()
        {
        }

        public void HideBanner()
        {
        }

        public void DestroyBanner()
        {
        }

        public void LoadReward()
        {
        }

        public bool HasReward()
        {
            return false;
        }

        public void ShowReward(string placement)
        {
            OnRewardShow?.Invoke(new ImpressionData());
        }

        public void LoadInterstitial()
        {
        }

        public bool HasInterstitial()
        {
            return false;
        }

        public void ShowInterstitial(string placement)
        {
            OnInterstitialShow?.Invoke(new ImpressionData());
        }
    }
}