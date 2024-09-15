using System;

namespace Varioqub
{
    public class VarioqubDummy : IVarioqub
    {
        public bool IsInitialized
        {
            get => true;
            set{}
        }

        public void ActivateWithConfiguration(VarioqubConfig config, Action<bool> onFetchConfigCallback)
        {
        }

        public string GetStringValue(string key, string defaultValue)
        {
            return defaultValue;
        }
    }
}