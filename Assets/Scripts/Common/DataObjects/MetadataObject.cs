using System;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public TransformData modelTransform;
        public float imageWidthInMeters;

        public static TransformData TransformToTransformData(Transform transform)
        {
            TransformData transformData = new TransformData();
            transformData.position = transform.position;
            transformData.rotation = transform.rotation;
            transformData.scale = transform.localScale;
            return transformData;
        }

        public static void TransformDataToTransfrom(TransformData transformData, Transform transform)
        {
            transform.position = transformData.position;
            transform.rotation = transformData.rotation;
            transform.localScale = transformData.scale;
        }
    }

    [Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
}

