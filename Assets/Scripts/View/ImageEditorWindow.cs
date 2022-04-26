using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.Container;
using TMPro;

namespace EAR.View
{
    public class ImageEditorWindow : MonoBehaviour
    {
        public event Action<ImageData> OnImageChanged;
        public event Action OnInteractionEnded;
        public event Action OnImageDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TransformInput transformInput;
        [SerializeField]
        private Toggle isVisible;
        [SerializeField]
        private DropdownHelper dropdown;
        [SerializeField]
        private Button deleteButton;

        private bool isPopulating = false;

        void Awake()
        {
            dropdown.OnDropdownValueChanged += (value) =>
            {
                if (isPopulating) return;
                ImageData data = new ImageData();
                data.assetId = (string)value;
                OnImageChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnImageDelete?.Invoke();
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                if (isPopulating) return;
                ImageData data = new ImageData();
                data.name = name;
                OnImageChanged?.Invoke(data);
            });
            nameInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());
            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                if (isPopulating) return;
                ImageData data = new ImageData();
                data.isVisible = isVisible;
                OnImageChanged?.Invoke(data);
                OnInteractionEnded?.Invoke();
            });
            transformInput.OnTransformChanged += (transformData) =>
            {
                if (isPopulating) return;
                ImageData data = new ImageData();
                data.transform = transformData;
                OnImageChanged?.Invoke(data);
            };
            transformInput.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

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
                if (assetObject.type == AssetObject.IMAGE_TYPE)
                {
                    dropdown.AddData(assetObject.assetId, assetObject.name);
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

        public void PopulateData(ImageData imageData)
        {
            isPopulating = true;
            if (imageData.assetId != null)
            {
                dropdown.SelectValue(imageData.assetId);
            }
            
            if (!string.IsNullOrEmpty(imageData.name))
            {
                nameInputField.text = imageData.name;
            }
            
            if (imageData.isVisible.HasValue)
            {
                isVisible.isOn = imageData.isVisible.Value;
            }

            if (imageData.transform != null)
            {
                transformInput.SetValue(imageData.transform);
            }
            isPopulating = false;
        }
    }
}

