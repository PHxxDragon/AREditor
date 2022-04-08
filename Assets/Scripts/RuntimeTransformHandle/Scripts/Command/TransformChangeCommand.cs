using UnityEngine;
using EAR;

namespace RuntimeHandle
{
    public class TransformChangeCommand : IUndoRedoCommand
    {
        private Transform target;
        private TransformData previous;
        private TransformData current;

        public TransformChangeCommand(Transform target, TransformData previous, TransformData current)
        {
            this.target = target;
            this.previous = previous;
            this.current = current;
        }
        public void Redo()
        {
            TransformData.TransformDataToTransfrom(current, target);
        }

        public void Undo()
        {
            TransformData.TransformDataToTransfrom(previous, target);
        }
    }
}

