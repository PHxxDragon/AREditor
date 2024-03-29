﻿using UnityEngine;
using System;
using EAR;

namespace RuntimeHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 21.10.2020
     */
    public class RuntimeTransformHandle : MonoBehaviour
    {
        public event Action<GameObject> OnStartInteraction;
        public event Action<GameObject> OnEndInteraction;

        public static float MOUSE_SENSITIVITY = 0.5f;
        
        public HandleAxes axes = HandleAxes.XYZ;
        public HandleSpace space = HandleSpace.LOCAL;
        public HandleType type = HandleType.POSITION;
        public HandleSnappingType snappingType = HandleSnappingType.RELATIVE;

        public Vector3 positionSnap = Vector3.zero;
        public float rotationSnap = 0;
        public Vector3 scaleSnap = Vector3.zero;

        public bool autoScale = false;
        public float autoScaleFactor = 1;
        public Camera handleCamera;

        public bool toolEnabled = false;

        public Transform target;

        private Vector3 _previousMousePosition;
        private HandleBase _previousAxis;
        
        private HandleBase _draggingHandle;

        private HandleType _previousType;
        private HandleAxes _previousAxes;
        private Transform _previousTarget;

        private PositionHandle _positionHandle;
        private RotationHandle _rotationHandle;
        private ScaleHandle _scaleHandle;

        public bool CheckIfMouseDraggingHandle()
        {
            HandleBase handle = null;
            Vector3 hitPoint = Vector3.zero;
            GetHandle(ref handle, ref hitPoint);

            return handle != null || _draggingHandle != null;
        }

        void Start()
        {
            if (handleCamera == null)
                handleCamera = Camera.main;

            _previousType = type;

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
                case GlobalStates.Mode.EditModel:
                case GlobalStates.Mode.EditARModule:
                    gameObject.SetActive(true);
                    break;
            }
        }

        void CreateHandles()
        {
            switch (type)
            {
                case HandleType.POSITION:
                    _positionHandle = gameObject.AddComponent<PositionHandle>().Initialize(this);
                    break;
                case HandleType.ROTATION:
                    _rotationHandle = gameObject.AddComponent<RotationHandle>().Initialize(this);
                    break;
                case HandleType.SCALE:
                    _scaleHandle = gameObject.AddComponent<ScaleHandle>().Initialize(this);
                    break;
            }
        }

        void Clear()
        {
            _draggingHandle = null;
            _previousTarget = null;

            if (_positionHandle) _positionHandle.Destroy();
            if (_rotationHandle) _rotationHandle.Destroy();
            if (_scaleHandle) _scaleHandle.Destroy();
        }

        public bool IsEnabled()
        {
            return (target != null && toolEnabled);
        }

        void Update()
        {

            if (!IsEnabled())
            {
                Clear();
                return;
            }

            if (autoScale)
                transform.localScale =
                    Vector3.one * (Vector3.Distance(handleCamera.transform.position, transform.position) * Mathf.Sin(handleCamera.fieldOfView * Mathf.Deg2Rad / 2f) * autoScaleFactor) / 15;
            
            if (_previousType != type || _previousAxes != axes || _previousTarget != target)
            {
                Clear();
                CreateHandles();
                _previousType = type;
                _previousAxes = axes;
                _previousTarget = target;
            }

            HandleBase handle = null;
            Vector3 hitPoint = Vector3.zero;
            GetHandle(ref handle, ref hitPoint);

            HandleOverEffect(handle);

            if (Input.GetMouseButton(0) && _draggingHandle != null)
            {
               _draggingHandle.Interact(_previousMousePosition);
                OnStartInteraction?.Invoke(target.gameObject);
            }

            if (Input.GetMouseButtonDown(0) && handle != null)
            {
                _draggingHandle = handle;
                _draggingHandle.StartInteraction(hitPoint);
            }

            if (Input.GetMouseButtonUp(0) && _draggingHandle != null)
            {
                _draggingHandle.EndInteraction();
                _draggingHandle = null;
                OnEndInteraction?.Invoke(target.gameObject);
            }

            _previousMousePosition = Input.mousePosition;

            UpdateSelfTransform();
            
        }

        private void UpdateSelfTransform()
        {
            transform.position = target.transform.position;
            if (space == HandleSpace.LOCAL || type == HandleType.SCALE)
            {
                transform.rotation = target.transform.rotation;
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }

        private void HandleOverEffect(HandleBase p_axis)
        {
            if (_draggingHandle == null && _previousAxis != null && _previousAxis != p_axis)
            {
                _previousAxis.SetDefaultColor();
            }

            if (p_axis != null && _draggingHandle == null)
            {
                p_axis.SetColor(Color.yellow);
            }

            _previousAxis = p_axis;
        }

        private void GetHandle(ref HandleBase p_handle, ref Vector3 p_hitPoint)
        {
            Ray ray = handleCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length == 0)
                return;

            foreach (RaycastHit hit in hits)
            {
                p_handle = hit.collider.gameObject.GetComponentInParent<HandleBase>();

                if (p_handle != null)
                {
                    p_hitPoint = hit.point;
                    return;
                }
            }
        }

        static public RuntimeTransformHandle Create(Transform p_target, HandleType p_handleType)
        {
            RuntimeTransformHandle runtimeTransformHandle = new GameObject().AddComponent<RuntimeTransformHandle>();
            runtimeTransformHandle.target = p_target;
            runtimeTransformHandle.type = p_handleType;

            return runtimeTransformHandle;
        }
    }
}