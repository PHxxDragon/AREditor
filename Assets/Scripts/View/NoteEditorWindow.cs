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
            nameInputField.text = noteData.name;
            isVisible.isOn = noteData.isVisible;
            textInputField.text = noteData.noteContent;
            backgroundColorSelector.SetColor(noteData.textBackgroundColor);
            borderWidth.SetValue(noteData.borderWidth.x);
            borderRadius.SetValue(noteData.textBorderRadius.x);
            borderColorSelector.SetColor(noteData.borderColor);
            fontSize.SetValue(noteData.fontSize);
            fontColor.SetColor(noteData.textColor);
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
