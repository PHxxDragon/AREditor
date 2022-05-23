using UnityEngine;
using UnityEngine.UI;
using EAR.AnimationPlayer;
using System;
using EAR.Entity;
using EAR.Container;
using EAR.Localization;

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
        private GameObject animationIndexContainer;
        [SerializeField]
        private GameObject actionTypeContainer;

        void Awake()
        {
            actionTypeDropdown.OnDropdownValueChanged += (value) =>
            {
                UpdateAnimationList();
                OnButtonActionChanged?.Invoke(GetData());
            };

            entityIdDropdown.OnDropdownValueChanged += (value) =>
            {
                UpdateActionList();
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

        private void AddType(ButtonActionData.ActionType actionType)
        {
            actionTypeDropdown.AddData(actionType.ToString(), ActionTypeToString(actionType));
        }

        private void UpdateActionList()
        {
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(entityIdDropdown.GetSelectedValue());
            if (baseEntity)
            {
                actionTypeContainer.gameObject.SetActive(true);
                actionTypeDropdown.ClearData();
                if (baseEntity is ModelEntity modelEntity) 
                {
                    AddType(ButtonActionData.ActionType.Show);
                    AddType(ButtonActionData.ActionType.Hide);
                    if (modelEntity.GetComponentInChildren<AnimPlayer>() != null)
                    {
                        AddType(ButtonActionData.ActionType.PlayAnimation);
                    }
                } else if (baseEntity is NoteEntity)
                {
                    AddType(ButtonActionData.ActionType.Show);
                    AddType(ButtonActionData.ActionType.Hide);
                } else if (baseEntity is SoundEntity)
                {
                    AddType(ButtonActionData.ActionType.PlaySound);
                    AddType(ButtonActionData.ActionType.StopSound);
                } else if (baseEntity is VideoEntity)
                {
                    AddType(ButtonActionData.ActionType.Show);
                    AddType(ButtonActionData.ActionType.Hide);
                    AddType(ButtonActionData.ActionType.PlayVideo);
                    AddType(ButtonActionData.ActionType.StopVideo);
                } else if (baseEntity is ButtonEntity)
                {
                    Debug.LogError("No action for button");
                } else if (baseEntity is ImageEntity)
                {
                    AddType(ButtonActionData.ActionType.Show);
                    AddType(ButtonActionData.ActionType.Hide);
                }
            } else
            {
                actionTypeContainer.gameObject.SetActive(false);
            }
            
        }

        private string ActionTypeToString(ButtonActionData.ActionType actionType)
        {
            return LocalizationManager.GetLocalizedText(actionType.ToString());
        }

        private void UpdateEntityList()
        {
            entityIdDropdown.ClearData();
            entityIdDropdown.AddData(string.Empty, LocalizationManager.GetLocalizedText("ChooseEntity"));
            BaseEntity[] entities = EntityContainer.Instance.GetEntities();
            foreach (BaseEntity baseEntity in entities)
            {
                if (!(baseEntity is ButtonEntity))
                {
                    entityIdDropdown.AddData(baseEntity.GetId(), baseEntity.GetEntityName());
                }
            }
        }

        private void UpdateAnimationList()
        {
            string value = actionTypeDropdown.GetSelectedValue();
            if (!string.IsNullOrEmpty(value))
            {
                ButtonActionData.ActionType actionType = (ButtonActionData.ActionType)Enum.Parse(typeof(ButtonActionData.ActionType), value);
                if (actionType == ButtonActionData.ActionType.PlayAnimation)
                {
                    ModelEntity modelEntity = EntityContainer.Instance.GetEntity(entityIdDropdown.GetSelectedValue()) as ModelEntity;
                    if (modelEntity)
                    {
                        AnimPlayer animPlayer = modelEntity.GetComponentInChildren<AnimPlayer>();
                        if (animPlayer)
                        {
                            animationIndexContainer.gameObject.SetActive(true);
                            animationIndexDropdown.ClearData();
                            for (int i = 0; i < animPlayer.GetAnimationCount(); i++)
                            {
                                animationIndexDropdown.AddData(i.ToString(), animPlayer.GetAnimationList()[i]);
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
                }
                else
                {
                    animationIndexContainer.gameObject.SetActive(false);
                }
            } else
            {
                animationIndexContainer.gameObject.SetActive(false);
            }
        }

        private ButtonActionData GetData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = (ButtonActionData.ActionType)Enum.Parse(typeof(ButtonActionData.ActionType), actionTypeDropdown.GetSelectedValue());
            buttonActionData.targetEntityId = entityIdDropdown.GetSelectedValue();
            if (animationIndexContainer.activeSelf)
                buttonActionData.animationIndex = int.Parse(animationIndexDropdown.GetSelectedValue());
            return buttonActionData;
        }

        public void PopulateData(ButtonActionData buttonActionData)
        {
            UpdateEntityList();
            if (!string.IsNullOrEmpty(buttonActionData.targetEntityId))
            {
                entityIdDropdown.SelectValue(buttonActionData.targetEntityId);
            }
            else
            {
                entityIdDropdown.SelectValue(string.Empty);
            }

            UpdateActionList();
            actionTypeDropdown.SelectValue(buttonActionData.actionType.ToString());

            UpdateAnimationList();
            animationIndexDropdown.SelectValue(buttonActionData.animationIndex.ToString());
        }
    }
}

