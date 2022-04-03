using System;
using UnityEngine;

namespace EAR.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public event Action<Selectable> OnObjectSelected;
        public event Action<Selectable> OnObjectDeselected;
        public event Action<IUndoRedoCommand> NewCommandEvent;

        private Selectable _currentSelection;

        void Start()
        {
            OnObjectSelected += AddSelectedOutline;
            OnObjectDeselected += RemoveSelectedOutline;

            ApplyGlobalState();
        }

        private void ApplyGlobalState()
        {
            gameObject.SetActive(GlobalStates.IsEnableEditor());
            GlobalStates.OnEnableEditorChange += (bool value) =>
            {
                gameObject.SetActive(value);
            };
        }

        public bool HasSelection()
        {
            return _currentSelection != null;
        }

        void Awake()
        {
            GlobalStates.OnIsPlayModeChange +=(isPlayMode) =>
            {
                if (isPlayMode)
                {
                    DeselectWithUndo();
                }
            };
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !GlobalStates.IsPlayMode() && !GlobalStates.IsMouseRaycastBlocked())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                foreach (RaycastHit hit in Physics.RaycastAll(ray))
                {
                    Selectable selectable = hit.transform.GetComponentInParent<Selectable>();
                    if (selectable != null && _currentSelection != selectable)
                    {
                        SelectObject(selectable);
                        IUndoRedoCommand command = new SelectCommand(selectable, SelectObject, DeselectObject);
                        NewCommandEvent?.Invoke(command);
                        return;
                    }
                }

                DeselectWithUndo();
            }
        }

        private void DeselectWithUndo()
        {
            if (_currentSelection != null)
            {
                IUndoRedoCommand command = new DeselectCommand(_currentSelection, DeselectObject, SelectObject);
                NewCommandEvent?.Invoke(command);
            }
            DeselectObject();
        }

        public IUndoRedoCommand DeselectAndGetCommand()
        {
            if (_currentSelection != null)
            {
                IUndoRedoCommand command = new DeselectCommand(_currentSelection, DeselectObject, SelectObject);
                DeselectObject();
                return command;
            } else
            {
                return null;
            }
            
        }

        private void SelectObject(Selectable selectable)
        {
            if (_currentSelection != null)
            {
                OnObjectDeselected(_currentSelection);
            }
            _currentSelection = selectable;
            OnObjectSelected(_currentSelection);
        }

        private void DeselectObject()
        {
            if (_currentSelection != null)
            {
                OnObjectDeselected(_currentSelection);
                _currentSelection = null;
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

