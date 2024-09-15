using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;

namespace MamboSdk.Settings
{
    public class MambooAdjustSettings: ScriptableObject
    {
        public string appToken = "";
#if mamboo_qa_build
        public AdjustEnvironment environment = AdjustEnvironment.Sandbox;
#else
        public AdjustEnvironment environment = AdjustEnvironment.Production;
#endif
        public string eventTokensJson = "";
    }
}