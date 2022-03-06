using UnityEngine;
using RuntimeHandle;
using EAR.Selection;
using EAR.EARCamera;

namespace EAR.Editor.Presenter
{
    public class RuntimeTransformHandlerPresenter : MonoBehaviour
    {
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private CameraController cameraController;

        void Start()
        {
            if (selectionManager != null && runtimeTransformHandle != null)
            {
                selectionManager.OnObjectSelected += AddTarget;
                selectionManager.OnObjectDeselected += RemoveTarget;
                selectionManager.CheckMouseRaycastBlocked += CheckIfMouseDraggingHandle;
            }
            if (cameraController != null && runtimeTransformHandle != null)
            {
                cameraController.CheckMouseRaycastBlocked += CheckIfMouseDraggingHandle;
            }
        }

        private void CheckIfMouseDraggingHandle(ref bool isOver)
        {
            if (runtimeTransformHandle.CheckIfMouseDraggingHandle())
            {
                isOver = true;
            }
        }

        private void AddTarget(Selectable target)
        {
            runtimeTransformHandle.target = target.transform;
        }

        private void RemoveTarget(Selectable target)
        {
            if (runtimeTransformHandle.target == target.transform)
            {
                runtimeTransformHandle.target = null;
            }
        }
    }
}

