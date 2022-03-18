using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class NoteEditorWindow : MonoBehaviour
    {
        public event Action<string> OnTextInputFieldChanged;

        public event Action<Color> OnBackgroundColorChanged;

        public event Action<float> OnBorderWidthChanged;
        public event Action<int> OnBorderRadiusChanged;
        public event Action<Color> OnBorderColorChanged;

        public event Action<int> OnFontSizeChanged;
        public event Action<Color> OnFontColorChanged;

        public event Action<float> OnBoxWidthChanged;
        public event Action<float> OnHeightChanged;

        public event Action<string> OnButtonTitleInputFieldChanged;

        public event Action<Color> OnButtonBackgroundColorChanged;

        public event Action<float> OnButtonBorderWidthChanged;
        public event Action<int> OnButtonBorderRadiusChanged;
        public event Action<Color> OnButtonBorderColorChanged;

        public event Action<int> OnButtonFontSizeChanged;
        public event Action<Color> OnButtonFontColorChanged;

        public event Action OnDeleteButtonClick;

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
        private InputFieldWithSlider height;

        [SerializeField]
        private TMP_InputField buttonInputField;
        [SerializeField]
        private ColorSelector buttonBackgroundColorSelector;
        [SerializeField]
        private InputFieldWithSlider buttonBorderWidth;
        [SerializeField]
        private InputFieldWithSlider buttonBorderRadius;
        [SerializeField]
        private ColorSelector buttonBorderColorSelector;
        [SerializeField]
        private InputFieldWithSlider buttonFontSize;
        [SerializeField]
        private ColorSelector buttonFontColor;

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
            textInputField.text = noteData.noteContent;
            backgroundColorSelector.SetColor(noteData.textBackgroundColor);
            borderWidth.SetValue(noteData.borderWidth.x);
            borderRadius.SetValue(noteData.textBorderRadius.x);
            borderColorSelector.SetColor(noteData.borderColor);
            fontSize.SetValue(noteData.fontSize);
            fontColor.SetColor(noteData.textColor);
            buttonInputField.text = noteData.buttonTitle;
            buttonBackgroundColorSelector.SetColor(noteData.buttonBackgroundColor);
            buttonBorderWidth.SetValue(noteData.buttonBorderWidth.x);
            buttonBorderRadius.SetValue(noteData.buttonBorderRadius.x);
            buttonBorderColorSelector.SetColor(noteData.buttonBorderColor);
            buttonFontSize.SetValue(noteData.buttonFontSize);
            buttonFontColor.SetColor(noteData.buttonTextColor);
        }
        

        void Start()
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

            height.OnValueChanged += (float value) =>
            {
                OnHeightChanged?.Invoke(value);
            };

            buttonInputField.onValueChanged.AddListener((string text) =>
            {
                OnButtonTitleInputFieldChanged?.Invoke(text);
            });

            buttonBackgroundColorSelector.OnColorChanged += (Color color) =>
            {
                OnButtonBackgroundColorChanged?.Invoke(color);
            };

            buttonBorderWidth.OnValueChanged += (float value) =>
            {
                OnButtonBorderWidthChanged?.Invoke(value);
            };

            buttonBorderRadius.OnValueChanged += (float value) =>
            {
                OnButtonBorderRadiusChanged?.Invoke((int)value);
            };

            buttonFontColor.OnColorChanged += (Color color) =>
            {
                OnButtonFontColorChanged?.Invoke(color);
            };

            buttonFontSize.OnValueChanged += (float value) =>
            {
                OnButtonFontSizeChanged?.Invoke((int) value);
            };

            buttonBorderColorSelector.OnColorChanged += (Color color) =>
            {
                OnButtonBorderColorChanged?.Invoke(color);
            };

            CloseEditor();
        }
    }
}
