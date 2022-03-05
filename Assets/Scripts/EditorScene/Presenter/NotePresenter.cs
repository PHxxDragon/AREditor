using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.EARCamera;
using EAR.UndoRedo;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class NotePresenter : MonoBehaviour
    {
        [SerializeField]
        private ToolBar toolBar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject notePrefab;
        [SerializeField]
        private GameObject notePreviewPrefab;
        [SerializeField]
        private GameObject noteContainer;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private NoteEditorWindow noteEditorWindow;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private UndoRedoManager undoRedoManager;

        private Note currentNote;

        void Start()
        {
            if (toolBar == null || objectPreviewAndAdd == null || noteEditorWindow == null || selectionManager == null || cameraController == null || undoRedoManager == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            toolBar.OnToolChanged += OnToolbarToolChanged;

            selectionManager.OnObjectSelected += (Selectable selectable) => {
                Note note = selectable.GetComponent<Note>();
                if (note != null)
                {
                    currentNote = note;
                    PopulateNoteDataToEditor();
                    noteEditorWindow.OpenEditor();
                }
            };
            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                Note note = selectable.GetComponent<Note>();
                if (note == currentNote)
                {
                    currentNote = null;
                    noteEditorWindow.CloseEditor();
                }
            };
            selectionManager.CheckMouseRaycastBlocked += (ref bool isBlocked) =>
            {
                if (toolBar.GetActiveTool() == ToolEnum.AddNote)
                {
                    isBlocked = true;
                }
            };

            noteEditorWindow.OnTextInputFieldChanged += (string value) =>
            {
                currentNote.SetText(value != "" ? value : " ");
            };
            noteEditorWindow.OnButtonTitleInputFieldChanged += (string value) =>
            {
                currentNote.SetButtonText(value != "" ? value : " ");
            };
            noteEditorWindow.OnDeleteButtonClick += () =>
            {
                Note note = currentNote;
                IUndoRedoCommand deselectCommand = selectionManager.DeselectAndGetCommand();
                note.gameObject.SetActive(false);
                IUndoRedoCommand command = new RemoveNoteCommand(note, deselectCommand);
                undoRedoManager.AddCommand(command);
            };

            noteEditorWindow.OnFontSizeChanged += (int value) =>
            {
                currentNote.SetFontSize(value);
            };

            noteEditorWindow.OnHeightChanged += (float value) =>
            {
                currentNote.SetHeight(value);
            };

            noteEditorWindow.OnBoxWidthChanged += (float value) =>
            {
                currentNote.SetBoxWidth(value);
            };

            cameraController.CheckKeyboardBlocked += (ref bool isBlocked) =>
            {
                if (noteEditorWindow.isActiveAndEnabled)
                {
                    isBlocked = true;
                }
            };
        }

        private void PopulateNoteDataToEditor()
        {
            noteEditorWindow.SetTextInputField(currentNote.GetText());
            noteEditorWindow.SetButtonTitleInputField(currentNote.GetButtonText());
            noteEditorWindow.SetFontSize(currentNote.GetFontSize());
            noteEditorWindow.SetBoxWidth(currentNote.GetBoxWidth());
            noteEditorWindow.SetHeight(currentNote.GetHeight());
        }

        private void OnToolbarToolChanged(ToolEnum prev, ToolEnum current)
        {
            if (current == ToolEnum.AddNote)
            {
                objectPreviewAndAdd.StartPreviewAndAdd(notePrefab, notePreviewPrefab, (GameObject note) =>
                {
                    note.transform.parent = noteContainer.transform;
                    toolBar.SetDefaultTool();
                    IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<Note>());
                    undoRedoManager.AddCommand(command);
                });
            } else
            {
                objectPreviewAndAdd.StopPreview();
            }
        }
    }

}
