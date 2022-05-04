using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EAR.Entity;
using System;

namespace EAR.View
{
    public class EntityRowView : MonoBehaviour
    {
        public event Action RowSelected;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private Toggle toggle;
        [SerializeField]
        private Sprite imageIcon;
        [SerializeField]
        private Sprite modelIcon;
        [SerializeField]
        private Sprite soundIcon;
        [SerializeField]
        private Sprite buttonIcon;
        [SerializeField]
        private Sprite noteIcon;
        [SerializeField]
        private Sprite videoIcon;

        private string entityId;

        void Awake()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    RowSelected?.Invoke();
                }
            });
        }

        public string GetName()
        {
            return nameText.text;
        }

        public void Select()
        {
            toggle.isOn = true;     
        }

        public void Deselect()
        {
            toggle.isOn = false;
        }

        public string GetEntityId()
        {
            return entityId;
        }

        public void SetName(string text)
        {
            nameText.text = text;
        }

        public void PopulateData(BaseEntity baseEntity)
        {
            entityId = baseEntity.GetId();
            nameText.text = baseEntity.GetEntityName();
            if (baseEntity is ImageEntity)
            {
                icon.sprite = imageIcon;
            } 
            else if (baseEntity is SoundEntity)
            {
                icon.sprite = soundIcon;
            }
            else if (baseEntity is ModelEntity)
            {
                icon.sprite = modelIcon;
            }
            else if (baseEntity is NoteEntity)
            {
                icon.sprite = noteIcon;
            }
            else if (baseEntity is VideoEntity)
            {
                icon.sprite = videoIcon;
            }
            else if (baseEntity is ButtonEntity)
            {
                icon.sprite = buttonIcon;
            }
        }
    }
}

