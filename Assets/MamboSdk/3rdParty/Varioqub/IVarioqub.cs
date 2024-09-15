using System;

namespace Varioqub
{
    public interface IVarioqub
    {
        bool IsInitialized { get; set; }
        void ActivateWithConfiguration(VarioqubConfig config, Action<bool> onFetchConfigCallback);
        string GetStringValue(string key, string defaultValue);
    }
}