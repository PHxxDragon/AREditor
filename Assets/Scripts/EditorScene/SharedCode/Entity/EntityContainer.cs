using System.Collections.Generic;
using UnityEngine;
using EAR.Entity;
using System.Linq;

public class EntityContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    private static EntityContainer instance;

    public static EntityContainer Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        } else
        {
            Debug.LogError("Two instance of entity container found");
        }

        BaseEntity.OnEntityCreated += (BaseEntity entity) =>
        {
            entity.transform.parent = container.transform;
            entityDict.Add(entity.GetId(), entity);
        };
        BaseEntity.OnEntityDestroy += (BaseEntity entity) =>
        {
            if (entityDict.ContainsKey(entity.GetId()))
            {
                entityDict.Remove(entity.GetId());
            }
        };
    }

    private Dictionary<string, BaseEntity> entityDict = new Dictionary<string, BaseEntity>();



    public BaseEntity GetEntity(string entityId)
    {
        return entityDict[entityId];
    }

    public BaseEntity[] GetEntities()
    {
        return entityDict.Values.ToArray();
    }


}
