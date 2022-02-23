using UnityEngine;
using EAR.AR;
using EAR.View;
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
        private ReactPlugin reactPlugin;

        void Start()
        {
            if (toolBar != null)
            {
                toolBar.SaveButtonClicked += SaveButtonClicked;
            }
        }

        private void SaveButtonClicked()
        {
            if (modelLoader.GetModel() != null)
            {
                MetadataObject metadataObject = new MetadataObject();
                metadataObject.modelTransform = TransformData.TransformToTransformData(modelLoader.GetModel().transform);
                if (imageHolder.gameObject.activeSelf)
                {
                    metadataObject.imageWidthInMeters = imageHolder.widthInMeter;
                }
                reactPlugin.Save(JsonUtility.ToJson(metadataObject));
            }
/*            ModuleParam param = LocalStorage.Load<ModuleParam>("param");
            webRequestHelper.SetModuleMetadata(param.token, param.moduleId, metadataObject, SetModuleMetadataSuccessCallback, SetModuleMetadataErrorCallback);*/
        }
    }
}

