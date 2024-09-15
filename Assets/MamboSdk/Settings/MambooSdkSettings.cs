using System;
using com.adjust.sdk;
using MamboSdk.Settings;
using UnityEngine;

namespace MamboSdk.Settings
{
    [CreateAssetMenu(fileName = "MambooSdkSettings", menuName = "Mamboo/SdkSettings")]
    public class MambooSdkSettings : ScriptableObject
    {
        private const string SETTING_RESOURCES_PATH = "Mamboo/MambooSdkSettings";
        private const string ADS_SETTING_RESOURCES_PATH = "Mamboo/AdsSettings";
        private const string PLAYER_SETTING_RESOURCES_PATH = "Mamboo/PlayerSettings";
        private const string ADJUST_SETTING_RESOURCES_PATH = "Mamboo/AdjustSettings";
        public const string APPMETRICA_SETTING_RESOURCES_PATH = "Mamboo/AppmetricaSettings";
        
        public bool qaBuild ;
        public bool useIronSource ;
        public bool useAppodeal ;
        public bool useAppmetrica;
        public bool useFirebase;
        public bool useIAP;
        
        public bool? AdsInitialized = null;
        public bool? isAnalyticsInitilized = null;
        
        public MambooAppmetricaSettings appmetricaSettings ;
        public MambooAdjustSettings adjustSettings;
        public MambooPlayerSettings playerSettings;
        public MambooAdsSettings adsSettings;

        public static MambooSdkSettings settings;
        
        public static MambooSdkSettings Load()
        {
            if (settings != null)
                return settings;
            
            settings = Resources.Load<MambooSdkSettings>(SETTING_RESOURCES_PATH);
            if (settings == null) 
                return settings;
            
            settings.adsSettings = Resources.Load<MambooAdsSettings>(ADS_SETTING_RESOURCES_PATH);
            settings.playerSettings = Resources.Load<MambooPlayerSettings>(PLAYER_SETTING_RESOURCES_PATH);
            settings.adjustSettings = Resources.Load<MambooAdjustSettings>(ADJUST_SETTING_RESOURCES_PATH);
            settings.appmetricaSettings = Resources.Load<MambooAppmetricaSettings>(APPMETRICA_SETTING_RESOURCES_PATH);
            return settings;
        }
        
        public Action<string, string, string> onAttributionChanged = (network, campaign, creative) =>
        {
            Debug.Log($"[Mamboo] Attribution: network={network} campaign={campaign} creative={creative}");
        };
    }
}