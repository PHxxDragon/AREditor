using System;

namespace EAR.Selection
{
    public class SelectCommand : IUndoRedoCommand
    {
        private Action<Selectable> actionRedo;
        private Action actionUndo;
        private Selectable selectable;
        public SelectCommand(Selectable selectable, Action<Selectable> actionRedo, Action actionUndo)
        {
            this.actionRedo = actionRedo;
            this.actionUndo = actionUndo;
            this.selectable = selectable;
        }
        public void Redo()
        {
            actionRedo.Invoke(selectable);
        }

        public void Undo()
        {
            actionUndo.Invoke();
        }
    }

}
