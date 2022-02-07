using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

namespace EAR 
{
    public class Utils : MonoBehaviour
    {
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (!instance)
                {
                    instance = new GameObject("Utils").AddComponent<Utils>();
                }
                return instance;
            }
        }

        public Sprite Texture2DToSprite(Texture2D texture2D)
        {
            return Sprite.Create(texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f)
                );
        }

        public void GetImageAsTexture2D(string imageUrl, Action<Texture2D, object> callback, Action<string, object> errorCallback = null, object param = null)
        {
            StartCoroutine(GetImageCoroutine(imageUrl, callback, errorCallback, param));
        }

        private IEnumerator GetImageCoroutine(string imageUrl, Action<Texture2D, object> callback, Action<string, object> errorCallback, object param)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                    errorCallback?.Invoke(uwr.error, param);
                }
                else
                {
                    // Get downloaded texture once the web request completes
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    callback?.Invoke(texture, param);
                }
            }
        }
    }
}

