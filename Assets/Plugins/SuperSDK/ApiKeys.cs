using System;
using UnityEngine.Serialization;

[Serializable]
public struct ApiKeys
{
    public bool paidVersion;

    //
    // public string MambooAdjustToken;
    // public string MambooAdjustEvents;
    public string AppMetricaAndroid;
    public string AppMetricaAppId;
    public string AppodealAppKey;
    public string IronSourceAppKey;
    public string AmazonAppKey;
    public string AmazonBannerSlotId;
    public string AmazonInterVideoId;
    public string AmazonRewardVideoId;
    public string AdmobAndroidAppId;
    public string BundleId;
    // public string SubscriptionId;


    public string ListAllKeys()
    {
        return
            $"Current Api Keys:\n" +
            $" Appodeal: {AppodealAppKey}\n" +
            $" Appmetrica: {AppMetricaAndroid}\n" +
            $" Appmetrica AppId: {AppMetricaAppId}\n" +
            $" Amazon: {AmazonAppKey}\n" +
            $" Amazon Banner ID: {AmazonBannerSlotId}\n Amazon Inter ID: {AmazonInterVideoId}\n" +
            $" Amazon Reward ID: {AmazonRewardVideoId}\n" +
            $" IronSource: {IronSourceAppKey}\n" +
            $" Admob Android: {AdmobAndroidAppId}\n" +
            $" BundleId: {BundleId}";
    }
}