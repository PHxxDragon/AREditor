using System;
using UnityEngine;

namespace EAR.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public event Action<Selectable> OnObjectSelected;
        public event Action<Selectable> OnObjectDeselected;
        public delegate void IsMouseRaycastBlocked(ref bool isBlocked);
        public event IsMouseRaycastBlocked CheckMouseRaycastBlocked;

        private Selectable _currentSelection;

        void Start()
        {
            OnObjectSelected += AddSelectedOutline;
            OnObjectDeselected += RemoveSelectedOutline;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool hasSelection = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                foreach (RaycastHit hit in Physics.RaycastAll(ray))
                {
                    Selectable selectable = hit.transform.GetComponentInParent<Selectable>();
                    if (selectable != null && _currentSelection != selectable)
                    {
                        hasSelection = true;
                        if (_currentSelection != null)
                        {
                            OnObjectDeselected(_currentSelection);
                        }
                        _currentSelection = selectable;
                        OnObjectSelected(_currentSelection);
                        break;
                    }
                }
                bool isBlocked = false;
                CheckMouseRaycastBlocked(ref isBlocked);
                if (!hasSelection && !isBlocked)
                {
                    if (_currentSelection != null)
                    {
                        OnObjectDeselected(_currentSelection);
                        _currentSelection = null;
                    }
                }

            }
        }

        private void ClearSelection()
        {
            if (_currentSelection != null)
            {
                OnObjectDeselected(_currentSelection);
                _currentSelection = null;
            }
        }

        private void AddSelectedOutline(Selectable selectable)
        {
            selectable.AddSelectedOutline();
        }

        private void RemoveSelectedOutline(Selectable selectable)
        {
            selectable.RemoveSelectedOutline();
        }

        void OnDisable()
        {
            ClearSelection();   
        }

        void OnDestroy()
        {
            ClearSelection();
        }
    }
}

