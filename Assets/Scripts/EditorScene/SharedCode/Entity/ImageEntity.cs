using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.AssetManager;

namespace EAR.Entity
{
    public class ImageEntity : BaseEntity
    {
        public static event Action<ImageEntity> OnImageEntityCreated;
        //public static event Action<ImageEntity> OnImageEntityChanged;

        [SerializeField]
        private Image image;

        private string assetId;

        public override bool IsValidEntity()
        {
            return assetId != "";
        }

        public ImageData GetImageData()
        {
            ImageData imageData = new ImageData();
            imageData.assetId = assetId;
            imageData.id = GetId();
            imageData.name = GetEntityName();
            imageData.transform = TransformData.TransformToTransformData(transform);
            return imageData;
        }

        public void SetImage(string assetId)
        {
            Texture2D image = AssetContainer.Instance.GetImage(assetId);
            this.image.sprite = Utils.Instance.Texture2DToSprite(image);
            this.assetId = assetId;
        }

        public static ImageEntity InstantNewEntity(ImageData imageData)
        {
            ImageEntity imagePrefab = AssetContainer.Instance.GetImagePrefab();
            Texture2D image = AssetContainer.Instance.GetImage(imageData.assetId);
            ImageEntity imageEntity = Instantiate(imagePrefab);
            imageEntity.assetId = imageData.assetId;
            imageEntity.SetId(imageData.id);
            imageEntity.SetEntityName(imageData.name);
            TransformData.TransformDataToTransfrom(imageData.transform, imageEntity.transform);
            imageEntity.image.sprite = Utils.Instance.Texture2DToSprite(image);
            OnImageEntityCreated?.Invoke(imageEntity);
            return imageEntity;
        }


        public string GetAssetId()
        {
            return assetId;
        }
    }
}

