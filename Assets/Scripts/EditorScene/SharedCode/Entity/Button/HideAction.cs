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
                    entity.enabled = false;
                }
            } catch (KeyNotFoundException)
            {
                Debug.Log("Key not found");
            }
            
        }
    }
}

