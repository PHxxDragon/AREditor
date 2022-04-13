using EAR.Screenshoter;
using EAR.Integration;
using UnityEngine;
using EAR.View;
using EAR.AR;
using EAR.Entity;
using System.Collections.Generic;
using RuntimeHandle;
using EAR.Container;
using EAR.UndoRedo;

namespace EAR.Editor.Presenter 
{
    public class ToolbarPresenter : MonoBehaviour
    {
        [SerializeField]
        private Screenshot screenshot;
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private Toolbar toolbar;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private SettingWindow environmentEditorWindow;
        [SerializeField]
        private EnvironmentController environmentController;
        [SerializeField]
        private UndoRedoManager undoRedoManager;

        void Start()
        {
            if (screenshot != null && reactPlugin != null && toolbar != null)
            {
                toolbar.ScreenshotButtonClicked += () =>
                {
                    screenshot.TakeScreenshot();
                };
                screenshot.OnScreenshotTake += (byte[] image) =>
                {
                    reactPlugin.SaveScreenshotToJs(image);
                };
            } else
            {
                Debug.Log("Unassigned references");
            }

            toolbar.SaveButtonClicked += SaveButtonClicked;

            toolbar.RedoButtonClicked += undoRedoManager.PerformRedo;
            toolbar.UndoButtonClicked += undoRedoManager.PerformUndo;

            if (toolbar != null && runtimeTransformHandle != null)
            {
                SetHandle(ToolEnum.Move, toolbar.GetActiveTool());
                toolbar.OnToolChanged += SetHandle;
            } else
            {
                Debug.Log("Unassigned references");
            }
            if (toolbar != null && runtimeTransformHandle != null)
            {
                toolbar.SettingButtonClicked += () =>
                {
                    environmentEditorWindow.gameObject.SetActive(true);
                };
            } else
            {
                Debug.Log("Unassigned references");
            }
        }

        private void SaveButtonClicked()
        {
            MetadataObject metadataObject = new MetadataObject();
            foreach (BaseEntity entity in EntityContainer.Instance.GetEntities())
            {
                if (entity is ModelEntity modelEntity)
                {
                    metadataObject.modelDatas.Add((ModelData) modelEntity.GetData());
                } else if (entity is ImageEntity imageEntity)
                {
                    metadataObject.imageDatas.Add((ImageData) imageEntity.GetData());
                } else if (entity is NoteEntity noteEntity)
                {
                    metadataObject.noteDatas.Add((NoteData) noteEntity.GetData());
                } else if (entity is ButtonEntity buttonEntity)
                {
                    metadataObject.buttonDatas.Add((ButtonData) buttonEntity.GetData());
                } else if (entity is SoundEntity soundEntity)
                {
                    metadataObject.soundDatas.Add((SoundData) soundEntity.GetData());
                }
            }

            metadataObject.ambientColor = RenderSettings.ambientLight;

            List<LightData> lightDatas = new List<LightData>();
            lightDatas.Add(environmentController.GetLightData());
            metadataObject.lightDatas = lightDatas;
            reactPlugin.Save(JsonUtility.ToJson(metadataObject));
        }

        private void SetHandle(ToolEnum prev, ToolEnum current)
        {
            switch (current)
            {
                case ToolEnum.Move:
                    runtimeTransformHandle.toolEnabled = true;
                    runtimeTransformHandle.type = HandleType.POSITION;
                    break;
                case ToolEnum.Rotate:
                    runtimeTransformHandle.toolEnabled = true;
                    runtimeTransformHandle.type = HandleType.ROTATION;
                    break;
                case ToolEnum.Scale:
                    runtimeTransformHandle.toolEnabled = true;
                    runtimeTransformHandle.type = HandleType.SCALE;
                    break;
                default:
                    runtimeTransformHandle.toolEnabled = false;
                    break;
            }
        }
    }
}

