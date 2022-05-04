using UnityEngine;
using EAR.Entity;
using System.Collections.Generic;
using EAR.Selection;
using EAR.Container;
using TMPro;
using System;

namespace EAR.View
{
    public class EntityListView : MonoBehaviour
    {
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private EntityRowView entityRowPrefab;
        [SerializeField]
        private SelectionManager selectionManager;
        [SerializeField]
        private TMP_InputField searchField;

        private Dictionary<string, EntityRowView> entityList = new Dictionary<string, EntityRowView>();

        void Awake()
        {
            BaseEntity.OnEntityCreated += AddEntity;
            BaseEntity.OnEntityDestroy += RemoveEntity;
            BaseEntity.OnEntityNameChanged += EntityNameChanged;
            selectionManager.OnObjectSelected += SelectEntity;
            selectionManager.OnObjectDeselected += DeselectEntity;
            searchField.onValueChanged.AddListener(OnSearch);
            gameObject.SetActive(false);
        }

        private void EntityNameChanged(BaseEntity entity)
        {
            if (entityList.TryGetValue(entity.GetId(), out EntityRowView entityRowView))
            {
                entityRowView.SetName(entity.GetEntityName());
            }
        }

        private void OnSearch(string keyword)
        {
            foreach (EntityRowView entityRowView in entityList.Values)
            {
                if (entityRowView.GetName().Contains(keyword))
                {
                    entityRowView.gameObject.SetActive(true);
                } else
                {
                    entityRowView.gameObject.SetActive(false);
                }
            }
        }

        private void SelectEntity(Selectable selectable)
        {
            BaseEntity baseEntity = selectable.GetComponent<BaseEntity>();
            if (baseEntity)
            {
                EntityRowView entityRowView;
                if (entityList.TryGetValue(baseEntity.GetId(), out entityRowView))
                {
                    entityRowView.Select();
                }
            }
        }

        private void DeselectEntity(Selectable selectable)
        {
            BaseEntity baseEntity = selectable.GetComponent<BaseEntity>();
            if (baseEntity)
            {
                EntityRowView entityRowView;
                if (entityList.TryGetValue(baseEntity.GetId(), out entityRowView))
                {
                    entityRowView.Deselect();
                }
            }
        }

        private void AddEntity(BaseEntity baseEntity)
        {
            EntityRowView entityRowView = Instantiate(entityRowPrefab, container.transform);
            entityRowView.PopulateData(baseEntity);
            entityRowView.RowSelected += () =>
            {
                BaseEntity entity = EntityContainer.Instance.GetEntity(entityRowView.GetEntityId());
                selectionManager.SelectObject(entity.GetComponent<Selectable>());
            };
            entityList.Add(baseEntity.GetId(), entityRowView);
        }

        private void RemoveEntity(BaseEntity baseEntity)
        {
            EntityRowView entityRowView;
            if (entityList.TryGetValue(baseEntity.GetId(), out entityRowView)) {
                Destroy(entityRowView.gameObject);
                entityList.Remove(baseEntity.GetId());
            }
        }

        void OnDestroy()
        {
            BaseEntity.OnEntityCreated -= AddEntity;
            BaseEntity.OnEntityDestroy -= RemoveEntity;
            BaseEntity.OnEntityNameChanged -= EntityNameChanged;
        }
    }

}
