using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.AssetManager;

namespace EAR.View
{
    public class ImageEditorWindow : MonoBehaviour
    {
        public event Action<string> OnImageAssetSelected;
        public event Action OnImageDelete;

        [SerializeField]
        private DropdownHelper dropdown;
        [SerializeField]
        private Button deleteButton;

        void Awake()
        {
            dropdown.OnDropdownValueChanged += (value) =>
            {
                OnImageAssetSelected?.Invoke((string) value);
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnImageDelete?.Invoke();
            });
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
            if (string.IsNullOrEmpty(imageData.assetId))
            {
                dropdown.SelectValue(string.Empty);
            } else
            {
                dropdown.SelectValue(imageData.assetId);
            }
        }
    }
}

