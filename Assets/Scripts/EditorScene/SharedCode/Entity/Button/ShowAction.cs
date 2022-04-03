using System.Collections.Generic;
using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class ShowAction : ButtonAction
    {
        public ShowAction(ButtonActionData buttonActionData): base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            Debug.Log("Show action execute");
            try
            {
                BaseEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId());
                if (entity.IsValidEntity() && entity.IsViewable())
                {
                    entity.gameObject.SetActive(true);
                }
            } catch (KeyNotFoundException)
            {
                Debug.Log("Key not found");
            }
            
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.Show;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

