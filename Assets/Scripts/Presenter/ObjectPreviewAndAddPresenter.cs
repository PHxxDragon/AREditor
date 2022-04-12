using UnityEngine;
using EAR.View;
using EAR.AddObject;
using EAR.Entity;
using EAR.Localization;

namespace EAR.Editor.Presenter
{
    public class ObjectPreviewAndAddPresenter : MonoBehaviour
    {
        [SerializeField]
        private ToolBar toolbar;
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
                            ImageEntity.InstantNewEntity(imageData);
                            toolbar.SetDefaultTool();
                        });
                        break;
                    case ToolEnum.AddButton:
                        objectPreviewAndAdd.StartPreviewAndAdd(buttonPreviewPrefab, (TransformData transformData) =>
                        {
                            ButtonData buttonData = new ButtonData();
                            buttonData.transform = transformData;
                            ButtonEntity buttonEntity = ButtonEntity.InstantNewEntity(buttonData);
                            toolbar.SetDefaultTool();
                        });
                        break;
                    case ToolEnum.AddModel:
                        objectPreviewAndAdd.StartPreviewAndAdd(modelPreviewPrefab, (TransformData transformData) =>
                        {
                            ModelData modelData = new ModelData();
                            modelData.transform = transformData;
                            ModelEntity.InstantNewEntity(modelData);
                            toolbar.SetDefaultTool();
                        });
                        break;
                    case ToolEnum.AddNote:
                        objectPreviewAndAdd.StartPreviewAndAdd(notePreviewPrefab, (TransformData transfomData) =>
                        {
                            NoteData noteData = new NoteData();
                            noteData.noteContent = LocalizationManager.GetLocalizedText("NoteFirstText");
                            noteData.noteTransformData = transfomData;
                            NoteEntity note = NoteEntity.InstantNewEntity(noteData);
                            toolbar.SetDefaultTool();
                        });
                        break;
                    case ToolEnum.AddSound:
                        if (current == ToolEnum.AddSound)
                        {
                            objectPreviewAndAdd.StartPreviewAndAdd(soundPreviewPrefab, (TransformData transformData) =>
                            {
                                SoundData soundData = new SoundData();
                                soundData.transform = transformData;
                                SoundEntity.InstantNewEntity(soundData);
                                toolbar.SetDefaultTool();
                            });
                        }
                        break;
                    default:
                        objectPreviewAndAdd.StopPreview();
                        break;
                }
            };
        }
    }
}
    
