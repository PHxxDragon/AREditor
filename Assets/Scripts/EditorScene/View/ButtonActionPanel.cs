using UnityEngine;
using UnityEngine.UI;
using System;
using EAR.Entity;

namespace EAR.View
{
    public class ButtonActionPanel : MonoBehaviour
    {
        public event Action<ButtonActionData> OnButtonActionChanged;
        public event Action OnDelete;

        [SerializeField]
        private DropdownHelper actionTypeDropdown;
        [SerializeField]
        private DropdownHelper entityIdDropdown;
        [SerializeField]
        private Button deleteButton;

        void Awake()
        {
            actionTypeDropdown.OnDropdownValueChanged += (value) =>
            {
                UpdateEntityList();
                OnButtonActionChanged?.Invoke(GetData());
            };

            entityIdDropdown.OnDropdownValueChanged += (value) =>
            {
                OnButtonActionChanged?.Invoke(GetData());
            };
            deleteButton.onClick.AddListener(() =>
            {
                OnDelete?.Invoke();
            });
        }

        private void UpdateActionList()
        {
            actionTypeDropdown.ClearData();
            foreach (ButtonActionData.ActionType type in Enum.GetValues(typeof(ButtonActionData.ActionType)))
            {
                actionTypeDropdown.AddData(type, type.ToString());
            }
        }

        private void UpdateEntityList()
        {
            ButtonActionData.ActionType actionType = (ButtonActionData.ActionType) actionTypeDropdown.GetSelectedValue();
            switch(actionType)
            {
                case ButtonActionData.ActionType.Hide:
                case ButtonActionData.ActionType.Show:
                    BaseEntity[] entities = EntityContainer.Instance.GetEntities();
                    PopulateDropdown(actionType, entities);
                    break;
            }
        }

        private void PopulateDropdown(ButtonActionData.ActionType actionType, BaseEntity[] entities)
        {
            entityIdDropdown.ClearData();
            entityIdDropdown.AddData(string.Empty, "Select an entity");

            foreach (BaseEntity baseEntity in entities)
            {
                if (IsEntitySelectable(actionType, baseEntity))
                {
                    entityIdDropdown.AddData(baseEntity.GetId(), baseEntity.GetEntityName());
                }
            }
        } 

        private bool IsEntitySelectable(ButtonActionData.ActionType actionType, BaseEntity baseEntity)
        {
            switch (actionType)
            {
                case ButtonActionData.ActionType.Show:
                case ButtonActionData.ActionType.Hide:
                    return baseEntity.IsViewable();
                default:
                    return false;
            }
        }

        private ButtonActionData GetData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = (ButtonActionData.ActionType) actionTypeDropdown.GetSelectedValue();
            buttonActionData.targetEntityId = (string) entityIdDropdown.GetSelectedValue();
            return buttonActionData;
        }

        public void PopulateData(ButtonActionData buttonActionData)
        {
            UpdateActionList();
            actionTypeDropdown.SelectValue(buttonActionData.actionType);

            UpdateEntityList();
            if (!string.IsNullOrEmpty(buttonActionData.targetEntityId))
            {
                entityIdDropdown.SelectValue(buttonActionData.targetEntityId);
            } else
            {
                entityIdDropdown.SelectValue(string.Empty);
            }
        }
    }
}

