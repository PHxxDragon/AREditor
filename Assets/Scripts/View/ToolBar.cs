using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private Toggle modelAddToggle;
        [SerializeField]
        private Toggle imageAddToggle;
        [SerializeField]
        private Toggle buttonAddToggle;
        [SerializeField]
        private Toggle soundAddToggle;

        [SerializeField]
        private Toggle playToggle;

        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button undoButton;
        [SerializeField]
        private Button redoButton;
        [SerializeField]
        private Button screenshotButton;
        [SerializeField]
        private Button settingButton;

        [SerializeField]
        private List<GameObject> hideForModelDetail;
        [SerializeField]
        private GameObject showInPlayMode;

        private ToolEnum activeTool;

        public event Action<ToolEnum, ToolEnum> OnToolChanged;
        public event Action SaveButtonClicked;
        public event Action UndoButtonClicked;
        public event Action RedoButtonClicked;
        public event Action ScreenshotButtonClicked;
        public event Action SettingButtonClicked;

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
            modelAddToggle.onValueChanged.AddListener(ModelToggleActive);
            imageAddToggle.onValueChanged.AddListener(ImageToggleActive);
            buttonAddToggle.onValueChanged.AddListener(ButtonToggleActive);
            soundAddToggle.onValueChanged.AddListener(SoundToggleActive);
            saveButton.onClick.AddListener(SaveButtonClick);
            undoButton.onClick.AddListener(UndoButtonClick);
            redoButton.onClick.AddListener(RedoButtonClick);
            screenshotButton.onClick.AddListener(ScreenshotButtonClick);
            settingButton.onClick.AddListener(SettingButtonClick);
            playToggle.onValueChanged.AddListener(PlayToggleClick);
            ApplyGlobalStates();
        }

        private void SoundToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.AddSound);
                activeTool = ToolEnum.AddSound;
            }
        }

        private void PlayToggleClick(bool arg0)
        {
            showInPlayMode.gameObject.SetActive(arg0);
            GlobalStates.SetIsPlayMode(arg0);
        }

        private void ButtonToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.AddButton);
                activeTool = ToolEnum.AddButton;
            }
        }

        private void ImageToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.AddImage);
                activeTool = ToolEnum.AddImage;
            }
        }

        private void ModelToggleActive(bool isOn)
        {
            if (isOn)
            {
                OnToolChanged?.Invoke(activeTool, ToolEnum.AddModel);
                activeTool = ToolEnum.AddModel;
            }
        }

        private void SettingButtonClick()
        {
            SettingButtonClicked?.Invoke();
        }

        public void SetDefaultTool()
        {
            moveToggle.isOn = true;
        }

        private void ApplyGlobalStates()
        {
            ApplyMode(GlobalStates.GetMode());
            GlobalStates.OnModeChange += (value) =>
            {
                ApplyMode(value);
            };
            screenshotButton.gameObject.SetActive(GlobalStates.IsEnableScreenshot());
            GlobalStates.OnEnableScreenshotChange += (bool value) =>
            {
                screenshotButton.gameObject.SetActive(value);
            };
        }

        private void ApplyMode(GlobalStates.Mode mode)
        {
            switch (mode)
            {
                case GlobalStates.Mode.ViewModel:
                    gameObject.SetActive(false);
                    hideForModelDetail.ForEach(gameObject => gameObject.SetActive(false));
                    break;
                case GlobalStates.Mode.EditModel:
                    hideForModelDetail.ForEach(gameObject => gameObject.SetActive(false));
                    gameObject.SetActive(true);
                    break;
                case GlobalStates.Mode.EditARModule:
                    gameObject.SetActive(true);
                    hideForModelDetail.ForEach(gameObject => gameObject.SetActive(true));
                    break;
            }
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
