using System.Collections.Generic;
using EAR.View;

namespace EAR.Localization
{
    public class LocalizationDropdownEvent : LocalizationEvent
    {
        public DropdownHelper dropdown;
        public List<string> keys;
        public List<string> objects;

        public override void ApplyLocalization()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                dropdown.SetData(objects[i], LocalizationManager.GetLocalizedText(keys[i]), i);
            }
        }
    }

}
