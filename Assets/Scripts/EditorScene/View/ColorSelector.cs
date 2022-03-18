using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class ColorSelector : MonoBehaviour
    {
        public event Action<Color> OnColorChanged;

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
        }

        public void SetColor(Color color)
        {
            image.color = color;
            OnColorChanged?.Invoke(color);
        }

        public Color GetColor()
        {
            return image.color;
        }
    }
}

