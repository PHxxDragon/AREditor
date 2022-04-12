using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.Container;

namespace EAR.Entity
{
    public class ImageEntity : VisibleEntity
    {
        private static int count = 1;
        [SerializeField]
        private Image image;

        private string assetId = "";

        protected override string GetDefaultName()
        {
            return "New image " + count++;
        }

        public ImageData GetImageData()
        {
            ImageData imageData = new ImageData();
            imageData.assetId = assetId;
            imageData.id = GetId();
            imageData.name = GetEntityName();
            imageData.transform = TransformData.TransformToTransformData(transform);
            imageData.isVisible = isVisible;
            return imageData;
        }

        public void SetImage(string assetId)
        {
            if (this.assetId == assetId || assetId == null)
            {
                return;
            }

            this.assetId = assetId;

            Texture2D image = AssetContainer.Instance.GetImage(assetId); 
            if (!image)
            {
                image = AssetContainer.Instance.GetDefaultImage();
            }
            this.image.sprite = Utils.Instance.Texture2DToSprite(image);
        }

        public void PopulateData(ImageData imageData)
        {
            if (imageData.isVisible.HasValue)
            {
                isVisible = imageData.isVisible.Value;
            }

            if (!string.IsNullOrEmpty(imageData.id))
            {
                SetId(imageData.id);
            }

            if (imageData.assetId != null)
            {
                SetImage(imageData.assetId);
            }

            if (!string.IsNullOrEmpty(imageData.name))
            {
                SetEntityName(imageData.name);
            }

            if (imageData.transform != null)
            {
                TransformData.TransformDataToTransfrom(imageData.transform, transform);
            }
        }

        public static ImageEntity InstantNewEntity(ImageData imageData)
        {
            ImageEntity imagePrefab = AssetContainer.Instance.GetImagePrefab();
            ImageEntity imageEntity = Instantiate(imagePrefab);
            imageEntity.PopulateData(imageData);
            OnEntityCreated?.Invoke(imageEntity);
            return imageEntity;
        }


        public string GetAssetId()
        {
            return assetId;
        }
    }
}

