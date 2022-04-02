using UnityEngine;
using System;

namespace EAR.Entity
{
    public class BaseEntity : MonoBehaviour
    {
        public static Action<BaseEntity> OnEntityCreated;
        public static Action<BaseEntity> OnEntityChanged;

        private string id;
        private string entityName = "Entity";

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

        protected void SetId(string id = "")
        {
            if (!string.IsNullOrEmpty(id))
            {
                this.id = id;
            } else
            {
                this.id = Guid.NewGuid().ToString();
            }
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
    }
}

