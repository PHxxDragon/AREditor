using UnityEngine;
using UnityEngine.Events;

namespace EAR.Localization
{
    public class LocalizationTextEvent : LocalizationEvent
    {
        public UnityEvent<string> OnLocalizeText;

        [SerializeField]
        private string key;

        public override void ApplyLocalization()
        {
            OnLocalizeText.Invoke(LocalizationManager.GetLocalizedText(key));
        }
    }
}

