using UnityEngine;
using System;

namespace EAR.AddObject
{
    public class ObjectPreviewAndAdd : MonoBehaviour
    {
        public delegate void IsMouseRaycastBlocked(ref bool isBlocked);
        public event IsMouseRaycastBlocked CheckMouseRaycastBlocked;

        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Collider raycastPlane;

        private bool isPreviewOn;
        private GameObject objectPrefab;
        private GameObject previewObject;
        private Action<GameObject> callback;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        public void StartPreviewAndAdd(GameObject objectPrefab, GameObject previewPrefab, Action<GameObject> callback = null)
        {
            isPreviewOn = true;
            this.objectPrefab = objectPrefab;
            previewObject = Instantiate(previewPrefab);
            previewObject.SetActive(false);
            this.callback = callback;
        }

        public void StopPreview()
        {
            if (isPreviewOn)
            {
                isPreviewOn = false;
                Destroy(previewObject);
                previewObject = null;
                objectPrefab = null;
            }
        }

        void Update()
        {
            if (isPreviewOn)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (raycastPlane.Raycast(ray, out hitInfo, 1000f))
                {
                    if (!previewObject.activeSelf)
                        previewObject.SetActive(true);

                    previewObject.gameObject.transform.position = hitInfo.point;
                    Vector3 cameraPos = mainCamera.transform.position;
                    Vector3 behindPos = 2 * previewObject.transform.position - cameraPos;
                    behindPos.y = 0;
                    previewObject.transform.LookAt(behindPos);
                    Bounds bounds = Utils.GetUIBounds(previewObject);
                    Vector3 pos = previewObject.transform.position;
                    pos.y = pos.y + bounds.extents.y - bounds.center.y;
                    previewObject.transform.position = pos;

                    if (Input.GetMouseButtonDown(0))
                    {
                        bool isBlocked = false;
                        CheckMouseRaycastBlocked?.Invoke(ref isBlocked);
                        if (!isBlocked)
                        {
                            GameObject note = Instantiate(objectPrefab);
                            note.transform.SetPositionAndRotation(previewObject.transform.position, previewObject.transform.rotation);
                            callback?.Invoke(note);
                            StopPreview();
                        }
                    }
                }
            }
        }
    }
}

