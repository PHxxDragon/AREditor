using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace EAR.View
{
    public class EnvironmentEditorWindow : MonoBehaviour
    {
        [SerializeField]
        private ColorSelector ambientColor;
        [SerializeField]
        private InputFieldWithSlider intensity;

        [SerializeField]
        private InputFieldWithSlider cameraFOV;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;

        private Color originalColor;
        private float colorExposure;

        void Awake()
        {
            ambientColor.OnColorChanged += (Color color) =>
            {
                originalColor = color;
                RenderSettings.ambientLight = ComposeHdrColor(originalColor, colorExposure);
            };

            intensity.OnValueChanged += (float value) =>
            {
                colorExposure = value;
                RenderSettings.ambientLight = ComposeHdrColor(originalColor, colorExposure);
            };

            cameraFOV.OnValueChanged += (float value) =>
            {
                mainCamera.fieldOfView = value;
            };

        }

        void Start()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                Debug.LogError("No Event System!");
                return;
            }
            if (raycaster == null)
            {
                Debug.Log("Unassigned references");
                return;
            }
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            cameraFOV.SetValue(mainCamera.fieldOfView);
        }

        public void SetAmbientColor(Color color)
        {
            RenderSettings.ambientLight = color;
            DecomposeHdrColor(color, out originalColor, out colorExposure);
            ambientColor.SetColor(originalColor);
            intensity.SetValue(colorExposure);
        }


        //https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ColorMutator.cs
        private const byte k_MaxByteForOverexposedColor = 191;
        private void DecomposeHdrColor(Color hdrColor, out Color original, out float exposure)
        {
            original = hdrColor;
            float maxColorComponent = hdrColor.maxColorComponent;
            if (maxColorComponent == 0f || maxColorComponent <= 1f && maxColorComponent >= 1 / 255f)
            {
                exposure = 0f;
            } else
            {
                float scaleFactor = (k_MaxByteForOverexposedColor / maxColorComponent) / 255f;
                exposure = Mathf.Log(1f / scaleFactor) / Mathf.Log(2f);

                original.r = scaleFactor * hdrColor.r;
                original.g = scaleFactor * hdrColor.g;
                original.b = scaleFactor * hdrColor.b;
            }
        }

        private Color ComposeHdrColor(Color original, float exposure)
        {
            return original * Mathf.Pow(2f, exposure);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                if (results.Count == 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
