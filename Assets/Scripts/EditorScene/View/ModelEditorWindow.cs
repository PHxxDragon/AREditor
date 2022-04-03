using UnityEngine;
using UnityEngine.UI;
using EAR.AssetManager;
using EAR.AnimationPlayer;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class ModelEditorWindow : MonoBehaviour
    {
        public event Action<string> OnModelAssetSelected;
        public event Action<int> OnDefaultAnimationSelected;
        public event Action OnModelDelete;

        [SerializeField]
        private DropdownHelper assetDropdown;
        [SerializeField]
        private GameObject animationPanel;
        [SerializeField]
        private DropdownHelper animationDropdown;
        [SerializeField]
        private Button deleteButton;

        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (obj) =>
            {
                OnModelAssetSelected?.Invoke((string)obj);
                UpdateAnimationDropdown((string)obj);
            };
            animationDropdown.OnDropdownValueChanged += (obj) =>
            {
                OnDefaultAnimationSelected?.Invoke((int)obj);
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnModelDelete?.Invoke();
            });
        }

        void Start()
        {
            //TODO
            assetDropdown.ClearData();
            assetDropdown.AddData(string.Empty, "Choose model asset");
            AssetContainer.Instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
                {
                    assetDropdown.AddData(assetObject.assetId, assetObject.name);
                }
            };
            CloseEditor();
        }

        private void UpdateAnimationDropdown(string assetId)
        {
            animationDropdown.ClearData();
            if (string.IsNullOrEmpty(assetId))
            {
                animationPanel.gameObject.SetActive(false);
            } else
            {
                AnimPlayer anim = AssetContainer.Instance.GetModel(assetId).GetComponent<AnimPlayer>();
                if (!anim)
                {
                    animationPanel.gameObject.SetActive(false);
                } else
                {
                    animationPanel.gameObject.SetActive(true);
                    List<string> animationList = anim.GetAnimationList();
                    for (int i = 0; i < animationList.Count; i++)
                    {
                        animationDropdown.AddData(i, animationList[i]);
                    }
                }
            }
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
                assetDropdown.SelectValue(string.Empty);
            } else
            {
                assetDropdown.SelectValue(modelData.assetId);
            }

            UpdateAnimationDropdown(modelData.assetId);
            animationDropdown.SelectValue(modelData.defaultAnimation);
        }
    }
}

