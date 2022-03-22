using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace EAR.View
{
    public class ColorPickerHandler : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster raycaster;
        [SerializeField]
        private FlexibleColorPicker flexibleColorPicker;

        private EventSystem eventSystem;
        private ColorSelector colorSelector;

        public void SetColor(Color color)
        {
            flexibleColorPicker.color = color;
        }

        public void SetColorSelector(ColorSelector colorSelector)
        {
            this.colorSelector = colorSelector;
        }

        void Start()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                Debug.LogError("No Event System!");
                return;
            }
            if (raycaster == null || flexibleColorPicker == null)
            {
                Debug.Log("Unassigned references");
                return;
            }
            flexibleColorPicker.onColorChange.AddListener(OnColorChange);
        }

        private void OnColorChange(Color arg0)
        {
            colorSelector.SetColor(arg0);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponentInParent<ColorPickerHandler>() != null || result.gameObject.GetComponentInParent<ColorSelector>() != null)
                    {
                        return;
                    }
                }
                gameObject.SetActive(false);
            }
        }
    }
}

