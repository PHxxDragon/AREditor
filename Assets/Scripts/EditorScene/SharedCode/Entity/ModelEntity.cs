using UnityEngine;
using System;

namespace EAR.Entity
{
    public class ModelEntity : BaseEntity
    {
        public static event Action<string, ModelEntity> OnModelEntityCreated;
        public static event Action<string, ModelEntity> OnModelEntityChanged;

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

        public void SetModel(string assetId, GameObject model)
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
            GameObject newChild = Instantiate(model);
            newChild.transform.parent = transform;
            if (prev != null)
                TransformData.TransformDataToTransfrom(prev, newChild.transform);
            OnModelEntityChanged?.Invoke(assetId, this);
        }

        public static ModelEntity InstantNewEntity(GameObject model, ModelData modelData)
        {
            ModelEntity modelEntity = new GameObject().AddComponent<ModelEntity>();
            GameObject child = Instantiate(model);
            child.transform.parent = modelEntity.transform;
            modelEntity.SetId(modelData.id);
            modelEntity.assetId = modelData.assetId;
            OnModelEntityCreated?.Invoke(modelData.assetId, modelEntity);
            return modelEntity;
        }

        public string GetAssetId()
        {
            return assetId;
        }
    }
}

