namespace EAR.View
{
    public class AddNoteCommand : IUndoRedoCommand
    {
        private Note note;
        public AddNoteCommand(Note note)
        {
            this.note = note;
        }
        public void Redo()
        {
            note.gameObject.SetActive(true);
        }

        public void Undo()
        {
            note.gameObject.SetActive(false);
        }
    }
}

