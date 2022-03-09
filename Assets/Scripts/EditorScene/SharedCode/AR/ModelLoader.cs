using UnityEngine;
using System;
using System.IO;
using TriLibCore;
using Piglet;
using UnityEngine.Networking;
using EAR.DownloadHandler;
using System.Collections;

namespace EAR.AR
{
    public class ModelLoader : MonoBehaviour
    {
        public event Action OnLoadStarted;
        public event Action OnLoadEnded;
        public event Action<string> OnLoadError;
        public event Action<float, string> OnLoadProgressChanged;

        [SerializeField]
        private GameObject modelContainer;

        private GameObject loadedModel;
        private GltfImportTask task;

        public GameObject GetModel()
        {
            return loadedModel;
        }

        public void LoadModel(string url, string extension, bool isZipFile)
        {
            StartCoroutine(LoadModelCoroutine(url, extension, isZipFile));
        }

        private IEnumerator LoadModelCoroutine(string url, string extension, bool isZipFile)
        {
            OnLoadStarted?.Invoke();
            bool error = false;
            byte[] data = null;
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                byte[] buffer = new byte[1024 * 1024];
                uwr.downloadHandler = new DecryptDownloadHandler(buffer);

                UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
                while (!operation.isDone)
                {
                    OnLoadProgressChanged?.Invoke(uwr.downloadProgress, "Downloading ");
                    yield return null;
                }
                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Connection Error: " + uwr.error);
                    error = true;
                    OnLoadError?.Invoke("Connection error");
                }
                else if (uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Protocol Error: " + uwr.error);
                    error = true;
                    OnLoadError?.Invoke(uwr.error);
                } else
                {
                    data = uwr.downloadHandler.data;
                }
            }
            if (!error)
            {
                if (extension == "gltf" || extension == "glb")
                {
                    LoadModelUsingPiglet(data);
                }
                else
                {
                    LoadModelUsingTrilib(data, extension, isZipFile);
                }
            }
        }

        //======================================piglet================================================

        private void LoadModelUsingPiglet(byte[] data)
        {
            task = RuntimeGltfImporter.GetImportTask(data);
            task.OnCompleted = OnComplete;
            task.OnException += OnException;
            task.OnProgress += OnProgress;
        }

        void Update()
        {
            if (task != null)
                task.MoveNext();
        }

        void OnProgress(GltfImportStep step, int completed, int total)
        {
            if (step == GltfImportStep.Download)
            {
                float downloadedMB = (float)completed / 1000000;
                if (total == 0)
                {
                    OnLoadProgressChanged?.Invoke(downloadedMB, string.Format("{0}: {1:0.00}MB", step, downloadedMB));
                }
                else
                {
                    float totalMB = (float)total / 1000000;
                    OnLoadProgressChanged?.Invoke(downloadedMB / totalMB, string.Format("{0}: {1:0.00}/{2:0.00}MB", step, downloadedMB, totalMB));
                }
            }
            else
            {
                OnLoadProgressChanged?.Invoke(((float)completed) / total, string.Format("{0}: {1}/{2}", step, completed, total));
            }
        }

        private void OnComplete(GameObject importedModel)
        {
            loadedModel = importedModel;
            //loadedModel.transform.parent = modelContainer.transform;
            OnLoadEnded?.Invoke();
            
        }

        private void OnException(Exception e)
        {
            OnLoadError?.Invoke("Error loading model");
        }

        //===========================================Trilib==========================================

        private void LoadModelUsingTrilib(byte[] data, string extension, bool isZipFile)
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            using MemoryStream memoryStream = new MemoryStream(data);
            if (isZipFile)
            {
                AssetLoaderZip.LoadModelFromZipStream(memoryStream, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
            }
            else
            {
                AssetLoader.LoadModelFromStream(memoryStream, "model." + extension, extension, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
            }
        }

        private void OnError(IContextualizedError obj)
        {
            OnLoadError?.Invoke("Error loading model");
        }

        private void OnProgress(AssetLoaderContext arg1, float arg2)
        {
            OnLoadProgressChanged?.Invoke(arg2, "Loading...");
        }

        private void OnMaterialsLoad(AssetLoaderContext obj)
        {
            OnLoadEnded?.Invoke();
        }

        private void OnLoad(AssetLoaderContext obj)
        {
            loadedModel = obj.RootGameObject;
        }
    }
}