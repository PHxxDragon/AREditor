using UnityEngine;
using System;
using System.IO;
using TriLibCore;
using Piglet;
using UnityEngine.Networking;
using EAR.DownloadHandler;
using System.Collections;
using System.Collections.Generic;

namespace EAR.AR
{
    public class ModelLoader : MonoBehaviour
    {
        public event Action<string> OnLoadStarted;
        public event Action<string, GameObject> OnLoadEnded;
        public event Action<string, string> OnLoadError;
        public event Action<float, string> OnLoadProgressChanged;

        private List<GltfImportTask> tasks = new List<GltfImportTask>();

        public void LoadModel(string assetId, string url, string extension, bool isZipFile)
        {
            StartCoroutine(LoadModelCoroutine(assetId, url, extension, isZipFile));
        }

        private IEnumerator LoadModelCoroutine(string assetId, string url, string extension, bool isZipFile)
        {
            OnLoadStarted?.Invoke(assetId);
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
                    OnLoadError?.Invoke(assetId, "Connection error");
                }
                else if (uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Protocol Error: " + uwr.error);
                    error = true;
                    OnLoadError?.Invoke(assetId, uwr.error);
                } else
                {
                    data = uwr.downloadHandler.data;
                }
            }
            if (!error)
            {
                if (extension == "gltf" || extension == "glb")
                {
                    LoadModelUsingPiglet(assetId, data);
                }
                else
                {
                    LoadModelUsingTrilib(assetId, data, extension, isZipFile);
                }
            }
        }

        //======================================piglet================================================

        private void LoadModelUsingPiglet(string assetId, byte[] data)
        {
            GltfImportTask task = RuntimeGltfImporter.GetImportTask(data);
            SetOnComplete(assetId, task);
            SetOnException(assetId, task);
            task.OnProgress += OnProgress;
            tasks.Add(task);
        }

        void Update()
        {
            List<GltfImportTask> removeTasks = null;
            foreach (GltfImportTask task in tasks)
            {
                if (task.State == GltfImportTask.ExecutionState.Completed)
                {
                    if (removeTasks == null)
                        removeTasks = new List<GltfImportTask>();
                    removeTasks.Add(task);
                } else
                {
                    task.MoveNext();
                }
            }
            if (removeTasks != null)
                tasks.RemoveAll((t) => removeTasks.Contains(t));
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

        private void SetOnComplete(string assetId, GltfImportTask task)
        {
            task.OnCompleted = (GameObject model) =>
            {
                OnLoadEnded?.Invoke(assetId, model);
            };
        }

        private void SetOnException(string assetId, GltfImportTask task)
        {
            task.OnException = (error) =>
            {
                OnLoadError?.Invoke(assetId, "Error loading model " + assetId);
            };
        }

        //===========================================Trilib==========================================

        private void LoadModelUsingTrilib(string assetId, byte[] data, string extension, bool isZipFile)
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            using MemoryStream memoryStream = new MemoryStream(data);

            if (isZipFile)
            {
                AssetLoaderZip.LoadModelFromZipStream(memoryStream, null, GetOnMaterialsLoad(assetId), OnProgress, GetOnError(assetId), null, assetLoaderOptions);
            }
            else
            {
                AssetLoader.LoadModelFromStream(memoryStream, "model." + extension, extension, null, GetOnMaterialsLoad(assetId), OnProgress, GetOnError(assetId), null, assetLoaderOptions);
            }
        }

        private Action<AssetLoaderContext> GetOnMaterialsLoad(string assetId)
        {
            return (AssetLoaderContext obj) =>
            {
                OnLoadEnded?.Invoke(assetId, obj.RootGameObject);
            };
        }

        private Action<IContextualizedError> GetOnError(string assetId)
        {
            return (obj) =>
            {
                OnLoadError?.Invoke(assetId, "Error loading model " + assetId);
            };
        }

        private void OnProgress(AssetLoaderContext arg1, float arg2)
        {
            OnLoadProgressChanged?.Invoke(arg2, "Loading...");
        }
    }
}