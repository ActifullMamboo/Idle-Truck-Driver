using System;
using UnityEngine;

namespace Varioqub
{
    public class VarioqubAndroid : IVarioqub
    {
        private const string VarioqubClass = "com.yandex.varioqub.config.Varioqub";
        private const string VarioqubAppmetricaAdapterClass = "com.yandex.varioqub.appmetricaadapter.AppMetricaAdapter";
        private const string UnityPlayerClass = "com.unity3d.player.UnityPlayer";
        private const string VarioqubSettingsBuilder = "com.yandex.varioqub.config.VarioqubSettings$Builder";
        private const string VarioqubBridgeClass = "com.mamboo.varioqubbridge.VarioqubBridge";

        public bool IsInitialized { get; set; }
        private Action<bool> onFetchConfigCallback;
        public void ActivateWithConfiguration(VarioqubConfig config, Action<bool> onFetchConfigCallback)
        {
            this.onFetchConfigCallback = onFetchConfigCallback;
            var settingsBuilder = new AndroidJavaObject(VarioqubSettingsBuilder, $"appmetrica.{config.AppMetricaAppId}");
            
            foreach (var configClientFeature in config.ClientFeatures)
            {
                settingsBuilder.Call<AndroidJavaObject>("withClientFeature", configClientFeature.Key, configClientFeature.Value);
            }
            
            var settings = settingsBuilder.Call<AndroidJavaObject>("build");

            var varioqubClass = new AndroidJavaClass(VarioqubClass);
            var context = new AndroidJavaClass(UnityPlayerClass).GetStatic<AndroidJavaObject>("currentActivity");

            var appMetricaAdapter =
                new AndroidJavaObject(VarioqubAppmetricaAdapterClass, context);

            varioqubClass.CallStatic("init", settings, appMetricaAdapter, context);
            
            
            FetchConfig();
        }

        public string GetStringValue(string key, string defaultValue)
        {
            var varioqubClass = new AndroidJavaClass(VarioqubClass);
            var value = varioqubClass.CallStatic<string>("getString", key, defaultValue);
            Debug.Log("VARIOQUB: " + value);

            return value;
        }

        private void FetchConfig()
        {
            var varioqubClass = new AndroidJavaClass(VarioqubClass);

            var callbackProxy = new OnFetchCompleteListenerProxy(ConfigFetched);
            varioqubClass.CallStatic("fetchConfig", callbackProxy);
        }

        private void ConfigFetched(bool onSuccess)
        {
            Debug.Log($"ConfigFetched status: {onSuccess}");
            if (onSuccess)
            {
                ActivateConfig();
                IsInitialized = true;
                onFetchConfigCallback?.Invoke(true);
                Debug.Log($"Varioqub Is Initialized");
            }
            else
            {
                onFetchConfigCallback?.Invoke(false);
            }
        }

        private void ActivateConfig()
        {
            var varioqubClass = new AndroidJavaClass(VarioqubBridgeClass);
            varioqubClass.CallStatic("activateConfig");
        }
    }
}