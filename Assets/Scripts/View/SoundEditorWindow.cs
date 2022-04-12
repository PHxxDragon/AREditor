using UnityEngine;
using UnityEngine.UI;
using EAR.Container;
using System;
using TMPro;

namespace EAR.View
{
    public class SoundEditorWindow : MonoBehaviour
    {
        public event Action<SoundData> OnSoundChanged;
        public event Action OnDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
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
                SoundData soundData = new SoundData();
                soundData.assetId = (string)obj;
                OnSoundChanged?.Invoke(soundData);
            };
            playAtStartToggle.onValueChanged.AddListener((value) =>
            {
                SoundData soundData = new SoundData();
                soundData.playAtStart = value;
                OnSoundChanged?.Invoke(soundData);
            });
            loopToggle.onValueChanged.AddListener((value) =>
            {
                SoundData soundData = new SoundData();
                soundData.loop = value;
                OnSoundChanged?.Invoke(soundData);
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                SoundData soundData = new SoundData();
                soundData.name = name;
                OnSoundChanged?.Invoke(soundData);
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
            assetDropdown.SelectValue(soundData.assetId);
            if (soundData.playAtStart.HasValue)
            {
                playAtStartToggle.isOn = soundData.playAtStart.Value;
            }
            
            if (soundData.loop.HasValue)
            {
                loopToggle.isOn = soundData.loop.Value;
            }
            
            nameInputField.text = soundData.name;
        }
    }
}

