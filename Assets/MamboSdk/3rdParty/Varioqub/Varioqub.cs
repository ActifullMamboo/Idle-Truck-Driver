using System;
using System.Collections.Generic;
using MamboSdk.RemoteConfig;
using MamboSdk.Settings;
using UnityEngine;

namespace Varioqub
{
    public class Varioqub
    {
        public const string VERSION = "0.6.0";
        private static Varioqub instance;
        public static Varioqub Instance => instance ??= new Varioqub();

        private static IVarioqub varioqub;

        private static IVarioqub varioqubInstance
        {
            get
            {
                if (varioqub != null) return varioqub;
#if UNITY_ANDROID
                if (Application.platform == RuntimePlatform.Android)
                {
                    varioqub = new VarioqubAndroid();
                }
#endif
                varioqub ??= new VarioqubDummy();

                return varioqub;
            }
        }

        public bool IsInitialized => varioqubInstance.IsInitialized;


        public void Initialize(VarioqubConfig varioqubConfig)
        {
            varioqubInstance.ActivateWithConfiguration(varioqubConfig, OnFetchConfigCallback);
        }

        private void OnFetchConfigCallback(bool fetchStatus)
        {
            if (fetchStatus == false)
            {
                Debug.Log("Set VarioqubDummy");
                varioqub = new VarioqubDummy();
            }
        }

        public string GetString(string key, string defaultValue)
        {
            Debug.Log($"try get VARIOQUB remote config {key}");
            var value = varioqubInstance.GetStringValue(key, defaultValue);
            Debug.Log($"VARIOQUB got data {key} : {value}");
            return value;
        }
    }
}