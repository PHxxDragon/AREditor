using System.Collections.Generic;
using EAR.Entity.EntityAction;
using EAR.AssetManager;
using UnityEngine;

namespace EAR.Entity
{
    public class ButtonEntity : BaseEntity
    {
        private string activatorEntityId;
        private readonly List<ButtonAction> actions = new List<ButtonAction>();

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
                }
            };
        }

        public static ButtonEntity InstantNewEntity(ButtonData buttonData)
        {
            ButtonEntity buttonPrefab = AssetContainer.Instance.GetButtonPrefab();
            ButtonEntity buttonEntity = Instantiate(buttonPrefab);
            buttonEntity.SetId(buttonData.id);
            buttonEntity.SetEntityName(buttonData.name);
            buttonEntity.SetActivatorEntityId(buttonData.activatorEntityId);
            TransformData.TransformDataToTransfrom(buttonData.transform, buttonEntity.transform);
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

