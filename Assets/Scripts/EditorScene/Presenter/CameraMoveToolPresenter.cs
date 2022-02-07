using UnityEngine;
using EAR.EARCamera;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class CameraMoveToolPresenter : MonoBehaviour
    {
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private ToolBar toolbar;

        void Start()
        {
            if (cameraController != null && toolbar != null)
            {
                if (toolbar.GetActiveTool() == ToolEnum.CameraRotate)
                {
                    cameraController.enabled = true;
                }
                else
                {
                    cameraController.enabled = false;
                }
                toolbar.OnToolChanged += OnToolChanged;
            }
        }

        private void OnToolChanged(ToolEnum prev, ToolEnum current)
        {
            if (current == ToolEnum.CameraRotate)
            {
                cameraController.enabled = true;
            }
            else
            {
                cameraController.enabled = false;
            }
        }
    }
}

