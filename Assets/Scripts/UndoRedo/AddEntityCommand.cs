using EAR.Selection;
using EAR.Container;
using EAR.Entity;
using UnityEngine;

namespace EAR.UndoRedo
{
    public class AddEntityCommand : IUndoRedoCommand
    {
        private EntityData entityData;
        private SelectionManager selectionManager;

        public AddEntityCommand(SelectionManager selectionManager, EntityData entityData)
        {
            this.entityData = entityData;
            this.selectionManager = selectionManager;
        }

        public void Redo()
        {
            BaseEntity baseEntity = EntityFactory.InstantNewEntity(entityData);
            selectionManager.SelectObject(baseEntity.GetComponent<Selectable>());
        }

        public void Undo()
        {
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(entityData.id);
            if (baseEntity)
            {
                Object.Destroy(baseEntity.gameObject);
            }
            else
            {
                Debug.LogWarning("Cannot find entity");
            }
        }
    }
}

