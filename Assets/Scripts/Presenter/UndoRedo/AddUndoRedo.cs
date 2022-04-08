using UnityEngine;
using RuntimeHandle;
using EAR.UndoRedo;
using EAR.Selection;
using EAR.View;

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
        [SerializeField]
        private ToolBar toolbar;

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
            if (toolbar != null && undoRedoManager != null)
            {
                toolbar.RedoButtonClicked += undoRedoManager.PerformRedo;
                toolbar.UndoButtonClicked += undoRedoManager.PerformUndo;
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

