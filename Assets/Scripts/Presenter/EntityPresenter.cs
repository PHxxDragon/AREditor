using EAR.Selection;
using EAR.View;
using EAR.Entity;
using EAR.EARCamera;
using UnityEngine;
using System;

namespace EAR.Editor.Presenter
{
    public class EntityPresenter : MonoBehaviour
    {
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private ImageEditorWindow imageEditorWindow;
        [SerializeField]
        private ModelEditorWindow modelEditorWindow;
        [SerializeField]
        private SoundEditorWindow soundEditorWindow;
        [SerializeField]
        private ButtonEditorWindow buttonEditorWindow;
        [SerializeField]
        private NoteEditorWindow noteEditorWindow;
        [SerializeField]
        private CameraController cameraController;

        private BaseEntity currentEntity;

        void Start()
        {
            selectionManager.OnObjectSelected += (Selectable selectable) =>
            {
                BaseEntity entity = selectable.GetComponent<BaseEntity>();
                if (entity is ImageEntity imageEntity)
                {
                    currentEntity = imageEntity;
                    imageEditorWindow.PopulateData(imageEntity.GetImageData());
                    imageEditorWindow.OpenEditor();
                } else if (entity is NoteEntity noteEntity)
                {
                    currentEntity = noteEntity;
                    noteEditorWindow.PopulateData(noteEntity.GetNoteData());
                    noteEditorWindow.OpenEditor();
                } else if (entity is ModelEntity modelEntity)
                {
                    currentEntity = modelEntity;
                    modelEditorWindow.PopulateData(modelEntity.GetModelData());
                    modelEditorWindow.OpenEditor();
                }
                else if (entity is ButtonEntity buttonEntity)
                {
                    currentEntity = buttonEntity;
                    buttonEditorWindow.PopulateData(buttonEntity.GetButtonData());
                    buttonEditorWindow.OpenEditor();
                }
                else if (entity is SoundEntity soundEntity)
                {
                    currentEntity = soundEntity;
                    soundEditorWindow.PopulateData(soundEntity.GetSoundData());
                    soundEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                currentEntity = null;
                BaseEntity entity = selectable.GetComponent<BaseEntity>();
                if (imageEditorWindow) imageEditorWindow.CloseEditor();
                if (noteEditorWindow) noteEditorWindow.CloseEditor();
                if (modelEditorWindow) modelEditorWindow.CloseEditor();
                if (soundEditorWindow) soundEditorWindow.CloseEditor();
                if (buttonEditorWindow) buttonEditorWindow.CloseEditor();
            };

            imageEditorWindow.OnImageDelete += () =>
            {
                CheckAndDestroy(typeof(ImageEntity));
            };

            imageEditorWindow.OnImageChanged += (ImageData imageData) =>
            {
                if (currentEntity is ImageEntity imageEntity)
                {
                    imageEntity.PopulateData(imageData);
                }
            };

            buttonEditorWindow.OnButtonDelete += () =>
            {
                CheckAndDestroy(typeof(ButtonEntity));
            };
            buttonEditorWindow.OnButtonDataChanged += (ButtonData buttonData) =>
            {
                if (currentEntity is ButtonEntity buttonEntity)
                {
                    buttonEntity.PopulateData(buttonData);
                }
            };


            modelEditorWindow.OnModelDelete += () =>
            {
                CheckAndDestroy(typeof(ModelEntity));
            };
            modelEditorWindow.OnModelChanged += (ModelData modelData) =>
            {
                if (currentEntity is ModelEntity modelEntity)
                {
                    modelEntity.PopulateData(modelData);
                }
            };

            noteEditorWindow.OnDeleteButtonClick += () =>
            {
                CheckAndDestroy(typeof(NoteEntity));
            };
            noteEditorWindow.OnNoteDataChanged += (NoteData noteData) =>
            {
                if (currentEntity is NoteEntity noteEntity)
                {
                    noteEntity.PopulateData(noteData);
                }
            };

            soundEditorWindow.OnDelete += () =>
            {
                CheckAndDestroy(typeof(SoundEntity));
            };
            soundEditorWindow.OnSoundChanged += (SoundData soundData) =>
            {
                if (currentEntity is SoundEntity soundEntity)
                {
                    soundEntity.PopulateData(soundData);
                }
            };

            cameraController.CheckKeyboardBlocked += (ref bool isBlocked) =>
            {
                if (noteEditorWindow.isActiveAndEnabled 
                || buttonEditorWindow.isActiveAndEnabled 
                || soundEditorWindow.isActiveAndEnabled 
                || modelEditorWindow.isActiveAndEnabled
                || imageEditorWindow.isActiveAndEnabled)
                {
                    isBlocked = true;
                }
            };
        }

        private void CheckAndDestroy(Type type)
        {
            if (currentEntity.GetType() == type)
            {
                Destroy(currentEntity);
            }
        }
    }
}

