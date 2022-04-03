using UnityEngine;
using UnityEngine.UI;
using EAR.AnimationPlayer;
using System;
using System.Collections.Generic;
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
        private DropdownHelper animationIndexDropdown;
        [SerializeField]
        private Button deleteButton;

        [SerializeField]
        private GameObject entityDropdownContainer;
        [SerializeField]
        private GameObject animationIndexContainer;

        void Awake()
        {
            actionTypeDropdown.OnDropdownValueChanged += (value) =>
            {
                UpdateEntityList();
                UpdateAnimationList();
                OnButtonActionChanged?.Invoke(GetData());
            };

            entityIdDropdown.OnDropdownValueChanged += (value) =>
            {
                UpdateAnimationList();
                OnButtonActionChanged?.Invoke(GetData());
            };
            animationIndexDropdown.OnDropdownValueChanged += (value) =>
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
            switch (actionType)
            {
                case ButtonActionData.ActionType.Hide:
                case ButtonActionData.ActionType.Show:
                    entityDropdownContainer.gameObject.SetActive(true);
                    entityIdDropdown.ClearData();
                    entityIdDropdown.AddData(string.Empty, "Select an entity");
                    BaseEntity[] entities = EntityContainer.Instance.GetEntities();
                    foreach (BaseEntity baseEntity in entities)
                    {
                        if (IsEntitySelectable(actionType, baseEntity))
                        {
                            entityIdDropdown.AddData(baseEntity.GetId(), baseEntity.GetEntityName());
                        }
                    }
                    break;
                case ButtonActionData.ActionType.PlayAnimation:
                    entityDropdownContainer.gameObject.SetActive(true);
                    entityIdDropdown.ClearData();
                    entityIdDropdown.AddData(string.Empty, "Select an entity");
                    entities = EntityContainer.Instance.GetEntities();
                    foreach (BaseEntity baseEntity in entities)
                    {
                        ModelEntity modelEntity = baseEntity as ModelEntity;
                        if (modelEntity && IsEntitySelectable(actionType, modelEntity))
                        {
                            entityIdDropdown.AddData(modelEntity.GetId(), modelEntity.GetEntityName());
                        }
                    }
                    break;
/*                case ButtonActionData.ActionType.PlaySound:
                    entityDropdownContainer.gameObject.SetActive(false);
                    break;*/
            }
        }

        private void UpdateAnimationList()
        {
            ButtonActionData.ActionType actionType = (ButtonActionData.ActionType)actionTypeDropdown.GetSelectedValue();
            if (actionType != ButtonActionData.ActionType.PlayAnimation)
            {
                animationIndexContainer.gameObject.SetActive(false);
            } else
            {
                try
                {
                    ModelEntity modelEntity = EntityContainer.Instance.GetEntity((string)entityIdDropdown.GetSelectedValue()) as ModelEntity;
                    if (modelEntity)
                    {
                        AnimPlayer animPlayer = modelEntity.GetComponentInChildren<AnimPlayer>();
                        if (animPlayer)
                        {
                            animationIndexContainer.gameObject.SetActive(true);
                            animationIndexDropdown.ClearData();
                            for (int i = 0; i < animPlayer.GetAnimationCount(); i++)
                            {
                                animationIndexDropdown.AddData(i, animPlayer.GetAnimationList()[i]);
                            }
                        }
                        else
                        {
                            animationIndexContainer.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        animationIndexContainer.gameObject.SetActive(false);
                    }
                } catch (KeyNotFoundException)
                {
                    animationIndexContainer.gameObject.SetActive(false);
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
                case ButtonActionData.ActionType.PlayAnimation:
                    ModelEntity modelEntity = baseEntity as ModelEntity;
                    if (modelEntity)
                    {
                        return modelEntity.GetComponentInChildren<AnimPlayer>() != null;
                    }
                    return false;
                default:
                    return false;
            }
        }

        private ButtonActionData GetData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = (ButtonActionData.ActionType) actionTypeDropdown.GetSelectedValue();
            if (entityDropdownContainer.activeSelf)
                buttonActionData.targetEntityId = (string) entityIdDropdown.GetSelectedValue();
            if (animationIndexContainer.activeSelf)
                buttonActionData.animationIndex = (int)animationIndexDropdown.GetSelectedValue();
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

            UpdateAnimationList();
            animationIndexDropdown.SelectValue(buttonActionData.animationIndex);
        }
    }
}

