using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public TransformData modelTransform;
/*        public float imageWidthInMeters;*/
        public List<NoteData> noteDatas = new List<NoteData>();
        public Color ambientColor = Color.white;
        public List<LightData> lightDatas = new List<LightData>();
    }
}

