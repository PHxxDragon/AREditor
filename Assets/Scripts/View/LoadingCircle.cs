using UnityEngine;

namespace EAR.View
{
    public class LoadingCircle : MonoBehaviour
    {
        [SerializeField]
        private GameObject toBeRotated;

        void Update()
        {
            Vector3 euler = toBeRotated.transform.rotation.eulerAngles;
            euler.z -= 5f;
            toBeRotated.transform.rotation = Quaternion.Euler(euler);
        }
    }
}

