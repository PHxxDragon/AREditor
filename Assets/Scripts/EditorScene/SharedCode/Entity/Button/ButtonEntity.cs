using System.Collections.Generic;
using System;
using EAR.Entity.EntityAction;
using EAR.AssetManager;
using UnityEngine;

namespace EAR.Entity
{
    public class ButtonEntity : BaseEntity
    {
        private string activatorEntityId;
        public readonly List<ButtonAction> actions = new List<ButtonAction>();

        void Awake()
        {
            GlobalStates.OnIsPlayModeChange += (isPlayMode) =>
            {
                try
                {
                    BaseEntity baseEntity = EntityContainer.Instance.GetEntity(activatorEntityId);
                    if (baseEntity.IsClickable())
                    {
                        if (isPlayMode)
                        {
                            EntityClickListener entityClickListener = baseEntity.gameObject.AddComponent<EntityClickListener>();
                            entityClickListener.OnEntityClicked += ActivateButton;
                        } else
                        {
                            EntityClickListener entityClickListener = baseEntity.gameObject.GetComponent<EntityClickListener>();
                            Destroy(entityClickListener);
                        }  
                    } else
                    {
                        Debug.LogError("Unclickable entity");
                    }
                } catch (KeyNotFoundException)
                {
                    Debug.Log("Key not found");
                } catch (ArgumentNullException)
                {
                    Debug.Log("Key null");
                }
            };
        }

        public ButtonData GetButtonData()
        {
            ButtonData buttonData = new ButtonData();
            buttonData.transform = TransformData.TransformToTransformData(transform);
            buttonData.name = GetEntityName();
            buttonData.id = GetId();
            buttonData.activatorEntityId = activatorEntityId;
            foreach (ButtonAction buttonAction in actions)
            {
                buttonData.actionDatas.Add(buttonAction.GetButtonActionData());
            }
            return buttonData;
        }

        public static ButtonEntity InstantNewEntity(ButtonData buttonData)
        {
            ButtonEntity buttonPrefab = AssetContainer.Instance.GetButtonPrefab();
            ButtonEntity buttonEntity = Instantiate(buttonPrefab);

            if (!string.IsNullOrEmpty(buttonData.id))
            {
                buttonEntity.SetId(buttonData.id);
            }
            
            if (!string.IsNullOrEmpty(buttonEntity.name))
            {
                buttonEntity.SetEntityName(buttonData.name);
            }
           
            if (!string.IsNullOrEmpty(buttonEntity.activatorEntityId))
            {
                buttonEntity.SetActivatorEntityId(buttonData.activatorEntityId);
            }

            if (buttonEntity.transform != null)
            {
                TransformData.TransformDataToTransfrom(buttonData.transform, buttonEntity.transform);
            }
            
            OnEntityCreated?.Invoke(buttonEntity);
            return buttonEntity;
        }

        public override bool IsViewable()
        {
            return false;
        }

        public void ActivateButton()
        {
            foreach(ButtonAction action in actions)
            {
                action.ExecuteAction();
            }
        }

        public void SetActivatorEntityId(string entityId)
        {
            activatorEntityId = entityId;
        }

        public string GetActivatorEntityId()
        {
            return activatorEntityId;
        }
    }
}

