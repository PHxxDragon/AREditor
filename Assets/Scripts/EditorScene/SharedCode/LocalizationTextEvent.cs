using UnityEngine;
using UnityEngine.Events;

namespace EAR.Localization
{
    public class LocalizationTextEvent : MonoBehaviour
    {
        public UnityEvent<string> OnLocalizeText;

        [SerializeField]
        private string key;

        public void ApplyLocalization(string text)
        {
            OnLocalizeText.Invoke(text);
        }

        public string GetKey()
        {
            return key;
        }
    }
}

