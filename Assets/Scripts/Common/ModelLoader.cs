using UnityEngine;
using System;
using System.IO;
using TriLibCore;
using Piglet;
using UnityEngine.Networking;
using EAR.DownloadHandler;
using System.Collections;
using System.Collections.Generic;
using EAR.AnimationPlayer;

namespace EAR.AR
{
    public class ModelLoader : MonoBehaviour
    {
        private List<GltfImportTask> tasks = new List<GltfImportTask>();

        public void LoadModel(string url, string extension, bool isZipFile, Action<GameObject> OnLoadEnded = null, Action<string> OnLoadError = null, Action<float, string> OnLoadProgressChanged = null)
        {
            StartCoroutine(LoadModelCoroutine(url, extension, isZipFile, OnLoadEnded, OnLoadError, OnLoadProgressChanged));
        }

        private IEnumerator LoadModelCoroutine(string url, string extension, bool isZipFile, Action<GameObject> OnLoadEnded = null, Action<string> OnLoadError = null, Action<float, string> OnLoadProgressChanged = null)
        {
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
                    LoadModelUsingPiglet(data, OnLoadEnded, OnLoadError, OnLoadProgressChanged);
                }
                else
                {
                    LoadModelUsingTrilib(data, extension, isZipFile, OnLoadEnded, OnLoadError, OnLoadProgressChanged);
                }
            }
        }

        //======================================piglet================================================

        private void LoadModelUsingPiglet(byte[] data, Action<GameObject> OnLoadEnded = null, Action<string> OnLoadError = null, Action<float, string> OnLoadProgressChanged = null)
        {
            GltfImportTask task = RuntimeGltfImporter.GetImportTask(data);
            SetOnComplete(task, OnLoadEnded);
            SetOnException(task, OnLoadError);
            SetOnProgress(task, OnLoadProgressChanged);
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

        void SetOnProgress(GltfImportTask task, Action<float, string> OnLoadProgressChanged = null)
        {
            task.OnProgress = (GltfImportStep step, int completed, int total) =>
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
            };
        }

        private void SetOnComplete(GltfImportTask task, Action<GameObject> OnLoadEnded = null)
        {
            task.OnCompleted = (GameObject model) =>
            {
                if (model.GetComponentInChildren<Animation>())
                {
                    model.AddComponent<AnimPlayer>();
                }
                StartCoroutine(AddCollider(model));
                OnLoadEnded?.Invoke(model);
            };
        }

        private void SetOnException(GltfImportTask task, Action<string> OnLoadError)
        {
            task.OnException = (error) =>
            {
                OnLoadError?.Invoke("Error loading model");
            };
        }

        //===========================================Trilib==========================================

        private void LoadModelUsingTrilib(byte[] data, string extension, bool isZipFile, Action<GameObject> OnLoadEnded = null, Action<string> OnLoadError = null, Action<float, string> OnLoadProgressChanged = null)
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            using MemoryStream memoryStream = new MemoryStream(data);

            if (isZipFile)
            {
                AssetLoaderZip.LoadModelFromZipStream(memoryStream, null, GetOnMaterialsLoad(), GetOnProgress(), GetOnError(), null, assetLoaderOptions);
            }
            else
            {
                AssetLoader.LoadModelFromStream(memoryStream, "model." + extension, extension, null, GetOnMaterialsLoad(), GetOnProgress(), GetOnError(), null, assetLoaderOptions);
            }
        }

        private Action<AssetLoaderContext> GetOnMaterialsLoad(Action<GameObject> OnLoadEnded = null)
        {
            return (AssetLoaderContext obj) =>
            {
                StartCoroutine(AddCollider(obj.RootGameObject));
                OnLoadEnded?.Invoke(obj.RootGameObject);
            };
        }

        private Action<IContextualizedError> GetOnError(Action<string> OnLoadError = null)
        {
            return (obj) =>
            {
                OnLoadError?.Invoke("Error loading model ");
            };
        }

        private Action<AssetLoaderContext, float> GetOnProgress(Action<float, string> OnLoadProgressChanged = null)
        {
            return (arg1, arg2) =>
            {
                OnLoadProgressChanged?.Invoke(arg2, "Loading...");
            };
        }

        private IEnumerator AddCollider(GameObject model)
        {
            BoxCollider boxCollider = model.GetComponent<BoxCollider>();
            if (boxCollider)
            {
                Destroy(boxCollider);
            }

            TransformData transformData = TransformData.TransformToTransformData(model.transform);
            TransformData.ResetTransform(model.transform);
            Bounds bound = Utils.GetModelBounds(model);
            BoxCollider collider = model.AddComponent<BoxCollider>();
            collider.center = bound.center;
            collider.size = bound.size;
            TransformData.TransformDataToTransfrom(transformData, model.transform);
            yield return null;
            /*            MeshFilter[] meshFilters = modelLoader.GetModel().gameObject.GetComponentsInChildren<MeshFilter>();
                        foreach (MeshFilter meshFilter in meshFilters)
                        {
                            if (meshFilter.GetComponent<Collider>() == null)
                            {
                                meshFilter.gameObject.AddComponent<BoxCollider>();
                                //Debug.Log("added");
                                yield return null;
                            }
                        }

                        SkinnedMeshRenderer[] skinnedMeshRenderers = modelLoader.GetModel().gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                        {
                            if (skinnedMeshRenderer.GetComponent<MeshCollider>() == null)
                            {
                                BoxCollider collider = skinnedMeshRenderer.gameObject.AddComponent<BoxCollider>();
                                //Debug.Log("added");
                                yield return null;
                            }
                        }*/
        }
    }
}