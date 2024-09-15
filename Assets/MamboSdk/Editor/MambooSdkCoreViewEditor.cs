using MamboSdk.Core;
using UnityEditor;

namespace MamboSdk.Editor
{
    [CustomEditor(typeof(MambooSdkVersion))]
    public class MambooSdkCoreViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Mamboo SDK Version", MambooSdkVersion.VERSION);
            base.OnInspectorGUI();
        }
    }
}