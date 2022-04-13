using EAR.Selection;
using EAR.Container;
using EAR.Entity;
using UnityEngine;

namespace EAR.UndoRedo
{
    public class ChangeEntityCommand : IUndoRedoCommand
    {
        private EntityData oldEntityData;
        private EntityData newEntityData;
        private SelectionManager selectionManager;

        public ChangeEntityCommand(SelectionManager selectionManager, EntityData oldEntityData, EntityData newEntityData)
        {
            this.oldEntityData = oldEntityData;
            this.newEntityData = newEntityData;
            this.selectionManager = selectionManager;
            if (oldEntityData.id != newEntityData.id)
            {
                Debug.LogError("Old and new id are not the same");
            }
        }

        public void Redo()
        {
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(newEntityData.id);
            if (baseEntity)
            {
                baseEntity.PopulateData(newEntityData);
                selectionManager.SelectObject(baseEntity.GetComponent<Selectable>());
            }
            else
            {
                Debug.LogError("Cannot find entity");
            }
        }

        public void Undo()
        {
            BaseEntity baseEntity = EntityContainer.Instance.GetEntity(newEntityData.id);
            if (baseEntity)
            {
                baseEntity.PopulateData(oldEntityData);
                selectionManager.SelectObject(baseEntity.GetComponent<Selectable>());
            }
            else
            {
                Debug.LogError("Cannot find entity");
            }
        }
    }
}

