using UnityEngine;
using System;

namespace EAR.AddObject
{
    public class ObjectPreviewAndAdd : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Collider raycastPlane;

        private bool isPreviewOn;
        private GameObject previewObject;
        private Action<TransformData> callback;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        public void StartPreviewAndAdd(GameObject previewPrefab, Action<TransformData> callback = null, Action<GameObject> previewInitCallback = null)
        {
            if (isPreviewOn)
            {
                StopPreview();
            }

            isPreviewOn = true;
            previewObject = Instantiate(previewPrefab);
            previewInitCallback?.Invoke(previewObject);
            previewObject.SetActive(false);
            this.callback = callback;
        }

        public void StopPreview()
        {
            isPreviewOn = false;
            Destroy(previewObject);
            previewObject = null;
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
/*                    Vector3 cameraPos = mainCamera.transform.position;
                    Vector3 behindPos = 2 * previewObject.transform.position - cameraPos;
                    behindPos.y = 0;
                    previewObject.transform.LookAt(behindPos);*/

                    Bounds bounds = Utils.GetUIBounds(previewObject);
                    Bounds emptyBound = new Bounds();
                    if (bounds == emptyBound)
                    {
                        bounds = Utils.GetModelBounds(previewObject);
                    }

                    Vector3 pos = previewObject.transform.position;
                    pos.y = pos.y + bounds.extents.y - bounds.center.y;
                    previewObject.transform.position = pos;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!GlobalStates.IsMouseRaycastBlocked())
                        {
                            callback?.Invoke(TransformData.TransformToTransformData(previewObject.transform));
                            StopPreview();
                        }
                    }
                }
            }
        }
    }
}

