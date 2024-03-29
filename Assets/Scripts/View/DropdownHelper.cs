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
        [SerializeField]
        private List<string> initObjectList;
        [SerializeField]
        private List<string> initNameList;

        private readonly List<string> objectList = new List<string>();
        private readonly Dictionary<string, int> objectToIndex = new Dictionary<string, int>();

        void Awake()
        {
            if (objectList.Count == 0 && objectToIndex.Count == 0)
            {
                dropdown.ClearOptions();
                for (int i = 0; i < initNameList.Count; i++)
                {
                    AddData(initObjectList[i], initNameList[i]);
                }
            } else if (initNameList.Count != 0 || initNameList.Count != 0)
            {
                Debug.LogError("Cannot add initial value for dropdown because option count is not 0");
            }

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
            string oldObj = objectList[index];
            objectList[index] = obj;
            objectToIndex.Remove(oldObj);
            objectToIndex[obj] = index;
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = name;
            dropdown.options[index] = optionData;
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
            if (objectList.Count > dropdown.value)
            {
                return objectList[dropdown.value];
            }
            return null;
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

