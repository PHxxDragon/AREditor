using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using EAR.Container;

namespace EAR.View
{
    public class VideoEditorWindow : MonoBehaviour
    {
        public event Action<VideoData> OnVideoChanged;
        public event Action OnInteractionEnded;
        public event Action OnDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TransformInput transformInput;
        [SerializeField]
        private DropdownHelper assetDropdown;
        [SerializeField]
        private Toggle isVisible;
        [SerializeField]
        private Toggle playAtStartToggle;
        [SerializeField]
        private Toggle loopToggle;
        [SerializeField]
        private Button deleteButton;

        private bool isPopulating = false;

        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (value) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.assetId = (string)value;
                OnVideoChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            };

            deleteButton.onClick.AddListener(() =>
            {
                OnDelete?.Invoke();
            });

            nameInputField.onValueChanged.AddListener((name) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.name = name;
                OnVideoChanged?.Invoke(data);
            });
            nameInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());

            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.isVisible = isVisible;
                OnVideoChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            });

            transformInput.OnTransformChanged += (transformData) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.transform = transformData;
                OnVideoChanged?.Invoke(data);
            };
            transformInput.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

            playAtStartToggle.onValueChanged.AddListener((value) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.playAtStart = value;
                OnVideoChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            });

            loopToggle.onValueChanged.AddListener((value) =>
            {
                if (isPopulating) return;
                VideoData data = new VideoData();
                data.loop = value;
                OnVideoChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            });

            if (AssetContainer.Instance)
            {
                SetAssetListener(AssetContainer.Instance);
            }
            else
            {
                AssetContainer.OnInstanceCreated += SetAssetListener;
            }

            CloseEditor();
        }

        private void SetAssetListener(AssetContainer instance)
        {
            instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.VIDEO_TYPE)
                {
                    assetDropdown.AddData(assetObject.assetId, assetObject.name);
                }
            };
        }

        public void OpenEditor()
        {
            gameObject.SetActive(true);
        }

        public void CloseEditor()
        {
            gameObject.SetActive(false);
        }

        public void PopulateData(VideoData videoData)
        {
            isPopulating = true;
            if (videoData.assetId != null)
            {
                assetDropdown.SelectValue(videoData.assetId);
            }

            if (!string.IsNullOrEmpty(videoData.name))
            {
                nameInputField.text = videoData.name;
            }

            if (videoData.isVisible.HasValue)
            {
                isVisible.isOn = videoData.isVisible.Value;
            }

            if (videoData.transform != null)
            {
                transformInput.SetValue(videoData.transform);
            }

            if (videoData.playAtStart.HasValue)
            {
                playAtStartToggle.isOn = videoData.playAtStart.Value;
            }

            if (videoData.loop.HasValue)
            {
                loopToggle.isOn = videoData.loop.Value;
            }
            isPopulating = false;
        }
    }
}

