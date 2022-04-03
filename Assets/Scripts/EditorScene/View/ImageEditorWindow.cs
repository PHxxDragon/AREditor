using System;
using UnityEngine;
using TMPro;
using EAR.AssetManager;
using System.Collections.Generic;

namespace EAR.View
{
    public class ImageEditorWindow : MonoBehaviour
    {
        public event Action<string> OnImageAssetSelected;

        [SerializeField]
        private DropdownHelper dropdown;

        void Awake()
        {
            dropdown.OnDropdownValueChanged += (value) =>
            {
                OnImageAssetSelected?.Invoke((string) value);
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

