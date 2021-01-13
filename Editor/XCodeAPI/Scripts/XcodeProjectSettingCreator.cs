#if UNITY_IOS

using UnityEngine;
using UnityEditor;


public class XcodeProjectSettingCreator : MonoBehaviour
{
    [MenuItem("Assets/Create/XcodeProjectSetting")]
    public static void CreateAsset()
    {
        string path = AssetDatabase.GenerateUniqueAssetPath(XCodeProjectMod.SETTING_DATA_PATH);
        XcodeProjectSetting data = ScriptableObject.CreateInstance<XcodeProjectSetting>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
    }
}
#endif