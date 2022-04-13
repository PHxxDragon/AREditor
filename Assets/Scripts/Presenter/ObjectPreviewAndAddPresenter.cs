using UnityEngine;
using EAR.View;
using EAR.AddObject;
using EAR.Entity;
using EAR.Localization;
using EAR.UndoRedo;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class ObjectPreviewAndAddPresenter : MonoBehaviour
    {
        [SerializeField]
        private Toolbar toolbar;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private UndoRedoManager undoRedoManager;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject imagePreviewPrefab;
        [SerializeField]
        private GameObject modelPreviewPrefab;
        [SerializeField]
        private GameObject soundPreviewPrefab;
        [SerializeField]
        private GameObject buttonPreviewPrefab;
        [SerializeField]
        private GameObject notePreviewPrefab;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                string key = current.ToString();
                switch(current)
                {
                    case ToolEnum.AddImage:
                        objectPreviewAndAdd.StartPreviewAndAdd(imagePreviewPrefab, (TransformData transform) =>
                        {
                            ImageData imageData = new ImageData();
                            imageData.transform = transform;
                            BaseEntity addedEntity = ImageEntity.InstantNewEntity(imageData);
                            toolbar.SetDefaultTool();
                            AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                            undoRedoManager.AddCommand(add);
                        });
                        break;
                    case ToolEnum.AddButton:
                        objectPreviewAndAdd.StartPreviewAndAdd(buttonPreviewPrefab, (TransformData transformData) =>
                        {
                            ButtonData buttonData = new ButtonData();
                            buttonData.transform = transformData;
                            BaseEntity addedEntity = ButtonEntity.InstantNewEntity(buttonData);
                            toolbar.SetDefaultTool();
                            AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                            undoRedoManager.AddCommand(add);
                        });
                        break;
                    case ToolEnum.AddModel:
                        objectPreviewAndAdd.StartPreviewAndAdd(modelPreviewPrefab, (TransformData transformData) =>
                        {
                            ModelData modelData = new ModelData();
                            modelData.transform = transformData;
                            BaseEntity addedEntity = ModelEntity.InstantNewEntity(modelData);
                            toolbar.SetDefaultTool();
                            AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                            undoRedoManager.AddCommand(add);
                        });
                        break;
                    case ToolEnum.AddNote:
                        objectPreviewAndAdd.StartPreviewAndAdd(notePreviewPrefab, (TransformData transfomData) =>
                        {
                            NoteData noteData = new NoteData();
                            noteData.noteContent = LocalizationManager.GetLocalizedText("NoteFirstText");
                            noteData.transform = transfomData;
                            BaseEntity addedEntity = NoteEntity.InstantNewEntity(noteData);
                            toolbar.SetDefaultTool();
                            AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                            undoRedoManager.AddCommand(add);
                        });
                        break;
                    case ToolEnum.AddSound:
                        objectPreviewAndAdd.StartPreviewAndAdd(soundPreviewPrefab, (TransformData transformData) =>
                        {
                            SoundData soundData = new SoundData();
                            soundData.transform = transformData;
                            BaseEntity addedEntity = SoundEntity.InstantNewEntity(soundData);
                            toolbar.SetDefaultTool();
                            AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                            undoRedoManager.AddCommand(add);
                        });
                        break;
                    default:
                        objectPreviewAndAdd.StopPreview();
                        break;
                }
            };
        }
    }
}
    
