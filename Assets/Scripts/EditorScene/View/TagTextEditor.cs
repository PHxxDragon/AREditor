using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

namespace EAR.View
{
    public class TagTextEditor : MonoBehaviour
    {
        [SerializeField]
        private Button boldButton;

        [SerializeField]
        private Button italicButton;

        [SerializeField]
        private Button fontColorButton;
        [SerializeField]
        private ColorSelector colorSelector;

        [SerializeField]
        private Button fontSizeButton;
        [SerializeField]
        private TMP_InputField fontSize;

        [SerializeField]
        private TMP_InputField inputField;

        void Start()
        {
            boldButton.onClick.AddListener(() =>
            {
                inputField.text = AddTagToString(inputField.text, GetBeginBoldTag(), GetEndBoldTag(), inputField.caretPosition, inputField.selectionAnchorPosition);
            });
            italicButton.onClick.AddListener(() =>
            {
                inputField.text = AddTagToString(inputField.text, GetBeginItalicTag(), GetEndItalicTag(), inputField.caretPosition, inputField.selectionAnchorPosition);
            });
            fontColorButton.onClick.AddListener(() =>
            {
                inputField.text = AddTagToString(inputField.text, GetBeginColorTag(colorSelector.GetColor()), GetEndColorTag(), inputField.caretPosition, inputField.selectionAnchorPosition);
            });
            fontSizeButton.onClick.AddListener(() =>
            {
                try
                {
                    int fontSize = int.Parse(inputField.text);
                    inputField.text = AddTagToString(inputField.text, GetBeginSizeTag(fontSize), GetEndSizeTag(), inputField.caretPosition, inputField.selectionAnchorPosition);
                } catch (FormatException)
                {

                }
            });
        }

        private string AddTagToString(string text, string beginTag, string endTag, int pos1, int pos2)
        {
            if (pos1 > pos2)
            {
                (pos1, pos2) = (pos2, pos1);
            }
            string beginText = text.Substring(0, pos1);
            string midText = text.Substring(pos1, pos2 - pos1);
            string endText = text.Substring(pos2);
            return beginText + beginTag + midText + endTag + endText;
        }

        private string GetBeginBoldTag()
        {
            return "<b>";
        }

        private string GetEndBoldTag()
        {
            return "</b>";
        }

        private string GetBeginItalicTag()
        {
            return "<i>";
        }

        private string GetEndItalicTag()
        {
            return "</i>";
        }

        private string GetBeginColorTag(Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">";
        }

        private string GetEndColorTag()
        {
            return "</color>";
        }

        private string GetBeginSizeTag(int size)
        {
            return "<size=" + size + ">";
        }

        private string GetEndSizeTag()
        {
            return "</size>";
        }
    }
}

