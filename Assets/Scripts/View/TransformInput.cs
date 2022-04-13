using System;
using UnityEngine;

namespace EAR.View
{
    public class TransformInput : MonoBehaviour
    {
        public event Action<TransformData> OnTransformChanged;
        public event Action OnInteractionEnded;

        [SerializeField]
        private Vector3Input position;
        [SerializeField]
        private Vector3Input rotation;
        [SerializeField]
        private Vector3Input scale;

        void Awake()
        {
            AddCallback();
            position.OnInteractionEnded += () => OnInteractionEnded?.Invoke();
            rotation.OnInteractionEnded += () => OnInteractionEnded?.Invoke();
            scale.OnInteractionEnded += () => OnInteractionEnded?.Invoke();
        }

        private void AddCallback()
        {
            position.OnValueChanged += CallCallback;
            rotation.OnValueChanged += CallCallback;
            scale.OnValueChanged += CallCallback;
        }

        private void RemoveCallback()
        {
            position.OnValueChanged -= CallCallback;
            rotation.OnValueChanged -= CallCallback;
            scale.OnValueChanged -= CallCallback;
        }

        private void CallCallback(Vector3 _)
        {
            TransformData transformData = new TransformData();
            transformData.position = position.GetValue();
            transformData.rotation = Quaternion.Euler(rotation.GetValue());
            transformData.scale = scale.GetValue();
            OnTransformChanged?.Invoke(transformData);
        }

        public void SetValue(TransformData transformData)
        {
            AddCallback();
            position.SetVector(transformData.position);
            rotation.SetVector(transformData.rotation.eulerAngles);
            scale.SetVector(transformData.scale);
            RemoveCallback();
        }
    }
}

