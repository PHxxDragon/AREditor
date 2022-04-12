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

        void Awake()
        {
            dropdown.OnDropdownValueChanged += (value) =>
            {
                ImageData data = new ImageData();
                data.assetId = (string)value;
                OnImageChanged?.Invoke(data);
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnImageDelete?.Invoke();
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                ImageData data = new ImageData();
                data.name = name;
                OnImageChanged?.Invoke(data);
            });
            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                ImageData data = new ImageData();
                data.isVisible = isVisible;
                OnImageChanged?.Invoke(data);
            });
            transformInput.OnTransformChanged += (transformData) =>
            {
                ImageData data = new ImageData();
                data.transform = transformData;
                OnImageChanged?.Invoke(data);
            };

        }

        void Start()
        {
            //TODO
            dropdown.ClearData();
            dropdown.AddData(string.Empty, "Choose image asset");
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.IMAGE_TYPE)
                {
                    dropdown.AddData(assetObject.assetId, assetObject.name);
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

        public void PopulateData(ImageData imageData)
        {
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
        }
    }
}

