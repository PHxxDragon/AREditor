using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public event Action<Selectable> OnObjectSelected;
        public event Action<Selectable> OnObjectDeselected;
        public event Action<IUndoRedoCommand> NewCommandEvent;

        private Selectable _currentSelection;
        private List<Selectable> selectionHistory = new List<Selectable>();

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
                RaycastHit[] hits = Physics.RaycastAll(ray);
                HashSet<Selectable> selectables = new HashSet<Selectable>();
                foreach (RaycastHit hit in hits)
                {
                    Selectable selectable = hit.transform.GetComponentInParent<Selectable>();
                    if (selectable) 
                    {
                        selectables.Add(selectable);
                    }
                }

                int minLastIndex = int.MaxValue;
                Selectable selectableWithMinLastIndex = null;
                foreach (Selectable selectable in selectables)
                {
                    int currentLastIndex = selectionHistory.LastIndexOf(selectable);
                    if (minLastIndex > currentLastIndex)
                    {
                        minLastIndex = currentLastIndex;
                        selectableWithMinLastIndex = selectable;
                    }
                    if (minLastIndex == -1)
                    {
                        break;
                    }
                }

                if (selectableWithMinLastIndex && selectableWithMinLastIndex != _currentSelection)
                {
                    selectionHistory.Add(selectableWithMinLastIndex);
                    SelectObject(selectableWithMinLastIndex);
                    IUndoRedoCommand command = new SelectCommand(selectableWithMinLastIndex, SelectObject, DeselectObject);
                    NewCommandEvent?.Invoke(command);
                }

                if (selectables.Count == 0)
                {
                    selectionHistory.Clear();
                    DeselectWithUndo();
                }
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

