/*using UnityEngine;
using TriLibCore;
using System;

namespace EAR.AR
{
    public class ModelLoader : MonoBehaviour
    {
        public event Action OnLoadStarted;
        public event Action OnLoadEnded;
        public event Action<float> OnLoadProgressChanged;

        [SerializeField]
        private GameObject modelContainer;

        private GameObject loadedModel;

        public GameObject GetModel()
        {
            return loadedModel;
        }

        public void LoadModel(string url)
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var webRequest = AssetDownloader.CreateWebRequest(url);
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, modelContainer, assetLoaderOptions);
            OnLoadStarted?.Invoke();
        }

        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        private void OnProgress(AssetLoaderContext arg1, float arg2)
        {
            OnLoadProgressChanged?.Invoke(arg2);
        }

        private void OnMaterialsLoad(AssetLoaderContext obj)
        {
            OnLoadEnded?.Invoke();
        }

        private void OnLoad(AssetLoaderContext obj)
        {
            AddMeshCollider(obj.RootGameObject);
            loadedModel = obj.RootGameObject;
        }

        private void AddMeshCollider(GameObject rootGameObject)
        {
            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.GetComponent<MeshCollider>() == null)
                {
                    meshFilter.gameObject.AddComponent<MeshCollider>();
                }
            }
            SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.GetComponent<MeshCollider>() == null)
                {
                    MeshCollider collider = skinnedMeshRenderer.gameObject.AddComponent<MeshCollider>();
                    collider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                }
            }
        }
    }
}
*/

using UnityEngine;
using System;
using Piglet;
namespace EAR.AR
{
    public class ModelLoader: MonoBehaviour
    {
        public event Action OnLoadStarted;
        public event Action OnLoadEnded;
        public event Action<float, string> OnLoadProgressChanged;

        [SerializeField]
        private GameObject modelContainer;

        private GameObject loadedModel;

        private GltfImportTask task;

        public GameObject GetModel()
        {
            return loadedModel;
        }

        public void LoadModel(string url)
        {
            task = RuntimeGltfImporter.GetImportTask(url);
            task.OnCompleted = OnComplete;
            task.OnException += OnException;
            task.OnProgress += OnProgress;
            OnLoadStarted?.Invoke();
        }

        void Update()
        {
            if (task != null)
                task.MoveNext();
        }

        void OnProgress(GltfImportStep step, int completed, int total)
        {
            OnLoadProgressChanged?.Invoke(((float) completed)/total, string.Format("{0}: {1}/{2}", step, completed, total));
        }

        private void OnComplete(GameObject importedModel)
        {
            loadedModel = importedModel;
            loadedModel.transform.parent = modelContainer.transform;
            OnLoadEnded?.Invoke();
        }

        private void OnException(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}