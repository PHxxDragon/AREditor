using UnityEngine;
using System.Collections.Generic;
using System;

namespace EAR.UndoRedo
{
    public class UndoRedoManager : MonoBehaviour
    {
        public event Action OnBeforeUndo;
        public event Action OnBeforeRedo;

        private Stack<IUndoRedoCommand> commandStack = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        public void AddCommand(IUndoRedoCommand command)
        {
            commandStack.Push(command);
            redoStack.Clear();
        }

        public void PerformUndo()
        {
            OnBeforeUndo?.Invoke();
            if (commandStack.Count > 0)
            {
                IUndoRedoCommand command = commandStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void PerformRedo()
        {
            OnBeforeRedo?.Invoke();
            if (redoStack.Count > 0)
            {
                IUndoRedoCommand command = redoStack.Pop();
                command.Redo();
                commandStack.Push(command);
            }
        }
    }
}

