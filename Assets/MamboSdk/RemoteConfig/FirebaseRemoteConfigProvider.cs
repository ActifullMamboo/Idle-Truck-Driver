#if mamboo_firebase
using Firebase.Extensions;
using Firebase.RemoteConfig;
using MamboSdk.Core;
using MamboSdk.Settings;

namespace MamboSdk.RemoteConfig
{
    public class FirebaseRemoteConfigProvider : IRemoteConfigProvider
    {
        private const string TAG = "FirebaseRemoteConfigProvider";
    
        public bool IsInitialized => isInitialized;
        private bool isInitialized;

        public void Initialize(MambooSdkSettings settings)
        {
            var defaults = new System.Collections.Generic.Dictionary<string, object>
                { { RemoteConfigKeys.FullscreenAdsTimeSwitchKey, 1500 } };
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(
                previousTask =>
                {
                    PrintRemoteConfigValue("Default");
                    FetchRemoteConfig();
                }
            );
        }

        public string GetString(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        }

        public int GetInt(string key)
        {
            return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
        }

        public bool GetBool(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
        }

        private void FetchRemoteConfig()
        {
            MambooLog.LogDebug(TAG,"Fetching data...");
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.FetchAsync().ContinueWithOnMainThread(
                previousTask =>
                {
                    if (previousTask.IsCompleted == false)
                    {
                        MambooLog.LogError(TAG,$"{nameof(remoteConfig.FetchAsync)} incomplete: Status '{previousTask.Status}'");
                        isInitialized = true;
                        return;
                    }

                    ActivateRetrievedRemoteConfigValues();
                });
        }

        private void ActivateRetrievedRemoteConfigValues()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                remoteConfig.ActivateAsync().ContinueWithOnMainThread(
                    previousTask =>
                    {
                        isInitialized = true;
                        PrintRemoteConfigValue("remote");
                        MambooLog.LogDebug(TAG,$"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                    });
            }
        }

        private void PrintRemoteConfigValue(string typeConfig)
        {
            MambooLog.LogDebug(TAG,$"Type remote config: {typeConfig}");
            MambooLog.LogDebug(TAG,$"{RemoteConfigKeys.FullscreenAdsTimeSwitchKey} : {GetInt(RemoteConfigKeys.FullscreenAdsTimeSwitchKey)}");
        }
    }
}
#endif