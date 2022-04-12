using UnityEngine;
using EAR.Entity;
using EAR.Selection;

namespace EAR.Editor.Presenter
{
    public class AddSelectionForModel : MonoBehaviour
    {
        void Awake()
        {
            BaseEntity.OnEntityCreated += AddSelection;
        }

        private void AddSelection(BaseEntity entity)
        {
            entity.gameObject.AddComponent<Selectable>();
        }
    }

}
