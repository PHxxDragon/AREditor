using System.Collections.Generic;
using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class PlaySoundAction : ButtonAction
    {
        public PlaySoundAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            try
            {
                SoundEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as SoundEntity;
                if (entity && entity.IsValidEntity())
                {
                    entity.PlaySound();
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
            buttonActionData.actionType = ButtonActionData.ActionType.PlaySound;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

