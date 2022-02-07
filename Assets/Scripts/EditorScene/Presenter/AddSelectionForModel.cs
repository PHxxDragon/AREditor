using UnityEngine;
using EAR.AR;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class AddSelectionForModel : MonoBehaviour
    {
        [SerializeField]
        private ModelLoader modelLoader;

        void Start()
        {
            if (modelLoader != null)
            {
                modelLoader.OnLoadEnded += AddSelection;
            }
        }

        private void AddSelection()
        {
            modelLoader.GetModel().gameObject.AddComponent<Selectable>();
        }
    }

}
