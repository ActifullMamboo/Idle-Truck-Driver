using System;
using System.Net;
using UnityEditor;
using UnityEngine;

public class ApiKeysReader
{
    public static void LoadAndParseSheetWithKeys()
    {
        try
        {
            using (var webClient = new WebClient())
            {
                string csvData = webClient.DownloadString("https://docs.google.com/spreadsheets/d/e/2PACX-1vTQo6Y_5QCb0jkgsk_pXAjJCbI7Ohr3NvhuXTYqtRPH6CikqjzBe5doYKNew_Wh7zMx7F0VWr5yG2Di/pub?output=csv");
                ProcessCSV(csvData);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to download or process CSV: " + ex.Message);
        }
    }

    static void ProcessCSV(string csvData)
    {
        Debug.Log(csvData);
        var lines = csvData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            Debug.Log("CSV is empty or malformed.");
            return;
        }
        
        var header = lines[0].Split(',');
        
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');

            if (values.Length > 0 && values[0] == PlayerSettings.productName) // Замените "123" на нужное значение AppID
            {
                SDKResources.Load().ApiKeys = new ApiKeys()
                {
                    BundleId = values[7],
                    AppMetricaAndroid = values[11],
                    AppMetricaAppId = values[12],
                    AppodealAppKey = values[10],
                    IronSourceAppKey = values[17],
                    AmazonAppKey = values[13],
                    AmazonBannerSlotId = values[14],
                    AmazonInterVideoId = values[15],
                    AmazonRewardVideoId = values[16],
                    AdmobAndroidAppId = values[18],
                    paidVersion = values[1] == "Paid"
                };

                break;
            }
        }
    }
}