using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class NoteEditorWindow : MonoBehaviour
    {
        public event Action OnDeleteButtonClick;
        public event Action<string> OnTextInputFieldChanged;
        public event Action<string> OnButtonTitleInputFieldChanged;

        [SerializeField]
        private TMP_InputField textInputField;
        [SerializeField]
        private TMP_InputField buttonTitleInputField;
        [SerializeField]
        private Button deleteButton;

        public void CloseEditor()
        {
            gameObject.SetActive(false);
        }

        public void OpenEditor()
        {
            gameObject.SetActive(true);
        }

        public void SetTextInputField(string value)
        {
            textInputField.text = value;
        }

        public void SetButtonTitleInputField(string value)
        {
            buttonTitleInputField.text = value;
        }

        void Start()
        {
            deleteButton.onClick.AddListener(() =>
            {
                OnDeleteButtonClick?.Invoke();
            });
            textInputField.onValueChanged.AddListener((string value) =>
            {
                OnTextInputFieldChanged?.Invoke(value);
            });
            buttonTitleInputField.onValueChanged.AddListener((string value) =>
            {
                OnButtonTitleInputFieldChanged?.Invoke(value);
            });

            CloseEditor();
        }
    }
}
