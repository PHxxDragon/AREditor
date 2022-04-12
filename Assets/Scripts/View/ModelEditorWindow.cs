using UnityEngine;
using UnityEngine.UI;
using EAR.Container;
using EAR.AnimationPlayer;
using System;
using System.Collections.Generic;
using TMPro;

namespace EAR.View
{
    public class ModelEditorWindow : MonoBehaviour
    {
        public event Action<ModelData> OnModelChanged;
        public event Action OnModelDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private Toggle isVisible;
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
                ModelData modelData = new ModelData();
                modelData.assetId = (string)obj;
                OnModelChanged?.Invoke(modelData);
                UpdateAnimationDropdown((string)obj);
            };
            animationDropdown.OnDropdownValueChanged += (obj) =>
            {
                ModelData modelData = new ModelData();
                modelData.defaultAnimation = (int)obj;
                OnModelChanged?.Invoke(modelData);
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnModelDelete?.Invoke();
            });

            assetDropdown.ClearData();
            assetDropdown.AddData(string.Empty, "Choose model asset");

            if (AssetContainer.Instance)
            {
                SetAssetListener(AssetContainer.Instance);
            } else
            {
                AssetContainer.OnInstanceCreated += SetAssetListener;
            }

            nameInputField.onValueChanged.AddListener((name) =>
            {
                ModelData modelData = new ModelData();
                modelData.name = name;
                OnModelChanged?.Invoke(modelData);
            });
            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                ModelData modelData = new ModelData();
                modelData.isVisible = isVisible;
                OnModelChanged?.Invoke(modelData);
            });
            CloseEditor();
        }

        private void SetAssetListener(AssetContainer instance)
        {
            instance.OnAssetObjectAdded += (AssetObject assetObject) =>
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
                {
                    assetDropdown.AddData(assetObject.assetId, assetObject.name);
                }
            };
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
            if (modelData.assetId != null)
            {
                assetDropdown.SelectValue(modelData.assetId);
                UpdateAnimationDropdown(modelData.assetId);
            }

            if (modelData.defaultAnimation.HasValue)
            {
                animationDropdown.SelectValue(modelData.defaultAnimation.Value);
            }

            if (!string.IsNullOrEmpty(modelData.name)) {
                nameInputField.text = modelData.name;
            }
            
            if (modelData.isVisible.HasValue)
            {
                isVisible.isOn = modelData.isVisible.Value;
            }
        }
    }
}

