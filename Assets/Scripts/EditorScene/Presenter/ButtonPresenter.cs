using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.AssetManager;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class ButtonPresenter : MonoBehaviour
    {
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

        //private ImageEntity currentImage;

        /*void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                if (current == ToolEnum.AddImage)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(imagePrefab, imagePreviewPrefab, (GameObject image) =>
                    {
                        image.transform.parent = container.transform;
                        toolbar.SetDefaultTool();
                        //TODO
                        *//*                        IUndoRedoCommand command = new AddNoteCommand(note.GetComponent<NoteEntity>());
                                                undoRedoManager.AddCommand(command);*//*
                    });
                }
                else
                {
                    objectPreviewAndAdd.StopPreview(imagePrefab);
                }
            };
            selectionManager.OnObjectSelected += (Selectable selectable) =>
            {
                ImageEntity imageEntity = selectable.GetComponent<ImageEntity>();
                if (imageEntity != null)
                {
                    currentImage = imageEntity;
                    imageEditorWindow.PopulateData(currentImage.GetImageData());
                    imageEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                ImageEntity imageEntity = selectable.GetComponent<ImageEntity>();
                if (imageEntity == currentImage)
                {
                    currentImage = null;
                    if (imageEditorWindow)
                        imageEditorWindow.CloseEditor();
                }
            };

            imageEditorWindow.OnModelAssetSelected += (string assetId) =>
            {
                Texture2D image = assetContainer.GetImage(assetId);
                currentImage.SetImage(assetId);
            };
        }*/
    }
}


