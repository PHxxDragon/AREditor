using EAR.Entity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EAR.Container;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class ButtonEditorWindow : MonoBehaviour
    {
        public event Action<ButtonData> OnButtonDataChanged;
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

        private List<ButtonActionData> buttonActionDatas;

        private void Awake()
        {
            listenerEntityId.OnDropdownValueChanged += (value) =>
            {
                ButtonData buttonData = new ButtonData();
                buttonData.activatorEntityId = (string)value;
                OnButtonDataChanged?.Invoke(buttonData);
            };
            addButton.onClick.AddListener(() =>
            {
                ButtonActionData buttonActionData = new ButtonActionData();
                buttonActionDatas.Add(buttonActionData);
                BindButtonActionDataPanel(buttonActionData);
                ButtonData buttonData = new ButtonData();
                buttonData.actionDatas = new List<ButtonActionData>(buttonActionDatas);
                OnButtonDataChanged?.Invoke(buttonData);
                OnButtonDataChanged?.Invoke(buttonData);
            });
            nameInputField.onValueChanged.AddListener((name) =>
            {
                ButtonData buttonData = new ButtonData();
                buttonData.name = name;
                OnButtonDataChanged?.Invoke(buttonData);
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
            if (buttonData.activatorEntityId != null)
            {
                listenerEntityId.SelectValue(buttonData.activatorEntityId);
            }
            if (buttonData.actionDatas != null)
            {
                PopulateData(buttonData.actionDatas);
            }
            if (!string.IsNullOrEmpty(buttonData.name))
            {
                nameInputField.text = buttonData.name;
            }
        }

        private void PopulateData(List<ButtonActionData> buttonActionDatas)
        {
            foreach (Transform child in actionContainer.transform)
            {
                Destroy(child.gameObject);
            }

            this.buttonActionDatas = new List<ButtonActionData>(buttonActionDatas);

            for (int i = 0; i < buttonActionDatas.Count; i++)
            {
                BindButtonActionDataPanel(this.buttonActionDatas[i]);
            }

            
        }

        private void BindButtonActionDataPanel(ButtonActionData buttonActionData)
        {
            ButtonActionPanel buttonActionPanel = Instantiate(actionPanelPrefab, actionContainer.transform);
            buttonActionPanel.PopulateData(buttonActionData);
            buttonActionPanel.OnButtonActionChanged += (buttonActionData) =>
            {
                buttonActionDatas[buttonActionPanel.transform.GetSiblingIndex()] = buttonActionData;
                ButtonData buttonData = new ButtonData();
                buttonData.actionDatas = new List<ButtonActionData>(buttonActionDatas);
                OnButtonDataChanged?.Invoke(buttonData);
            };
            buttonActionPanel.OnDelete += () =>
            {
                buttonActionDatas.RemoveAt(buttonActionPanel.transform.GetSiblingIndex());
                ButtonData buttonData = new ButtonData();
                buttonData.actionDatas = new List<ButtonActionData>(buttonActionDatas);
                OnButtonDataChanged?.Invoke(buttonData);
                Destroy(buttonActionPanel.gameObject);
            };
        }

        private bool IsEntitySelectable(BaseEntity baseEntity)
        {
            return baseEntity.IsClickable();
        }
    }
}

