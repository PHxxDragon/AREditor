using UnityEngine;
using System.IO;
using System;
using System.Collections;

namespace EAR.Screenshoter
{
    public class Screenshot : MonoBehaviour
    {
        public event Action<byte[]> OnScreenshotTake;

        [SerializeField]
        private Camera screenshotCamera;
        [SerializeField]
        private Camera targetCamera;

        private bool takeScreenshotOnNextFrame;

        void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }
            gameObject.SetActive(false);
        }

        void OnPostRender()
        {
            if (takeScreenshotOnNextFrame)
            {
                takeScreenshotOnNextFrame = false;
                RenderTexture renderTexture = screenshotCamera.targetTexture;

                Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
                renderResult.ReadPixels(rect, 0, 0);

                byte[] bytes = renderResult.EncodeToPNG();
                StartCoroutine(SaveScreenshotCoroutine(bytes));

                RenderTexture.ReleaseTemporary(renderTexture);
                screenshotCamera.targetTexture = null;
            }
        }

        private IEnumerator SaveScreenshotCoroutine(byte[] bytes)
        {
            yield return null;
            OnScreenshotTake?.Invoke(bytes);
            screenshotCamera.gameObject.SetActive(false);
        }

        public void TakeScreenshot()
        {
            screenshotCamera.gameObject.SetActive(true);
            CopyTransform(targetCamera.transform, screenshotCamera.transform);
            screenshotCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
            takeScreenshotOnNextFrame = true;
            
        }
        private void CopyTransform(Transform from, Transform to)
        {
            to.SetPositionAndRotation(from.position, from.rotation);
            to.localScale = from.localScale;
        }
    }
}

