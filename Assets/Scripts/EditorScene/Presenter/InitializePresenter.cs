using UnityEngine;
using EAR.Integration;
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
        private ModelLoader modelLoader;
        [SerializeField]
        private ImageHolder imageHolder;
        [SerializeField]
        private float scaleToSize = 0.5f;
        [SerializeField]
        private float distanceToPlane = 0f;
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

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private GameObject canvas;


        private MetadataObject metadata;

        void Awake()
        {
            Debug.Log("start in presenter");
            if (reactPlugin != null)
            {
                reactPlugin.LoadModuleCalledEvent += LoadModuleCallback;
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

        private void DisableUnusedComponents()
        {
            toolBar.gameObject.SetActive(false);
            selectionManager.gameObject.SetActive(false);
            runtimeTransformHandle.gameObject.SetActive(false);
            undoRedoManager.gameObject.SetActive(false);
        }

        private void LoadModuleCallback(ModuleARInformation moduleAR)
        {
            if (!moduleAR.enableEditor)
            {
                DisableUnusedComponents();
            }
            modelLoader.LoadModel(moduleAR.modelUrl, moduleAR.extension, moduleAR.isZipFile);
            modelLoader.OnLoadError += OnLoadError;
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

        private void OnLoadError(string error)
        {
            Modal modal = Instantiate(modalPrefab, canvas.transform);
            modal.SetModalContent("Error", error);
            modal.DisableCancelButton();
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

