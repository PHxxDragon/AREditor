using UnityEngine;

namespace EAR.View
{
    public class PointToCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }
        void Update()
        {
            transform.LookAt(2 * transform.position - mainCamera.transform.position);
        }
    }
}

