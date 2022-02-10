using UnityEngine;
using EAR.Integration;
using EAR.WebRequest;
using EAR.AR;

namespace EAR.Editor.Presenter
{
    public class ReactPluginPresenter : MonoBehaviour
    {
        [SerializeField]
        ReactPlugin reactPlugin;
        [SerializeField]
        WebRequestHelper webRequestHelper;
        [SerializeField]
        ModelLoader modelLoader;
        [SerializeField]
        ImageHolder imageHolder;

        private MetadataObject metadata;

        void Awake()
        {
            Debug.Log("start in presenter");
            if (reactPlugin != null)
            {
                reactPlugin.LoadModelCalledEvent += LoadModelCalledEventSubscriber;
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

#if UNITY_EDITOR == true
        void Start()
        {
            Param param = new Param();
            param.moduleId = 1;
            param.token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwiaWF0IjoxNjQzMTg0NzYzLCJleHAiOjE2NTA5NjA3NjN9.ug48VT5DFJRoIiqc06y57qSzOLsOfJYnY5Mmp--UiOs";
            LoadModelCalledEventSubscriber(param);
        }
#endif

        private void LoadModelCalledEventSubscriber(Param obj)
        {
            Debug.Log("called in presenter: " + obj.token + " Module id: " + obj.moduleId);
            LocalStorage.Save("param", obj);
            webRequestHelper.GetModuleInformation(obj.token, obj.moduleId, LoadModuleCallback, LoadModuleErrorCallback);
        }

        private void LoadModuleCallback(ModuleARInformation moduleAR)
        {
            modelLoader.LoadModel(moduleAR.modelUrl);
            imageHolder.LoadImage(moduleAR.imageUrl);
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(moduleAR.metadataString);
            if (metadataObject == null)
            {
                modelLoader.OnLoadEnded += InitDataForModel;
            } else
            {
                imageHolder.widthInMeter = metadataObject.imageWidthInMeters;
                if (modelLoader.GetModel() != null)
                {
                    TransformData.TransformDataToTransfrom(metadataObject.modelTransform, modelLoader.GetModel().transform);
                }
                else
                {
                    modelLoader.OnLoadEnded += SetDataForModel;
                    metadata = metadataObject;
                }
            }
            

        }

        private void LoadModuleErrorCallback(string error)
        {
            Debug.LogError(error);
        }

        private void SetDataForModel()
        {
            TransformData.TransformDataToTransfrom(metadata.modelTransform, modelLoader.GetModel().transform);
        }

        private void InitDataForModel()
        {
            Vector3 position = modelLoader.GetModel().transform.position;
            position.y += 2;
            modelLoader.GetModel().transform.position = position;
        }
    }
}

