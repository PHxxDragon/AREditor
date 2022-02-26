using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public event Action<ModuleARInformation> LoadModuleCalledEvent;

        [DllImport("__Internal")]
        private static extern void SceneLoaded();

        [DllImport("__Internal")]
        private static extern void SaveMetadata(string metadata);

        void Start()
        {
            Debug.Log("start in react plugin");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        SceneLoaded();
#endif
#if UNITY_EDITOR == true
            LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/models/2/1645864666845_wild_west_saloon.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true}");
#endif
        }

        public void Save(string metadata)
        {
            Debug.Log(metadata);
            SaveMetadata(metadata);
        }

        public void LoadModule(string paramJson)
        {
            Debug.Log("Load module called: " + paramJson);
            ModuleARInformation param = JsonUtility.FromJson<ModuleARInformation>(paramJson);
            LoadModuleCalledEvent?.Invoke(param);
        }
    }
}
