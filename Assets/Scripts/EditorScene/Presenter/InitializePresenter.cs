using UnityEngine;
using EAR.Integration;
using EAR.AR;
using EAR.EARCamera;
using EAR.View;
using EAR.Entity;
using EAR.AssetManager;
using System.Collections;
using System.Collections.Generic;
using System;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private float scaleToSize = 1f;
        [SerializeField]
        private float distanceToPlane = 0f;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private EnvironmentEditorWindow environmentEditorWindow;

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
                        modelLoader.LoadModel(assetObject.assetId, assetObject.url, assetObject.extension, assetObject.isZipFile);
                        break;
                    case AssetObject.IMAGE_TYPE:
                        LoadImage(assetObject.assetId, assetObject.url);
                        break;
                    case AssetObject.SOUND_TYPE:
                        LoadSound(assetObject.assetId, assetObject.url, assetObject.extension);
                        break;
                    default:
                        assetCount -= 1;
                        break;
                }
                assetObjectDict.Add(assetObject.assetId, assetObject);
            }

            modelLoader.OnLoadEnded += OnLoadEnded;
            modelLoader.OnLoadError += OnLoadError;

            StartCoroutine(LoadMetadataAfterAssets(assetInformation));
        }

        private void LoadSound(string assetId, string url, string extension)
        {
            Utils.Instance.GetSound(url, extension,
            (audioClip) =>
            {
                AssetContainer.Instance.AddSound(assetObjectDict[assetId], audioClip);
                assetCount -= 1;
            }, (error) =>
            {
                Modal modal = Instantiate(modalPrefab, canvas.transform);
                modal.SetModalContent("Error", error);
                modal.DisableCancelButton();
            });
        }

        private void LoadImage(string assetId, string url)
        {
            Utils.Instance.GetImageAsTexture2D(url,
            (image) =>
            {
                AssetContainer.Instance.AddImage(assetObjectDict[assetId], image);
                assetCount -= 1;
            }, (error) =>
            {
                Modal modal = Instantiate(modalPrefab, canvas.transform);
                modal.SetModalContent("Error", error);
                modal.DisableCancelButton();
            });
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
            AssetContainer.Instance.AddModel(assetObjectDict[assetId], model);
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
                        ModelEntity modelEntity = ModelEntity.InstantNewEntity(modelData);
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
                    NoteEntity note = NoteEntity.InstantNewEntity(noteData);
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

            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(EntityContainer.Instance.gameObject));
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
                    modelData.assetId = assetObject.assetId;
                    ModelEntity modelEntity = ModelEntity.InstantNewEntity(modelData);
                    Bounds bounds = Utils.GetModelBounds(modelEntity.gameObject);
                    float ratio = scaleToSize / bounds.extents.magnitude;
                    modelEntity.transform.position = -(bounds.center * ratio) + new Vector3(0, distanceToPlane + bounds.extents.y * ratio, 0);
                    modelEntity.transform.localScale *= ratio;
                    break;
                }
            }

            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(EntityContainer.Instance.gameObject));
        }
    }
}

