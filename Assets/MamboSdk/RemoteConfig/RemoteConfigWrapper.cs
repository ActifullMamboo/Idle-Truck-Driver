using System;
using System.Collections.Generic;
using System.Linq;
using MamboSdk.Settings;

namespace MamboSdk.RemoteConfig
{
    public class RemoteConfigWrapper
    {
        private readonly Dictionary<RemoteConfigProviderType, IRemoteConfigProvider> _configProviders;

        public RemoteConfigWrapper(MambooSdkSettings mambooSdkSettings)
        {
            _configProviders = new Dictionary<RemoteConfigProviderType, IRemoteConfigProvider>()
            {
#if mamboo_appmetrica
                { RemoteConfigProviderType.Varioqub, new VarioqubRemoteConfigProvider() },
#else
                {RemoteConfigProviderType.Varioqub, new DummyRemoteConfigProvider() },
#endif
#if mamboo_firebase
                {RemoteConfigProviderType.Firebase, new FirebaseRemoteConfigProvider() },
#else
                { RemoteConfigProviderType.Firebase, new DummyRemoteConfigProvider() },
#endif
            };

            InitAllProviders(mambooSdkSettings);
        }

        private void InitAllProviders(MambooSdkSettings mambooSdkSettings)
        {
            foreach (var configProvider in _configProviders.Values)
            {
                configProvider.Initialize(mambooSdkSettings);
            }
        }

        public bool IsAllProviderInitialized()
        {
            return _configProviders.All(pair => IsProviderInitialized(pair.Key));
        }
        
        public bool IsProviderInitialized(RemoteConfigProviderType providerType)
        {
            return _configProviders.ContainsKey(providerType) && _configProviders[providerType].IsInitialized;
        }

        public string GetString(RemoteConfigProviderType providerType, string key)
        {
            if (_configProviders.ContainsKey(providerType))
            {
                return _configProviders[providerType].GetString(key);
            }
            else
            {
                throw new ArgumentException($"Provider of type {providerType.ToString()} not found.");
            }
        }

        public int GetInt(RemoteConfigProviderType providerType, string key)
        {
            if (_configProviders.ContainsKey(providerType))
            {
                return _configProviders[providerType].GetInt(key);
            }
            else
            {
                throw new ArgumentException($"Provider of type {providerType.ToString()} not found.");
            }
        }

        public bool GetBool(RemoteConfigProviderType providerType, string key)
        {
            if (_configProviders.ContainsKey(providerType))
            {
                return _configProviders[providerType].GetBool(key);
            }
            else
            {
                throw new ArgumentException($"Provider of type {providerType.ToString()} not found.");
            }
        }

        private void AddProvider(RemoteConfigProviderType providerType, IRemoteConfigProvider provider)
        {
            _configProviders[providerType] = provider;
        }
    }
}