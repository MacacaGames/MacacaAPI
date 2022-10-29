#if UNITY_IOS

using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

#endif
using System.Linq;
using UnityEngine;
using UnityEditor.iOS.Xcode.Extensions;

public class XCodeProjectMod : MonoBehaviour
{
    public static string SETTING_DATA_PATH = "Assets/Resources/XcodeProjectSetting.asset";

#if UNITY_EDITOR

    [PostProcessBuild(200)]
    private static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
            return;
        PBXProject pbxProject = null;
        XcodeProjectSetting setting = null;
        string pbxProjPath = PBXProject.GetPBXProjectPath(buildPath);
        string unityTargetGuid = null;
        string unityFrameworkTargetGuid = null;
        string unityTestTargetGuid = null;
        Debug.Log("开始设置.XCodeProj");

        setting = Resources.Load<XcodeProjectSetting>("XcodeProjectSetting");
        pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(pbxProjPath));
        unityTargetGuid = pbxProject.GetUnityMainTargetGuid();
        unityFrameworkTargetGuid = pbxProject.GetUnityFrameworkTargetGuid();
        unityTestTargetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());

        pbxProject.SetBuildProperty(unityTargetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.SetBuildProperty(unityFrameworkTargetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.SetBuildProperty(unityTestTargetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.SetBuildProperty(pbxProject.TargetGuidByName("Unity-iPhone"), XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");

        pbxProject.SetBuildProperty(unityTargetGuid, XcodeProjectSetting.DEVELOPMENT_TEAM, setting.DevelopmentTeam);
        pbxProject.SetBuildProperty(unityTargetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_EXCEPTIONS, setting.EnableCppEcceptions ? "YES" : "NO");
        pbxProject.SetBuildProperty(unityTargetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_RTTI, setting.EnableCppRtti ? "YES" : "NO");
        pbxProject.SetBuildProperty(unityTargetGuid, XcodeProjectSetting.GCC_ENABLE_OBJC_EXCEPTIONS, setting.EnableObjcExceptions ? "YES" : "NO");
        if (!string.IsNullOrEmpty(setting.CLanguageDialect))
        {
            pbxProject.SetBuildProperty(unityTargetGuid, "GCC_C_LANGUAGE_STANDARD", setting.CLanguageDialect);
        }

        if (!string.IsNullOrEmpty(setting.CopyDirectoryPath))
            DirectoryProcessor.CopyAndAddBuildToXcode(pbxProject, unityTargetGuid, setting.CopyDirectoryPath, buildPath, "");

        //编译器标记（Compiler flags）
        foreach (XcodeProjectSetting.CompilerFlagsSet compilerFlagsSet in setting.CompilerFlagsSetList)
        {
            foreach (string targetPath in compilerFlagsSet.TargetPathList)
            {
                if (!pbxProject.ContainsFileByProjectPath(targetPath))
                    continue;
                string fileGuid = pbxProject.FindFileGuidByProjectPath(targetPath);
                List<string> flagsList = pbxProject.GetCompileFlagsForFile(unityTargetGuid, fileGuid);
                flagsList.Add(compilerFlagsSet.Flags);
                pbxProject.SetCompileFlagsForFile(unityTargetGuid, fileGuid, flagsList);
            }
        }
        //引用 EmbedFrameworks
        foreach (var item in setting.EmbedFrameworks)
        {
            var fileGuid = pbxProject.AddFile(Application.dataPath + "/" + item.folder + item.files, "Frameworks/" + item.folder + item.files, PBXSourceTree.Sdk);
            PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, unityTargetGuid, fileGuid);
        }

        //引用内部框架
        foreach (var item in setting.FrameworkList)
        {
            pbxProject.AddFrameworkToProject(unityTargetGuid, item.name, !item.require);
        }

        //引用.tbd文件
        foreach (var tbd in setting.TbdList)
        {
            if (tbd.target == XcodeProjectSetting.XCodeTarget.UnityFrameworkTarget)
            {
                pbxProject.AddFileToBuild(unityFrameworkTargetGuid, pbxProject.AddFile("usr/lib/" + tbd, "Frameworks/" + tbd, PBXSourceTree.Sdk));
            }
            else
            {
                pbxProject.AddFileToBuild(unityTargetGuid, pbxProject.AddFile("usr/lib/" + tbd, "Frameworks/" + tbd, PBXSourceTree.Sdk));
            }
        }

        //设置OTHER_LDFLAGS
        pbxProject.UpdateBuildProperty(unityTargetGuid, XcodeProjectSetting.LINKER_FLAG_KEY, setting.LinkerFlagArray, null);
        //设置Framework Search Paths
        pbxProject.UpdateBuildProperty(unityTargetGuid, XcodeProjectSetting.FRAMEWORK_SEARCH_PATHS_KEY, setting.FrameworkSearchPathArray, null);


        File.WriteAllText(pbxProjPath, pbxProject.WriteToString());

        bool needModifyCapability =
            setting.EnableIcloudKeyValue ||
            setting.EnableIcloudDocument ||
            setting.EnableCloudKit ||
            setting.AddIcloudDefaultContainer ||
            setting.AddPushNotifaction ||
            setting.AssociatedDomains?.Length > 0 ||
            setting.AddSignInWithApple;

        //Add Capability
        if (needModifyCapability)
        {
            ProjectCapabilityManager projectCapabilityManager = new ProjectCapabilityManager(pbxProjPath, "Unity-iPhone/mmk.entitlements", "Unity-iPhone");
            if (setting.EnableIcloudKeyValue ||
                setting.EnableIcloudDocument ||
                setting.EnableCloudKit ||
                setting.AddIcloudDefaultContainer)
            {
                projectCapabilityManager.AddiCloud(setting.EnableIcloudKeyValue, setting.EnableIcloudDocument, setting.EnableCloudKit, setting.AddIcloudDefaultContainer, setting.IcloudCustomContainer);
            }
            if (setting.AddPushNotifaction)
            {
                Debug.Log("AddPushNotifaction ");
                projectCapabilityManager.AddPushNotifications(true);
                projectCapabilityManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
            }

            if (setting.AddSignInWithApple)
            {
                // projectCapabilityManager.AddSignInWithApple(setting.AddSignInRequire);
                projectCapabilityManager.AddSignInWithApple();
            }

            if (setting.AssociatedDomains.Length > 0)
            {
                projectCapabilityManager.AddAssociatedDomains(setting.AssociatedDomains);

            }
            projectCapabilityManager.WriteToFile();
        }


        //已经存在的文件，拷贝替换
        foreach (XcodeProjectSetting.CopeFiles file in setting.CopeFilesList)
        {
            File.Copy(Application.dataPath + file.sourcePath, buildPath + file.copyPath, true);
        }



        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.h", buildPath + "/Classes/UnityAppController.h", true);
        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.mm", buildPath + "/Classes/UnityAppController.mm", true);

        //设置Plist
        InfoPlistProcessor.SetInfoPlist(buildPath, setting);
    }

#endif
}
#endif