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
        public event Action OnInteractionEnded;
        public event Action OnDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TransformInput transformInput;
        [SerializeField]
        private DropdownHelper assetDropdown;
        [SerializeField]
        private Toggle playAtStartToggle;
        [SerializeField]
        private Toggle loopToggle;
        [SerializeField]
        private Button deleteButton;

        private bool isPopulating = false;
        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (obj) =>
            {
                if (isPopulating) return;
                SoundData soundData = new SoundData();
                soundData.assetId = (string)obj;
                OnSoundChanged?.Invoke(soundData);
                OnInteractionEnded?.Invoke();
            };
            playAtStartToggle.onValueChanged.AddListener((value) =>
            {
                if (isPopulating) return;
                SoundData soundData = new SoundData();
                soundData.playAtStart = value;
                OnSoundChanged?.Invoke(soundData);
                OnInteractionEnded?.Invoke();
            });
            loopToggle.onValueChanged.AddListener((value) =>
            {
                if (isPopulating) return;
                SoundData soundData = new SoundData();
                soundData.loop = value;
                OnSoundChanged?.Invoke(soundData);
                OnInteractionEnded?.Invoke();
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                if (isPopulating) return;
                SoundData soundData = new SoundData();
                soundData.name = name;
                OnSoundChanged?.Invoke(soundData);
            });
            nameInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());
            transformInput.OnTransformChanged += (value) =>
            {
                if (isPopulating) return;
                SoundData soundData = new SoundData();
                soundData.transform = value;
                OnSoundChanged?.Invoke(soundData);
            };
            transformInput.OnInteractionEnded += () => OnInteractionEnded?.Invoke();
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
            isPopulating = true;
            if (soundData.assetId != null)
            {
                assetDropdown.SelectValue(soundData.assetId);
            }
            if (soundData.playAtStart.HasValue)
            {
                playAtStartToggle.isOn = soundData.playAtStart.Value;
            }
            
            if (soundData.loop.HasValue)
            {
                loopToggle.isOn = soundData.loop.Value;
            }
            if (!string.IsNullOrEmpty(soundData.name))
            {
                nameInputField.text = soundData.name;
            }
            if (soundData.transform != null)
            {
                transformInput.SetValue(soundData.transform);
            }
            isPopulating = false;
        }
    }
}

