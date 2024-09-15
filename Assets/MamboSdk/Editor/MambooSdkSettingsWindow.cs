using System;
using System.Collections.Generic;
using System.Linq;
#if mamboo_iap
using MamboSdk.Purchasing;
#endif
using MamboSdk.Settings;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace MamboSdk.Editor
{
    public class MambooSdkSettingsWindow : EditorWindow
    {
        private int tabIndex;

        private static MambooSdkSettings settings;
        private static MambooSdkSettingsWindow window;
#if mamboo_iap
        private static PurchaseConfig purchaseConfig;
#endif
        private readonly string[] tabs = {"General", "Adjust", "Ads", "Analytics"};

        [MenuItem("Mamboo/SDK Settings")]
        public static void ShowWindow()
        {
            settings = MambooSdkSettings.Load();
            
            if (settings == null)
                settings = CreateAsset<MambooSdkSettings>("MambooSdkSettings");
            
            if (settings.adjustSettings == null)
                settings.adjustSettings = CreateAsset<MambooAdjustSettings>("AdjustSettings");
            
            if (settings.playerSettings == null)
                settings.playerSettings = CreateAsset<MambooPlayerSettings>("PlayerSettings");
            
            if (settings.adsSettings == null)
                settings.adsSettings = CreateAsset<MambooAdsSettings>("AdsSettings");
            
            if (settings.appmetricaSettings == null)
                settings.appmetricaSettings = CreateAsset<MambooAppmetricaSettings>("AppmetricaSettings");
#if mamboo_iap
            purchaseConfig = PurchaseConfig.Load();
            if (purchaseConfig == null)
                purchaseConfig = CreateAsset<PurchaseConfig>("PurchaseConfig");
#endif
            window = GetWindow<MambooSdkSettingsWindow>("Mamboo Settings");
        }

        void OnGUI()
        {
            if (settings == null)
                settings = CreateAsset<MambooSdkSettings>("MambooSdkSettings");
            
            tabIndex = GUILayout.Toolbar(tabIndex, tabs);
            switch (tabIndex)
            {
                case 0:
                    OnGeneralTab();
                    EditorUtility.SetDirty(settings);
                    break;
                case 1:
                    OnAdjustTab();
                    EditorUtility.SetDirty(settings.adjustSettings);
                    break;
                case 2:
                    OnAdsTab();
                    EditorUtility.SetDirty(settings.adsSettings);
                    break;
                case 3:
                    OnAnalyticsTab();
                    EditorUtility.SetDirty(settings.appmetricaSettings);
                    break;
            }
        }

        private void OnGeneralTab()
        {
            try
            {
                GUILayout.Label("General Settings", EditorStyles.boldLabel);
                
                GUILayout.Label("QA Build", EditorStyles.label);
                settings.qaBuild = GUILayout.Toggle(settings.qaBuild, string.Empty);

                GUILayout.Label("Use Appodeal", EditorStyles.label);
                settings.useAppodeal = GUILayout.Toggle(settings.useAppodeal, string.Empty);

                GUILayout.Label("Use Iron Source", EditorStyles.label);
                settings.useIronSource = GUILayout.Toggle(settings.useIronSource, string.Empty);
                
                GUILayout.Label("Use In-App Purchases", EditorStyles.label);
                settings.useIAP = GUILayout.Toggle(settings.useIAP, string.Empty);
                
                GUILayout.Label("Use Firebase", EditorStyles.label);
                settings.useFirebase = GUILayout.Toggle(settings.useFirebase, string.Empty);
                
                GUILayout.Label("Use Appmetrica", EditorStyles.label);
                settings.useAppmetrica = GUILayout.Toggle(settings.useAppmetrica, string.Empty);

                if (GUILayout.Button("Apply and Recompile"))
                {
                    window.Close();
                    UpdateDefines();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnAdjustTab()
        {
            try
            {
                GUILayout.Label("Adjust Settings", EditorStyles.boldLabel);
            
                GUILayout.Label("App token", EditorStyles.label);
                settings.adjustSettings.appToken = GUILayout.TextField(settings.adjustSettings.appToken, 25, GUILayout.Width(100));
            
                GUILayout.Label("Events json", EditorStyles.label);
                settings.adjustSettings.eventTokensJson = GUILayout.TextField(settings.adjustSettings.eventTokensJson, GUILayout.Width(500));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnAdsTab()
        {
            GUILayout.Label("Ads Settings", EditorStyles.boldLabel);
            
            GUILayout.Label("Iron Source App Key", EditorStyles.label);
            settings.adsSettings.IronSourceAppKey = GUILayout.TextField(settings.adsSettings.IronSourceAppKey, 50, GUILayout.Width(150));
            
            GUILayout.Label("Appodeal App Key", EditorStyles.label);
            settings.adsSettings.AppodealAppKey = GUILayout.TextField(settings.adsSettings.AppodealAppKey, 50, GUILayout.Width(150));          
            
            GUILayout.Label("Amazon App Key", EditorStyles.label);
            settings.adsSettings.AmazonApiKey = GUILayout.TextField(settings.adsSettings.AmazonApiKey, 50, GUILayout.Width(150));

            GUILayout.Label("Amazon Banner Slot Id", EditorStyles.label);
            settings.adsSettings.AmazonBannerSlotId = GUILayout.TextField(settings.adsSettings.AmazonBannerSlotId, 50, GUILayout.Width(150));

            GUILayout.Label("Amazon Interstitial Video Slot Id", EditorStyles.label);
            settings.adsSettings.AmazonInterstitialVideoSlotId = GUILayout.TextField(settings.adsSettings.AmazonInterstitialVideoSlotId, 50, GUILayout.Width(150));

            GUILayout.Label("Amazon Rewarded Video Slot Id", EditorStyles.label);
            settings.adsSettings.AmazonRewardedVideoSlotId = GUILayout.TextField(settings.adsSettings.AmazonRewardedVideoSlotId, 50, GUILayout.Width(150));
        }

        private void OnAnalyticsTab()
        {
            GUILayout.Label("Analytics Settings", EditorStyles.boldLabel);
            
            GUILayout.Label("Appmetrica Android Key", EditorStyles.label);
            settings.appmetricaSettings.androidKey = GUILayout.TextField(settings.appmetricaSettings.androidKey, 100, GUILayout.Width(300));
            
            GUILayout.Label("Appmetrica App Key", EditorStyles.label);
            settings.appmetricaSettings.appId = GUILayout.TextField(settings.appmetricaSettings.appId, 100, GUILayout.Width(300));
        }

        private void UpdateDefines()
        {
            DefinesManager.Toggle("mamboo_is_mediation", settings.useIronSource);
            DefinesManager.Toggle("mamboo_appodeal_mediation", settings.useAppodeal);
            DefinesManager.Toggle("mamboo_qa_build", settings.qaBuild);
            DefinesManager.Toggle("mamboo_iap", settings.useIAP);
            DefinesManager.Toggle("mamboo_firebase", settings.useFirebase);
            DefinesManager.Toggle("mamboo_appmetrica", settings.useAppmetrica);
            
            DefinesManager.UpdateDefines();
        }

        private static T CreateAsset<T>(string assetName) where T: ScriptableObject
        {
            var asset = CreateInstance<T>();
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Mamboo"))
                AssetDatabase.CreateFolder("Assets/Resources", "Mamboo");

            AssetDatabase.CreateAsset(asset, $"Assets/Resources/Mamboo/{assetName}.asset");
            return asset;
        }
        
        private static class DefinesManager
        {
            private static readonly List<string> addSymbols = new List<string>();
            private static readonly List<string> removeSymbols = new List<string>();

            public static void Toggle(string define, bool use)
            {
                if (use)
                {
                    addSymbols.Add(define);
                }
                else
                {
                    removeSymbols.Add(define);
                }
            }

            public static void UpdateDefines()
            {
                var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                var allDefines = definesString.Split(';').ToList();

                if (addSymbols.All(x => allDefines.Any(y => x == y)) &&
                    removeSymbols.All(x => allDefines.All(y => x != y)))
                    return;
                allDefines.AddRange(addSymbols.Except(allDefines));
                var newDefines = allDefines.Except(removeSymbols);
                
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", newDefines)
                );
                addSymbols.Clear();
                removeSymbols.Clear();
            }
        }
    }
}
