using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class DropdownHelper : MonoBehaviour
    {
        public event Action<string> OnDropdownValueChanged;

        [SerializeField]
        private TMP_Dropdown dropdown;

        private readonly List<string> objectList = new List<string>();
        private readonly Dictionary<string, int> objectToIndex = new Dictionary<string, int>();

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

        public void SetData(string obj, string name, int index)
        {
            if (index >= objectList.Count)
            {
                AddData(obj, name);
            } else
            {
                string oldObj = objectList[index];
                objectList[index] = obj;
                objectToIndex.Remove(oldObj);
                objectToIndex[obj] = index;
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = name;
                dropdown.options[index] = optionData;
            }
        }

        public void AddData(string obj, string name)
        {
            objectList.Add(obj);
            objectToIndex[obj] = objectList.Count - 1;
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = name;
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            optionDatas.Add(optionData);
            dropdown.AddOptions(optionDatas);
        }

        public string GetSelectedValue()
        {
            return objectList[dropdown.value];
        }

        public void SelectValue(string value)
        {
            try
            {
                dropdown.value = objectToIndex[value];
            } catch (KeyNotFoundException)
            {
                dropdown.value = 0;
            } catch (ArgumentNullException)
            {
                dropdown.value = 0;
            }
        }
    }
}

