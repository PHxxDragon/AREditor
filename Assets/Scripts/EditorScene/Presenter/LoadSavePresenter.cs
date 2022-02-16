using UnityEngine;
using EAR.AR;
using EAR.View;
using EAR.WebRequest;
using EAR.Integration;

namespace EAR.Editor.Presenter
{
    public class LoadSavePresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageHolder imageHolder;

        [SerializeField]
        private ModelLoader modelLoader;

        [SerializeField]
        private ToolBar toolBar;

        [SerializeField]
        private WebRequestHelper webRequestHelper;

        void Start()
        {
            if (toolBar != null)
            {
                toolBar.SaveButtonClicked += SaveButtonClicked;
            }
        }

        private void SaveButtonClicked()
        {
            MetadataObject metadataObject = new MetadataObject();
            metadataObject.imageWidthInMeters = imageHolder.widthInMeter;
            metadataObject.modelTransform = TransformData.TransformToTransformData(modelLoader.GetModel().transform);
            ModuleParam param = LocalStorage.Load<ModuleParam>("param");
            webRequestHelper.SetModuleMetadata(param.token, param.moduleId, metadataObject, SetModuleMetadataSuccessCallback, SetModuleMetadataErrorCallback);
        }

        private void SetModuleMetadataSuccessCallback()
        {

        }

        private void SetModuleMetadataErrorCallback(string error)
        {
            Debug.Log(error);
        }
    }
}

