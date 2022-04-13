using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class ColorSelector : MonoBehaviour
    {
        public event Action<Color> OnColorChanged;
        public event Action OnInteractionEnded;

        [SerializeField]
        private Image image;
        [SerializeField]
        private ColorPickerHandler colorPicker;
        [SerializeField]
        private Button button;

        void Start()
        {
            if (colorPicker == null)
            {
                Debug.Log("Unassigned references");
                return;
            }
            button.onClick.AddListener(ColorSelectorButtonClick);
        }

        private void ColorSelectorButtonClick()
        {
            colorPicker.gameObject.SetActive(true);
            colorPicker.SetColorSelector(this);
            colorPicker.SetColor(image.color);
            colorPicker.OnColorSelectorChanged += ColorSelectorChangedHandler;
        }

        private void ColorSelectorChangedHandler(ColorSelector colorSelector)
        {
            if (colorSelector != this)
            {
                colorPicker.OnColorSelectorChanged -= ColorSelectorChangedHandler;
                OnInteractionEnded?.Invoke();
            }
        }

        public void SetColor(Color color)
        {
            if (image.color != color)
            {
                image.color = color;
                OnColorChanged?.Invoke(color);
            }
        }

        public Color GetColor()
        {
            return image.color;
        }
    }
}

