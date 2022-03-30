using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public List<ModelData> modelDatas = new List<ModelData>();
        public List<NoteData> noteDatas = new List<NoteData>();
        public List<LightData> lightDatas = new List<LightData>();
        public Color ambientColor = Color.white;
    }

    [Serializable]
    public class ImageData
    {
        public string id;
        public string name;

        public TransformData transform;
        public string assetId;
    }

    [Serializable]
    public class ModelData
    {
        public string id;
        public string name;

        public TransformData transform;
        public string assetId;
    }

    [Serializable]
    public class NoteData
    {
        public string id;
        public string name;

        public TransformData noteTransformData;
        public RectTransformData noteContentRectTransformData;

        public string noteContent;
        public Color textBackgroundColor;

        public Vector4 borderWidth;
        public Vector4 textBorderRadius;
        public Color borderColor;

        public int fontSize;
        public Color textColor;

        public float boxWidth;
    }

    [Serializable]
    public class LightData
    {
        public string id;
        public string name;

        public LightType lightType = LightType.Directional;
        public Color color = Color.black;
        public float intensity = 1f;
        public Vector3 direction = new Vector3(0, -1, 0);

        public LightData()
        {
        }

        public LightData(LightType lightType, Color color, float intensity, Vector3 direction)
        {
            this.lightType = lightType;
            this.color = color;
            this.intensity = intensity;
            this.direction = direction;
        }
    }
}

