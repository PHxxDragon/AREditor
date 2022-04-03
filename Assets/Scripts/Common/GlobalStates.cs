using System;

namespace EAR
{
    public class GlobalStates
    {
        public static Action<bool> OnEnableEditorChange;
        public static Action<bool> OnEnableScreenshotChange;
        public static Action<bool> OnIsPlayModeChange;

        public delegate void MouseRaycastHandler(ref bool isBlocked);
        public static event MouseRaycastHandler CheckMouseRaycastBlocked;

        private static bool enableEditor = true;
        private static bool enableScreenshot = true;
        private static bool isPlayMode = false;

        public static bool IsMouseRaycastBlocked()
        {
            bool isBlocked = false;
            CheckMouseRaycastBlocked(ref isBlocked);
            return isBlocked;
        }

        public static bool IsPlayMode()
        {
            return isPlayMode;
        }

        public static bool IsEnableEditor()
        {
            return enableEditor;
        }

        public static bool IsEnableScreenshot()
        {
            return enableScreenshot;
        }

        public static void SetIsPlayMode(bool value)
        {
            if (isPlayMode != value)
            {
                isPlayMode = value;
                OnIsPlayModeChange?.Invoke(value);
            }
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

