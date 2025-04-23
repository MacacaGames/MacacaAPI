using UnityEngine;
using System.Collections.Generic;

public class XcodeProjectSetting : ScriptableObject
{
    public enum XCodeTarget
    {
        UnityTarget,
        UnityFrameworkTarget
    }

    public const string PROJECT_ROOT = "$(PROJECT_DIR)/";
    public const string IMAGE_XCASSETS_DIRECTORY_NAME = "Unity-iPhone";
    public const string LINKER_FLAG_KEY = "OTHER_LDFLAGS";
    public const string FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";
    public const string LIBRARY_SEARCH_PATHS_KEY = "LIBRARY_SEARCH_PATHS";
    public const string ENABLE_BITCODE_KEY = "ENABLE_BITCODE";
    public const string DEVELOPMENT_TEAM = "DEVELOPMENT_TEAM";
    public const string GCC_ENABLE_CPP_EXCEPTIONS = "GCC_ENABLE_CPP_EXCEPTIONS";
    public const string GCC_ENABLE_CPP_RTTI = "GCC_ENABLE_CPP_RTTI";
    public const string GCC_ENABLE_OBJC_EXCEPTIONS = "GCC_ENABLE_OBJC_EXCEPTIONS";
    public const string INFO_PLIST_NAME = "Info.plist";

    public const string URL_TYPES_KEY = "CFBundleURLTypes";
    public const string URL_TYPE_ROLE_KEY = "CFBundleTypeRole";
    public const string URL_IDENTIFIER_KEY = "CFBundleURLName";
    public const string URL_SCHEMES_KEY = "CFBundleURLSchemes";
    public const string APPLICATION_QUERIES_SCHEMES_KEY = "LSApplicationQueriesSchemes";
    public const string ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES_KEY = "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES";

    #region XCodeproj
    public bool EnableBitCode = true;
    public bool EnableCppEcceptions = true;
    public bool EnableCppRtti = true;
    public bool EnableObjcExceptions = true;
    [Header("Empty to use default")]
    public string CLanguageDialect = "";
    [Header("iCloud Setting")]
    public bool EnableIcloudKeyValue = false;
    public bool EnableIcloudDocument = false;
    public bool EnableCloudKit = false;
    public bool AddIcloudDefaultContainer = false;
    public string[] IcloudCustomContainer = null;
    [Header("Push Setting")]
    public bool AddPushNotifaction = false;
    [Header("Apple SignIn Setting")]
    public bool AddSignInWithApple = false;
    public bool AddSignInRequire = false;
    [Header("Other Setting")]
    public string[] AssociatedDomains = null;
    public bool AlwaysEmbedSwiftStandardLibrary = false;

    //複製XCode内的文件的路徑
    public string CopyDirectoryPath = "";
    //AppleDevelopment内AppID表示
    public string DevelopmentTeam = "";
    //引用的内部Framework
    public List<FrameworkItem> LinkBinaryWithLibraries = new List<FrameworkItem>() { };
    [System.Serializable]
    public struct FrameworkItem
    {
        public XCodeTarget xCodeTarget;
        public string name;
        public bool require;
    }
    public List<EmbedFramework> EmbedFrameworks = new List<EmbedFramework>();
    //引用的内部.tbd
    public List<TargetItem> TbdList = new List<TargetItem>() { };
    //設定OtherLinkerFlag
    public List<OtherLinkerFlagItem> LinkerFlagArray = new List<OtherLinkerFlagItem>() {
        new OtherLinkerFlagItem{
            target = XCodeTarget.UnityTarget,
            items = new string[]{
                "-ObjC", "-ld64"
            }
        },
        new OtherLinkerFlagItem{
            target = XCodeTarget.UnityFrameworkTarget,
            items = new string[]{
                "-ObjC", "-ld64"
            }
        }
    };
    //設定FrameworkSearchPath
    public string[] FrameworkSearchPathArray = new string[] { "$(inherited)", "$(PROJECT_DIR)/Frameworks" };
    [System.Serializable]
    public struct EmbedFramework
    {
        public string folder;
        public string files;
        public bool pathRelativeToUnityProject;
    }
    #region 針對文件標記 Flags
    [System.Serializable]
    public struct CompilerFlagsSet
    {
        public string Flags;
        public List<string> TargetPathList;

        public CompilerFlagsSet(string flags, List<string> targetPathList)
        {
            Flags = flags;
            TargetPathList = targetPathList;
        }
    }

    public List<CompilerFlagsSet> CompilerFlagsSetList = new List<CompilerFlagsSet>()
    {
        /*new CompilerFlagsSet ("-fno-objc-arc", new List<string> () {"Plugin/Plugin.mm"})*/ //实例，请勿删除
    };
    #endregion

    #endregion

    #region 複製
    [System.Serializable]
    public struct CopeFiles
    {
        public string sourcePath;
        public string copyPath;

        public CopeFiles(string sourcePath, string copyPath)
        {
            this.sourcePath = sourcePath;
            this.copyPath = copyPath;
        }
    }

    public List<CopeFiles> CopeFilesList = new List<CopeFiles>() { };
    #endregion

    #region info.Plist
    //白名单
    public List<string> ApplicationQueriesSchemes = new List<string>() { };

    //iOS10新的特性
    [System.Serializable]
    public struct PrivacyData
    {
        public PrivacyData(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        public string name;
        public string description;
    }
    public List<PrivacyData> privacySensiticeData = new List<PrivacyData>() { new PrivacyData("NSPhotoLibraryAddUsageDescription", "Please enable for save ScreenShot.") };

    #region 第三方平台URL Scheme
    [System.Serializable]
    public struct BundleUrlType
    {
        public string identifier;
        public List<string> bundleSchmes;

        public BundleUrlType(string identifier, List<string> bundleSchmes)
        {
            this.identifier = identifier;
            this.bundleSchmes = bundleSchmes;
        }
    }

    public List<BundleUrlType> BundleUrlTypeList = new List<BundleUrlType>() { };
    #endregion
    #region SKAdNetwork
    [System.Serializable]
    public struct SKAdNetworkItems
    {
        public string provider;
        public string value;
    }
    public List<SKAdNetworkItems> sKAdNetworkItems = new List<SKAdNetworkItems>(){
        new SKAdNetworkItems{
            provider= "Unity",
            value = "4DZT52R2T5.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "AdMob",
            value = "cstr6suwn9.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "ironSource",
            value = "SU67R6K2V3.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "Facebook_1",
            value = "v9wttpbfk9.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "Facebook_2",
            value = "n38lu8286q.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "AdColony",
            value = "4pfyvq9l8r.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "Tapjoy",
            value = "ecpz2srf59.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "Pangle_NonCN",
            value = "22mmun2rn5.skadnetwork"
        },
        new SKAdNetworkItems{
            provider= "Pangle_CN",
            value = "238da6jt44.skadnetwork"
        },
    };
    #endregion

    //放置后台需要开启的功能
    public List<string> BackgroundModes = new List<string>() { };

    [System.Serializable]
    public struct Capability
    {
        public string capabilityTypeName;
        public string entitlementFileName;
    }
    public List<Capability> AddCapability = new List<Capability>() { };
    public List<PlistItem> InfoPlist = new List<PlistItem>() { };
    [System.Serializable]
    public struct PlistItem
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public struct TargetItem
    {
        public XCodeTarget target;
        public string item;
    }

    [System.Serializable]
    public struct OtherLinkerFlagItem
    {
        public XCodeTarget target;
        public string[] items;
    }
    #endregion
}