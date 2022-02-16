using UnityEngine;
using EAR.Integration;
using EAR.WebRequest;
using EAR.AR;
using EAR.EARCamera;
using EAR.View;
using EAR.Selection;
using RuntimeHandle;
using EAR.UndoRedo;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private WebRequestHelper webRequestHelper;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private ImageHolder imageHolder;
        [SerializeField]
        private float scaleToSize = 1f;
        [SerializeField]
        private float distanceToPlane = 0.5f;
        [SerializeField]
        private CameraController cameraController;

        // for disable when not used
        [SerializeField]
        private ToolBar toolBar;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private UndoRedoManager undoRedoManager;


        private MetadataObject metadata;

        void Awake()
        {
            Debug.Log("start in presenter");
            if (reactPlugin != null)
            {
                reactPlugin.LoadModuleCalledEvent += LoadModuleCalledEventSubscriber;
                reactPlugin.LoadModelCalledEvent += LoadModelCalledEventSubscriber;
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

        private void LoadModelCalledEventSubscriber(ModelParam obj)
        {
            Debug.Log("Event called!" + obj.url + " extension: " + obj.extension);
            modelLoader.LoadModel(obj.url, obj.extension);
            modelLoader.OnLoadEnded += InitMetadataForModel;
            DisableUnusedComponents();
        }

        private void DisableUnusedComponents()
        {
            toolBar.gameObject.SetActive(false);
            selectionManager.gameObject.SetActive(false);
            runtimeTransformHandle.gameObject.SetActive(false);
            undoRedoManager.gameObject.SetActive(false);
        }

#if UNITY_EDITOR == true
/*        void Start()
        {
            ModuleParam param = new ModuleParam();
            param.moduleId = 1;
            param.token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwiaWF0IjoxNjQ0NTc5MTE3LCJleHAiOjE2NTIzNTUxMTd9.z8zYBiJ684KiF0xv4CHLJQhQBWxu0ZBEMzGfqJq99X0";
            LoadModuleCalledEventSubscriber(param);
        }*/
#endif

        private void LoadModuleCalledEventSubscriber(ModuleParam obj)
        {
            Debug.Log("called in presenter: " + obj.token + " Module id: " + obj.moduleId);
            LocalStorage.Save("param", obj);
            webRequestHelper.GetModuleInformation(obj.token, obj.moduleId, LoadModuleCallback, LoadModuleErrorCallback);
        }

        private void LoadModuleCallback(ModuleARInformation moduleAR)
        {
            modelLoader.LoadModel(moduleAR.modelUrl, moduleAR.extension);
            imageHolder.LoadImage(moduleAR.imageUrl);
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(moduleAR.metadataString);
            if (metadataObject == null)
            {
                InitMetadata();
            }
            else
            {
                ApplyMetadata(metadataObject);
            }            
        }

        private void ApplyMetadata(MetadataObject metadataObject)
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

        private void LoadModuleErrorCallback(string error)
        {
            Debug.LogError(error);
        }

        private void SetDataForModel()
        {
            TransformData.TransformDataToTransfrom(metadata.modelTransform, modelLoader.GetModel().transform);
            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(modelLoader.GetModel()));
        }

        private void InitMetadata()
        {
            modelLoader.OnLoadEnded += InitMetadataForModel;
        }

        private void InitMetadataForModel()
        {
            GameObject model = modelLoader.GetModel();
            Bounds bounds = Utils.GetModelBounds(modelLoader.GetModel());
            float ratio = scaleToSize / bounds.extents.magnitude;
            model.transform.position = -(bounds.center * ratio) + new Vector3(0, distanceToPlane + bounds.extents.y * ratio, 0);
            model.transform.localScale *= ratio;
            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(model));
        }
    }
}

