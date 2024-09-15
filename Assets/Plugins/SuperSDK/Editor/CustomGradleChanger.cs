using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

public class AmazonDataReplacer
{
    string mainTemplatePath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");
    string manifestTemplatePath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
    public void SetupTemplates()
    {
        UpdateMainTemplate();
        UpdateManifest();
        Debug.Log("Gradle templates configured with minSdkVersion 21");
    }

    void UpdateMainTemplate()
    {
        if (File.Exists(mainTemplatePath))
        {
            var b = File.ReadAllLines(mainTemplatePath);

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i].StartsWith(@"        minSdkVersion"))
                {
                    b[i] = @"        minSdkVersion 21";
                    File.WriteAllLines(mainTemplatePath, b);
                }
            }
        }
        else
        {
            Debug.LogError("MainTemplate was not found, please check PlayerSettings -> Android -> 'Roll to the bottom' -> Publishing Settings -> Build -> 'Custom Main Gradle Template'. Checkbox must be in true!");
        }
    }

    void UpdateManifest()
    {
        if (File.Exists(manifestTemplatePath))
        {
            string manifestContent = File.ReadAllText(manifestTemplatePath);

            string receiverElement = @"
<receiver android:name=""com.amazon.device.iap.ResponseReceiver"" android:exported=""true"" android:permission=""com.amazon.inapp.purchasing.Permission.NOTIFY"">
  <intent-filter>
    <action android:name=""com.amazon.inapp.purchasing.NOTIFY"" />
  </intent-filter>
</receiver>";

            manifestContent = Regex.Replace(manifestContent, @"<\/application>", receiverElement + "\n</application>");

            // Добавляем элементы вне <application> тега
            string featureElements = @"<uses-feature android:name=""android.hardware.location"" android:required=""false"" />
<uses-feature android:name=""android.hardware.location.gps"" android:required=""false"" />";

            if (!manifestContent.Contains("<uses-feature android:name=\"android.hardware.location\""))
            {
                manifestContent = manifestContent.Replace("</manifest>", featureElements + "\n</manifest>");
            }

            File.WriteAllText(manifestTemplatePath, manifestContent);
        }
    }
}