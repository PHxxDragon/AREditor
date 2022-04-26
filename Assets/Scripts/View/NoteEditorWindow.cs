using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class NoteEditorWindow : MonoBehaviour
    {
        public event Action<NoteData> OnNoteDataChanged;
        public event Action OnInteractionEnded;
        public event Action OnDeleteButtonClick;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TransformInput transformInput;
        [SerializeField]
        private Toggle isVisible;
        [SerializeField]
        private TMP_InputField textInputField;
        [SerializeField]
        private ColorSelector backgroundColorSelector;
        [SerializeField]
        private InputFieldWithSlider borderWidth;
        [SerializeField]
        private InputFieldWithSlider borderRadius;
        [SerializeField]
        private ColorSelector borderColorSelector;
        [SerializeField]
        private InputFieldWithSlider fontSize;
        [SerializeField]
        private ColorSelector fontColor;
        [SerializeField]
        private InputFieldWithSlider boxWidth;

        [SerializeField]
        private Button deleteButton;

        private bool isPopulating;

        public void CloseEditor()
        {
            gameObject.SetActive(false);
        }

        public void OpenEditor()
        {
            gameObject.SetActive(true);
        }

        public void PopulateData(NoteData noteData)
        {
            isPopulating = true;
            if (!string.IsNullOrEmpty(noteData.name))
            {
                nameInputField.text = noteData.name;
            }
            
            if (noteData.isVisible.HasValue)
            {
                isVisible.isOn = noteData.isVisible.Value;
            }

            if (!string.IsNullOrEmpty(noteData.noteContent))
            {
                textInputField.text = noteData.noteContent;
            }
            
            if (noteData.textBackgroundColor.HasValue)
            {
                backgroundColorSelector.SetColor(noteData.textBackgroundColor.Value);
            }
            
            if (noteData.borderWidth.HasValue)
            {
                borderWidth.SetValue(noteData.borderWidth.Value.x);
            }
            
            if (noteData.textBorderRadius.HasValue)
            {
                borderRadius.SetValue(noteData.textBorderRadius.Value.x);
            }
            
            if (noteData.borderColor.HasValue)
            {
                borderColorSelector.SetColor(noteData.borderColor.Value);
            }
            
            if (noteData.fontSize.HasValue)
            {
                fontSize.SetValue(noteData.fontSize.Value);
            }
            
            if (noteData.textColor.HasValue)
            {
                fontColor.SetColor(noteData.textColor.Value);
            }
            
            if (noteData.boxWidth.HasValue)
            {
                boxWidth.SetValue(noteData.boxWidth.Value);
            }
            
            if (noteData.transform != null)
            {
                transformInput.SetValue(noteData.transform);
            }
            isPopulating = false;
        }
        

        void Awake()
        {
            deleteButton.onClick.AddListener(() =>
            {
                OnDeleteButtonClick?.Invoke();
            });
            textInputField.onValueChanged.AddListener((string value) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.noteContent = value;
                OnNoteDataChanged?.Invoke(noteData);
            });
            textInputField.onEndEdit.AddListener((value) =>
            {
                OnInteractionEnded?.Invoke();
            });

            backgroundColorSelector.OnColorChanged += (Color color) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.textBackgroundColor = color;
                OnNoteDataChanged?.Invoke(noteData);
            };
            backgroundColorSelector.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            borderWidth.OnValueChanged += (float value) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.borderWidth = new Vector4(value, value, value, value);
                OnNoteDataChanged?.Invoke(noteData);
            };
            borderWidth.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            borderRadius.OnValueChanged += (float value) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.textBorderRadius = new Vector4(value, value, value, value);
                OnNoteDataChanged?.Invoke(noteData);
            };
            borderRadius.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            borderColorSelector.OnColorChanged += (Color color) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.borderColor = color;
                OnNoteDataChanged?.Invoke(noteData);
            };
            borderColorSelector.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            fontSize.OnValueChanged += (float value) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.fontSize = (int) value;
                OnNoteDataChanged?.Invoke(noteData);
            };
            fontSize.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            fontColor.OnColorChanged += (Color color) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.textColor = color;
                OnNoteDataChanged?.Invoke(noteData);
            };
            fontColor.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            boxWidth.OnValueChanged += (float value) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.boxWidth = value;
                OnNoteDataChanged?.Invoke(noteData);
            };
            boxWidth.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            nameInputField.onValueChanged.AddListener((name) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.name = name;
                OnNoteDataChanged?.Invoke(noteData);
            });
            nameInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());

            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.isVisible = isVisible;
                OnNoteDataChanged?.Invoke(noteData);
                OnInteractionEnded?.Invoke();
            });

            transformInput.OnTransformChanged += (TransformData data) =>
            {
                if (isPopulating) return;
                NoteData noteData = new NoteData();
                noteData.transform = data;
                OnNoteDataChanged?.Invoke(noteData);
            };
            transformInput.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            CloseEditor();
        }
    }
}
