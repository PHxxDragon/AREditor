using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public event Action<ModuleARInformation> LoadModuleCalledEvent;
        public event Action<string> LanguageChangeEvent;

        [DllImport("__Internal")]
        private static extern void SceneLoaded();

        [DllImport("__Internal")]
        private static extern void SaveMetadata(string metadata);

        [DllImport("__Internal")]
        private static extern void SaveScreenshot(byte[] array, int size);

        void Start()
        {
            Debug.Log("start in react plugin");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        SceneLoaded();
#endif
#if UNITY_EDITOR == true
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646739907080_enc_old_retro_television.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646743962550_old_retro_television.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646795054315_enc_aa.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"obj\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646803972049_blender_chan.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
#endif
        }

        public void Save(string metadata)
        {
            Debug.Log(metadata);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SaveMetadata(metadata);
#endif
        }

        public void LoadModule(string paramJson)
        {
            Debug.Log("Load module called: " + paramJson);
            ModuleARInformation param = JsonUtility.FromJson<ModuleARInformation>(paramJson);
            LoadModuleCalledEvent?.Invoke(param);
        }

        public void SetLanguage(string language)
        {
            LanguageChangeEvent?.Invoke(language);
        }

        public void SaveScreenshotToJs(byte[] image)
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SaveScreenshot(image, image.Length);
#endif
        }
    }
}
