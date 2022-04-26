using System.Collections.Generic;
using TMPro;

namespace EAR.Localization
{
    public class LocalizationDropdownEvent : LocalizationEvent
    {
        public TMP_Dropdown dropdown;
        public List<string> keys;

        public override void ApplyLocalization()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = LocalizationManager.GetLocalizedText(keys[i]);
                if (i < dropdown.options.Count)
                {
                    dropdown.options[i] = optionData;
                }
                else
                {
                    dropdown.options.Add(optionData);
                }
            }
        }
    }

}
