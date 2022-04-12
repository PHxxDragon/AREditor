using System.Collections.Generic;
using EAR.Entity.EntityAction;
using EAR.Container;
using UnityEngine;

namespace EAR.Entity
{
    public class ButtonEntity : InvisibleEntity
    {
        private static int count = 1;
        private string activatorEntityId = "";
        public readonly List<ButtonAction> actions = new List<ButtonAction>();

        protected override string GetDefaultName()
        {
            return "New button " + count++;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(activatorEntityId);
            if (baseEntity && baseEntity.IsClickable())
            {
                EntityClickTarget entityClickListener = baseEntity.gameObject.AddComponent<EntityClickTarget>();
                entityClickListener.OnEntityClicked += ActivateButton;
            }
        }

        public override void ResetEntityState()
        {
            base.ResetEntityState();
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(activatorEntityId);
            if (baseEntity && baseEntity.IsClickable())
            {
                EntityClickTarget entityClickListener = baseEntity.gameObject.GetComponent<EntityClickTarget>();
                Destroy(entityClickListener);
            }
        }

        public ButtonData GetButtonData()
        {
            ButtonData buttonData = new ButtonData();
            buttonData.transform = TransformData.TransformToTransformData(transform);
            buttonData.name = GetEntityName();
            buttonData.id = GetId();
            buttonData.activatorEntityId = activatorEntityId;
            buttonData.actionDatas = new List<ButtonActionData>();
            foreach (ButtonAction buttonAction in actions)
            {
                buttonData.actionDatas.Add(buttonAction.GetButtonActionData());
            }
            return buttonData;
        }

        public void PopulateData(ButtonData buttonData)
        {
            if (!string.IsNullOrEmpty(buttonData.id))
            {
                SetId(buttonData.id);
            }

            if (!string.IsNullOrEmpty(buttonData.name))
            {
                SetEntityName(buttonData.name);
            }

            if (buttonData.activatorEntityId != null)
            {
                SetActivatorEntityId(buttonData.activatorEntityId);
            }

            if (buttonData.transform != null)
            {
                TransformData.TransformDataToTransfrom(buttonData.transform, transform);
            }

            if (buttonData.actionDatas != null)
            {
                foreach (ButtonActionData buttonActionData in buttonData.actionDatas)
                {
                    actions.Add(ButtonActionFactory.CreateButtonAction(buttonActionData));
                }
            }
            
        }

        public static ButtonEntity InstantNewEntity(ButtonData buttonData)
        {
            ButtonEntity buttonPrefab = AssetContainer.Instance.GetButtonPrefab();
            ButtonEntity buttonEntity = Instantiate(buttonPrefab);
            buttonEntity.PopulateData(buttonData);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
