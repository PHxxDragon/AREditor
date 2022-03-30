using EAR.Entity;

namespace EAR.View
{
    public class AddNoteCommand : IUndoRedoCommand
    {
        private NoteEntity note;
        public AddNoteCommand(NoteEntity note)
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

