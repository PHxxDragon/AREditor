using EAR.Selection;
using EAR.Container;
using EAR.Entity;
using UnityEngine;

namespace EAR.UndoRedo
{
    public class RemoveEntityCommand : IUndoRedoCommand
    {
        private EntityData entityData;
        private SelectionManager selectionManager;

        public RemoveEntityCommand(SelectionManager selectionManager, EntityData entityData)
        {
            this.entityData = entityData;
            this.selectionManager = selectionManager;
        }

        public void Redo()
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

        public void Undo()
        {
            BaseEntity baseEntity = EntityFactory.InstantNewEntity(entityData);
            selectionManager.SelectObject(baseEntity.GetComponent<Selectable>());
        }
    }
}

