using EAR.Selection;
using EAR.EARCamera;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class ApplySelectionToCamera : MonoBehaviour
    {
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private CameraController cameraController;

        private GameObject current;
        void Start()
        {
            selectionManager.OnObjectSelected += (selectable) =>
            {
                current = selectable.gameObject;
            };
            selectionManager.OnObjectDeselected += (selectable) =>
            {
                current = null;
            };

            cameraController.BeforeFocus += () =>
            {
                if (current)
                {
                    Bounds bounds = Utils.GetUIBounds(current);
                    if (bounds == new Bounds())
                    {
                        bounds = Utils.GetModelBounds(current);
                    }
                    cameraController.SetFocus(bounds);
                } else
                {
                    cameraController.SetFocus();
                }

            };
        }
    }
}

