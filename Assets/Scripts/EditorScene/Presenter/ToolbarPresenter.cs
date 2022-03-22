using EAR.Screenshoter;
using EAR.Integration;
using UnityEngine;
using EAR.View;
using EAR.AR;
using System.Collections.Generic;
using RuntimeHandle;

namespace EAR.Editor.Presenter 
{
    public class ToolbarPresenter : MonoBehaviour
    {
        [SerializeField]
        private Screenshot screenshot;
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ToolBar toolBar;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private GameObject noteContainer;
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private EnvironmentEditorWindow environmentEditorWindow;

        void Start()
        {
            if (screenshot != null && reactPlugin != null && toolBar != null)
            {
                toolBar.ScreenshotButtonClicked += () =>
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

            if (toolBar != null && noteContainer != null)
            {
                toolBar.SaveButtonClicked += SaveButtonClicked;
            }
            else
            {
                Debug.Log("Unassigned references");
            }

            if (toolBar != null && runtimeTransformHandle != null)
            {
                SetHandle(ToolEnum.Move, toolBar.GetActiveTool());
                toolBar.OnToolChanged += SetHandle;
            } else
            {
                Debug.Log("Unassigned references");
            }
            if (toolBar != null && runtimeTransformHandle != null)
            {
                toolBar.SettingButtonClicked += () =>
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
            if (modelLoader.GetModel() != null)
            {
                MetadataObject metadataObject = new MetadataObject();
                metadataObject.modelTransform = TransformData.TransformToTransformData(modelLoader.GetModel().transform);

                List<NoteData> noteDatas = new List<NoteData>();
                foreach (Transform child in noteContainer.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        Note note = child.GetComponent<Note>();
                        if (note != null)
                        {
                            noteDatas.Add(note.GetNoteData());
                        }
                    }
                }
                metadataObject.noteDatas = noteDatas;
                metadataObject.ambientColor = RenderSettings.ambientLight;
                reactPlugin.Save(JsonUtility.ToJson(metadataObject));
            }
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

