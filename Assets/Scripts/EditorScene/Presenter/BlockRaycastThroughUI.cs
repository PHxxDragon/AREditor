using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace EAR.Editor.Presenter
{
    public class BlockRaycastThroughUI : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster raycaster;

        private EventSystem eventSystem;


        void Awake()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                Debug.LogError("No Event System!");
            }
            GlobalStates.CheckMouseRaycastBlocked += CheckIfRaycastBlockedByUI;
        }

        private void CheckIfRaycastBlockedByUI(ref bool isBlocked)
        {
            if (eventSystem == null)
            {
                return;
            }

            GraphicRaycaster[] graphicRaycasters = raycaster.GetComponentsInChildren<GraphicRaycaster>();
            foreach (GraphicRaycaster caster in graphicRaycasters)
            {
                PointerEventData pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                caster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    isBlocked = true;
                }
            }
        }
    }
}

