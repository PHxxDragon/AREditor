using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class AnimationBar : MonoBehaviour
    {
        public event Action<bool> PlayToggleClickEvent;
        public event Action<float> SliderValueChangeEvent;
        public event Action<int> DropdownValueChangeEvent;

        [SerializeField]
        private Toggle toggle;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_Dropdown dropdown;

        void Start()
        {
            toggle.onValueChanged.AddListener(OnToggleClick);
            slider.onValueChanged.AddListener(OnSliderHold);
            dropdown.onValueChanged.AddListener(OnDropdownSelect);
        }

        public void SetPlayToggle(bool isOn)
        {
            toggle.isOn = isOn;
        }

        public void SetDropdownOption(List<string> options, int value)
        {
            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            foreach (string option in options)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = option;
                optionDatas.Add(optionData);
            }
            dropdown.AddOptions(optionDatas);
            dropdown.value = value;
        }

        public void SetSliderValue(float value)
        {
            slider.value = value;
        }

        private void OnDropdownSelect(int value)
        {
            DropdownValueChangeEvent?.Invoke(value);
        }

        private void OnSliderHold(float value)
        {
            SliderValueChangeEvent?.Invoke(value);
        }

        private void OnToggleClick(bool isOn)
        {
            PlayToggleClickEvent?.Invoke(isOn);
        }
    }
}

