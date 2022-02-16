using UnityEngine;

namespace EAR
{
    public class ApplicationConfigurationHolder : MonoBehaviour
    {
        private ApplicationConfiguration applicationConfiguration;

        private static ApplicationConfigurationHolder instance;

        public static ApplicationConfigurationHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<ApplicationConfigurationHolder>();

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

