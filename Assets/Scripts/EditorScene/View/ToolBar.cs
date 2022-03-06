using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class ToolBar: MonoBehaviour
    {
        [SerializeField]
        private Toggle moveToggle;
        [SerializeField]
        private Toggle rotateToggle;
        [SerializeField]
        private Toggle scaleToggle;
        [SerializeField]
        private Toggle noteAddToggle;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button undoButton;
        [SerializeField]
        private Button redoButton;
        [SerializeField]
        private Button screenshotButton;

        private ToolEnum activeTool;

        public event Action<ToolEnum, ToolEnum> OnToolChanged;
        public event Action SaveButtonClicked;
        public event Action UndoButtonClicked;
        public event Action RedoButtonClicked;
        public event Action ScreenshotButtonClicked;

        public ToolEnum GetActiveTool()
        {
            return activeTool;
        }

        void Awake()
        {
            moveToggle.isOn = true;
            activeTool = ToolEnum.Move;
        }

        void Start()
        {
            moveToggle.onValueChanged.AddListener(MoveToggleActive);
            rotateToggle.onValueChanged.AddListener(RotateToggleActive);
            scaleToggle.onValueChanged.AddListener(ScaleToggleActive);
            noteAddToggle.onValueChanged.AddListener(NoteToggleActive);
            saveButton.onClick.AddListener(SaveButtonClick);
            undoButton.onClick.AddListener(UndoButtonClick);
            redoButton.onClick.AddListener(RedoButtonClick);
            screenshotButton.onClick.AddListener(ScreenshotButtonClick);
            ApplyGlobalStates();
        }

        public void SetDefaultTool()
        {
            moveToggle.isOn = true;
        }

        private void ApplyGlobalStates()
        {
            gameObject.SetActive(GlobalStates.IsEnableEditor());
            GlobalStates.OnEnableEditorChange += (bool value) =>
            {
                gameObject.SetActive(value);
            };
            screenshotButton.gameObject.SetActive(GlobalStates.IsEnableScreenshot());
            GlobalStates.OnEnableScreenshotChange += (bool value) =>
            {
                screenshotButton.gameObject.SetActive(value);
            };
        }

        private void ScreenshotButtonClick()
        {
            ScreenshotButtonClicked?.Invoke();
        }

        private void RedoButtonClick()
        {
            RedoButtonClicked?.Invoke();
        }

        private void UndoButtonClick()
        {
            UndoButtonClicked?.Invoke();
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

        private void NoteToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.AddNote);
                activeTool = ToolEnum.AddNote;
            }
        }

        private void SaveButtonClick()
        {
            SaveButtonClicked?.Invoke();
        }
    }
}

