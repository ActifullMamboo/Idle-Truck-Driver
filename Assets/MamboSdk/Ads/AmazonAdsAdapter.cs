#if mamboo_is_mediation
using System;
using AmazonAds;
using MamboSdk.Settings;
using UnityEngine;

namespace Mamboo.Ads
{
    public class AmazonAdsAdapter
    {
        private string amazonApiKey;
        private string amazonBannerSlotId;
        private string amazonInterstitialVideoSlotId;
        private string amazonRewardedVideoSlotId;
        
        private APSBannerAdRequest bannerAdRequest;
        
        private APSInterstitialAdRequest interstitialAdRequest;
        private APSVideoAdRequest videoAdRequest;

        public void Initialize(MambooSdkSettings settings)
        {
#if UNITY_EDITOR
            return;
#endif 
           
            amazonApiKey = settings.adsSettings.AmazonApiKey;
            amazonBannerSlotId = settings.adsSettings.AmazonBannerSlotId;
            amazonInterstitialVideoSlotId = settings.adsSettings.AmazonInterstitialVideoSlotId;
            amazonRewardedVideoSlotId = settings.adsSettings.AmazonRewardedVideoSlotId;
            
            Amazon.Initialize(amazonApiKey);
            Amazon.EnableTesting(false);
            Amazon.EnableLogging(true);
            Amazon.UseGeoLocation(true);
            Amazon.IsLocationEnabled();
            Amazon.SetMRAIDPolicy(Amazon.MRAIDPolicy.CUSTOM);
            Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.IRON_SOURCE));
            Amazon.SetMRAIDSupportedVersions(new string[] { "1.0", "2.0", "3.0" });
        }

        public void LogInfo(string message) => Debug.Log($"[AmazonAdsAdapter] {message}");
        public void LogError(string message) => Debug.LogError($"[AmazonAdsAdapter] {message}");

        public void RequestAmazonRewardedVideo(Action<string> onSuccess, Action onFailed)
        {
            videoAdRequest = new APSVideoAdRequest(320, 480, amazonRewardedVideoSlotId);
            videoAdRequest.onSuccess += response =>
            {
                var networkData = APSMediationUtils.GetRewardedNetworkData(amazonRewardedVideoSlotId, response);
                LogInfo($"RequestAmazonRewardedVideo: {networkData}");
                onSuccess?.Invoke(networkData);
            };
            videoAdRequest.onFailedWithError += error =>
            {
                LogError($"RequestAmazonRewardedVideo: {error.GetCode()} - {error.GetMessage()}");
                onFailed?.Invoke();
            };

            LogInfo($"Start RequestAmazonRewardedVideo");
            videoAdRequest.LoadAd();
        }

        public void RequestAmazonBanner(Action<string> onSuccess, Action onFailed)
        {
            const int width = 320;
            const int height = 50;

            bannerAdRequest = new APSBannerAdRequest(width, height, amazonBannerSlotId);
            bannerAdRequest.onFailedWithError += (adError) =>
            {
                LogError($"RequestAmazonBanner: {adError.GetCode()} - {adError.GetMessage()}");
                onFailed?.Invoke();
            };
            bannerAdRequest.onSuccess += (adResponse) =>
            {
                var networkData = APSMediationUtils.GetBannerNetworkData(amazonBannerSlotId, adResponse);
                LogInfo($"RequestAmazonRewardedVideo: {networkData}");
                onSuccess?.Invoke(networkData);
            };
            bannerAdRequest.LoadAd();
        }
        
        public void RequestAmazonInterstitialVideo(Action<string> onSuccess, Action onFailed)
        {
            videoAdRequest = new APSVideoAdRequest(320, 480, amazonInterstitialVideoSlotId);

            videoAdRequest.onSuccess += (adResponse) =>
            {
                var networkData = APSMediationUtils.GetInterstitialNetworkData(amazonInterstitialVideoSlotId, adResponse);
                LogInfo($"RequestAmazonRewardedVideo: {networkData}");
                onSuccess?.Invoke(networkData);
            };
            
            videoAdRequest.onFailedWithError += (adError) =>
            {
                LogError($"RequestAmazonBanner: {adError.GetCode()} - {adError.GetMessage()}");
                onFailed?.Invoke();
            };

            videoAdRequest.LoadAd();
        }
    }
}
#endif