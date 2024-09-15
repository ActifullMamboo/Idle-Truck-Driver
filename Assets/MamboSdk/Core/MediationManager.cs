using System;
using System.Collections;
using System.Collections.Generic;
using MamboSdk.Ads;
using MamboSdk.Core;
using MamboSdk.RemoteConfig;
using UnityEngine;

namespace MamboSdk
{
    public class MediationManager
    {
        private const string TAG = "MediationManager";
        
        public Action ChangeMediationRule;
        
        private PlayTimeManager playTimeManager;
        private int playTimeToChangeForIronSourceMediation;
        private bool isProfitableUser;
        private readonly Dictionary<MambooAdFormat, AdProvider> IronSourceSetUp = new Dictionary<MambooAdFormat, AdProvider>
        {
            { MambooAdFormat.Interstitial, AdProvider.IronSource },
            { MambooAdFormat.Reward, AdProvider.IronSource },
            { MambooAdFormat.Banner, AdProvider.IronSource },
        };
        private readonly Dictionary<MambooAdFormat, AdProvider> AppodealSetUp = new Dictionary<MambooAdFormat, AdProvider>
        {
            { MambooAdFormat.Interstitial, AdProvider.Appodeal },
            { MambooAdFormat.Reward, AdProvider.Appodeal },
            { MambooAdFormat.Banner, AdProvider.IronSource },
        };

        private bool IsChangeMediation => playTimeToChangeForIronSourceMediation != 0 && 
                                          playTimeManager.PlayTimePlayerInSeconds > playTimeToChangeForIronSourceMediation;


        public void Initialize()
        {
            playTimeManager = MambooSdkCore.Instance.PlayTimeManager;
            playTimeToChangeForIronSourceMediation = 
                MambooSdkCore.Instance.RemoteConfigWrapper.GetInt(RemoteConfigProviderType.Firebase,RemoteConfigKeys.FullscreenAdsTimeSwitchKey);
            isProfitableUser =
                MambooSdkCore.Instance.RemoteConfigWrapper.GetBool(RemoteConfigProviderType.Varioqub,RemoteConfigKeys.IsProfitableUser);

            if (IsChangeMediation == false && playTimeToChangeForIronSourceMediation != 0 && isProfitableUser)
                playTimeManager.StartCoroutine(MonitorAdMediationChange());
        }
        
        public Dictionary<MambooAdFormat, AdProvider> GetAdFormatMediation()
        {
            return isProfitableUser ? (IsChangeMediation ? IronSourceSetUp : AppodealSetUp) : IronSourceSetUp;
        }

        private IEnumerator MonitorAdMediationChange()
        {
            yield return new WaitUntil(() => IsChangeMediation);
            var adBannerProviderType = GetAdFormatMediation()[MambooAdFormat.Banner];
            if (adBannerProviderType != IronSourceSetUp[MambooAdFormat.Banner])
            {
                OnRefreshBanner?.Invoke(IronSourceSetUp[MambooAdFormat.Banner], adBannerProviderType);
                yield break;
            }

            if (adBannerProviderType != AppodealSetUp[MambooAdFormat.Banner])
            {
                OnRefreshBanner?.Invoke(AppodealSetUp[MambooAdFormat.Banner], adBannerProviderType);
                yield break;
            }
            
        }

        public event Action<AdProvider, AdProvider> OnRefreshBanner;
    }
}