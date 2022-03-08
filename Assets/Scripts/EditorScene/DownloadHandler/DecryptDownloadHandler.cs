using System.Security.Cryptography;
using UnityEngine.Networking;
using System;
using System.IO;
using UnityEngine;

namespace EAR.DownloadHandler
{
    public class DecryptDownloadHandler : DownloadHandlerScript
    {
        private const int IV_LENGTH = 16;

        private Aes aes;
        private FileStream fileStream;
        private CryptoStream cryptoStream;
        private bool aesInitialized;

        private ulong contentLength;
        private ulong downloadedLength;

        private string path;
        private byte[] key;
        private byte[] IV;
        private int currentIVLength;
        public DecryptDownloadHandler(byte[] buffer, byte[] key, string path) : base(buffer)
        {
            this.path = path;
            this.key = key;
            contentLength = 0;
            downloadedLength = 0;
            currentIVLength = 0;
            IV = new byte[IV_LENGTH];
            aesInitialized = false;
        }

        protected override float GetProgress()
        {
            if (contentLength != 0)
            {
                return (float)downloadedLength / contentLength;
            }
            return 1;
        }

        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            this.contentLength = contentLength;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            int offset = 0;
            if (currentIVLength < 16)
            {
                int addedIVLength = Math.Min(IV_LENGTH - currentIVLength, dataLength);
                Buffer.BlockCopy(data, 0, IV, currentIVLength, addedIVLength);
                currentIVLength += addedIVLength;
                offset = addedIVLength;
            }

            if (currentIVLength > 16)
            {
                Debug.LogError("Error receiving IV");
                return false;
            }

            if (currentIVLength == 16)
            {
                if (!aesInitialized)
                {
                    aes = Aes.Create();
                    aes.Key = key;
                    aes.IV = IV;
                    aes.Mode = CipherMode.CBC;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    fileStream = File.Create(path);
                    cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Write);
                    aesInitialized = true;
                }

                cryptoStream.Write(data, offset, dataLength - offset);

            }
            return true;
        }

        protected override void CompleteContent()
        {
            if (aesInitialized)
            {
                cryptoStream.Dispose();
                fileStream.Dispose();
                aes.Dispose();
            }
        }
    }
}
