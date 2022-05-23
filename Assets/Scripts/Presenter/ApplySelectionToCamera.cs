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
        [SerializeField]
        private GameObject container;

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
                switch (GlobalStates.GetMode())
                {
                    case GlobalStates.Mode.EditARModule:
                        if (current)
                        {
                            Bounds bounds = Utils.GetUIBounds(current);
                            if (bounds == new Bounds())
                            {
                                bounds = Utils.GetModelBounds(current);
                            }
                            cameraController.SetFocus(bounds);
                        }
                        else
                        {
                            cameraController.SetFocus();
                        }
                        break;
                    default:
                        Bounds bounds1 = Utils.GetEntityBounds(container);
                        cameraController.SetFocus(bounds1);
                        break;
                }
                

            };
        }
    }
}

