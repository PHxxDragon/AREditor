using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public event Action<Selectable> OnObjectSelected;
        public event Action<Selectable> OnObjectDeselected;

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
            ApplyMode(GlobalStates.GetMode());
            GlobalStates.OnModeChange += (value) =>
            {
                ApplyMode(value);
            };
        }

        private void ApplyMode(GlobalStates.Mode mode)
        {
            switch (mode)
            {
                case GlobalStates.Mode.ViewModel:
                    gameObject.SetActive(false);
                    break;
                case GlobalStates.Mode.EditARModule:
                case GlobalStates.Mode.EditModel:
                    gameObject.SetActive(true);
                    break;
            }
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
                    DeselectObject();
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
                }

                if (selectables.Count == 0)
                {
                    selectionHistory.Clear();
                    DeselectObject();
                }
            }
        }

        public void SelectObject(Selectable selectable)
        {
            if (_currentSelection != null)
            {
                OnObjectDeselected?.Invoke(_currentSelection);
            }
            if (selectable)
            {
                _currentSelection = selectable;
                _currentSelection.DestroyEvent += DeselectObject;
                OnObjectSelected?.Invoke(_currentSelection);
            }
        }

        public void DeselectObject()
        {
            if (_currentSelection != null)
            {
                _currentSelection.DestroyEvent -= DeselectObject;
                OnObjectDeselected?.Invoke(_currentSelection);
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
            DeselectObject();   
        }

        void OnDestroy()
        {
            DeselectObject();
        }
    }
}

