using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.AssetManager;

namespace EAR.Entity
{
    public class ImageEntity : VisibleEntity
    {
        private static int count = 1;
        [SerializeField]
        private Image image;

        private string assetId;

        public override string GetDefaultName()
        {
            return "New image " + count++;
        }

        public override bool IsValidEntity()
        {
            return !string.IsNullOrEmpty(assetId);
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
            Texture2D image = AssetContainer.Instance.GetImage(assetId); 
            if (!image)
            {
                AssetContainer.Instance.GetDefaultImage();
            }
            this.image.sprite = Utils.Instance.Texture2DToSprite(image);
            this.assetId = assetId;
            //OnEntityChanged?.Invoke(this);
        }

        public static ImageEntity InstantNewEntity(ImageData imageData)
        {
            ImageEntity imagePrefab = AssetContainer.Instance.GetImagePrefab();
            ImageEntity imageEntity = Instantiate(imagePrefab);

            if (!string.IsNullOrEmpty(imageData.id))
            {
                imageEntity.SetId(imageData.id);
            }

            if (imageData.assetId != null)
            {
                Texture2D image = AssetContainer.Instance.GetImage(imageData.assetId);
                imageEntity.image.sprite = Utils.Instance.Texture2DToSprite(image);
                imageEntity.assetId = imageData.assetId;
            }

            if (!string.IsNullOrEmpty(imageData.name))
            {
                imageEntity.SetEntityName(imageData.name);
            }

            if (imageData.transform != null)
            {
                TransformData.TransformDataToTransfrom(imageData.transform, imageEntity.transform);
            }

            OnEntityCreated?.Invoke(imageEntity);
            return imageEntity;
        }


        public string GetAssetId()
        {
            return assetId;
        }
    }
}

