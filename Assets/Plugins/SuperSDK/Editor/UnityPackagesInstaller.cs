using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public static class UnityPackagesInstaller
{
    public static EditorWindow downloadWindow = null;

    private static Dictionary<string, string> packagesToInstall;
    private static Dictionary<string, string> installedPackages = new Dictionary<string, string>();
    private static IEnumerator<KeyValuePair<string, string>> packageEnumerator;
    private static AddRequest addRequest;
    private static ListRequest listRequest;
    private static bool isAddingPackage;
    private static bool isOperationInProgress;

    public static void Start()
    {
        Debug.Log("Start fetching missing components...");
        packagesToInstall = new Dictionary<string, string>
        {
            { "com.unity.purchasing", "4.1.2" },
            { "com.unity.nuget.newtonsoft-json", "3.0.1" },
            { "com.appodeal.appodeal-unity-plugin-upm","https://github.com/appodeal/appodeal-unity-plugin-upm.git#v3.0.2"}
        };

        StartInstallation();
    }

    private static void StartInstallation()
    {
        if (isAddingPackage || isOperationInProgress)
        {
            Debug.LogWarning("A package installation is already in progress.");
            return;
        }

        // List installed packages
        listRequest = Client.List();
        EditorApplication.update += ListProgress;
    }

    private static void ListProgress()
    {
        if (listRequest.IsCompleted)
        {
            if (listRequest.Status == StatusCode.Success)
            {
                foreach (var package in listRequest.Result)
                {
                    installedPackages[package.name] = package.version;
                }

                packageEnumerator = packagesToInstall.GetEnumerator();
                InstallNextPackage();
            }
            else
            {
                Debug.LogError("Failed to list installed packages.");
            }

            EditorApplication.update -= ListProgress;
        }
    }

    private static void InstallNextPackage()
    {
        if (packageEnumerator.MoveNext())
        {
            var currentPackage = packageEnumerator.Current;
            string packageId = currentPackage.Key;
            string packageSpec = currentPackage.Value;

            if (installedPackages.ContainsKey(packageId) && installedPackages[packageId] == packageSpec)
            {
                Debug.Log($"Package {packageId} is already installed with version {packageSpec}.");
                InstallNextPackage(); // Move to the next package
                return;
            }

            if (IsGitUrl(packageSpec))
            {
                addRequest = Client.Add(packageSpec);
            }
            else
            {
                addRequest = Client.Add($"{packageId}@{packageSpec}");
            }

            isAddingPackage = true;
            isOperationInProgress = true;
            EditorApplication.update += Progress;
        }
        else
        {
            downloadWindow = PackagesDownloader.Start();
            Debug.Log("All unity packages were set up!");
        }
    }

    private static bool IsGitUrl(string packageSpec)
    {
        return packageSpec.StartsWith("http://") || packageSpec.StartsWith("https://");
    }

    private static void Progress()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log($"Successfully added package: {addRequest.Result.packageId}");
                installedPackages[addRequest.Result.packageId] = addRequest.Result.version;
            }
            else
            {
                Debug.LogError($"Failed to add package: {addRequest.Error.message}");
            }

            isAddingPackage = false;
            isOperationInProgress = false;
            InstallNextPackage();
            EditorApplication.update -= Progress;
        }
    }
}