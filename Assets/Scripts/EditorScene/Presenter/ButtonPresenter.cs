using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.Entity;
using EAR.Entity.EntityAction;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class ButtonPresenter : MonoBehaviour
    {
        private const string KEY = "ButtonPresenter";

        [SerializeField]
        private ToolBar toolbar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject buttonPreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private ButtonEditorWindow buttonEditorWindow;

        private ButtonEntity currentButton;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) =>
            {
                if (current == ToolEnum.AddButton)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(KEY, buttonPreviewPrefab, (TransformData transformData) =>
                    {
                        ButtonData buttonData = new ButtonData();
                        buttonData.transform = transformData;
                        ButtonEntity buttonEntity = ButtonEntity.InstantNewEntity(buttonData);
                        toolbar.SetDefaultTool();
                        //TODO
/*                        IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<NoteEntity>());
                        undoRedoManager.AddCommand(command);*/
                    });
                }
                else
                {
                    objectPreviewAndAdd.StopPreview(KEY);
                }
            };

            

            selectionManager.OnObjectSelected += (Selectable selectable) =>
            {
                ButtonEntity buttonEntity = selectable.GetComponent<ButtonEntity>();
                if (buttonEntity != null)
                {
                    currentButton = buttonEntity;
                    buttonEditorWindow.PopulateData(currentButton.GetButtonData());
                    buttonEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                ButtonEntity buttonEntity = selectable.GetComponent<ButtonEntity>();
                if (buttonEntity == currentButton)
                {
                    currentButton = null;
                    if (buttonEditorWindow)
                        buttonEditorWindow.CloseEditor();
                }
            };

            buttonEditorWindow.OnListenerEntityIdChanged += (entityId) =>
            {
                currentButton.SetActivatorEntityId(entityId);
            };

            buttonEditorWindow.OnButtonActionDataAdded += (buttonActionData) =>
            {
                currentButton.actions.Add(ButtonActionFactory.CreateButtonAction(buttonActionData));
            };
            buttonEditorWindow.OnButtonActionDataDelete += (index) =>
            {
                currentButton.actions.RemoveAt(index);
            };
            buttonEditorWindow.OnButtonActionDataChanged += (index, buttonActionData) =>
            {
                currentButton.actions[index] = ButtonActionFactory.CreateButtonAction(buttonActionData);
            };

        }
    }
}


