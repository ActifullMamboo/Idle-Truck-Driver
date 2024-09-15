#if mamboo_iap
using UnityEngine.Purchasing.Extension;

namespace MamboSdk
{
    public class AmazonCustomModule: AbstractPurchasingModule 
    {
        public override void Configure()
        {
            var amazonStoreDriver = new AmazonIAPStoreDriver();
            var amazonStoreExtension = new AmazonStoreExtension();
            
            RegisterStore("AmazonApps", amazonStoreDriver);
            BindExtension<IAmazonStoreExtensions>(amazonStoreExtension);
        }
    }
}
#endif