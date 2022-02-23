using UnityEngine;
using System.Collections.Generic;

namespace EAR.UndoRedo
{
    public class UndoRedoManager : MonoBehaviour
    {
        private Stack<IUndoRedoCommand> commandStack = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        public void AddCommand(IUndoRedoCommand command)
        {
            commandStack.Push(command);
            redoStack.Clear();
        }

        public void PerformUndo()
        {
            if (commandStack.Count > 0)
            {
                IUndoRedoCommand command = commandStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void PerformRedo()
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

