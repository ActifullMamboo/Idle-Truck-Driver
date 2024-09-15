using MamboSdk.Settings;

namespace MamboSdk.RemoteConfig
{
    public interface IRemoteConfigProvider
    {
        bool IsInitialized { get; }
        void Initialize(MambooSdkSettings settings);
        string GetString(string key);
        int GetInt(string key);
        bool GetBool(string key);
    }
}