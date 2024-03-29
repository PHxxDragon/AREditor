using UnityEngine;
using EAR.Integration;
using EAR.EARCamera;
using EAR.View;
using EAR.Container;

namespace EAR.Editor.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ProgressBar progressBar;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject loadingPanel;

        void Awake()
        {
            if (reactPlugin != null)
            {
                reactPlugin.LoadModuleCalledEvent += LoadModuleCallback;
                reactPlugin.OnSetMode += (mode) =>
                {
                    GlobalStates.SetMode(mode);
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
                progressBar.DisableProgressBar();
            }, progressBar.SetProgress);
        }

        private void LoadMetadata(AssetInformation assetInformation)
        {
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(assetInformation.metadata);
            if (metadataObject == null)
            {
                EntityContainer.Instance.InitMetadata(assetInformation.assets);
            }
            else
            {
                EntityContainer.Instance.ApplyMetadata(metadataObject);
            }
            loadingPanel.SetActive(false);
        }
    }
}

