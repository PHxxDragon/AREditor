using System.Collections.Generic;
using UnityEngine;
using EAR.Entity;

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
    }

    private Dictionary<string, BaseEntity> entityDict = new Dictionary<string, BaseEntity>();

    public BaseEntity GetEntity(string entityId)
    {
        return entityDict[entityId];
    }

    public GameObject GetContainer()
    {
        return container;
    }


}
