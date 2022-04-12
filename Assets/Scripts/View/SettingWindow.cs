using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace EAR.View
{
    public class SettingWindow : MonoBehaviour
    {
        [SerializeField]
        private ColorSelector ambientColor;
        [SerializeField]
        private InputFieldWithSlider ambientIntensity;

        [SerializeField]
        private ColorSelector directionalColor;
        [SerializeField]
        private InputFieldWithSlider directionalIntensity;

        [SerializeField]
        private InputFieldWithSlider cameraFOV;

        [SerializeField]
        private EnvironmentController environmentController;

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
                if (originalColor != color)
                {
                    originalColor = color;
                    environmentController.SetAmbientLight(ComposeHdrColor(originalColor, colorExposure), false);
                }
            };

            ambientIntensity.OnValueChanged += (float value) =>
            {
                if (colorExposure != value)
                {
                    colorExposure = value;
                    environmentController.SetAmbientLight(ComposeHdrColor(originalColor, colorExposure), false);
                }
            };

            cameraFOV.OnValueChanged += (float value) =>
            {
                mainCamera.fieldOfView = value;
            };
            directionalColor.OnColorChanged += (Color color) =>
            {
                environmentController.SetDirectionalLightColor(color);
            };
            directionalIntensity.OnValueChanged += (float value) =>
            {
                environmentController.SetDirectionalLightIntensity(value);
            };

            environmentController.OnAmbientLightChanged += (Color color) =>
            {
                DecomposeHdrColor(color, out originalColor, out colorExposure);
                ambientColor.SetColor(originalColor);
                ambientIntensity.SetValue(colorExposure);
            };

            environmentController.OnDirectionalLightDataChanged += (LightData data) =>
            {
                directionalColor.SetColor(data.color);
                directionalIntensity.SetValue(data.intensity);
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

            gameObject.SetActive(false);
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
