using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class NoteEditorWindow : MonoBehaviour
    {
        public event Action OnDeleteButtonClick;
        public event Action<string> OnTextInputFieldChanged;
        public event Action<string> OnButtonTitleInputFieldChanged;
        public event Action<int> OnFontSizeChanged;
        public event Action<float> OnBoxWidthChanged;
        public event Action<float> OnHeightChanged;

        [SerializeField]
        private TMP_InputField textInputField;
        [SerializeField]
        private TMP_InputField buttonTitleInputField;
        [SerializeField]
        private Button deleteButton;
        [SerializeField]
        private TMP_InputField fontSizeInputField;
        [SerializeField]
        private Slider boxWidthSlider;
        [SerializeField]
        private TMP_InputField boxWidthInputField;
        [SerializeField]
        private Slider heightSlider;
        [SerializeField]
        private TMP_InputField heightInputField;

        private int fontSizeMin = 1;
        private int fontSizeMax = 80;
        private float boxWidthMin = 1;
        private float boxWidhtMax = 1500;
        private float heightMin = 0.1f;
        private float heightMax = 3f;

        public void CloseEditor()
        {
            gameObject.SetActive(false);
        }

        public void OpenEditor()
        {
            gameObject.SetActive(true);
        }

        public void SetFontSize (int value)
        {
            fontSizeInputField.text = value.ToString();
        }

        public void SetBoxWidth (float value)
        {
            boxWidthSlider.value = value;
        }

        public void SetHeight(float value)
        {
            heightSlider.value = value;
        }

        public void SetTextInputField(string value)
        {
            textInputField.text = value;
        }

        public void SetButtonTitleInputField(string value)
        {
            buttonTitleInputField.text = value;
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
            buttonTitleInputField.onValueChanged.AddListener((string value) =>
            {
                OnButtonTitleInputFieldChanged?.Invoke(value);
            });

            fontSizeInputField.onValueChanged.AddListener((string value) =>
            {
                try
                {
                    int fontSize = int.Parse(value);
                    int clampedFontSize = Utils.Clamp(fontSize, fontSizeMin, fontSizeMax);

                    if (clampedFontSize != fontSize)
                    {
                        fontSizeInputField.text = clampedFontSize.ToString();
                    }
                    else
                    {
                        OnFontSizeChanged?.Invoke(clampedFontSize);
                    }
                } catch (FormatException)
                {

                }
               
            });

            boxWidthInputField.onValueChanged.AddListener((string value) =>
            {
                try
                {
                    float boxWidth = float.Parse(value);
                    float clampedBoxWidth = Utils.Clamp(boxWidth, boxWidthMin, boxWidhtMax);
                    if (clampedBoxWidth != boxWidth)
                    {
                        boxWidthInputField.text = clampedBoxWidth.ToString();
                    }
                    else
                    {
                        boxWidthSlider.value = clampedBoxWidth;
                    }
                } catch (FormatException)
                {

                }
                
            });

            boxWidthSlider.minValue = boxWidthMin;
            boxWidthSlider.maxValue = boxWidhtMax;

            boxWidthSlider.onValueChanged.AddListener((float value) => {
                boxWidthInputField.text = value.ToString();
                OnBoxWidthChanged?.Invoke(value);
            });

            heightInputField.onValueChanged.AddListener((string value) =>
            {
                try
                {
                    float height = float.Parse(value);
                    float clampedHeight = Utils.Clamp(height, heightMin, heightMax);
                    if (clampedHeight != height)
                    {
                        heightInputField.text = clampedHeight.ToString();
                    }
                    else
                    {
                        heightSlider.value = clampedHeight;
                    }
                } catch (FormatException)
                {

                }
                
            });

            heightSlider.minValue = heightMin;
            heightSlider.maxValue = heightMax;

            heightSlider.onValueChanged.AddListener((float value) => {
                heightInputField.text = value.ToString();
                OnHeightChanged?.Invoke(value);
            });


            CloseEditor();
        }
    }
}
