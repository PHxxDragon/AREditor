using UnityEngine;
using UnityEngine.UI;
using EAR.AssetManager;
using System;

namespace EAR.View
{
    public class SoundEditorWindow : MonoBehaviour
    {
        public event Action<string> OnSoundAssetSelected;
        public event Action<bool> OnPlayAtStartChanged;
        public event Action<bool> OnLoopChanged;
        public event Action OnDelete;

        [SerializeField]
        private DropdownHelper assetDropdown;
        [SerializeField]
        private Toggle playAtStartToggle;
        [SerializeField]
        private Toggle loopToggle;
        [SerializeField]
        private Button deleteButton;

        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (obj) =>
            {
                OnSoundAssetSelected?.Invoke((string)obj);
            };
            playAtStartToggle.onValueChanged.AddListener((value) =>
            {
                OnPlayAtStartChanged?.Invoke(value);
            });
            loopToggle.onValueChanged.AddListener((value) =>
            {
                OnLoopChanged?.Invoke(value);
            });
            deleteButton.onClick.AddListener(() =>
            {
                OnDelete?.Invoke();
            });
        }

        void Start()
        {
            //TODO
            assetDropdown.ClearData();
            assetDropdown.AddData(string.Empty, "Choose sound asset");
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.SOUND_TYPE)
                {
                    assetDropdown.AddData(assetObject.assetId, assetObject.name);
                }
            };
            CloseEditor();
        }

        public void OpenEditor()
        {
            gameObject.SetActive(true);
        }

        public void CloseEditor()
        {
            gameObject.SetActive(false);
        }

        public void PopulateData(SoundData soundData)
        {
            if (string.IsNullOrEmpty(soundData.assetId))
            {
                assetDropdown.SelectValue(string.Empty);
            }
            else
            {
                assetDropdown.SelectValue(soundData.assetId);
            }

            playAtStartToggle.isOn = soundData.playAtStart;
            loopToggle.isOn = soundData.loop;
        }
    }
}

