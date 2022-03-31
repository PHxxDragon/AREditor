using UnityEngine;
using EAR.Integration;
using EAR.AR;
using EAR.EARCamera;
using EAR.View;
using EAR.Entity;
using EAR.AssetManager;
using System.Collections;
using System.Collections.Generic;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private float scaleToSize = 0.5f;
        [SerializeField]
        private float distanceToPlane = 0f;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private EnvironmentEditorWindow environmentEditorWindow;
        [SerializeField]
        private AssetContainer assetContainer;

        [SerializeField]
        private GameObject container;
        [SerializeField]
        private NoteEntity notePrefab;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private GameObject canvas;

        private MetadataObject metadata;
        private int assetCount;
        private Dictionary<string, AssetObject> assetObjectDict = new Dictionary<string, AssetObject>();

        void Awake()
        {
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

        private void LoadModuleCallback(AssetInformation assetInformation)
        {
            assetCount = assetInformation.assets.Count;
            foreach (AssetObject assetObject in assetInformation.assets)
            {
                switch(assetObject.type)
                {
                    case AssetObject.MODEL_TYPE:
                        modelLoader.LoadModel(assetObject.assetsId, assetObject.url, assetObject.extension, assetObject.isZipFile);
                        break;
                    default:
                        assetCount -= 1;
                        break;
                }
                assetObjectDict.Add(assetObject.assetsId, assetObject);
            }

            modelLoader.OnLoadEnded += OnLoadEnded;
            modelLoader.OnLoadError += OnLoadError;

            StartCoroutine(LoadMetadataAfterAssets(assetInformation));
        }

        private IEnumerator LoadMetadataAfterAssets(AssetInformation assetInformation)
        {
            while (true)
            {
                if (assetCount != 0)
                {
                    yield return new WaitForSecondsRealtime(0.2f);
                } else
                {
                    break;
                }
            }

            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(assetInformation.metadataString);
            if (metadataObject == null)
            {
                InitMetadata(assetInformation);
            }
            else
            {
                ApplyMetadata(metadataObject);
            }
        }

        private void OnLoadEnded(string assetId, GameObject model)
        {
            assetContainer.AddModel(assetObjectDict[assetId], model);
            assetCount -= 1;
        }

        private void OnLoadError(string assetId, string error)
        {
            Modal modal = Instantiate(modalPrefab, canvas.transform);
            modal.SetModalContent("Error", error);
            modal.DisableCancelButton();
        }

        private void ApplyMetadata(MetadataObject metadataObject)
        {
            if (metadataObject.modelDatas != null)
            {
                foreach(ModelData modelData in metadataObject.modelDatas)
                {
                    try
                    {
                        GameObject model = assetContainer.GetModel(modelData.assetId);
                        ModelEntity modelEntity = ModelEntity.InstantNewEntity(model, modelData);
                        modelEntity.gameObject.transform.parent = container.transform;
                    } catch (KeyNotFoundException)
                    {
                        Debug.Log("Missing asset found");
                    }
                }
            }
            
            if (metadataObject.noteDatas != null)
            {
                foreach(NoteData noteData in metadataObject.noteDatas)
                {
                    NoteEntity note = NoteEntity.InstantNewEntity(notePrefab, noteData);
                    note.gameObject.transform.parent = container.transform;
                }
            }

            environmentEditorWindow.SetAmbientColor(metadata.ambientColor);
            if (metadata.lightDatas.Count > 0)
            {
                environmentEditorWindow.SetDirectionalLight(metadata.lightDatas[0]);
            } else
            {
                environmentEditorWindow.SetDirectionalLight(new LightData());
            }

            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(container));
        }

        private void InitMetadata(AssetInformation assetInformation)
        {
            environmentEditorWindow.SetAmbientColor(Color.white);
            environmentEditorWindow.SetDirectionalLight(new LightData());

            foreach (AssetObject assetObject in assetInformation.assets)
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
                {
                    ModelData modelData = new ModelData();
                    modelData.assetId = assetObject.assetsId;
                    GameObject model = assetContainer.GetModel(modelData.assetId);
                    ModelEntity modelEntity = ModelEntity.InstantNewEntity(model, modelData);
                    modelEntity.gameObject.transform.parent = container.transform;
                    Bounds bounds = Utils.GetModelBounds(modelEntity.gameObject);
                    float ratio = scaleToSize / bounds.extents.magnitude;
                    model.transform.position = -(bounds.center * ratio) + new Vector3(0, distanceToPlane + bounds.extents.y * ratio, 0);
                    model.transform.localScale *= ratio;
                    break;
                }
            }

            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(container));
        }
    }
}

