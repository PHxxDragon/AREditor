using UnityEngine;
using UnityEngine.UI;
using EAR.Container;
using EAR.AnimationPlayer;
using System;
using System.Collections.Generic;
using System.Collections;
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
        [SerializeField]
        private GameObject[] hideWhenInEditModel;
        [SerializeField]
        private RectTransform[] heightSum;

        private bool isPopulating;
        private bool needResize;

        void Awake()
        {
            assetDropdown.OnDropdownValueChanged += (obj) =>
            {
                UpdateAnimationDropdown(obj);
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.assetId = obj;
                OnModelChanged?.Invoke(modelData);
                OnInteractionEnded?.Invoke();
            };
            animationDropdown.OnDropdownValueChanged += (obj) =>
            {
                if (isPopulating) return;
                ModelData modelData = new ModelData();
                modelData.defaultAnimation = int.Parse(obj);
                OnModelChanged?.Invoke(modelData);
                OnInteractionEnded?.Invoke();
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnModelDelete?.Invoke();
            });

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

            if (AssetContainer.Instance)
            {
                SetAssetListener(AssetContainer.Instance);
            }
            else
            {
                AssetContainer.OnInstanceCreated += SetAssetListener;
            }

            ApplyMode(GlobalStates.GetMode());
            GlobalStates.OnModeChange += ApplyMode;

            CloseEditor();
        }

        void OnEnable()
        {
            if (needResize)
            {
                StartCoroutine(FitWindow());
                needResize = false;
            }
        }

        private void ApplyMode(GlobalStates.Mode mode)
        {
            switch(mode)
            {
                case GlobalStates.Mode.EditARModule:
                case GlobalStates.Mode.ViewARModule:
                    foreach (GameObject gameObject in hideWhenInEditModel)
                    {
                        gameObject.SetActive(true);
                    }
                    if (gameObject.activeSelf) StartCoroutine(FitWindow());
                    else needResize = true;
                    break;
                case GlobalStates.Mode.EditModel:
                case GlobalStates.Mode.ViewModel:
                    foreach (GameObject gameObject in hideWhenInEditModel)
                    {
                        gameObject.SetActive(false);
                    }
                    if (gameObject.activeSelf) StartCoroutine(FitWindow());
                    else needResize = true;
                    break;
            }
        }

        private IEnumerator FitWindow()
        {
            yield return new WaitForEndOfFrame();
            float height = 50;
            foreach (RectTransform rect in heightSum)
            {
                height += rect.rect.height;
            }
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
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
            if (GlobalStates.GetMode() == GlobalStates.Mode.EditModel || GlobalStates.GetMode() == GlobalStates.Mode.ViewModel)
            {
                return;
            }

            animationDropdown.ClearData();
            if (string.IsNullOrEmpty(assetId))
            {
                animationPanel.gameObject.SetActive(false);
            } else
            {
                GameObject model = AssetContainer.Instance.GetModel(assetId);
                if (!model)
                {
                    animationPanel.gameObject.SetActive(false);
                } else
                {
                    AnimPlayer anim = model.GetComponent<AnimPlayer>();
                    if (!anim)
                    {
                        animationPanel.gameObject.SetActive(false);
                    }
                    else
                    {
                        animationPanel.gameObject.SetActive(true);
                        List<string> animationList = anim.GetAnimationList();
                        for (int i = 0; i < animationList.Count; i++)
                        {
                            animationDropdown.AddData(i.ToString(), animationList[i]);
                        }
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
                animationDropdown.SelectValue(modelData.defaultAnimation.Value.ToString());
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

