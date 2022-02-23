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
            LoadModule("{\"modelUrl\":\"https://firebasestorage.googleapis.com/v0/b/education-ar-c395d.appspot.com/o/models%2F1%2Fmodels_1_Bodacious%20Maimu-Duup%20(2).zip?alt=media&token=6d0d07bd-75ba-4db8-b344-ed96be01ba63\",\"extension\":\"zip\", \"imageUrl\": \"dfsfd\", \"metadataString\":\"\"}");
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

        public void LoadModel(string paramJson)
        {
            Debug.Log("Load model called: " + paramJson);
            ModelParam param = JsonUtility.FromJson<ModelParam>(paramJson);
            LoadModelCalledEvent?.Invoke(param);
        }
    }
}
