using UnityEngine;

[CreateAssetMenu(fileName = "SDKResources", menuName = "SuperSDK/SDKResources")]
public class SDKResources : ScriptableObject
{
    #region ApiSetup
    public ApiKeys ApiKeys;
    #endregion

    #region EditorUtils
    public bool IsAmazonSettedUp;
    public bool IsPackagesInstalled;
    #endregion

    public static SDKResources settings;

    public static SDKResources Load()
    {
        if (settings != null)
            return settings;

        settings = Resources.Load<SDKResources>("SDKResources");
        if (settings == null)
            return settings;

        return settings;
    }

    public void Install()
    {
        // SDKInstall.Install();
    }
    
}