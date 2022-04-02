using UnityEngine;
using TMPro;
using EAR.AssetManager;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class ModelEditorWindow : MonoBehaviour
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
        }

        void Start()
        {
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
                {
                    assets.Add(assetObject);
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                    optionData.text = assetObject.name;
                    List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                    optionDatas.Add(optionData);
                    dropdown.AddOptions(optionDatas);
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
            int index = 0;
            for (int i = 0; i < assets.Count; i++) { 
                if (assets[i].assetsId == modelData.assetId)
                {
                    index = i + 1;
                }
            }           
            dropdown.value = index;
        }
    }
}

