using UnityEngine;
using EAR.View;
using EAR.AddObject;
using EAR.Selection;
using EAR.Entity;

namespace EAR.Editor.Presenter
{
    public class SoundPresenter : MonoBehaviour
    {
        private const string KEY = "SoundPresenter";

        [SerializeField]
        private ToolBar toolbar;
        [SerializeField]
        private ObjectPreviewAndAdd objectPreviewAndAdd;
        [SerializeField]
        private GameObject soundPreviewPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private SoundEditorWindow soundEditorWindow;

        private SoundEntity currentSound;

        void Start()
        {
            toolbar.OnToolChanged += (ToolEnum prev, ToolEnum current) => {
                if (current == ToolEnum.AddSound)
                {
                    objectPreviewAndAdd.StartPreviewAndAdd(KEY, soundPreviewPrefab, (TransformData transformData) =>
                    {
                        SoundData soundData = new SoundData();
                        soundData.transform = transformData;
                        SoundEntity.InstantNewEntity(soundData);
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
                SoundEntity soundEntity = selectable.GetComponent<SoundEntity>();
                if (soundEntity != null)
                {
                    currentSound = soundEntity;
                    soundEditorWindow.PopulateData(currentSound.GetSoundData());
                    soundEditorWindow.OpenEditor();
                }
            };

            selectionManager.OnObjectDeselected += (Selectable selectable) =>
            {
                SoundEntity soundEntity = selectable.GetComponent<SoundEntity>();
                if (soundEntity == currentSound)
                {
                    currentSound = null;
                    if (soundEditorWindow)
                        soundEditorWindow.CloseEditor();
                }
            };

            soundEditorWindow.OnSoundAssetSelected += (string assetId) =>
            {
                currentSound.SetAudioClip(assetId);
            };

            soundEditorWindow.OnDelete += () =>
            {
                //TODO
                GameObject toDestroy = currentSound.gameObject;
                selectionManager.DeselectAndGetCommand();
                Destroy(toDestroy);
            };

            soundEditorWindow.OnLoopChanged += (value) =>
            {
                currentSound.SetLoop(value);
            };

            soundEditorWindow.OnPlayAtStartChanged += (value) =>
            {
                currentSound.SetPlayAtStart(value);
            };
        }
    }
}
