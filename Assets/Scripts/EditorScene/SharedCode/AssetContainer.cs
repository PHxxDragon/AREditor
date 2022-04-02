using UnityEngine;
using System.Collections.Generic;
using System;
using EAR.Entity;

namespace EAR.AssetManager
{
    public class AssetContainer : MonoBehaviour
    {
        private static AssetContainer instance;

        public static AssetContainer Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("Two instance of asset container found");
            }
        }

        public Action<AssetObject> OnAssetObjectAdded;

        private readonly Dictionary<string, (AssetObject, GameObject)> models = new Dictionary<string, (AssetObject, GameObject)>();
        private readonly Dictionary<string, (AssetObject, Texture2D)> images = new Dictionary<string, (AssetObject, Texture2D)>();

        [SerializeField]
        private GameObject disabledContainer;

        [SerializeField]
        private NoteEntity notePrefab;

        [SerializeField]
        private ImageEntity imagePrefab;

        [SerializeField]
        private ButtonEntity buttonPrefab;

        public List<AssetObject> GetModelAssets()
        {
            List<AssetObject> assetObjects = new List<AssetObject>();
            foreach (var tuple in models.Values)
            {
                assetObjects.Add(tuple.Item1);
            }
            return assetObjects;
        }

        public List<AssetObject> GetImageAssets()
        {
            List<AssetObject> assetObjects = new List<AssetObject>();
            foreach (var tuple in images.Values)
            {
                assetObjects.Add(tuple.Item1);
            }
            return assetObjects;
        }

        public void AddModel(AssetObject assetObject, GameObject model)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            models.Add(assetObject.assetsId, (assetObject, model));
            model.transform.parent = disabledContainer.transform;
        }

        public GameObject GetModel(string assetId)
        {
            return models[assetId].Item2;
        }

        public void AddImage(AssetObject assetObject, Texture2D image)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            images.Add(assetObject.assetsId, (assetObject, image));
        }

        public Texture2D GetImage(string assetId)
        {
            return images[assetId].Item2;
        }

        public NoteEntity GetNotePrefab()
        {
            return notePrefab;
        }

        public ImageEntity GetImagePrefab()
        {
            return imagePrefab;
        }

        public ButtonEntity GetButtonPrefab()
        {
            return buttonPrefab;
        }
    }
}

