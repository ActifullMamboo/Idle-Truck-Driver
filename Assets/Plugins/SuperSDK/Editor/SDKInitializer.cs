using UnityEditor;

public static class SDKInitializer
{
    static SDKInitializer()
    {
        OpenCustomSDKEditor();
    }

    private static void OpenCustomSDKEditor()
    {
        EditorApplication.delayCall += () => {
            SuperSDKEditor.ShowWindow();
        };
    }
}
