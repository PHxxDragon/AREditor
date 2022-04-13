using EAR.Selection;
using EAR.View;
using EAR.Entity;
using EAR.EARCamera;
using EAR.UndoRedo;
using UnityEngine;
using System;

namespace EAR.Editor.Presenter
{
    public class EntityPresenter : MonoBehaviour
    {
        [SerializeField]
        private Toolbar toolbar;
        [SerializeField]
        private UndoRedoManager undoRedoManager;
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
        private bool entityModified = false;
        private EntityData entityData;

        void Start()
        {
            selectionManager.OnObjectSelected += (Selectable selectable) =>
            {
                BaseEntity entity = selectable.GetComponent<BaseEntity>();
                if (entity is ImageEntity imageEntity)
                {
                    currentEntity = imageEntity;
                    imageEditorWindow.PopulateData((ImageData) imageEntity.GetData());
                    imageEditorWindow.OpenEditor();
                } else if (entity is NoteEntity noteEntity)
                {
                    currentEntity = noteEntity;
                    noteEditorWindow.PopulateData((NoteData) noteEntity.GetData());
                    noteEditorWindow.OpenEditor();
                } else if (entity is ModelEntity modelEntity)
                {
                    currentEntity = modelEntity;
                    modelEditorWindow.PopulateData((ModelData) modelEntity.GetData());
                    modelEditorWindow.OpenEditor();
                }
                else if (entity is ButtonEntity buttonEntity)
                {
                    currentEntity = buttonEntity;
                    buttonEditorWindow.PopulateData((ButtonData) buttonEntity.GetData());
                    buttonEditorWindow.OpenEditor();
                }
                else if (entity is SoundEntity soundEntity)
                {
                    currentEntity = soundEntity;
                    soundEditorWindow.PopulateData((SoundData) soundEntity.GetData());
                    soundEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                EndModify();
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
                    StartModify();
                    imageEntity.PopulateData(imageData);
                }
            };
            imageEditorWindow.OnInteractionEnded += EndModify;

            buttonEditorWindow.OnButtonDelete += () =>
            {
                CheckAndDestroy(typeof(ButtonEntity));
            };
            buttonEditorWindow.OnButtonDataChanged += (ButtonData buttonData) =>
            {
                if (currentEntity is ButtonEntity buttonEntity)
                {
                    StartModify();
                    buttonEntity.PopulateData(buttonData);
                }
            };
            buttonEditorWindow.OnInteractionEnded += EndModify;

            modelEditorWindow.OnModelDelete += () =>
            {
                CheckAndDestroy(typeof(ModelEntity));
            };
            modelEditorWindow.OnModelChanged += (ModelData modelData) =>
            {
                if (currentEntity is ModelEntity modelEntity)
                {
                    StartModify();
                    modelEntity.PopulateData(modelData);
                }
            };
            modelEditorWindow.OnInteractionEnded += EndModify;

            noteEditorWindow.OnDeleteButtonClick += () =>
            {
                CheckAndDestroy(typeof(NoteEntity));
            };
            noteEditorWindow.OnNoteDataChanged += (NoteData noteData) =>
            {
                if (currentEntity is NoteEntity noteEntity)
                {
                    StartModify();
                    noteEntity.PopulateData(noteData);
                }
            };
            noteEditorWindow.OnInteractionEnded += EndModify;

            soundEditorWindow.OnDelete += () =>
            {
                CheckAndDestroy(typeof(SoundEntity));
            };
            soundEditorWindow.OnSoundChanged += (SoundData soundData) =>
            {
                if (currentEntity is SoundEntity soundEntity)
                {
                    StartModify();
                    soundEntity.PopulateData(soundData);
                }
            };
            soundEditorWindow.OnInteractionEnded += EndModify;

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

            toolbar.DuplicateButtonClicked += () =>
            {
                if (currentEntity)
                {
                    EntityData entityData = currentEntity.GetData();
                    entityData.id = null;
                    entityData.name = null;
                    entityData.transform.position += new Vector3(0.1f, 0.1f, 0.1f);
                    BaseEntity addedEntity = EntityFactory.InstantNewEntity(entityData);
                    AddEntityCommand add = new AddEntityCommand(selectionManager, addedEntity.GetData());
                    undoRedoManager.AddCommand(add);
                }
            };

            undoRedoManager.OnBeforeRedo += EndModify;
            undoRedoManager.OnBeforeUndo += EndModify;
        }

        void Update()
        {
            if (currentEntity && currentEntity.transform.hasChanged)
            {
                TransformData transformData = TransformData.TransformToTransformData(currentEntity.transform);
                if (currentEntity is ImageEntity)
                {
                    ImageData imageData = new ImageData();
                    imageData.transform = transformData;
                    imageEditorWindow.PopulateData(imageData);
                }
                else if (currentEntity is NoteEntity)
                {
                    NoteData noteData = new NoteData();
                    noteData.transform = transformData;
                    noteEditorWindow.PopulateData(noteData);
                }
                else if (currentEntity is ModelEntity)
                {
                    ModelData modelData = new ModelData();
                    modelData.transform = transformData;
                    modelEditorWindow.PopulateData(modelData);
                }
                else if (currentEntity is ButtonEntity)
                {
                    ButtonData buttonData = new ButtonData();
                    buttonData.transform = transformData;
                    buttonEditorWindow.PopulateData(buttonData);
                }
                else if (currentEntity is SoundEntity)
                {
                    SoundData soundData = new SoundData();
                    soundData.transform = transformData;
                    soundEditorWindow.PopulateData(soundData);
                }
                StartModify();
                currentEntity.transform.hasChanged = false;
            } else if (Input.GetMouseButtonUp(0))
            {
                EndModify();
            }
        }

        private void StartModify()
        {
            if (!entityModified)
            {
                entityModified = true;
                entityData = currentEntity.GetData();
            }
        }

        private void EndModify()
        {
            if (entityModified)
            {
                ChangeEntityCommand change = new ChangeEntityCommand(selectionManager, entityData, currentEntity.GetData());
                undoRedoManager.AddCommand(change);
                entityModified = false;
            }
        }

        private void CheckAndDestroy(Type type)
        {
            if (currentEntity && currentEntity.GetType() == type)
            {
                RemoveEntityCommand remove = new RemoveEntityCommand(selectionManager, currentEntity.GetData());
                undoRedoManager.AddCommand(remove);
                Destroy(currentEntity.gameObject);
            }
        }
    }
}

