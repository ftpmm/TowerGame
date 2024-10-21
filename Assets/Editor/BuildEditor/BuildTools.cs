using System;
using System.Collections.Generic;
using UnityEditor;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEngine;
using lzengine;
using System.IO;
using YooAsset.Editor;

namespace lzengine_editor
{
    public class BuildTools
    {
        [MenuItem("工具/打包/一键打包(Android)")]
        public static void BuildAll()
        {
            BuildHotfix();

            LocalizationEditorTool.RedirectLocalizationRes();

            string bundleVersion = BuildBundles();
            //复制ab到测试cdn目录
            CopyBundlesToH5(bundleVersion);

            BuildTargetPlayer();
        }

        [MenuItem("工具/打包/打热更AB")]
        public static void BuildHotfixBundle()
        {
            BuildHotfix();

            LocalizationEditorTool.RedirectLocalizationRes();

            string bundleVersion = BuildBundles();
            //复制ab到测试cdn目录
            CopyBundlesToH5(bundleVersion);
        }

        private static void CopyBundlesToH5(string bundleVersion)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            string buildABPath = Path.GetFullPath(Application.dataPath + "/../Bundles/" + target.ToString() + "/PackZhs/" + bundleVersion);
            string H5Path = Path.GetFullPath(Application.dataPath + "/../H5/" + target.ToString() + "/AssetBundles");
            if(Directory.Exists(H5Path))
            {
                Directory.Delete(H5Path, true);
            }
            Directory.CreateDirectory(H5Path);

            string[] files = Directory.GetFiles(buildABPath);
            foreach (string file in files)
            {
                string h5FilePath = H5Path + "/" + Path.GetFileName(file);
                File.Copy(file, h5FilePath, true);
            }
        }

        private static void BuildTargetPlayer()
        {
            string buildPath = Path.GetFullPath(Application.dataPath + "/../AutoBuild");
            if(Directory.Exists(buildPath) )
            {
                Directory.Delete(buildPath, true );
            }
            Directory.CreateDirectory(buildPath);

            var bTarget = EditorUserBuildSettings.activeBuildTarget;

            var packName = PlayerSettings.productName;

            string targetPath = buildPath + "/" + bTarget.ToString();
            if(Directory.Exists(targetPath) )
            {
                Directory.Delete(targetPath, true);
            }
            Directory.CreateDirectory(targetPath);

            BuildPlayerOptions op = new BuildPlayerOptions();
            op.scenes = new[] { "Assets/Scenes/Main.unity" };
            op.locationPathName = targetPath + "/" + packName + ".apk";
            op.target = bTarget;
            op.targetGroup = BuildPipeline.GetBuildTargetGroup(bTarget);
            op.target = bTarget;
            op.options = BuildOptions.None;

            var report = BuildPipeline.BuildPlayer(op);
            if(report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log("打包成功 包路径=" + targetPath);
            }
            else
            {
                Debug.LogError("打包失败!!!");
            }
        }

        private static string BuildBundles()
        {
            var bTarget = EditorUserBuildSettings.activeBuildTarget;
            EBuildPipeline BuildPipeline = EBuildPipeline.BuiltinBuildPipeline;
            var lNames = Enum.GetNames(typeof(ELocalization));
            string bundleVersion = string.Empty;
            foreach (var tmpName in lNames)
            {
                string rootPath = Path.GetFullPath(Application.dataPath + "/Res/" + tmpName.ToLower());
                if (!Directory.Exists(rootPath))
                {
                    continue;
                }
                string PackageName = "Pack" + tmpName;

                var buildMode = AssetBundleBuilderSetting.GetPackageBuildMode(PackageName, EBuildPipeline.BuiltinBuildPipeline);
                var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(PackageName, BuildPipeline);
                var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(PackageName, BuildPipeline);
                var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(PackageName, BuildPipeline);
                var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(PackageName, BuildPipeline);

                BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
                buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
                buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
                buildParameters.BuildPipeline = BuildPipeline.ToString();
                buildParameters.BuildTarget = bTarget;
                buildParameters.BuildMode = buildMode;
                buildParameters.PackageName = PackageName;
                buildParameters.PackageVersion = GetDefaultPackageVersion();
                buildParameters.VerifyBuildingResult = true;
                buildParameters.FileNameStyle = fileNameStyle;
                buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
                buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
                buildParameters.EncryptionServices = null;
                buildParameters.CompressOption = compressOption;

                BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
                var buildResult = pipeline.Run(buildParameters, true);

                if (buildResult.Success)
                {
                    Debug.LogFormat("构建 {0}的 Bundles 完成", tmpName);
                }

                bundleVersion = GetDefaultPackageVersion();
            }

            return bundleVersion;
        }

        private static void BuildHotfix()
        {
            PrebuildCommand.GenerateAll();
            CompileDllCommand.CompileDllActiveBuildTarget();

            var bTarget = EditorUserBuildSettings.activeBuildTarget;

            string genDllPath = Path.GetFullPath(Application.dataPath + "/../HybridCLRData/HotUpdateDlls/" + bTarget.ToString());
            string genMetaPath = Path.GetFullPath(Application.dataPath + "/../HybridCLRData/AssembliesPostIl2CppStrip/" + bTarget.ToString());
            List<string> dllList = new List<string>()
            {
                "Game.dll"
            };
            List<string> metaList = new List<string>()
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
            };

            var lNames = Enum.GetNames(typeof(ELocalization));
            foreach(var tmpName in lNames)
            {
                string rootPath = Path.GetFullPath(Application.dataPath + "/Res/" + tmpName.ToLower() + "/hotfix");
                if(!Directory.Exists(rootPath))
                {
                    continue;
                }
                string dllPath = rootPath + "/dll";
                if(Directory.Exists(dllPath))
                {
                    Directory.Delete(dllPath, true);
                }
                Directory.CreateDirectory(dllPath);

                for(int i = 0; i < dllList.Count; i++)
                {
                    File.Copy(genDllPath + "/" + dllList[i], dllPath + "/" + dllList[i] + ".bytes", true);
                }

                string metaPath = rootPath + "/meta";
                if(Directory.Exists(rootPath + "/meta"))
                {
                    Directory.Delete(rootPath + "/meta", true);
                }
                Directory.CreateDirectory(rootPath + "/meta");

                for (int i = 0; i < metaList.Count; i++)
                {
                    File.Copy(genMetaPath + "/" + metaList[i], metaPath + "/" + metaList[i] + ".bytes", true);
                }
            }

            AssetDatabase.Refresh();
        }

        private static string GetDefaultPackageVersion()
        {
            int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
        }
    }
}
