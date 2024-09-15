using UnityEditor;
using UnityEngine;

public class SuperSDKEditor : EditorWindow
{
    private SDKResources resources;

    string currentProductName = "";

    [MenuItem("Tools/SuperSDK/SDK Window")]
    public static void ShowWindow()
    {
        GetWindow<SuperSDKEditor>("Super SDK");
    }

    private void OnGUI()
    {
        if (resources == null)
        {
            resources = SDKResources.Load();
        }

        if (GUILayout.Button("Update Api Keys"))
        {
            ApiKeysReader.LoadAndParseSheetWithKeys();

            Debug.Log(resources.ApiKeys.ListAllKeys());
        }

        GUILayout.BeginVertical();

        GUILayout.Label("Super SDK Settings", EditorStyles.boldLabel);
        PlayerSettingsLayout();

        if (!resources.IsPackagesInstalled)
        {
            PackageInstaller();
        }
        else
        {
            GUILayout.Label("Packages is setted up!");
        }

        GUILayout.EndVertical();
    }

    private void PackageInstaller()
    {
        GUILayout.Label("Packages setup", EditorStyles.whiteLabel);
        GUILayout.BeginHorizontal();

        if (UnityPackagesInstaller.downloadWindow == null)
        {
            if (GUILayout.Button("Download and import packages"))
            {
                UnityPackagesInstaller.Start();
            }
        }
        else if (UnityPackagesInstaller.downloadWindow != null)
        {
            GUILayout.Label("Downloading and installing...", EditorStyles.whiteLabel);
        }

        GUILayout.EndHorizontal();
    }

    private void PlayerSettingsLayout()
    {
        GUILayout.Label("Player Settings", EditorStyles.whiteLabel);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Product name:");

        currentProductName = GUILayout.TextField(currentProductName);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Применить настройки Player Settings"))
        {
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            PlayerSettings.productName = currentProductName;
            PlayerSettings.companyName = "amazon";
        }

        if (!resources.IsAmazonSettedUp)
        {
            if (GUILayout.Button("Применить настройки Amazon"))
            {
                AmazonDataReplacer gradle = new();
                gradle.SetupTemplates();
                resources.IsAmazonSettedUp = true;
            }
        }
    }
}
