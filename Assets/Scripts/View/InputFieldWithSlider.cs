using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class InputFieldWithSlider : MonoBehaviour
    {
        public event Action<float> OnValueChanged;

        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private float min;
        [SerializeField]
        private float max;

        public void SetValue(float value)
        {
            slider.value = value;
        }

        public void SetMin(float min)
        {
            slider.minValue = min;
            this.min = min;
        }

        public void SetMax(float max)
        {
            slider.maxValue = max;
            this.max = max;
        }

        void Awake()
        {
            slider.minValue = min;
            slider.maxValue = max;

            inputField.onValueChanged.AddListener((string value) =>
            {
                try
                {
                    float floatValue = float.Parse(value);
                    float clampedValue = Utils.Clamp(floatValue, min, max);
                    if (clampedValue != floatValue)
                    {
                        inputField.text = clampedValue.ToString();
                    }
                    else
                    {
                        slider.value = floatValue;
                    }
                }
                catch (FormatException)
                {

                }
            });
           
            slider.onValueChanged.AddListener((float value) =>
            {
                inputField.text = value.ToString();
                OnValueChanged?.Invoke(value);
            });

            inputField.text = slider.value.ToString();
        }
    }
}

