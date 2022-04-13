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
        public event Action OnInteractionEnded;
        public event Action OnModelDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TransformInput transformInput;
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

        private bool isPopulating;

        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (obj) =>
            {
                UpdateAnimationDropdown((string)obj);
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.assetId = (string)obj;
                OnModelChanged?.Invoke(modelData);
                OnInteractionEnded?.Invoke();
            };
            animationDropdown.OnDropdownValueChanged += (obj) =>
            {
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.defaultAnimation = (int)obj;
                OnModelChanged?.Invoke(modelData);
                OnInteractionEnded?.Invoke();
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
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.name = name;
                OnModelChanged?.Invoke(modelData);
            });
            nameInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());

            isVisible.onValueChanged.AddListener((isVisible) =>
            {
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.isVisible = isVisible;
                OnModelChanged?.Invoke(modelData);
                OnInteractionEnded?.Invoke();
            });

            transformInput.OnTransformChanged += (TransformData data) =>
            {
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.transform = data;
                OnModelChanged?.Invoke(modelData);
            };
            transformInput.OnInteractionEnded += () => OnInteractionEnded?.Invoke();

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
            isPopulating = true;
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

            if (modelData.transform != null)
            {
                transformInput.SetValue(modelData.transform);
            }
            isPopulating = false;
        }
    }
}

