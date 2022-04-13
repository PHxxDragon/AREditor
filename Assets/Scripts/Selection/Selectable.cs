using UnityEngine;
using System;

namespace EAR.Selection
{
    //[RequireComponent(typeof(Outline))]
    public class Selectable : MonoBehaviour
    {
        public event Action DestroyEvent;
/*        private readonly Color outlineColor = Color.yellow;
        private readonly float outlineWidth = 6f;*/

        //private Outline outline;

        public void AddSelectedOutline()
        {
            //outline.enabled = true;
        }

        public void RemoveSelectedOutline()
        {
            //outline.enabled = false;
        }

        void Awake()
        {
            InitializeOutlineComponent();
        }

        void OnDestroy()
        {
            DestroyEvent?.Invoke();
        }

        private void InitializeOutlineComponent()
        {
            /*outline = GetComponent<Outline>();
            outline.OutlineWidth = outlineWidth;
            outline.OutlineColor = outlineColor;
            outline.enabled = false;*/
        }
    }
}

