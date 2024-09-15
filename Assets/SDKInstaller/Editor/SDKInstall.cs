using System.Collections.Generic;
using System.Linq;
using MamboSdk.Settings;
using UnityEditor;
using UnityEngine;

public class SDKInstall
{
    private static MambooSdkSettings mamboSettings;
    private static ApiKeys apiKeys;

    [MenuItem("Tools/SuperSDK/Install Keys")]
    public static void Install()
    {
        mamboSettings = MambooSdkSettings.Load();

        var resource = SDKResources.Load();

        apiKeys = resource.ApiKeys;

        PlayerSettings.applicationIdentifier = apiKeys.BundleId;

        // mamboSettings.adjustSettings.appToken = apiKeys.MambooAdjustToken;
        // mamboSettings.adjustSettings.eventTokensJson = apiKeys.MambooAdjustEvents;

        mamboSettings.useIAP = !apiKeys.paidVersion;

        mamboSettings.useAppmetrica = true;
        mamboSettings.appmetricaSettings.androidKey = apiKeys.AppMetricaAndroid;
        mamboSettings.appmetricaSettings.appId = apiKeys.AppMetricaAppId;
        // mamboSettings.appmetricaSettings.

        mamboSettings.useAppodeal = true;
        mamboSettings.adsSettings.AppodealAppKey = apiKeys.AppodealAppKey;

        mamboSettings.useIronSource = !apiKeys.paidVersion;
        mamboSettings.adsSettings.IronSourceAppKey = apiKeys.IronSourceAppKey;

        mamboSettings.adsSettings.AmazonApiKey = apiKeys.AmazonAppKey;
        mamboSettings.adsSettings.AmazonBannerSlotId = apiKeys.AmazonBannerSlotId;
        mamboSettings.adsSettings.AmazonInterstitialVideoSlotId = apiKeys.AmazonInterVideoId;
        mamboSettings.adsSettings.AmazonRewardedVideoSlotId = apiKeys.AmazonRewardVideoId;

        mamboSettings.useFirebase = true;

        Debug.Log($"<color=green>Now open ADS/MediationDeveloper Settings/MediatedNetworkSettings, and past this:</color> AdMobAndroid{apiKeys.AdmobAndroidAppId}");
        Debug.Log("<color=red>This key was added to your clipboard, just ctrl+v</color>");

        EditorGUIUtility.systemCopyBuffer = apiKeys.AdmobAndroidAppId;

        DefinesManager.Toggle("mamboo_is_mediation", mamboSettings.useIronSource);
        DefinesManager.Toggle("mamboo_appodeal_mediation", mamboSettings.useAppodeal);
        DefinesManager.Toggle("mamboo_qa_build", mamboSettings.qaBuild);
        DefinesManager.Toggle("mamboo_iap", mamboSettings.useIAP);
        DefinesManager.Toggle("mamboo_firebase", mamboSettings.useFirebase);
        DefinesManager.Toggle("mamboo_appmetrica", mamboSettings.useAppmetrica);

        DefinesManager.UpdateDefines();
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