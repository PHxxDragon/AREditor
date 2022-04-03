using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.Entity;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class ImagePresenter : MonoBehaviour
    {
        private const string KEY = "ImagePresenter";
        [SerializeField]
        private ToolBar toolbar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject imagePreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private ImageEditorWindow imageEditorWindow;

        private ImageEntity currentImage;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                if (current == ToolEnum.AddImage)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(KEY, imagePreviewPrefab, (TransformData transform) =>
                    {
                        ImageData imageData = new ImageData();
                        imageData.transform = transform;
                        ImageEntity.InstantNewEntity(imageData);
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

            imageEditorWindow.OnImageAssetSelected += (string assetId) =>
            {
                currentImage.SetImage(assetId);
            };

            imageEditorWindow.OnImageDelete += () =>
            {
                //TODO
                GameObject toDestroy = currentImage.gameObject;
                selectionManager.DeselectAndGetCommand();
                Destroy(toDestroy);
            };
        }
    }
}

