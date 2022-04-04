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
        private readonly Dictionary<string, (AssetObject, AudioClip)> sounds = new Dictionary<string, (AssetObject, AudioClip)>();

        [SerializeField]
        private GameObject disabledContainer;

        [SerializeField]
        private GameObject modelPrefab;

        [SerializeField]
        private NoteEntity notePrefab;

        [SerializeField]
        private Texture2D defaultTexture;

        [SerializeField]
        private ImageEntity imagePrefab;

        [SerializeField]
        private ButtonEntity buttonPrefab;

        [SerializeField]
        private SoundEntity soundPrefab;

        public void AddModel(AssetObject assetObject, GameObject model)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            models.Add(assetObject.assetId, (assetObject, model));
            model.transform.parent = disabledContainer.transform;
        }

        public GameObject GetModel(string assetId)
        {
            return models[assetId].Item2;
        }

        public void AddImage(AssetObject assetObject, Texture2D image)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            images.Add(assetObject.assetId, (assetObject, image));
        }

        public Texture2D GetImage(string assetId)
        {
            return images[assetId].Item2;
        }

        public void AddSound(AssetObject assetObject, AudioClip sound)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            sounds.Add(assetObject.assetId, (assetObject, sound));
        }

        public AudioClip GetSound(string assetId)
        {
            return sounds[assetId].Item2;
        }

        public GameObject GetModelPrefab()
        {
            return modelPrefab;
        }

        public NoteEntity GetNotePrefab()
        {
            return notePrefab;
        }

        public ImageEntity GetImagePrefab()
        {
            return imagePrefab;
        }

        public Texture2D GetDefaultImage()
        {
            return defaultTexture;
        }

        public ButtonEntity GetButtonPrefab()
        {
            return buttonPrefab;
        }

        public SoundEntity GetSoundPrefab()
        {
            return soundPrefab;
        }
    }
}

