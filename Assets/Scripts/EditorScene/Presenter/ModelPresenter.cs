using EAR.View;
using UnityEngine;
using EAR.AddObject;
using EAR.Selection;
using EAR.Entity;

namespace EAR.Editor.Presenter
{
    public class ModelPresenter : MonoBehaviour
    {
        private const string KEY = "ModelPrefab";

        [SerializeField]
        private ToolBar toolbar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject modelPreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private ModelEditorWindow modelEditorWindow;

        private ModelEntity currentModel;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                if (current == ToolEnum.AddModel)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(KEY, modelPreviewPrefab, (TransformData transformData) =>
                    {
                        ModelData modelData = new ModelData();
                        modelData.transform = transformData;
                        ModelEntity.InstantNewEntity(modelData);
                        toolbar.SetDefaultTool();
                        //TODO
/*                        IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<NoteEntity>());
                        undoRedoManager.AddCommand(command);*/
                    });
                } else
                {
                    objectPreviewAndAdd.StopPreview(KEY);
                }
            };
            selectionManager.OnObjectSelected += (Selectable selectable) =>
            {
                ModelEntity modelEntity = selectable.GetComponent<ModelEntity>();
                if (modelEntity != null)
                {
                    currentModel = modelEntity;
                    modelEditorWindow.PopulateData(currentModel.GetModelData());
                    modelEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                ModelEntity modelEntity = selectable.GetComponent<ModelEntity>();
                if (modelEntity == currentModel)
                {
                    currentModel = null;
                    if (modelEditorWindow) 
                        modelEditorWindow.CloseEditor();
                }
            };

            modelEditorWindow.OnModelAssetSelected += (string assetId) =>
            {
                currentModel.SetModel(assetId);
            };
        }
    }
}

