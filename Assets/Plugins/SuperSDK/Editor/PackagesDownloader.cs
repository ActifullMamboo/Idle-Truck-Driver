using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PackagesDownloader : EditorWindow
{
    private float downloadProgress = 0f;
    private bool isDownloading = false;

    public static EditorWindow Start()
    {
        return GetWindow<PackagesDownloader>();
    }

    private void OnGUI()
    {
        if (!isDownloading)
        {
            EditorCoroutineUtility.StartCoroutine(DownloadPackage(), this);
        }

        if (isDownloading)
        {
            EditorGUILayout.LabelField("Downloading...", GUILayout.Width(100));
            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.ProgressBar(rect, downloadProgress, "Progress");
            Repaint();
        }
    }

    private System.Collections.IEnumerator DownloadPackage()
    {
        string url = "https://firebasestorage.googleapis.com/v0/b/supersdk-db8a4.appspot.com/o/AutoAmazonSDK%200.4a.unitypackage?alt=media&token=1d47c62d-151e-49cb-b3e1-30cd4dbae599";  // Замените на ваш URL
        string downloadPath = Path.Combine(Application.persistentDataPath, "DownloadedPackage.unitypackage");

        if (File.Exists(downloadPath))
        {
            ImportPackage(downloadPath);
        }
        else
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                isDownloading = true;
                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                {
                    downloadProgress = webRequest.downloadProgress;
                    yield return null;
                }

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error downloading package: {webRequest.error}");
                    GetWindow<PackagesDownloader>().Close();
                    isDownloading = false;
                    yield break;
                }

                // Сохранение файла
                File.WriteAllBytes(downloadPath, webRequest.downloadHandler.data);
                Debug.Log("Package downloaded successfully.");

                ImportPackage(downloadPath);
            }
        }
    }

    private void ImportPackage(string downloadPath)
    {
        AssetDatabase.ImportPackage(downloadPath, true);
        isDownloading = false;
        Debug.Log("Package imported successfully.");
        GetWindow<PackagesDownloader>().Close();
    }
}