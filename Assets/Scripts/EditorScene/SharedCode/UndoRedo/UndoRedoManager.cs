using UnityEngine;
using System.Collections.Generic;

namespace EAR.UndoRedo
{
    public class UndoRedoManager : MonoBehaviour
    {
        [SerializeField]
        private KeyCode undoKeycode = KeyCode.Z;

        private Stack<IUndoRedoCommand> commandStack = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        public void AddCommand(IUndoRedoCommand command)
        {
            commandStack.Push(command);
            redoStack.Clear();
        }

        void Update()
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                if (Input.GetKeyDown(undoKeycode))
                {
                    PerformUndo();
                } else if (Input.GetKeyDown(KeyCode.R))
                {
                    PerformRedo();
                }
            }
        }

        private void PerformUndo()
        {
            if (commandStack.Count > 0)
            {
                IUndoRedoCommand command = commandStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        private void PerformRedo()
        {
            if (redoStack.Count > 0)
            {
                IUndoRedoCommand command = redoStack.Pop();
                command.Redo();
                commandStack.Push(command);
            }
        }
    }
}

