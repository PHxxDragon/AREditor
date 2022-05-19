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

        private const float clickDuration = 0.2f;

        private bool mouseDown;
        private float remainTime;

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
                case GlobalStates.Mode.ViewARModule:
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
                mouseDown = true;
                remainTime = clickDuration;
            }

            if (mouseDown)
            {
                remainTime -= Time.deltaTime;

                if (remainTime < 0)
                {
                    mouseDown = false;
                    remainTime = 0;
                } else if (Input.GetMouseButtonUp(0))
                {
                    mouseDown = false;
                    remainTime = 0;
                    HandleClick();
                }
            }
        }

        private void HandleClick()
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

            selectables.Add(null);

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

            if (selectableWithMinLastIndex != _currentSelection)
            {
                selectionHistory.Add(selectableWithMinLastIndex);

                if (selectionHistory.Count > 40)
                {
                    selectionHistory.RemoveRange(0, 20);
                }

                if (selectableWithMinLastIndex)
                {
                    SelectObject(selectableWithMinLastIndex);
                } else
                {
                    DeselectObject();
                }
            }
        }

        public void SelectObject(Selectable selectable)
        {
            if (selectable == _currentSelection)
                return;

            DeselectObject();

            if (selectable)
            {
                _currentSelection = selectable;
                _currentSelection.DestroyEvent += DeselectObject;
                OnObjectSelected?.Invoke(_currentSelection);
            }
        }

        public void DeselectObject(Selectable selectable)
        {
            if (selectable == _currentSelection)
            {
                DeselectObject();
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

