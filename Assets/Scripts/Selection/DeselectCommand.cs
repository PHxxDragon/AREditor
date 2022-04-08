using System;

namespace EAR.Selection
{
    public class DeselectCommand: IUndoRedoCommand
    {
        private Action actionRedo;
        private Action<Selectable> actionUndo;
        private Selectable selectable;

        public DeselectCommand(Selectable selectable, Action actionRedo, Action<Selectable> actionUndo)
        {
            this.selectable = selectable;
            this.actionRedo = actionRedo;
            this.actionUndo = actionUndo;
        }

        public void Redo()
        {
            actionRedo.Invoke();
        }

        public void Undo()
        {
            actionUndo.Invoke(selectable);
        }

    }
}

