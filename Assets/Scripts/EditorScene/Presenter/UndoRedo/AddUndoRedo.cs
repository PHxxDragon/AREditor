using UnityEngine;
using RuntimeHandle;
using EAR.UndoRedo;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class AddUndoRedo : MonoBehaviour
    {
        [SerializeField]
        private RuntimeTransformHandle runtimeTransformHandle;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private UndoRedoManager undoRedoManager;

        void Awake()
        {
            if (runtimeTransformHandle != null && undoRedoManager != null)
            {
                runtimeTransformHandle.NewCommandEvent += AddCommandHandler;
            } else
            {
                Debug.LogWarning("Unassigned references");
            }

            if (selectionManager != null && undoRedoManager != null)
            {
                selectionManager.NewCommandEvent += AddCommandHandler;
            } else
            {
                Debug.LogWarning("Unassiged references");
            }
        }

        private void AddCommandHandler(IUndoRedoCommand obj)
        {
            undoRedoManager.AddCommand(obj);
        }
    }
}

