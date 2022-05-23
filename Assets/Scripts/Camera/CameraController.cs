using UnityEngine;
using System.Linq;
using System;

namespace EAR.EARCamera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Speed")]
        public float RotateSpeed = 2f;
        public float MoveSpeed = 2f;
        public float ScrollSpeed = 20f;
        public float LookSpeed = 150f;
        public float ResetSpeed = 6f;

        [Header("Dampening")]
        public float OrbitDampending = 10f;
        public float ScrollDampening = 6f;
        public float MoveDampening = 6f;

        [Header("Controls")]
        public KeyCode[] RotateAroundKey = { KeyCode.LeftAlt, KeyCode.RightAlt};
        public KeyCode[] LookAroundKey = { KeyCode.LeftControl, KeyCode.RightControl };
        public KeyCode ResetKey = KeyCode.F;

        public event Action BeforeFocus;

        private Transform cameraTransform;
        private Transform cameraAnchorTransform;

        private Vector3 _Rotation;
        private bool isRotating = false;

        private float _Distance;
        private bool isZooming = false;

        private Vector3 _AnchorPosition;
        private bool isMoving = false;

        private float _LookRotationX;
        private float _LookRotationY;
        private bool isLooking = false;

        private float _ResetT = 1f;
        private Vector3 _DefaultPosition;
        private Quaternion _DefaultRotation;
        private float _DefaultDistance;
        private Vector3 _StartResetPosition;
        private Quaternion _StartResetRotation;
        private float _StartResetDistance;
        private bool isReseting = false;

        private Camera _camera;

        void Start()
        {
            cameraTransform = transform;
            cameraAnchorTransform = transform.parent;
            _DefaultPosition = cameraAnchorTransform.position;
            _DefaultRotation = cameraAnchorTransform.rotation;
            _DefaultDistance = cameraTransform.position.magnitude;
            _Distance = _DefaultDistance;
            _camera = cameraTransform.GetComponent<Camera>();
            SetFocus();
            Focus();
        }

        void Update()
        {
            ResetBools();
            ProcessInputs();
            UpdateLocals();
        }

        public void SetFocus(Bounds bounds)
        {
            float radius = bounds.extents.magnitude != 0 ? bounds.extents.magnitude : 1;
            float distance = radius / (Mathf.Sin(_camera.fieldOfView * Mathf.Deg2Rad / 2f));
            Vector3 position = bounds.center;
            Quaternion rotation = Quaternion.Euler(30, 0, 0);

            _DefaultPosition = position;
            _DefaultRotation = rotation;
            _DefaultDistance = distance;
        }

        public void SetFocus()
        {
            Bounds bounds = new Bounds();
            bounds.center = new Vector3(0, 0.5f, 0);
            bounds.size = new Vector3(1, 1, 1);
            SetFocus(bounds);
        }

        private void Focus()
        {
            BeforeFocus?.Invoke();
            _ResetT = 0f;
            _StartResetPosition = cameraAnchorTransform.transform.position;
            _StartResetRotation = cameraAnchorTransform.transform.rotation;
            _StartResetDistance = _Distance;
            isReseting = true;
        }

        private void ProcessInputs()
        {
            if (Input.GetMouseButton(0))
            {
                if (!GlobalStates.IsMouseRaycastBlocked())
                {
                    if (RotateAroundKey.Any((key) => Input.GetKey(key)))
                    {
                        _Rotation.y += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * RotateSpeed;
                        _Rotation.x -= Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * RotateSpeed;
                        _Rotation.x = ClampRotation(_Rotation.x, -90f, 90f);
                        isRotating = true;
                    }
                    else if (LookAroundKey.Any((key) => Input.GetKey(key)))
                    {
                        _LookRotationX = -Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * LookSpeed * Time.deltaTime;
                        _LookRotationY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * LookSpeed * Time.deltaTime;
                        isLooking = true;
                    }
                    else
                    {
                        float distanceScreenSpace = _Distance * Mathf.Sin(_camera.fieldOfView * Mathf.Deg2Rad / 2f);
                        float x = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * MoveSpeed * distanceScreenSpace * Time.deltaTime;
                        float y = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * MoveSpeed * distanceScreenSpace * Time.deltaTime;
                        Vector3 moveDirection = new Vector3(x, y, 0);
                        moveDirection = Quaternion.Euler(_Rotation) * moveDirection;
                        _AnchorPosition -= moveDirection;
                        isMoving = true;
                    }
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                if (!GlobalStates.IsMouseRaycastBlocked())
                {
                    float ScrollAmount = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel"), -1, 1) * ScrollSpeed;

                    //Makes camera zoom faster the further away
                    ScrollAmount *= (_Distance * 0.3f);

                    _Distance += ScrollAmount * -1f;
                    _Distance = Mathf.Clamp(_Distance, 0.015f, 300f);
                    isZooming = true;
                }
            }
            
            if (Input.GetKeyDown(ResetKey) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                Focus();
            }
        }

        private float ClampRotation(float rotation, float min, float max)
        {
            rotation = (rotation + 180 -  ((int) rotation + 180) / 360 * 360) - 180;
            return Mathf.Clamp(rotation, min, max);
        }

        private void UpdateLocals()
        {
            if (!isRotating)
            {
                _Rotation = cameraAnchorTransform.rotation.eulerAngles;
            }
            if (!isMoving)
            {
                _AnchorPosition = cameraAnchorTransform.position;
            }
            if (!isZooming)
            {
                _Distance = cameraTransform.localPosition.magnitude;
            }
        }

        private void ResetBools()
        {
            isRotating = false;
            isMoving = false;
            isZooming = false;
            isLooking = false;
        }

        void LateUpdate()
        {
            if (isRotating)
            {
                Quaternion QT = Quaternion.Euler(_Rotation.x, _Rotation.y, 0);
                cameraAnchorTransform.rotation = Quaternion.Lerp(cameraAnchorTransform.rotation, QT, Time.deltaTime * OrbitDampending * 2f);
                //Debug.Log(Quaternion.Lerp(Quaternion.Euler(new Vector3(29.6f, 184.8f, 180f)), Quaternion.Euler(new Vector3(150.6f, 4.7f, 0.0f)), 0.090109f).eulerAngles);
            }

            if (isZooming)
            {
                if (cameraTransform.localPosition.z != _Distance * -1f)
                {
                    cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraTransform.localPosition.z, _Distance * -1f, Time.deltaTime * ScrollDampening));
                }
            }

            if (isMoving)
            {
                //cameraAnchorTransform.position = Vector3.Lerp(cameraAnchorTransform.position, _AnchorPosition, MoveDampening);
                cameraAnchorTransform.position = _AnchorPosition;
            }

            if (isLooking)
            {
                cameraAnchorTransform.RotateAround(cameraTransform.position, Vector3.up, _LookRotationX);
                cameraAnchorTransform.RotateAround(cameraTransform.position, cameraTransform.right, _LookRotationY);
            }

            if (isReseting)
            {
                _ResetT += Time.deltaTime * ResetSpeed;
                if (_ResetT >= 1)
                {
                    isReseting = false;
                    _ResetT = 1;
                }

                cameraAnchorTransform.position = Vector3.Lerp(_StartResetPosition, _DefaultPosition, _ResetT);
                cameraAnchorTransform.rotation = Quaternion.Lerp(_StartResetRotation, _DefaultRotation, _ResetT);
                cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_StartResetDistance * -1f, _DefaultDistance * -1f, _ResetT));
            }
        }
    }
}