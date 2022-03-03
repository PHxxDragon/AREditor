using System;

namespace EAR
{
    public class GlobalStates
    {
        public static Action<bool> OnEnableEditorChange;
        public static Action<bool> OnEnableScreenshotChange;

        private static bool enableEditor = true;
        private static bool enableScreenshot = true;

        public static bool IsEnableEditor()
        {
            return enableEditor;
        }

        public static bool IsEnableScreenshot()
        {
            return enableScreenshot;
        }

        public static void SetEnableEditor(bool value)
        {
            if (enableEditor != value)
            {
                enableEditor = value;
                OnEnableEditorChange?.Invoke(value);
            }
        }

        public static void SetEnableScreenshot(bool value)
        {
            if (enableScreenshot != value)
            {
                enableScreenshot = value;
                OnEnableScreenshotChange?.Invoke(value);
            }
        }
    }
}

