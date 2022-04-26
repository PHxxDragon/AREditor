using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

namespace EAR.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset localizationDatabase;
        [SerializeField]
        private GameObject canvas;

        private static List<Dictionary<string, string>> database = new List<Dictionary<string, string>>();
        private static Dictionary<string, int> codeToIndex = new Dictionary<string, int>();

        private static string currentLocale;

        private bool loaded = false;
        void Awake()
        {
            string[] lines = localizationDatabase.text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string[] tokens = SplitCSVLine(lines[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                database.Add(new Dictionary<string, string>());
                codeToIndex[tokens[i]] = i - 1;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens1 = SplitCSVLine(lines[i]);
                if (tokens1.Length != tokens.Length)
                {
                    Debug.LogError("Error reading CSV file, incorrect number of token in line " + i);
                    break;
                }
                for (int j = 1; j < tokens1.Length; j++)
                {
                    database[j - 1][tokens1[0]] = tokens1[j];
                }
            }
            loaded = true;
        }

        public static string GetLocalizedText(string key)
        {
            if (currentLocale != null && 
                database[codeToIndex[currentLocale]] != null && 
                database[codeToIndex[currentLocale]].ContainsKey(key))
            {
                return database[codeToIndex[currentLocale]][key];
            } else
            {
                Debug.LogError("Cannot find key in database");
                return "";
            }
            
        }

        public void ApplyLocalization(string locale)
        {
            currentLocale = locale;
            StartCoroutine(ApplyLocalizationCoroutine(locale));
        }

        private IEnumerator ApplyLocalizationCoroutine(string locale)
        {
            int i = 0;
            while (i < 10)
            {
                if (loaded) {
                    LocalizationEvent[] localizationEvents = canvas.GetComponentsInChildren<LocalizationEvent>(true);
                    foreach (LocalizationEvent localizationEvent in localizationEvents)
                    {
                        localizationEvent.ApplyLocalization();
                    }
                    break;
                } else
                {
                    i++;
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        private string[] SplitCSVLine(string line)
        {
            int afterLastComma = 0;
            int numOfQuote = 0;
            List<string> result = new List<string>();
            for (int currentChar = 0; currentChar < line.Length; currentChar++)
            {
                if (line[currentChar] == ',' && (numOfQuote % 2 == 0))
                {
                    string token = line.Substring(afterLastComma, currentChar - afterLastComma);
                    if (token[0] == '"' && token[token.Length - 1] == '"')
                    {
                        token = token.Substring(1, token.Length - 2);
                    }
                    result.Add(token);
                    afterLastComma = currentChar + 1;
                }
                else if (line[currentChar] == '\"')
                {
                    numOfQuote++;
                }
            }
            result.Add(line.Substring(afterLastComma));
            return result.ToArray();
        }
    }

}
