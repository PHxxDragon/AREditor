using EAR.Screenshoter;
using EAR.Integration;
using UnityEngine;
using EAR.View;

namespace EAR.Editor.Presenter 
{
    public class ScreenshotPresenter : MonoBehaviour
    {
        [SerializeField]
        private Screenshot screenshot;
        [SerializeField]
        private ReactPlugin reactPlugin;
        [SerializeField]
        private ToolBar toolBar;


        void Start()
        {
            if (screenshot != null && reactPlugin != null && toolBar != null)
            {
                toolBar.ScreenshotButtonClicked += () =>
                {
                    screenshot.TakeScreenshot();
                };
                screenshot.OnScreenshotTake += (byte[] image) =>
                {
                    reactPlugin.SaveScreenshotToJs(image);
                };
            } else
            {
                Debug.Log("Unassigned references");
            }
        }
    }
}

