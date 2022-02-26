using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public event Action<ModuleARInformation> LoadModuleCalledEvent;
        public event Action<ModelParam> LoadModelCalledEvent;


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
            LoadModule("{\"modelUrl\":\"http://localhost:4000/wolf_with_animations.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"obj\",\"enableEditor\":true}");
#endif
        }
        //https://ear-storage.s3.ap-southeast-1.amazonaws.com/models/1/1645813567940_Powerful Jarv-Bruticus.zip
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

        public void LoadModel(string paramJson)
        {
            Debug.Log("Load model called: " + paramJson);
            ModelParam param = JsonUtility.FromJson<ModelParam>(paramJson);
            LoadModelCalledEvent?.Invoke(param);
        }
    }
}
