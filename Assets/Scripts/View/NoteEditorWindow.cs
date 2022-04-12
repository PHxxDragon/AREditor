using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class NoteEditorWindow : MonoBehaviour
    {
        public event Action<string> OnNameChanged;
        public event Action<bool> OnVisibilityChanged;

        public event Action<string> OnTextInputFieldChanged;

        public event Action<Color> OnBackgroundColorChanged;

        public event Action<float> OnBorderWidthChanged;
        public event Action<int> OnBorderRadiusChanged;
        public event Action<Color> OnBorderColorChanged;

        public event Action<int> OnFontSizeChanged;
        public event Action<Color> OnFontColorChanged;

        public event Action<float> OnBoxWidthChanged;

        public event Action OnDeleteButtonClick;

        [SerializeField]
        private TMP_InputField nameInputField;
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
            
        }
        

        void Awake()
        {
            deleteButton.onClick.AddListener(() =>
            {
                OnDeleteButtonClick?.Invoke();
            });
            textInputField.onValueChanged.AddListener((string value) =>
            {
                OnTextInputFieldChanged?.Invoke(value);
            });

            backgroundColorSelector.OnColorChanged += (Color color) =>
            {
                OnBackgroundColorChanged?.Invoke(color);
            };

            borderWidth.OnValueChanged += (float value) =>
            {
                OnBorderWidthChanged?.Invoke(value);
            };

            borderRadius.OnValueChanged += (float value) =>
            {
                OnBorderRadiusChanged?.Invoke((int) value);
            };

            borderColorSelector.OnColorChanged += (Color color) =>
            {
                OnBorderColorChanged?.Invoke(color);
            };

            fontSize.OnValueChanged += (float value) =>
            {
                OnFontSizeChanged?.Invoke((int)value);
            };

            fontColor.OnColorChanged += (Color color) =>
            {
                OnFontColorChanged?.Invoke(color);
            };

            boxWidth.OnValueChanged += (float value) =>
            {
                OnBoxWidthChanged?.Invoke(value);
            };
            nameInputField.onValueChanged.AddListener((name) =>
            {
                OnNameChanged?.Invoke(name);
            });
            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                OnVisibilityChanged?.Invoke(isVisible);
            });

            CloseEditor();
        }
    }
}
