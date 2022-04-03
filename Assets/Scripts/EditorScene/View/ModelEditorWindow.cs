using UnityEngine;
using EAR.AssetManager;
using System;

namespace EAR.View
{
    public class ModelEditorWindow : MonoBehaviour
    {
        public event Action<string> OnModelAssetSelected;

        [SerializeField]
        private DropdownHelper dropdown;

        void Awake()
        {
            dropdown.OnDropdownValueChanged += (obj) =>
            {
                OnModelAssetSelected?.Invoke((string)obj);
            };
        }

        void Start()
        {
            //TODO
            dropdown.ClearData();
            dropdown.AddData(string.Empty, "Choose model asset");
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
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

        public void PopulateData(ModelData modelData)
        {
            if (string.IsNullOrEmpty(modelData.assetId))
            {
                dropdown.SelectValue(string.Empty);
            } else
            {
                dropdown.SelectValue(modelData.assetId);
            }
        }
    }
}

