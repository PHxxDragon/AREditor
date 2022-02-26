using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EAR.Download 
{
    public class Downloader : MonoBehaviour
    {
        private const string MODEL_FOLDER_NAME = "Models";

        public void DownloadAndGetFilePath(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, Action<string> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
        {
            string extension = Path.GetExtension(resourceUrl).Substring(1);
            Debug.Log("extension: " + extension);
            StartCoroutine(DownloadAndGetFilePathCoroutine(resourceUrl, resourceKey, resourceName, extension, callback, progressCallback, errorCallback));
        }

        private string GetModelFilePath(string hashedFilename, string extension)
        {
            return string.Format("{0}/{1}/{2}.{3}", Application.persistentDataPath, MODEL_FOLDER_NAME, hashedFilename, extension);
        }

        private string GetHashString(string inputString)
        {
            using HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        private IEnumerator DownloadAndGetFilePathCoroutine(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, Action<string> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
        {
            string hashedFilename = GetHashString(resourceKey);
            string filePath = GetModelFilePath(hashedFilename, resourceExtension);
            if (File.Exists(filePath))
            {
                callback?.Invoke(filePath);
                yield break;
            }

            using UnityWebRequest uwr = UnityWebRequest.Get(resourceUrl);
            UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
            while (!operation.isDone)
            {
                progressCallback?.Invoke(uwr.downloadProgress);
                yield return null;
            }
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error");
                errorCallback?.Invoke("Connection error");
            }
            else if (uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Protocol Error");
                errorCallback?.Invoke(uwr.error);
            }
            else
            {
                byte[] data = uwr.downloadHandler.data;
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, data);
                callback?.Invoke(filePath);
            }
        }
    }
}
