using UnityEngine;
using EAR.AR;
using EAR.Selection;
using System.Collections;

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
            StartCoroutine(AddCollider());
        }

        private IEnumerator AddCollider()
        {
            Debug.Log("cccc");
            MeshFilter[] meshFilters = modelLoader.GetModel().gameObject.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.GetComponent<Collider>() == null)
                {
                    meshFilter.gameObject.AddComponent<BoxCollider>();
                    Debug.Log("aaaa");
                    yield return null;
                }
            }

            SkinnedMeshRenderer[] skinnedMeshRenderers = modelLoader.GetModel().gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.GetComponent<MeshCollider>() == null)
                {
                    BoxCollider collider = skinnedMeshRenderer.gameObject.AddComponent<BoxCollider>();
                    Debug.Log("bbbb");
                    yield return null;
                }
            }
        }
    }

}
