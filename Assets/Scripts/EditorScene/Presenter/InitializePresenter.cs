using UnityEngine;
using EAR.Integration;
using EAR.AR;
using EAR.EARCamera;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ModelLoader modelLoader;
/*        [SerializeField]
        private ImageHolder imageHolder;*/
        [SerializeField]
        private float scaleToSize = 0.5f;
        [SerializeField]
        private float distanceToPlane = 0f;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private EnvironmentEditorWindow environmentEditorWindow;

        [SerializeField]
        private GameObject noteContainer;
        [SerializeField]
        private Note notePrefab;

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
                reactPlugin.OnSetEnableEditor += (enable) =>
                {
                    GlobalStates.SetEnableEditor(enable);
                };
                reactPlugin.OnSetEnableScreenshot += (enable) =>
                {
                    GlobalStates.SetEnableScreenshot(enable);
                };
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

        private void LoadModuleCallback(ModuleARInformation moduleAR)
        {
            GlobalStates.SetEnableEditor(moduleAR.enableEditor);
            GlobalStates.SetEnableScreenshot(moduleAR.enableScreenshot);
            modelLoader.LoadModel(moduleAR.modelUrl, moduleAR.extension, moduleAR.isZipFile);
            modelLoader.OnLoadError += OnLoadError;
            //imageHolder.LoadImage(moduleAR.imageUrl);
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
/*            if (metadataObject.imageWidthInMeters != 0)
            {
                imageHolder.widthInMeter = metadataObject.imageWidthInMeters;
            }*/
            if (modelLoader.GetModel() != null)
            {
                TransformData.TransformDataToTransfrom(metadataObject.modelTransform, modelLoader.GetModel().transform);
            }
            else
            {
                modelLoader.OnLoadEnded += SetDataForModel;
                metadata = metadataObject;
            }
            
            if (metadataObject.noteDatas != null)
            {
                foreach(NoteData noteData in metadataObject.noteDatas)
                {
                    Note note = Instantiate(notePrefab, noteContainer.transform);
                    note.PopulateData(noteData);
                }
            }
            environmentEditorWindow.SetAmbientColor(metadata.ambientColor);
        }

        private void SetDataForModel()
        {
            TransformData.TransformDataToTransfrom(metadata.modelTransform, modelLoader.GetModel().transform);
            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(modelLoader.GetModel()));
        }

        private void InitMetadata()
        {
            modelLoader.OnLoadEnded += InitMetadataForModel;
            environmentEditorWindow.SetAmbientColor(Color.white);
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

