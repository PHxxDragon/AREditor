using UnityEngine;
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
