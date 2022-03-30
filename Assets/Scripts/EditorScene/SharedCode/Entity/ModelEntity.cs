using UnityEngine;

namespace EAR.Entity
{
    public class ModelEntity : BaseEntity
    {
        private string assetId;

        public ModelData GetModelData()
        {
            ModelData modelData = new ModelData();
            modelData.id = GetId();
            modelData.assetId = assetId;
            modelData.name = GetEntityName();
            modelData.transform = TransformData.TransformToTransformData(transform);
            return modelData;
        }

        public static ModelEntity InstantNewEntity(GameObject model, ModelData modelData)
        {
            ModelEntity modelEntity = Instantiate(model).AddComponent<ModelEntity>();
            modelEntity.SetId(modelData.id);
            modelEntity.assetId = modelData.assetId;
            return modelEntity;
        }

        public string GetAssetId()
        {
            return assetId;
        }
    }
}

