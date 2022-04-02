using UnityEngine;
using System;
using EAR.AssetManager;

namespace EAR.Entity
{
    public class ModelEntity : BaseEntity
    {
        private string assetId;

        public override bool IsValidEntity()
        {
            return assetId != "";
        }

        public ModelData GetModelData()
        {
            ModelData modelData = new ModelData();
            modelData.id = GetId();
            modelData.assetId = assetId;
            modelData.name = GetEntityName();
            modelData.transform = TransformData.TransformToTransformData(transform);
            return modelData;
        }

        public void SetModel(string assetId)
        {
            if (this.assetId == assetId)
            {
                return;
            }

            this.assetId = assetId;
            TransformData prev = null;
            foreach (Transform child in transform)
            {
                prev = TransformData.TransformToTransformData(child);
                Destroy(child.gameObject);
            }
            GameObject newChild = Instantiate(AssetContainer.Instance.GetModel(assetId));
            newChild.transform.parent = transform;
            if (prev != null)
                TransformData.TransformDataToTransfrom(prev, newChild.transform);
            OnEntityChanged?.Invoke(this);
        }

        public static ModelEntity InstantNewEntity(ModelData modelData)
        {
            ModelEntity modelEntity = new GameObject().AddComponent<ModelEntity>();
            GameObject model = AssetContainer.Instance.GetModel(modelData.assetId);
            GameObject child = Instantiate(model);
            child.transform.parent = modelEntity.transform;
            modelEntity.SetId(modelData.id);
            modelEntity.assetId = modelData.assetId;
            OnEntityCreated?.Invoke(modelEntity);
            return modelEntity;
        }

        public string GetAssetId()
        {
            return assetId;
        }
    }
}

