using UnityEngine;
using RuntimeHandle;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class RuntimeTransformHandlerPresenter : MonoBehaviour
    {
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private SelectionManager selectionManager;

        void Start()
        {
            GlobalStates.CheckMouseRaycastBlocked += CheckIfMouseDraggingHandle;
            selectionManager.OnObjectSelected += AddTarget;
            selectionManager.OnObjectDeselected += RemoveTarget;
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

