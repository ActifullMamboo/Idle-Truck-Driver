using System;
using System.Collections.Generic;
using MamboSdk.Settings;
using UnityEngine;
using Varioqub;

namespace MamboSdk.RemoteConfig
{
    public class VarioqubRemoteConfigProvider : IRemoteConfigProvider
    {
        private Dictionary<string,string> remoteConfigs;
        public bool IsInitialized => Varioqub.Varioqub.Instance.IsInitialized;

        public void Initialize(MambooSdkSettings settings)
        {
            remoteConfigs = new Dictionary<string, string>()
            {
                { RemoteConfigKeys.IsProfitableUser, "1" }
            };

            var appMetricaAppId = string.IsNullOrEmpty(settings.appmetricaSettings.appId)
                ? 0
                : Convert.ToInt32(settings.appmetricaSettings.appId);
            if (appMetricaAppId == 0) Debug.LogError("You didn't put in the appmetrics application id");
            var varioqubConfig = new VarioqubConfig()
            {
                ClientFeatures = remoteConfigs,
                AppMetricaAppId = appMetricaAppId,
            };
            Varioqub.Varioqub.Instance.Initialize(varioqubConfig);
        }

        public string GetString(string key)
        {
            if (remoteConfigs.TryGetValue(key, out var defaultValue))
            {
                return Varioqub.Varioqub.Instance.GetString(key, defaultValue);
            }

            PrintErrorAboutInitDefaultRemoteConfig();
            return string.Empty;
        }

        public int GetInt(string key)
        {
            if (remoteConfigs.TryGetValue(key, out var defaultValue))
            {
                var result = Varioqub.Varioqub.Instance.GetString(key, defaultValue);
                if (int.TryParse(result, out int parsedValue))
                {
                    return parsedValue;
                }

                Debug.LogError("!!! Can't parse result to int, sdk give the default value of type !!!");
                return 0;
            }

            PrintErrorAboutInitDefaultRemoteConfig();
            return 0;
        }

        public bool GetBool(string key)
        {
            if (remoteConfigs.TryGetValue(key, out var defaultValue))
            {
                var result = Varioqub.Varioqub.Instance.GetString(key, defaultValue);
                if (result == "1")
                {
                    return true;
                }

                Debug.LogError("!!! Can't parse result to bool, sdk give the default value of type !!!");
                return false;
            }

            PrintErrorAboutInitDefaultRemoteConfig();
            return false;
        }

        private static void PrintErrorAboutInitDefaultRemoteConfig()
        {
            Debug.Log("!!! This key is not in the default remote config, sdk give the default value of type. To fix this, add data to the dictionary with default values !!!");
        }
    }
}