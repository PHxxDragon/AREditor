using UnityEngine;
using EAR.Entity;
using EAR.Selection;
using System.Collections;

namespace EAR.Editor.Presenter
{
    public class AddSelectionForModel : MonoBehaviour
    {
        void Start()
        {
            ModelEntity.OnModelEntityCreated += AddSelection;
            ModelEntity.OnModelEntityChanged += AddSelection;
        }

        private void AddSelection(string assetId, ModelEntity model)
        {
            Selectable selectable = model.GetComponent<Selectable>();
            if (!selectable)
            {
                model.gameObject.AddComponent<Selectable>();
            }
            StartCoroutine(AddCollider(model.gameObject));
        }

        private IEnumerator AddCollider(GameObject model)
        {
            BoxCollider boxCollider = model.GetComponent<BoxCollider>();
            if (boxCollider)
            {
                Destroy(boxCollider);
            }

            TransformData transformData = TransformData.TransformToTransformData(model.transform);
            TransformData.ResetTransform(model.transform);
            Bounds bound = Utils.GetModelBounds(model);
            BoxCollider collider = model.AddComponent<BoxCollider>();
            collider.center = bound.center - model.transform.position;
            collider.size = bound.size;
            TransformData.TransformDataToTransfrom(transformData, model.transform);
            yield return null;
/*            MeshFilter[] meshFilters = modelLoader.GetModel().gameObject.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.GetComponent<Collider>() == null)
                {
                    meshFilter.gameObject.AddComponent<BoxCollider>();
                    //Debug.Log("added");
                    yield return null;
                }
            }

            SkinnedMeshRenderer[] skinnedMeshRenderers = modelLoader.GetModel().gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                if (skinnedMeshRenderer.GetComponent<MeshCollider>() == null)
                {
                    BoxCollider collider = skinnedMeshRenderer.gameObject.AddComponent<BoxCollider>();
                    //Debug.Log("added");
                    yield return null;
                }
            }*/
        }
    }

}
