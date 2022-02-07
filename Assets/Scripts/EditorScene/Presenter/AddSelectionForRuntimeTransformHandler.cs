using UnityEngine;
using RuntimeHandle;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class AddSelectionForRuntimeTransformHandler : MonoBehaviour
    {
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private SelectionManager selectionManager;

        void Start()
        {
            if (selectionManager != null && runtimeTransformHandle != null)
            {
                selectionManager.OnObjectSelected += AddTarget;
                selectionManager.OnObjectDeselected += RemoveTarget;
                selectionManager.CheckMouseRaycastBlocked += CheckIfMouseOverHandle;
            }
        }

        private void CheckIfMouseOverHandle(ref bool isOver)
        {
            if (runtimeTransformHandle.CheckIfMouseOverHandle())
            {
                isOver = true;
            }
        }

        private void AddTarget(Selectable target)
        {
            runtimeTransformHandle.target = target.transform;
            runtimeTransformHandle.selectionEnabled = true;
        }

        private void RemoveTarget(Selectable target)
        {
            if (runtimeTransformHandle.target == target.transform)
            {
                runtimeTransformHandle.target = null;
                runtimeTransformHandle.selectionEnabled = false;
            }
        }
    }
}

