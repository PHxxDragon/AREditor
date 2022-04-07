using UnityEngine;
using EAR.Integration;
using EAR.AR;
using EAR.EARCamera;
using EAR.View;
using EAR.AssetManager;
using EAR.Container;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private ProgressBar progressBar;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private GameObject canvas;

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
            progressBar.EnableProgressBar();
            AssetContainer.Instance.LoadAssets(assetInformation.assets, () =>
            {
                progressBar.DisableProgressBar();
                LoadMetadata(assetInformation);
            }, (error) => {
                Modal modal = Instantiate(modalPrefab, canvas.transform);
                modal.SetModalContent("Error", error);
                modal.DisableCancelButton();
            }, progressBar.SetProgress);
        }

        private void LoadMetadata(AssetInformation assetInformation)
        {
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(assetInformation.metadataString);
            if (metadataObject == null)
            {
                EntityContainer.Instance.InitMetadata(assetInformation.assets);
            }
            else
            {
                EntityContainer.Instance.ApplyMetadata(metadataObject);
            }

            cameraController.SetDefaultCameraPosition(Utils.GetModelBounds(EntityContainer.Instance.gameObject));
        }
    }
}

