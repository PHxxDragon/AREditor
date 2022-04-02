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
            try
            {
                BaseEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId());
                if (entity.IsViewable())
                {
                    entity.enabled = true;
                }
            } catch (KeyNotFoundException)
            {
                Debug.Log("Key not found");
            }
            
        }
    }
}

