using UnityEngine;

namespace EAR
{
    public class ApplicationConfigurationStatic : MonoBehaviour
    {
        private ApplicationConfiguration applicationConfiguration;

        private static ApplicationConfigurationStatic instance;

        public static ApplicationConfigurationStatic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<ApplicationConfigurationStatic>();

                    instance.applicationConfiguration = Resources.Load<ApplicationConfiguration>("ApplicationConfiguration");
                    Debug.Log(instance.applicationConfiguration.GetServerName());
                    Debug.Log(instance.applicationConfiguration.GetARModulePath(5));
                    Debug.Log(instance.applicationConfiguration.GetModelPath(5));
                }
                return instance;
            }
        }

        public ApplicationConfiguration GetApplicationConfiguration()
        {
            return applicationConfiguration;
        }
    }
}

