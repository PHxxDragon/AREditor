using UnityEngine;

namespace EAR.Entity
{
    public class BaseEntity : MonoBehaviour
    {
        private string id;
        private string entityName;

        protected void SetId(string id = "")
        {
            if (id != "")
            {
                this.id = id;
            } else
            {
                this.id = System.Guid.NewGuid().ToString();
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

