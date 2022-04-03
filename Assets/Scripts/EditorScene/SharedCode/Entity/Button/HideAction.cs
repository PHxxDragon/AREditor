using System.Collections.Generic;
using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class HideAction : ButtonAction
    {
        public HideAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }

        public override void ExecuteAction()
        {
            try
            {
                BaseEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId());
                if (entity.IsViewable())
                {
                    entity.gameObject.SetActive(false);
                }
            } catch (KeyNotFoundException)
            {
                Debug.Log("Key not found");
            }
            
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.Hide;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

