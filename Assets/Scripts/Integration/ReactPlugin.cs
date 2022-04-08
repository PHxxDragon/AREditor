using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public event Action<AssetInformation> LoadModuleCalledEvent;
        public event Action<string> LanguageChangeEvent;
        public event Action<GlobalStates.Mode> OnSetMode;
        public event Action<bool> OnSetEnableScreenshot;

        [DllImport("__Internal")]
        private static extern void SceneLoaded();

        [DllImport("__Internal")]
        private static extern void SaveMetadata(string metadata);

        [DllImport("__Internal")]
        private static extern void SaveScreenshot(byte[] array, int size);

        void Start()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        SceneLoaded();
#endif
#if UNITY_EDITOR == true
            SetLanguage("vi");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646739907080_enc_old_retro_television.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646743962550_old_retro_television.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646795054315_enc_aa.zip\",\"imageUrl\":\"\",\"metadataString\":\"\",\"extension\":\"obj\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            //LoadModule("{\"modelUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646803972049_blender_chan.zip\",\"imageUrl\":\"https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/3/1646979043558_Gawr_Gura (1).png\",\"metadataString\":\"\",\"extension\":\"gltf\",\"isZipFile\":true,\"enableEditor\":true,\"enableScreenshot\":true}");
            AssetInformation assetInformation = new AssetInformation();
            /*            AssetObject assetObject = new AssetObject();
                        assetObject.assetId = "alksdjfals;df";
                        assetObject.extension = "gltf";
                        assetObject.isZipFile = true;
                        assetObject.name = "Model 1";
                        assetObject.type = AssetObject.MODEL_TYPE;
                        assetObject.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/models/1/1646803972049_blender_chan.zip";
                        assetInformation.assets.Add(assetObject);*/
            AssetObject assetObject2 = new AssetObject();
            assetObject2.assetId = "akjsdflasvkcvhxvuiy";
            assetObject2.extension = "gltf";
            assetObject2.isZipFile = true;
            assetObject2.name = "Model wolf wolf";
            assetObject2.type = AssetObject.MODEL_TYPE;
            assetObject2.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/models/3/1648694213397_enc_wolf.zip";
            assetInformation.assets.Add(assetObject2);
/*            AssetObject assetObject3 = new AssetObject();
            assetObject3.assetId = "lkjfioewuffewffdsf";
            assetObject3.name = "image 1112";
            assetObject3.type = AssetObject.IMAGE_TYPE;
            assetObject3.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/module/ar/1/1649394059699_1646980495109_Gawr_Gura.bmp";
            assetInformation.assets.Add(assetObject3);
            AssetObject assetObject4 = new AssetObject();
            assetObject4.assetId = "lakjdfadfdsafaf";
            assetObject4.name = "Sound 1111";
            assetObject4.type = AssetObject.SOUND_TYPE;
            assetObject4.url = "http://localhost:4000/sound.wav";
            assetObject4.extension = "wav";
            assetInformation.assets.Add(assetObject4);
            assetInformation.metadataString = LocalStorage.Load("abcd");*/
            LoadModule(JsonUtility.ToJson(assetInformation));
            SetMode(2);
#endif
        }

        public void Save(string metadata)
        {
            Debug.Log(metadata);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SaveMetadata(metadata);
#endif
#if UNITY_EDITOR == true
            LocalStorage.Save("abcd", metadata);
#endif
        }

        public void LoadModule(string paramJson)
        {
            Debug.Log("Load module called: " + paramJson);
            AssetInformation param = JsonUtility.FromJson<AssetInformation>(paramJson);
            LoadModuleCalledEvent?.Invoke(param);
        }

        public void SetMode(int mode)
        {
            Debug.Log("mode: " + mode);
            OnSetMode?.Invoke((GlobalStates.Mode) mode);
        }

        public void SetEnableScreenshot(int enable)
        {
            Debug.Log("enable screenshot: " + enable);
            OnSetEnableScreenshot?.Invoke(enable != 0);
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
