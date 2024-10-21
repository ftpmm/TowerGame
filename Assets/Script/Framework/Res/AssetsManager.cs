using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace lzengine
{
    public enum EAssetServiceMode
    {
        Editor,
        Local,
        Remote,
    }

    /// <summary>
    /// 内置资源Services
    /// </summary>
    public class GameBuildinQueryServices : IBuildinQueryServices
    {
        public bool Query(string packageName, string fileName)
        {
            string filePath = AssetsManager.BuildInFolderPrex + fileName;
            return StreamingAssetsHelper.FileExists(filePath);
        }
    }

    /// <summary>
    /// 加密services
    /// </summary>
    public class GameDecryptionServices : IDecryptionServices
    {
        public AssetBundle LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            managedStream = null;
            return null;
        }

        public AssetBundleCreateRequest LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            managedStream = null;
            return null;
        }
    }

    public class GameRemoteServices : IRemoteServices
    {
        public string defaultHostServer = "http://192.168.101.15/H5/Android/AssetBundles";
        public string fallbackHostServer = "http://192.168.101.15/H5/Android/AssetBundles";

        public GameRemoteServices(string defaultURL, string fallbackURL)
        {
            defaultHostServer = defaultURL;
            fallbackHostServer = fallbackURL;
        }

        public string GetRemoteFallbackURL(string fileName)
        {
            return Path.Combine(fallbackHostServer, fileName);
        }

        public string GetRemoteMainURL(string fileName)
        {
            return Path.Combine(defaultHostServer, fileName);
        }
    }

    public class AssetsManager:Singleton<AssetsManager>
    {
        private bool _isInit = false;
        public bool IsInit { get { return _isInit; } }

        private bool _isUpdateFinish = false;
        public bool IsUpdateFinish { get { return _isUpdateFinish; } }

        private string mPackName = string.Empty;

        private string mResLocalizationPath = string.Empty;

        public static string BuildInFolderPrex = "";

        /// <summary>
        /// 正在进行的加载
        /// </summary>
        Dictionary<string, HandleBase> loadingHandlerDict = new Dictionary<string, HandleBase>();

        public IEnumerator InitAsync(EAssetServiceMode mode)
        {
            YooAssets.Initialize();

            string localStr = LocalizationMgr.Instance.CurLocalization.ToString();

            mPackName = "Pack" + localStr;

            mResLocalizationPath = "Assets/Res/" + localStr.ToLower();

            BuildInFolderPrex = "AssetBundles/" + mPackName + "/";

            // 创建默认的资源包
            var package = YooAssets.CreatePackage(mPackName);
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(package);

            switch (mode)
            {
                case EAssetServiceMode.Editor:
                    yield return InitializeYooAssetEditor(package);
                    break;
                case EAssetServiceMode.Local:
                    yield return InitializeYooAssetLocal(package);
                    break;
                case EAssetServiceMode.Remote:
                    yield return InitializeYooAssetRemote(package);
                    break;
                default:
                    yield return InitializeYooAssetLocal(package);
                    break;
            }
        }

        private IEnumerator InitializeYooAssetEditor(ResourcePackage pack)
        {
            var initParams = new EditorSimulateModeParameters();
            initParams.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), mPackName);
            yield return pack.InitializeAsync(initParams);

            _isInit = true;
        }

        private IEnumerator InitializeYooAssetLocal(ResourcePackage pack)
        {
            var initParameters = new OfflinePlayModeParameters();
            yield return pack.InitializeAsync(initParameters);

            string packVersion = pack.GetPackageVersion();
            LZDebug.Log("本地资源版本号：" + packVersion);

            _isInit = true;
        }

        private IEnumerator InitializeYooAssetRemote(ResourcePackage pack)
        {
            string defaultHostServer = "http://192.168.101.15/H5/Android/AssetBundles";
            string fallbackHostServer = "http://192.168.101.15/H5/Android/AssetBundles";

            var initParameters = new HostPlayModeParameters();
            initParameters.BuildinQueryServices = new GameBuildinQueryServices();
            initParameters.DecryptionServices = new GameDecryptionServices();
            initParameters.RemoteServices = new GameRemoteServices(defaultHostServer, fallbackHostServer);
            var initOperation = pack.InitializeAsync(initParameters);
            yield return initOperation;

            if (initOperation.Status == EOperationStatus.Succeed)
            {
                LZDebug.Log("资源管理器初始化成功。");
            }
            else
            {
                LZDebug.LogError($"资源管理器初始化失败 ：{initOperation.Error}");
            }

            string packVersion = pack.GetPackageVersion();
            LZDebug.Log("本地资源版本号：" + packVersion);

            _isInit = true;
        }

        public IEnumerator StartUpdate()
        {
            yield return UpdatePackageVersion();
        }

        private IEnumerator UpdatePackageVersion()
        {
            var pack = YooAssets.GetPackage(mPackName);
            var op = pack.UpdatePackageVersionAsync();
            yield return op;

            if(op.Status == EOperationStatus.Succeed)
            {
                string packVersion = op.PackageVersion;
                LZDebug.Log($"资源版本更新成功 : {packVersion}");

                yield return UpdatePackageManifest(packVersion);
            }
            else
            {
                _isUpdateFinish = true;
                LZDebug.Log($"更新资源版本失败!");
            }
        }

        private IEnumerator UpdatePackageManifest(string packVersion)
        {
            bool savePackageVersion = true;
            var package = YooAssets.GetPackage(mPackName);
            var operation = package.UpdatePackageManifestAsync(packVersion, savePackageVersion);
            yield return operation;

            if (operation.Status == EOperationStatus.Succeed)
            {
                //更新成功
                LZDebug.Log($"更新 Manifest 成功");

                yield return DownLoadBundles();
            }
            else
            {
                _isUpdateFinish = true;
                //更新失败
                Debug.LogError(operation.Error);
            }
        }

        private IEnumerator DownLoadBundles()
        {
            var downOp = YooAssets.CreateResourceDownloader(4, 3);
            if(downOp.TotalDownloadCount == 0)
            {
                LZDebug.Log("待下载文件数为0，不需要下载");
                _isUpdateFinish = true;
                yield break;
            }

            downOp.BeginDownload();
            yield return downOp;

            if (downOp.Status == EOperationStatus.Succeed)
            {
                LZDebug.Log("下载热更AB完成了");
                _isUpdateFinish = true;
            }
            else
            {
                _isUpdateFinish = true;
            }

        }

        private string GetAssetPath(string path)
        {
            return mResLocalizationPath + "/" + path;
        }

        public void LoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            path = GetAssetPath(path);
            CoroutineRunner.Run(Internal_LoadAssetAsync(path, callBack));
        }

        public AssetHandle LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            path = GetAssetPath(path);
            AssetHandle ret = YooAssets.LoadAssetAsync<T>(path);
            return ret;
        }

        private IEnumerator Internal_LoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            var handler = YooAssets.LoadAssetAsync<T>(path);
            yield return handler;
            if(handler.Status == EOperationStatus.Succeed)
            {
                if(callBack != null)
                {
                    callBack(handler.AssetObject as T);
                }
            }
            else
            {
                if(callBack != null)
                {
                    callBack(default(T));
                }
            }
        }

        public AllAssetsHandle LoadAllAssetsAsync<T>(string path) where T:UnityEngine.Object
        {
            path = GetAssetPath(path);
            return YooAssets.LoadAllAssetsAsync<T>(path);
        }

        public void LoadSceneAsync(string path, LoadSceneMode sceneMode, Action<bool> callBack)
        {
            path = GetAssetPath(path);
            CoroutineRunner.Run(Internal_LoadSceneAsync(path, sceneMode, callBack));
        }

        private IEnumerator Internal_LoadSceneAsync(string path, LoadSceneMode sceneMode, Action<bool> callBack)
        {
            var handler = YooAssets.LoadSceneAsync(path, sceneMode);
            yield return handler;
            if (handler.Status == EOperationStatus.Succeed)
            {
                if (callBack != null)
                {
                    callBack(true);
                }
            }
            else
            {
                if (callBack != null)
                {
                    callBack(false);
                }
            }
        }

        public void Destroy(string path)
        {
            var pack = YooAssets.GetPackage(mPackName);
            pack.UnloadUnusedAssets();

            YooAssets.DestroyPackage(mPackName);
        }

        public void DestroyGameObject(GameObject go)
        {
            if(go == null)
            {
                return;
            }
            GameObject.Destroy(go);
        }
    }
}
