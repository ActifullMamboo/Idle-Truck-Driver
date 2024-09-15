#if mamboo_iap
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MamboSdk.Purchasing
{
    [CreateAssetMenu(menuName = "PurchaseConfig", fileName = "Mamboo/PurchaseConfig")]
    public class PurchaseConfig : ScriptableObject
    {
        private const string SETTING_RESOURCES_PATH = "Mamboo/PurchaseConfig";

        public static PurchaseConfig Load()
        {
            var settings = Resources.Load<PurchaseConfig>(SETTING_RESOURCES_PATH);
            return settings;
        }

        [Serializable]
        public class PurchaseItem
        {
            [SerializeField]
            private double _priceIos;

            [SerializeField] 
            private double _priceAndroid;

            [SerializeField]
            private string _productId;

            [SerializeField]
            private ProductType _productType;

            public double Price => Application.platform == RuntimePlatform.Android || Application.isEditor ? _priceAndroid : _priceIos;

            public string ProductId => _productId;

            public ProductType ProductType => _productType;
        }

        [SerializeField]
        private List<PurchaseItem> _products;

        public List<PurchaseItem> Products => _products;
    }
}
#endif 