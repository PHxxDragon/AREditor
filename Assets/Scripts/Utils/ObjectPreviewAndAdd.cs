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


                    Bounds bounds = Utils.GetUIBounds(previewObject);
                    Bounds emptyBound = new Bounds();
                    if (bounds == emptyBound)
                    {
                        bounds = Utils.GetModelBounds(previewObject, false);
                    }

                    Vector3 pos = previewObject.transform.position;
                    pos.y = pos.y + bounds.extents.y - bounds.center.y;
                    previewObject.transform.position = pos;

                    float distance = Math.Abs(mainCamera.transform.localPosition.z);
                    float expectedRadius = distance * Mathf.Sin(mainCamera.fieldOfView * Mathf.Deg2Rad / 2f);
                    float currentRadius = bounds.extents.magnitude;

                    previewObject.transform.localScale *= expectedRadius / currentRadius;

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

