using EAR.Entity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class ButtonEditorWindow : MonoBehaviour
    {
        public event Action<string> OnNameChanged;
        public event Action<string> OnListenerEntityIdChanged;
        public event Action<int, ButtonActionData> OnButtonActionDataChanged;
        public event Action<ButtonActionData> OnButtonActionDataAdded;
        public event Action<int> OnButtonActionDataDelete;
        public event Action OnButtonDelete;

        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private DropdownHelper listenerEntityId;
        [SerializeField]
        private GameObject actionContainer;
        [SerializeField]
        private ButtonActionPanel actionPanelPrefab;
        [SerializeField]
        private Button addButton;
        [SerializeField]
        private Button deleteButton;

        private void Awake()
        {
            listenerEntityId.OnDropdownValueChanged += (value) =>
            {
                OnListenerEntityIdChanged?.Invoke((string)value);
            };
            addButton.onClick.AddListener(() =>
            {
                ButtonActionData buttonActionData = new ButtonActionData();
                OnButtonActionDataAdded?.Invoke(buttonActionData);
                BindButtonActionDataPanel(buttonActionData);
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                OnNameChanged?.Invoke(name);
            });
            deleteButton.onClick.AddListener(() =>
            {
                OnButtonDelete?.Invoke();
            });
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

        private void UpdateEntityList()
        {
            //TODO
            listenerEntityId.ClearData();
            listenerEntityId.AddData(string.Empty, "Choose an entity");
            BaseEntity[] baseEntities = EntityContainer.Instance.GetEntities();
            foreach (BaseEntity baseEntity in baseEntities)
            {
                if (IsEntitySelectable(baseEntity))
                {
                    listenerEntityId.AddData(baseEntity.GetId(), baseEntity.GetEntityName());
                }
            }
        }

        public void PopulateData(ButtonData buttonData)
        {
            UpdateEntityList();
            listenerEntityId.SelectValue(buttonData.activatorEntityId);
            PopulateData(buttonData.actionDatas);
            nameInputField.name = buttonData.name;
        }

        private void PopulateData(List<ButtonActionData> buttonActionDatas)
        {
            foreach (Transform child in actionContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < buttonActionDatas.Count; i++)
            {
                BindButtonActionDataPanel(buttonActionDatas[i]);
            }
        }

        private void BindButtonActionDataPanel(ButtonActionData buttonActionData)
        {
            ButtonActionPanel buttonActionPanel = Instantiate(actionPanelPrefab, actionContainer.transform);
            buttonActionPanel.PopulateData(buttonActionData);
            buttonActionPanel.OnButtonActionChanged += (buttonActionData) =>
            {
                OnButtonActionDataChanged?.Invoke(buttonActionPanel.transform.GetSiblingIndex(), buttonActionData);
            };
            buttonActionPanel.OnDelete += () =>
            {
                OnButtonActionDataDelete?.Invoke(buttonActionPanel.transform.GetSiblingIndex());
                Destroy(buttonActionPanel.gameObject);
            };
        }

        private bool IsEntitySelectable(BaseEntity baseEntity)
        {
            return baseEntity.IsValidEntity() && baseEntity.IsClickable();
        }
    }
}

