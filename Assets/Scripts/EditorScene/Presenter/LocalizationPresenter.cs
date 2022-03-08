using EAR.Integration;
using EAR.Localization;
using UnityEngine;

namespace EAR.Editor.Presenter
{
    public class LocalizationPresenter : MonoBehaviour
    {
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private LocalizationManager localizationManager;

        void Start()
        {
            if (reactPlugin != null && localizationManager != null)
            {
                reactPlugin.LanguageChangeEvent += (string locale) =>
                {
                    localizationManager.ApplyLocalization(locale);
                };
            } else
            {
                Debug.Log("Unassigned references");
            }
            
        }
    }
}

