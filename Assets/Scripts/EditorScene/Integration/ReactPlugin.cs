using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace EAR.Integration
{
    public class ReactPlugin : MonoBehaviour
    {
        public event Action<Param> LoadModelCalledEvent;

        [DllImport("__Internal")]
        private static extern void SceneLoaded();

        void Start()
        {
            Debug.Log("start in react plugin");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        SceneLoaded();
#endif
        }

        public void LoadModel(string paramJson)
        {
            Debug.Log("Load model called: " + paramJson);
            Param param = JsonUtility.FromJson<Param>(paramJson);
            LoadModelCalledEvent?.Invoke(param);
        }
    }
}
