#if mamboo_iap
using System;
using UnityEngine.Purchasing;

namespace MamboSdk
{
    public interface IAmazonStoreExtensions : IStoreExtension
    {
        void RestoreTransactions(Action<bool> callback);
    }
}
#endif