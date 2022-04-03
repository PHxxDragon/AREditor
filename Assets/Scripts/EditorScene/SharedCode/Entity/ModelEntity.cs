using UnityEngine;
using EAR.AssetManager;

namespace EAR.Entity
{
    public class ModelEntity : BaseEntity
    {
        private string assetId;

        public override bool IsValidEntity()
        {
            return !string.IsNullOrEmpty(assetId);
        }

        public override bool IsClickable()
        {
            return !string.IsNullOrEmpty(assetId);
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

            GameObject model = string.IsNullOrEmpty(assetId) ? AssetContainer.Instance.GetModelPrefab() : AssetContainer.Instance.GetModel(assetId);
            GameObject newChild = Instantiate(model);
            newChild.transform.parent = transform;
            if (prev != null)
            {
                TransformData.TransformDataToTransfrom(prev, newChild.transform);
            }
            OnEntityChanged?.Invoke(this);
        }

        public static ModelEntity InstantNewEntity(ModelData modelData)
        {
            ModelEntity modelEntity = new GameObject().AddComponent<ModelEntity>();
            if (!string.IsNullOrEmpty(modelData.assetId))
            {
                GameObject model = AssetContainer.Instance.GetModel(modelData.assetId);
                GameObject child = Instantiate(model);
                modelEntity.assetId = modelData.assetId;
                child.transform.parent = modelEntity.transform;
            } else
            {
                GameObject model = AssetContainer.Instance.GetModelPrefab();
                GameObject child = Instantiate(model);
                child.transform.parent = modelEntity.transform;
            }

            if (!string.IsNullOrEmpty(modelData.name))
            {
                modelEntity.SetEntityName(modelData.name);
            } else
            {
                modelEntity.SetEntityName("New model");
            }
            
            if (modelData.transform != null)
            {
                TransformData.TransformDataToTransfrom(modelData.transform, modelEntity.transform);
            }
            
            if (!string.IsNullOrEmpty(modelData.id))
            {
                modelEntity.SetId(modelData.id);
            }
            
            OnEntityCreated?.Invoke(modelEntity);
            return modelEntity;
        }

        public string GetAssetId()
        {
            return assetId;
        }
    }
}

