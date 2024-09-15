using System.Collections.Generic;
using System.Linq;
using MamboSdk.Ads;
using UnityEngine;

namespace MamboSdk.Analytics
{
    public class ImpressionAchivements : MonoBehaviour
    {
        private List<ImpressionData> impressionsQueue = new List<ImpressionData>();
        public static ImpressionAchivements instance;

        private void Update()
        {
            if (!impressionsQueue.Any()) return;
            impressionsQueue.ForEach(HandleImpression);
            impressionsQueue.RemoveAll(x => true);
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;

            AdsManager.Instance.OnRewardShownEvent += (imprData) => impressionsQueue.Add(imprData);
            AdsManager.Instance.OnInterstitialShownEvent += (imprData) => impressionsQueue.Add(imprData);
        }

        private void HandleImpression(ImpressionData imprData)
        {
            MambooAnalyticsWrapper.Instance.TrackAdRevenue(imprData.Revenue, "USD", imprData.AdFormat, imprData.NetworkName);
            MambooAnalyticsWrapper.Instance.TrackEvent("total_revenue",new Dictionary<string, object>
            {
                {"value", imprData.Revenue},
                {"currency", "USD"}
            });
            MambooAnalyticsWrapper.Instance.TrackEvent("ad_impression_mediation",new Dictionary<string, object>
            {
                {"ad_platform", "iron source"},
                {"ad_source", imprData.NetworkName},
                {"ad_unit_name", imprData.AdUnitIdentifier},
                {"ad_format", imprData.AdFormat},
                {"currency", "USD"},
                {"value", imprData.Revenue}
            });
        }
    }
}