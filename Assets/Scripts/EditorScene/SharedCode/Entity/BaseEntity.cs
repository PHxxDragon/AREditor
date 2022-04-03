using UnityEngine;
using System;

namespace EAR.Entity
{
    public class BaseEntity : MonoBehaviour
    {
        public static Action<BaseEntity> OnEntityCreated;
        public static Action<BaseEntity> OnEntityChanged;
        public static Action<BaseEntity> OnEntityDestroy;

        private string id = Guid.NewGuid().ToString();
        private string entityName = "Entity";

        private void Awake()
        {
            GlobalStates.OnIsPlayModeChange += (isPlayMode) =>
            {
                if (isPlayMode)
                {
                    StartDefaultState();
                } else
                {
                    ResetEntityState();
                }
            };
        }

        public virtual void StartDefaultState()
        {

        }

        public virtual void ResetEntityState()
        {

        }

        public virtual bool IsClickable()
        {
            return false;
        }

        public virtual bool IsViewable()
        {
            return true;
        }

        public virtual bool IsValidEntity()
        {
            return true;
        }

        protected void SetId(string id)
        {
            this.id = id;
        }

        public void SetEntityName(string entityName)
        {
            this.entityName = entityName;
        }

        public string GetId()
        {
            return id;
        }

        public string GetEntityName()
        {
            return entityName;
        }

        void OnDestroy()
        {
            OnEntityDestroy?.Invoke(this);
        }
    }
}

