using RuntimeHandle;
using UnityEngine;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class MoveToolPresenter : MonoBehaviour
    {
        [SerializeField]
        private ToolBar toolbar;

        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;


        void Start()
        {
            if (toolbar != null && runtimeTransformHandle != null)
            {
                SetHandle(ToolEnum.CameraRotate, toolbar.GetActiveTool());
                toolbar.OnToolChanged += SetHandle;
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

