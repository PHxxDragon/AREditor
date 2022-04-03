using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class DropdownHelper : MonoBehaviour
    {
        public event Action<object> OnDropdownValueChanged;

        [SerializeField]
        private TMP_Dropdown dropdown;

        private readonly List<object> objectList = new List<object>();
        private readonly Dictionary<object, int> objectToIndex = new Dictionary<object, int>();

        void Awake()
        {
            
            dropdown.onValueChanged.AddListener((value) =>
            {
                OnDropdownValueChanged?.Invoke(objectList[value]);
            });
        }

        public void ClearData()
        {
            objectList.Clear();
            objectToIndex.Clear();
            dropdown.ClearOptions();
        }

        public void PopulateData(List<object> objectCollection, List<string> nameCollection)
        {
            if (objectCollection.Count != nameCollection.Count)
            {
                Debug.LogError("Collection size does not match");
                return;
            }
            objectList.Clear();
            objectToIndex.Clear();
            dropdown.ClearOptions();

            objectList.AddRange(objectCollection);
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < objectCollection.Count; i++)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = nameCollection[i];
                optionDatas.Add(optionData);
                objectToIndex.Add(objectList[i], i);
            }
            dropdown.AddOptions(optionDatas);
        }

        public void AddData(object obj, string name)
        {
            objectList.Add(obj);
            objectToIndex[obj] = objectList.Count - 1;
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = name;
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            optionDatas.Add(optionData);
            dropdown.AddOptions(optionDatas);
        }

        public void RemoveData(object obj)
        {
            if (!objectToIndex.ContainsKey(obj))
            {
                return;
            }

            object selected = GetSelectedValue();

            int index = objectToIndex[obj];
            objectList.RemoveAt(index);
            objectToIndex.Remove(obj);

            dropdown.options.RemoveAt(index);
            if (obj != selected)
            {
                SelectValue(selected);
            } else
            {
                dropdown.value = 0;
            }
        }

        public object GetSelectedValue()
        {
            return objectList[dropdown.value];
        }

        public void SelectValue(object value)
        {
            try
            {
                dropdown.value = objectToIndex[value];
            } catch (KeyNotFoundException)
            {
                Debug.Log("Key not found: " + value);
            }
        }
    }
}

