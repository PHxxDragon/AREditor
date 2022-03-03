using UnityEngine;
using EAR.AR;
using EAR.View;
using EAR.Integration;
using System.Collections.Generic;

namespace EAR.Editor.Presenter
{
    public class LoadSavePresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageHolder imageHolder;

        [SerializeField]
        private ModelLoader modelLoader;

        [SerializeField]
        private ToolBar toolBar;

        [SerializeField]
        private ReactPlugin reactPlugin;

        [SerializeField]
        private GameObject noteContainer;

        void Start()
        {
            if (toolBar != null && noteContainer != null)
            {
                toolBar.SaveButtonClicked += SaveButtonClicked;
            } else
            {
                Debug.Log("Unassigned references");
            }
        }

        private void SaveButtonClicked()
        {
            if (modelLoader.GetModel() != null)
            {
                MetadataObject metadataObject = new MetadataObject();
                metadataObject.modelTransform = TransformData.TransformToTransformData(modelLoader.GetModel().transform);
                if (imageHolder.gameObject.activeSelf)
                {
                    metadataObject.imageWidthInMeters = imageHolder.widthInMeter;
                }

                List<NoteData> noteDatas = new List<NoteData>();
                foreach (Transform child in noteContainer.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        Note note = child.GetComponent<Note>();
                        if (note != null)
                        {
                            noteDatas.Add(note.GetNoteData());
                        }
                    }
                }
                metadataObject.noteDatas = noteDatas;
                reactPlugin.Save(JsonUtility.ToJson(metadataObject));
            }
        }
    }
}

