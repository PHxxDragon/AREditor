using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class ToolBar: MonoBehaviour
    {
        [SerializeField]
        private Toggle cameraRotateToggle;
        [SerializeField]
        private Toggle moveToggle;
        [SerializeField]
        private Toggle rotateToggle;
        [SerializeField]
        private Toggle scaleToggle;
        [SerializeField]
        private Button saveButton;

        private ToolEnum activeTool;

        public event Action<ToolEnum, ToolEnum> OnToolChanged;
        public event Action SaveButtonClicked;

        public ToolEnum GetActiveTool()
        {
            return activeTool;
        }

        void Awake()
        {
            cameraRotateToggle.isOn = true;
            activeTool = ToolEnum.CameraRotate;
        }

        void Start()
        {
            cameraRotateToggle.onValueChanged.AddListener(CameraRotateToggleActive);
            moveToggle.onValueChanged.AddListener(MoveToggleActive);
            rotateToggle.onValueChanged.AddListener(RotateToggleActive);
            scaleToggle.onValueChanged.AddListener(ScaleToggleActive);
            saveButton.onClick.AddListener(SaveButtonClick);
        }

        private void CameraRotateToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.CameraRotate);
                activeTool = ToolEnum.CameraRotate;
            }
        }

        private void MoveToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.Move);
                activeTool = ToolEnum.Move;
            }
        }

        private void RotateToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.Rotate);
                activeTool = ToolEnum.Rotate;
            }
        }
        private void ScaleToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.Scale);
                activeTool = ToolEnum.Scale;
            }
        }

        private void SaveButtonClick()
        {
            SaveButtonClicked?.Invoke();
        }
    }
}

