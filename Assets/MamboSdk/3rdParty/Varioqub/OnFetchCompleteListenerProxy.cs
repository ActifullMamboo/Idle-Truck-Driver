using System;
using UnityEngine;

namespace Varioqub
{
    class OnFetchCompleteListenerProxy : AndroidJavaProxy
    {
        private readonly Action<bool> OnConfigFetched;
        public OnFetchCompleteListenerProxy(Action<bool> configFetched) : base("com.yandex.varioqub.config.OnFetchCompleteListener")
        {
            OnConfigFetched = configFetched;
        }

        public void onSuccess()
        {
            Debug.Log("VARIOQUB: FETCH SUCCESS");
            
            var varioqubClass = new AndroidJavaClass("com.yandex.varioqub.config.Varioqub");
            var id = varioqubClass.CallStatic<string>("getId");
            Debug.Log("VARIOQUB ID: " + id);
            OnConfigFetched?.Invoke(true);
        }

        public void onError(string message, AndroidJavaObject error)
        {
            var errorMessage = message;
            Debug.LogError("VARIOQUB: FETCH ERROR: " + errorMessage);
            OnConfigFetched?.Invoke(false);
        }
    }
}