using UnityEngine;
using System.Collections.Generic;

namespace EAR.AssetManager
{
    public class AssetContainer : MonoBehaviour
    {
        private readonly Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

        [SerializeField]
        private GameObject disabledContainer;

        public void AddModel(string assetId, GameObject model)
        {
            models.Add(assetId, model);
            model.transform.parent = disabledContainer.transform;
        }

        public GameObject GetModel(string assetId)
        {
            return models[assetId];
        }

        public void AddImage(string assetId, Texture2D image)
        {
            images.Add(assetId, image);
        }

        public Texture2D GetImage(string assetId)
        {
            return images[assetId];
        }
    }
}

