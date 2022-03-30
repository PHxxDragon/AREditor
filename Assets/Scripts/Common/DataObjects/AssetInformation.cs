using System;
using System.Collections.Generic;

namespace EAR
{
    [Serializable]
    public class AssetInformation
    {
        public string metadataString;
        public string imageUrl;
        public List<AssetObject> assets = new List<AssetObject>();
    }

    [Serializable]
    public class AssetObject
    {
        public const string MODEL_TYPE = "model";
        public const string IMAGE_TYPE = "image";
        public const string FONT_TYPE = "font";
        public const string VIDEO_TYPE = "video";
        public const string SOUND_TYPE = "sound";

        public string assetsId;
        public string url;
        public string type;

        //type == "model"
        public string extension;
        public bool isZipFile;
    }
}

