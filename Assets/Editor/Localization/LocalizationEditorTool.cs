using lzengine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using YooAsset.Editor;

namespace lzengine_editor
{
    public class LocalizationEditorTool
    {
        [MenuItem("工具/本地化/所有多语言目录资源重定向")]
        public static void RedirectLocalizationRes()
        {
            var strArr = Enum.GetNames(typeof(ELocalization));
            for(int i = 0; i < strArr.Length; i++)
            {
                string localStr = strArr[i].ToLower();
                string localDict = Application.dataPath + "/Res/" + localStr;

                if(!Directory.Exists(localDict) || localStr == "zhs")
                {
                    continue;
                }

                string[] prefabFiles = Directory.GetFiles(localDict, "*.prefab", SearchOption.AllDirectories);
                for(int p = 0; p < prefabFiles.Length; p++)
                {
                    string prefabAssetPath = prefabFiles[p].Replace(Application.dataPath, "Assets");
                    GameObject prefabAsset  = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
                    var prefabIns = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
                    if(prefabIns != null)
                    {
                        bool findAsset = false;

                        var imgs = prefabIns.GetComponents<Image>();
                        foreach (Image image in imgs)
                        {
                            if (image.sprite != null)
                            {
                                findAsset |= ReplaceSprite(image, localStr);
                            }
                        }

                        var spRenders = prefabIns.GetComponents<SpriteRenderer>();
                        foreach (SpriteRenderer spRender in spRenders)
                        {
                            if (spRender.sprite != null)
                            {
                                findAsset |= ReplaceSprite(spRender, localStr);
                            }
                        }
                        spRenders = prefabIns.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer spRender in spRenders)
                        {
                            if (spRender.sprite != null)
                            {
                                findAsset |= ReplaceSprite(spRender, localStr);
                            }
                        }

                        if (findAsset)
                        {
                            PrefabUtility.SaveAsPrefabAsset(prefabIns, prefabAssetPath);
                        }

                        GameObject.DestroyImmediate(prefabIns);
                    }
                    
                }

                string[] rawFiles = Directory.GetFiles(localDict, "*.prefab", SearchOption.AllDirectories);
                for (int a = 0; a < rawFiles.Length; a++)
                {
                    RelaceRawFile(rawFiles[a], localStr);
                }

                rawFiles = Directory.GetFiles(localDict, "*.anim", SearchOption.AllDirectories);
                for(int a = 0; a < rawFiles.Length; a++)
                {
                    RelaceRawFile(rawFiles[a], localStr);
                }
                rawFiles = Directory.GetFiles(localDict, "*.controllor", SearchOption.AllDirectories);
                for (int a = 0; a < rawFiles.Length; a++)
                {
                    RelaceRawFile(rawFiles[a], localStr);
                }
                rawFiles = Directory.GetFiles(localDict, "*.asset", SearchOption.AllDirectories);
                for (int a = 0; a < rawFiles.Length; a++)
                {
                    RelaceRawFile(rawFiles[a], localStr);
                    RelaceRawFileEx(rawFiles[a], localStr);
                }
                rawFiles = Directory.GetFiles(localDict, "*.unity", SearchOption.AllDirectories);
                for (int a = 0; a < rawFiles.Length; a++)
                {
                    RelaceRawFile(rawFiles[a], localStr);
                }
            }

            //处理assetcollect
            string collectFile = "Assets/Res/AssetBundleCollectorSetting.asset";
            var collectSetting = AssetDatabase.LoadAssetAtPath<AssetBundleCollectorSetting>(collectFile);
            AssetBundleCollectorPackage zhsPack = null;
            var packs = collectSetting.Packages;
            Dictionary<string, AssetBundleCollectorPackage> packsDict = new Dictionary<string, AssetBundleCollectorPackage>();
            for(int i = 0; i < packs.Count; i++)
            {
                packsDict[packs[i].PackageName] = packs[i];
                if (packs[i].PackageName == "PackZhs")
                {
                    zhsPack = packs[i];
                }
            }
            var newPacks = new List<AssetBundleCollectorPackage>();
            newPacks.AddRange(packs);

            for (int i = 0; i < strArr.Length; i++)
            {
                string str = strArr[i];
                string localDictPath = Application.dataPath + "/Res/" + str.ToLower();
                if(!Directory.Exists(localDictPath))
                {
                    continue;
                }

                string packName = "Pack" + str.Substring(0, 1).ToUpper() + str.Substring(1);
                if(packName.CompareTo(zhsPack.PackageName) == 0)
                {
                    continue;
                }
                bool hasPack = packsDict.ContainsKey(packName);
                if(hasPack)
                {
                    continue;
                }

                AssetBundleCollectorPackage newPack = new AssetBundleCollectorPackage();
                newPack.PackageName = packName;
                newPacks.Add(newPack);
                newPack.Groups.Clear();
                for(int g = 0; g < zhsPack.Groups.Count; g++)
                {
                    AssetBundleCollectorGroup newGroup = new AssetBundleCollectorGroup();
                    newPack.Groups.Add(newGroup);
                    var oldGroup = zhsPack.Groups[g];
                    newGroup.GroupName = oldGroup.GroupName;
                    newGroup.GroupDesc = oldGroup.GroupDesc;
                    newGroup.AssetTags = oldGroup.AssetTags;
                    newGroup.ActiveRuleName = oldGroup.ActiveRuleName;
                    for(int c = 0; c < zhsPack.Groups[g].Collectors.Count; c++)
                    {
                        AssetBundleCollector newCollector = new AssetBundleCollector();
                        newGroup.Collectors.Add(newCollector);
                        var oldCollector = zhsPack.Groups[g].Collectors[c];
                        string guidStr = oldCollector.CollectorGUID;
                        string assetPath = AssetDatabase.GUIDToAssetPath(guidStr);

                        Regex reg = new Regex(@"/Res/(.+?)/");
                        string directAssetPath = reg.Replace(assetPath, "/Res/" + str + "/");

                        if (assetPath.CompareTo(directAssetPath) == 0)
                        {
                            continue;
                        }
                        string newGuid = AssetDatabase.AssetPathToGUID(directAssetPath);
                        newCollector.CollectorGUID = newGuid;
                        newCollector.CollectPath = directAssetPath;
                        newCollector.CollectorType = oldCollector.CollectorType;
                        newCollector.AddressRuleName = oldCollector.AddressRuleName;
                        newCollector.PackRuleName = oldCollector.PackRuleName;
                        newCollector.FilterRuleName = oldCollector.FilterRuleName;
                        newCollector.AssetTags = oldCollector.AssetTags;
                        newCollector.UserData = oldCollector.UserData;
                    }
                    
                }
            }

            collectSetting.Packages = newPacks;

            if(!File.Exists(collectFile))
            {
                AssetDatabase.CreateAsset(collectSetting, collectFile);
            }
            
            AssetDatabase.SaveAssets();
        }

        private static void RelaceRawFile(string fileName, string localStr)
        {
            bool isChange = false;
            var lineArr = File.ReadAllLines(fileName);
            for (int l = 0; l < lineArr.Length; l++)
            {
                string line = lineArr[l];
                Regex guidReg = new Regex(@"guid:(.+?),");
                var match = guidReg.Match(line);
                string guidStr = match.Groups[1].Value.Trim();
                string assetPath = AssetDatabase.GUIDToAssetPath(guidStr);

                Regex reg = new Regex(@"/Res/(.+?)/");
                string directAssetPath = reg.Replace(assetPath, "/Res/" + localStr + "/");

                if (assetPath.CompareTo(directAssetPath) == 0)
                {
                    continue;
                }
                string directGuidStr = AssetDatabase.AssetPathToGUID(directAssetPath);
                if (string.IsNullOrEmpty(directGuidStr))
                {
                    continue;
                }

                lineArr[l] = guidReg.Replace(line, "guid:" + directGuidStr + ",");
                isChange = true;
            }

            if (isChange)
            {
                File.WriteAllLines(fileName, lineArr);
            }
        }

        private static void RelaceRawFileEx(string fileName, string localStr)
        {
            bool isChange = false;
            var lineArr = File.ReadAllLines(fileName);
            for (int l = 0; l < lineArr.Length; l++)
            {
                string line = lineArr[l];
                Regex guidReg = new Regex(@"CollectorGUID:(.+)");
                var match = guidReg.Match(line);
                string guidStr = match.Groups[1].Value.Trim();
                string assetPath = AssetDatabase.GUIDToAssetPath(guidStr);

                Regex reg = new Regex(@"/Res/(.+?)/");
                string directAssetPath = reg.Replace(assetPath, "/Res/" + localStr + "/");

                if (assetPath.CompareTo(directAssetPath) == 0)
                {
                    continue;
                }
                string directGuidStr = AssetDatabase.AssetPathToGUID(directAssetPath);
                if (string.IsNullOrEmpty(directGuidStr))
                {
                    continue;
                }

                lineArr[l] = guidReg.Replace(line, "CollectorGUID:" + directGuidStr);
                isChange = true;
            }

            if (isChange)
            {
                File.WriteAllLines(fileName, lineArr);
            }
        }

        private static bool ReplaceSprite(Image spContainer, string targetLang)
        {
            bool findAsset = false;
            string assetPath = AssetDatabase.GetAssetPath(spContainer.sprite.texture);
            Regex reg = new Regex(@"/Res/(.+?)/");
            string directAssetPath = reg.Replace(assetPath, "/Res/" + targetLang + "/");
            if (assetPath.CompareTo(directAssetPath) == 0)
            {
                return false;
            }
            string guidStr = AssetDatabase.AssetPathToGUID(directAssetPath);
            if (string.IsNullOrEmpty(guidStr))
            {
                return false;
            }

            var directSpriteArr = AssetDatabase.LoadAllAssetsAtPath(directAssetPath);
            foreach (var directAsset in directSpriteArr)
            {
                if (directAsset is Sprite)
                {
                    if (directAsset.name.CompareTo(spContainer.sprite.name) == 0)
                    {
                        findAsset = true;
                        spContainer.sprite = (directAsset as Sprite);
                        break;
                    }
                }
            }

            return findAsset;
        }

        private static bool ReplaceSprite(SpriteRenderer spContainer, string targetLang)
        {
            bool findAsset = false;
            string assetPath = AssetDatabase.GetAssetPath(spContainer.sprite.texture);
            Regex reg = new Regex(@"/Res/(.+?)/");
            string directAssetPath = reg.Replace(assetPath, "/Res/" + targetLang + "/");
            if (assetPath.CompareTo(directAssetPath) == 0)
            {
                return false;
            }
            string guidStr = AssetDatabase.AssetPathToGUID(directAssetPath);
            if (string.IsNullOrEmpty(guidStr))
            {
                return false;
            }

            var directSpriteArr = AssetDatabase.LoadAllAssetsAtPath(directAssetPath);
            foreach (var directAsset in directSpriteArr)
            {
                if (directAsset is Sprite)
                {
                    if (directAsset.name.CompareTo(spContainer.sprite.name) == 0)
                    {
                        findAsset = true;
                        spContainer.sprite = (directAsset as Sprite);
                        break;
                    }
                }
            }

            return findAsset;
        }
    }
}
