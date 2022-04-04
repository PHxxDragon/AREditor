using System.Collections.Generic;
using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class StopSoundAction : ButtonAction
    {
        public StopSoundAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            Debug.Log("Show action execute");
            try
            {
                SoundEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as SoundEntity;
                if (entity && entity.IsValidEntity())
                {
                    entity.StopSound();
                }
            }
            catch (KeyNotFoundException)
            {
                Debug.Log("Key not found");
            }

        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.StopSound;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

