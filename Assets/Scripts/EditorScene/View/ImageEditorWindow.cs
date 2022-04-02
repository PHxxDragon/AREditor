using System;
using UnityEngine;
using TMPro;
using EAR.AssetManager;
using System.Collections.Generic;

namespace EAR.View
{
    public class ImageEditorWindow : MonoBehaviour
    {
        public event Action<string> OnModelAssetSelected;

        [SerializeField]
        private TMP_Dropdown dropdown;

        private List<AssetObject> assets = new List<AssetObject>();

        void Awake()
        {
            dropdown.onValueChanged.AddListener((index) =>
            {
                if (index != 0)
                {
                    OnModelAssetSelected?.Invoke(assets[index - 1].assetsId);
                }
            });
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.IMAGE_TYPE)
                {
                    assets.Add(assetObject);
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                    optionData.text = assetObject.name;
                    List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                    optionDatas.Add(optionData);
                    dropdown.AddOptions(optionDatas);
                }
            };
        }

        void Start()
        {
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
            int index = 0;
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].assetsId == imageData.assetId)
                {
                    index = i + 1;
                }
            }
            dropdown.value = index;
        }
    }
}

