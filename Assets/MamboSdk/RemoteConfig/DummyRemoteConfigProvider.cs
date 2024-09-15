using System.Collections.Generic;
using MamboSdk.Settings;

namespace MamboSdk.RemoteConfig
{
    public class DummyRemoteConfigProvider : IRemoteConfigProvider
    {
        public bool IsInitialized { get; private set; }
        private Dictionary<string, object> defaultValues = new Dictionary<string, object>();

        public void Initialize(MambooSdkSettings settings)
        {
            defaultValues[RemoteConfigKeys.FullscreenAdsTimeSwitchKey] = 0;
            defaultValues[RemoteConfigKeys.IsProfitableUser] = true;
            IsInitialized = true;
        }

        public string GetString(string key)
        {
            if (defaultValues.TryGetValue(key, out object value) && value is string stringValue)
            {
                return stringValue;
            }
            return null;
        }

        public int GetInt(string key)
        {
            if (defaultValues.TryGetValue(key, out object value) && value is int intValue)
            {
                return intValue;
            }
            return 0;
        }

        public bool GetBool(string key)
        {
            if (defaultValues.TryGetValue(key, out object value) && value is bool boolValue)
            {
                return boolValue;
            }
            return false;
        }
    }
}