using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.EARCamera;
using EAR.UndoRedo;
using EAR.Entity;
using EAR.Localization;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class NotePresenter : MonoBehaviour
    {
        private const string KEY = "NotePresenter";

        [SerializeField]
        private ToolBar toolBar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject notePreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private NoteEditorWindow noteEditorWindow;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private UndoRedoManager undoRedoManager;

        private NoteEntity currentNote;

        void Start()
        {
            if (toolBar == null || objectPreviewAndAdd == null || noteEditorWindow == null || selectionManager == null || cameraController == null || undoRedoManager == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            toolBar.OnToolChanged += OnToolbarToolChanged;

            selectionManager.OnObjectSelected += (Selectable selectable) => {
                NoteEntity note = selectable.GetComponent<NoteEntity>();
                if (note != null)
                {
                    currentNote = note;
                    PopulateNoteDataToEditor();
                    noteEditorWindow.OpenEditor();
                }
            };
            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                NoteEntity note = selectable.GetComponent<NoteEntity>();
                if (note == currentNote)
                {
                    currentNote = null;
                    if (noteEditorWindow) noteEditorWindow.CloseEditor();
                }
            };
            //TODO
/*            selectionManager.CheckMouseRaycastBlocked += (ref bool isBlocked) =>
            {
                if (toolBar.GetActiveTool() == ToolEnum.AddNote)
                {
                    isBlocked = true;
                }
            };*/

            noteEditorWindow.OnTextInputFieldChanged += (string value) =>
            {
                currentNote.SetText(value != "" ? value : " ");
            };

            noteEditorWindow.OnDeleteButtonClick += () =>
            {
                NoteEntity note = currentNote;
                IUndoRedoCommand deselectCommand = selectionManager.DeselectAndGetCommand();
                note.gameObject.SetActive(false);
                IUndoRedoCommand command = new RemoveNoteCommand(note, deselectCommand);
                undoRedoManager.AddCommand(command);
            };

            noteEditorWindow.OnFontSizeChanged += (int value) =>
            {
                currentNote.SetFontSize(value);
            };

            noteEditorWindow.OnBoxWidthChanged += (float value) =>
            {
                currentNote.SetBoxWidth(value);
            };

            noteEditorWindow.OnBackgroundColorChanged += (Color color) =>
            {
                currentNote.SetTextBackgroundColor(color);
            };

            noteEditorWindow.OnBorderRadiusChanged += (int radius) =>
            {
                currentNote.SetTextBorderRadius(new Vector4(radius, radius, radius, radius));
            };

            noteEditorWindow.OnFontColorChanged += (Color color) =>
            {
                currentNote.SetTextColor(color);
            };

            noteEditorWindow.OnBorderColorChanged += (Color color) =>
            {
                currentNote.SetBorderColor(color);
            };

            noteEditorWindow.OnBorderWidthChanged += (float width) =>
            {
                currentNote.SetTextBorderWidth(new Vector4(width, width, width, width));
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
            noteEditorWindow.PopulateData(currentNote.GetNoteData());
        }

        private void OnToolbarToolChanged(ToolEnum prev, ToolEnum current)
        {
            if (current == ToolEnum.AddNote)
            {
                objectPreviewAndAdd.StartPreviewAndAdd(KEY, notePreviewPrefab, (TransformData transfomData) =>
                {
                    NoteData noteData = new NoteData();
                    noteData.noteContent = LocalizationManager.GetLocalizedText("NoteFirstText");
                    noteData.noteTransformData = transfomData;
                    NoteEntity note = NoteEntity.InstantNewEntity(noteData);
                    toolBar.SetDefaultTool();
                    IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<NoteEntity>());
                    undoRedoManager.AddCommand(command);
                }, (GameObject note) =>
                {
                    note.GetComponent<NoteEntity>().SetText(LocalizationManager.GetLocalizedText("NoteFirstText"));
                });
            } else
            {
                objectPreviewAndAdd.StopPreview(KEY);
            }
        }
    }

}
