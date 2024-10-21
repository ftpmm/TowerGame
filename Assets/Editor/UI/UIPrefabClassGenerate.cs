using UnityEditor;
using System;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.InputSystem.OnScreen;
using UnityEditor.Compilation;
using UnityEditor.VersionControl;

[InitializeOnLoad]
public class UIPrefabClassGenerate
{
    public struct MemberInfo
    {
        public UnityEngine.Object obj;
        public Type objType;
    }

    public static Dictionary<string, Type> MemberMap = new Dictionary<string, Type>() {
        { "m_go",typeof(GameObject) },
        { "m_txt",typeof(Text) },
        { "m_img",typeof(Image) },
        { "m_rawImg",typeof(RawImage) },
        { "m_sli",typeof(Slider) },
        { "m_scroll",typeof(ScrollRect) },
        { "m_trans",typeof(Transform) },
        { "m_rectTrans",typeof(RectTransform) },
        { "m_VLay",typeof(VerticalLayoutGroup) },
        { "m_HLay",typeof(HorizontalLayoutGroup) },
        { "m_spRd",typeof(SpriteRenderer) },
        { "m_rd",typeof(Renderer) },
        { "m_input",typeof(InputField) },
        { "m_joy",typeof(OnScreenStick) },
        //不够可以这里继续加
    };

    static UIPrefabClassGenerate()
    {
        // 打开Prefab编辑界面回调
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        // Prefab被保存之前回调
        PrefabStage.prefabSaving += OnPrefabSaving;
        // Prefab被保存之后回调
        PrefabStage.prefabSaved += OnPrefabSaved;
        // 关闭Prefab编辑界面回调
        PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        EditorApplication.update += OnCompilationFinish;
    }

    static void OnCompilationFinish()
    {
        if (!string.IsNullOrEmpty(closeAssetPath))
        {
            float deltaTime = Time.realtimeSinceStartup - genDelay;
            Debug.Log("delay = " + genDelay);
            if (deltaTime >= 1f)
            {
                string tmpStr = closeAssetPath;
                closeAssetPath = string.Empty;

                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(tmpStr);
                var rootObj = GameObject.Find("Engine/UIRoot");

                var assetGo = GameObject.Instantiate(prefabAsset, rootObj.transform) as GameObject;
                assetGo.transform.localScale = Vector3.one;

                GenUIPrefabMono(assetGo, tmpStr);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
    }
    static void OnPrefabStageOpened(PrefabStage stage)
    {
        
    }

    private static string closeAssetPath;
    private static float genDelay = 1f;

    static void OnPrefabStageClosing(PrefabStage stage)
    {
        genDelay = Time.realtimeSinceStartup;
        closeAssetPath = stage.assetPath;
    }

    static void OnPrefabSaving(GameObject go)
    {

    }

    static void OnPrefabSaved(GameObject go)
    {
    }

    [MenuItem("工具/UI/生成选中UIprefab的mono")]
    static void GenSelectUIPrefabMono()
    {
        var Select = Selection.activeObject;

        var path = AssetDatabase.GetAssetPath(Select);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        List<string> fileList = new List<string>();
        if(Directory.Exists(path))
        {
            var files = Directory.GetFiles(path, "*.prefab");
            fileList.AddRange(files);
        }
        else if(File.Exists(path) && path.EndsWith(".prefab"))
        {
            fileList.Add(path);
        }

        for(int i = 0; i < fileList.Count; i++)
        {
            var filePath = fileList[i];
            if(filePath.IndexOf("prefab/ui") == -1)
            {
                continue;
            }
            filePath.Replace(Application.dataPath, "Assets/");
            var goAsset = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
            var rootObj = GameObject.Find("Engine/UIRoot");

            var go = GameObject.Instantiate(goAsset, rootObj.transform) as GameObject;
            if(goAsset != null)
            {
                GenUIPrefabMono(go, filePath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    static void GenUIPrefabMono(GameObject go, string assetPath)
    {
        if (assetPath.IndexOf("prefab/ui") == -1)
        {
            return;
        }

        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        string monoFileName = fileName + "Mono";

        var assembly = System.Reflection.Assembly.GetAssembly(typeof(lzengine.LZGame));

        Type monoType = null;
        Type type = assembly.GetType(monoFileName);
        if (type != null)
        {
            monoType = type;
        }

        Type[] allTypes = assembly.GetTypes();
        for (int i = 0; i < allTypes.Length; i++)
        {
            if (allTypes[i].Name == monoFileName)
            {
                monoType = allTypes[i];
                break;
            }
        }

        string monoFilePath = Path.GetFullPath(Application.dataPath + "/Script/Game/UI/MonoGen/" + monoFileName + ".cs");

        if (monoType != null)
        {
            var oldMono = go.GetComponents(monoType);
            if (oldMono != null)
            {
                foreach(var cc in oldMono)
                {
                    Component.DestroyImmediate(cc);
                }
                
            }

            if (File.Exists(monoFilePath))
            {
                File.Delete(monoFilePath);
            }
        }

        //获取UIPrefab里的所有变量
        Dictionary<string, MemberInfo> memberDict = new Dictionary<string, MemberInfo>();
        GetMembers(go.transform, go.transform, memberDict);


        //创建新的Mono文件
        string monoClassFile =
            "//using UnityEngine;\n" +
            "//using UnityEngine.UI;\n\n" +
            "namespace lzengine {\n" +
            "   public class " + monoFileName + ":UIBaseMono\n" +
            "   {\n";

        foreach (var member in memberDict)
        {
            monoClassFile += "       public " + member.Value.objType.FullName + " " + member.Key + ";\n";
        }
        monoClassFile += "   }\n}\n";

        File.WriteAllText(monoFilePath, monoClassFile, System.Text.Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        BindMono2Prefab(assetPath, go);
    }

    static void BindMono2Prefab(string assetPath, GameObject go)
    {
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        string monoFileName = fileName + "Mono";

        Type monoType = null;
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(lzengine.LZGame));
        var type = assembly.GetType(monoFileName);
        if (type != null)
        {
            monoType = type;
        }

        var allTypes = assembly.GetTypes();
        for (int i = 0; i < allTypes.Length; i++)
        {
            if (allTypes[i].Name == monoFileName)
            {
                monoType = allTypes[i];
                break;
            }
        }

        //获取UIPrefab里的所有变量
        Dictionary<string, MemberInfo> memberDict = new Dictionary<string, MemberInfo>();
        GetMembers(go.transform, go.transform, memberDict);

        if (monoType != null)
        {
            var newCom = go.AddComponent(monoType);
            var fields = newCom.GetType().GetFields();
            foreach (var f in fields)
            {
                if (f.IsPublic)
                {
                    string propName = f.Name;
                    if(memberDict.ContainsKey(propName))
                    {
                        var objValue = memberDict[propName].obj;
                        f.SetValue(newCom, memberDict[propName].obj);
                    }
                }
            }
        }

        PrefabUtility.SaveAsPrefabAsset(go, assetPath);

        GameObject.DestroyImmediate(go);
    }

    static void GetMembers(Transform uiTrans, Transform trans, Dictionary<string, MemberInfo> ret)
    {
        int count = trans.childCount;
        for(int i = 0; i < count; i++) 
        { 
            Transform childTrans = trans.GetChild(i);
            foreach(var memNamePrex in MemberMap.Keys)
            {
                if (childTrans.name.StartsWith(memNamePrex))
                {
                    if (ret.ContainsKey(childTrans.name))
                    {
                        Debug.LogErrorFormat("{0}存在相同类型的变量名{1}", uiTrans.name, childTrans.name);
                        continue;
                    }

                    UnityEngine.Object tmpObj = null;
                    if(memNamePrex == "m_go")
                    {
                        tmpObj = childTrans.gameObject;
                    }
                    else
                    {
                        tmpObj = childTrans.GetComponent(MemberMap[memNamePrex]);
                    }
                    ret[childTrans.name] = new MemberInfo()
                    {
                        obj = tmpObj,
                        objType = MemberMap[memNamePrex],
                    };
                }
            }
            GetMembers(uiTrans, childTrans, ret);
        }
        
    }
}