using UnityEngine;

namespace EAR
{
    [CreateAssetMenu(menuName = "EAR/Configuration")]
    public class ApplicationConfiguration : ScriptableObject
    {
        [SerializeField]
        private string serverName;

        [SerializeField]
        private string loginPath;

        [SerializeField]
        private string profilePath;

        [SerializeField]
        private string armodulePath;

        [SerializeField]
        private string modelPath;

        public string GetServerName()
        {
            return "http://localhost:3000/";
        }

        public string GetLoginPath()
        {
            return "http://localhost:3000/" + loginPath;
        }

        public string GetProfilePath()
        {
            return "http://localhost:3000/" + profilePath;
        }

        public string GetWorkspacePath()
        {
            return "";
        }

        public string GetARModulePath(int moduleId)
        {
            return "http://localhost:3000/" + "api/v1/modules/ar" + "/" + moduleId;
        }

        public string GetModelPath(int modelId)
        {
            return "http://localhost:3000/" + "api/v1/models" + "/" + modelId;
        }
    }
}

