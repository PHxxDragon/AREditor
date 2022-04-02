using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class ButtonActionFactory
    {
        public ButtonAction CreateButtonAction(ButtonActionData buttonActionData)
        {
            switch(buttonActionData.actionType)
            {
                case ButtonActionData.ActionType.Show:
                    return null;
                case ButtonActionData.ActionType.Hide:
                    return null;
                case ButtonActionData.ActionType.PlayAnimation:
                    return null;
                default:
                    return null;
            }
        }
    }
}
