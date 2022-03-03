namespace EAR.View
{
    public class RemoveNoteCommand : IUndoRedoCommand
    {
        private Note note;
        private IUndoRedoCommand deselectCommand;
        public RemoveNoteCommand(Note note, IUndoRedoCommand deselectCommand)
        {
            this.note = note;
            this.deselectCommand = deselectCommand;
        }
        public void Redo()
        {
            deselectCommand.Redo();
            note.gameObject.SetActive(false);
        }

        public void Undo()
        {
            note.gameObject.SetActive(true);
            deselectCommand.Undo();
        }
    }
}

