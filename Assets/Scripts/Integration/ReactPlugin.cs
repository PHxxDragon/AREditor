using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.IO;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public const string SAVE_SUCCESS = "success";
        public const string SAVE_FAILED = "failed";

        public event Action<AssetInformation> LoadModuleCalledEvent;
        public event Action<string> LanguageChangeEvent;
        public event Action<GlobalStates.Mode> OnSetMode;
        public event Action<bool> OnSetEnableScreenshot;

/*        [SerializeField]
        private TMP_Text saveStatus;*/

        [DllImport("__Internal")]
        private static extern void SetFullScreen(int isOn);

        [DllImport("__Internal")]
        private static extern void SceneLoaded();

        [DllImport("__Internal")]
        private static extern void SaveMetadata(string metadata);

        [DllImport("__Internal")]
        private static extern void SaveScreenshot(byte[] array, int size);

        [DllImport("__Internal")]
        private static extern void SetUnsaved(int isUnsaved);

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
            AssetObject assetObject = new AssetObject();
            assetObject.assetId = "alksdjfalsdf";
            assetObject.extension = "mp4";
            assetObject.isZipFile = false;
            assetObject.name = "Video 1";
            assetObject.type = AssetObject.VIDEO_TYPE;
            assetObject.url = "https://www.w3schools.com/html/mov_bbb.mp4";
            assetObject.predownload = false;
            assetInformation.assets.Add(assetObject);
            AssetObject assetObject10 = new AssetObject();
            assetObject10.assetId = "akjsdflasvkcvhxvuiy8888888";
            assetObject10.extension = "gltf";
            assetObject10.isZipFile = true;
            assetObject10.name = "Model pyramid";
            assetObject10.type = AssetObject.MODEL_TYPE;
            assetObject10.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/models/4/1652290530546_enc_sierpinski.zip";
            assetInformation.assets.Add(assetObject10);
            AssetObject assetObject2 = new AssetObject();
            assetObject2.assetId = "akjsdflasvkcvhxvuiy";
            assetObject2.extension = "gltf";
            assetObject2.isZipFile = true;
            assetObject2.name = "Model wolf wolf";
            assetObject2.type = AssetObject.MODEL_TYPE;
            assetObject2.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/models/4/1652290319091_enc_bullet_physics_demolition_animation_1.zip";
            assetInformation.assets.Add(assetObject2);
            AssetObject assetObject3 = new AssetObject();
            assetObject3.assetId = "lkjfioewuffewffdsf";
            assetObject3.name = "image 1112";
            assetObject3.type = AssetObject.IMAGE_TYPE;
            assetObject3.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/test/users/1/files/1650808671357_Pricing-Background.png";
            assetInformation.assets.Add(assetObject3);
            AssetObject assetObject4 = new AssetObject();
            assetObject4.assetId = "lakjdfadfdsafaf";
            assetObject4.name = "Sound 1111";
            assetObject4.type = AssetObject.SOUND_TYPE;
            assetObject4.url = "https://www.w3schools.com/html/horse.mp3";
            assetObject4.extension = "mp3";
            assetInformation.assets.Add(assetObject4);
            AssetObject assetObject5 = new AssetObject();
            assetObject5.assetId = "lakjdfadfdsafafvvvvv";
            assetObject5.name = "Font 1111";
            assetObject5.type = AssetObject.FONT_TYPE;
            assetObject5.url = "https://ear-storage.s3.ap-southeast-1.amazonaws.com/users/4/files/1652712275553_Roboto-Black.ttf";
            assetObject5.extension = "ttf";
            assetInformation.assets.Add(assetObject5);
            if (File.Exists(Path.Combine(Application.persistentDataPath, "metadata", "metadata.txt")))
            {
                //assetInformation.metadata = File.ReadAllText(Path.Combine(Application.persistentDataPath, "metadata", "metadata.txt"));
            }
            LoadModule(JsonUtility.ToJson(assetInformation));
            SetMode(0);
            SetEnableScreenshot(0);
#endif
        }

        void Awake()
        {

            GlobalStates.OnSavableChange += (isSavable) =>
            {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                SetUnsaved(isSavable != GlobalStates.SaveStatus.Saved ? 1 : 0);
#endif
            };
        }

        public void Save(string metadata)
        {
            GlobalStates.SetSavable(GlobalStates.SaveStatus.Saving);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SaveMetadata(metadata);
#endif
#if UNITY_EDITOR == true
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "metadata")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "metadata"));
            }
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "metadata", "metadata.txt"), metadata);
            SetSaveStatus(SAVE_SUCCESS);
#endif
        }

        public void SetFullScreen(bool isOn)
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SetFullScreen(isOn ? 1 : 0);
#endif
#if UNITY_EDITOR == true
            //Debug.Log(isOn);
#endif
        }

        public void LoadModule(string paramJson)
        {
            Debug.Log("Load module called: " + paramJson);
            AssetInformation param = JsonUtility.FromJson<AssetInformation>(paramJson);
            LoadModuleCalledEvent?.Invoke(param);
        }

        public void SetSaveStatus(string status)
        {
            if (status == SAVE_SUCCESS)
            {
                GlobalStates.SetSavable(GlobalStates.SaveStatus.Saved);
            } else
            {
                GlobalStates.SetSavable(GlobalStates.SaveStatus.Savable);
            }
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
