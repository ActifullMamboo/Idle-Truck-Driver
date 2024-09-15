using System;
using System.Collections.Generic;
using MamboSdk.Settings;

namespace MamboSdk.Ads
{
    public delegate void AdsFailDelegate(string adFormat, AdsFailInfo failInfo);
    public class AdsFailInfo
    {
        public string Code = string.Empty;
        public string Message = string.Empty;
        public string Platform = string.Empty;
        public Dictionary<string, string> AdditionalInfo = new Dictionary<string, string>();
    }
    
    public class ImpressionData
    {
        public double Revenue { get; set; }
        public MambooAdFormat AdFormat { get; set; } 
        public string NetworkName { get; set; } 
        public string AdUnitIdentifier { get; set; }
    }
    
    public interface IAdsService
    {
        string PlatformName();
        event Action OnBannerLoaded;
        event Action OnRewardAvailable;
        event Action<ImpressionData> OnRewardShow;
        event Action OnRewardReceived;
        event Action OnRewardHidden;
        event AdsFailDelegate OnRewardFailed;
        event AdsFailDelegate OnRewardDisplayFailed;
        
        
        event Action OnInterstitialAvailable;
        event Action<ImpressionData> OnInterstitialShow;
        event Action OnInterstitialHidden;
        event AdsFailDelegate OnInterstitialFailed;
        event AdsFailDelegate OnInterstitialDisplayFailed;
        

        void Initialize(MambooSdkSettings settings, Action onInitialized);
        bool IsInitialized();

        void ShowBanner();
        void HideBanner();
        void DestroyBanner();

        void LoadReward();
        bool HasReward();
        void ShowReward(string placement);

        void LoadInterstitial();
        bool HasInterstitial();
        void ShowInterstitial(string placement);
    }

    public enum MambooAdFormat
    {
        Reward,
        Interstitial,
        Banner
    }
}