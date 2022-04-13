using TMPro;
using UnityEngine;
using System;

namespace EAR.View
{
    public class Vector3Input : MonoBehaviour
    {
        public event Action<Vector3> OnValueChanged;
        public event Action OnInteractionEnded;

        [SerializeField]
        private TMP_InputField xInputField;
        [SerializeField]
        private TMP_InputField yInputField;
        [SerializeField]
        private TMP_InputField zInputField;

        void Awake()
        {
            AddListeners();
            xInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());
            yInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());
            zInputField.onEndEdit.AddListener((text) => OnInteractionEnded?.Invoke());
        }

        public Vector3 GetValue()
        {
            float x = float.Parse(xInputField.text);
            float y = float.Parse(yInputField.text);
            float z = float.Parse(zInputField.text);
            return new Vector3(x, y, z);
        }

        private void CallListener(string _)
        {
            float x = float.Parse(xInputField.text);
            float y = float.Parse(yInputField.text);
            float z = float.Parse(zInputField.text);
            OnValueChanged?.Invoke(new Vector3(x, y, z));
        }

        private void RemoveListeners()
        {
            xInputField.onValueChanged.RemoveListener(CallListener);
            yInputField.onValueChanged.RemoveListener(CallListener);
            zInputField.onValueChanged.RemoveListener(CallListener);
        }

        private void AddListeners()
        {
            xInputField.onValueChanged.AddListener(CallListener);
            yInputField.onValueChanged.AddListener(CallListener);
            zInputField.onValueChanged.AddListener(CallListener);
        }

        public void SetVector(Vector3 value)
        {
            RemoveListeners();
            xInputField.text = value.x.ToString();
            yInputField.text = value.y.ToString();
            zInputField.text = value.z.ToString();
            AddListeners();
        }
    }
}

