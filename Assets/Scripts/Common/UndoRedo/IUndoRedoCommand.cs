namespace EAR
{
    public interface IUndoRedoCommand
    {
        public void Undo();
        public void Redo();
    }
}

