using System.Collections.Generic;
using System;
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
            BaseEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId());
            if (entity.IsValidEntity() && entity.IsViewable())
            {
                entity.gameObject.SetActive(false);
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

