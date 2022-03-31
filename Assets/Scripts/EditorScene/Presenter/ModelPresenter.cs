using EAR.View;
using UnityEngine;
using EAR.AddObject;
using EAR.Selection;
using EAR.Entity;
using EAR.AssetManager;

namespace EAR.Editor.Presenter
{
    public class ModelPresenter : MonoBehaviour
    {
        [SerializeField]
        private ToolBar toolbar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private GameObject modelPrefab;
        [SerializeField]
        private GameObject modelPreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private ModelEditorWindow modelEditorWindow;
        [SerializeField]
        private AssetContainer assetContainer;

        private ModelEntity currentModel;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                if (current == ToolEnum.AddModel)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(modelPrefab, modelPreviewPrefab, (GameObject model) =>
                    {
                        model.transform.parent = container.transform;
                        toolbar.SetDefaultTool();
                        //TODO
/*                        IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<NoteEntity>());
                        undoRedoManager.AddCommand(command);*/
                    });
                } else
                {

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
                GameObject model = assetContainer.GetModel(assetId);
                currentModel.SetModel(assetId, model);
            };
        }
    }
}

