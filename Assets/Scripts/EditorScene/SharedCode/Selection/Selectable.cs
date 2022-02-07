using UnityEngine;

namespace EAR.Selection
{
    [RequireComponent(typeof(Outline))]
    public class Selectable : MonoBehaviour
    {
        private readonly Color outlineColor = Color.yellow;
        private readonly float outlineWidth = 6f;

        private Outline outline;

        public void AddSelectedOutline()
        {
            outline.enabled = true;
        }

        public void RemoveSelectedOutline()
        {
            outline.enabled = false;
        }

        void Awake()
        {
            InitializeOutlineComponent();
        }

        private void InitializeOutlineComponent()
        {
            outline = GetComponent<Outline>();
            outline.OutlineWidth = outlineWidth;
            outline.OutlineColor = outlineColor;
            outline.enabled = false;
        }
    }
}

