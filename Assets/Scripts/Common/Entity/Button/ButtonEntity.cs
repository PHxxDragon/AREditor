using System.Collections.Generic;
using EAR.Entity.EntityAction;
using EAR.Container;
using UnityEngine;

namespace EAR.Entity
{
    public class ButtonEntity : InvisibleEntity
    {
        private static int count = 1;
        private string activatorEntityId;
        public readonly List<ButtonAction> actions = new List<ButtonAction>();

        protected override string GetDefaultName()
        {
            return "New button " + count++;
        }

        protected override void Awake()
        {
            base.Awake();

            GlobalStates.OnIsPlayModeChange += (isPlayMode) =>
            {
                BaseEntity baseEntity = EntityContainer.Instance.GetEntity(activatorEntityId);
                if (baseEntity && baseEntity.IsClickable())
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
                    Debug.Log("Unclickable entity");
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

            if (!string.IsNullOrEmpty(buttonData.name))
            {
                buttonEntity.SetEntityName(buttonData.name);
            }

            if (!string.IsNullOrEmpty(buttonData.activatorEntityId))
            {
                buttonEntity.SetActivatorEntityId(buttonData.activatorEntityId);
            }

            if (buttonData.transform != null)
            {
                TransformData.TransformDataToTransfrom(buttonData.transform, buttonEntity.transform);
            }

            foreach (ButtonActionData buttonActionData in buttonData.actionDatas)
            {
                buttonEntity.actions.Add(ButtonActionFactory.CreateButtonAction(buttonActionData));
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

