using UnityEngine.UI;
using UnityEngine;
using TMPro;
using EAR.Localization;
using DG.Tweening;
using Nobi.UiRoundedCorners;

namespace EAR.Entity
{
    public class NoteEntity : BaseEntity
    {
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private RectTransform noteContainer;
        [SerializeField]
        private HorizontalLayoutGroup textHorizontalLayoutGroup;
        [SerializeField]
        private RectTransform imageBox;
        [SerializeField]
        private Image textBackground;
        [SerializeField]
        private ImageWithIndependentRoundedCorners imageCorners;
        [SerializeField]
        private Image borderBackground;
        [SerializeField]
        private RectTransform border;
        [SerializeField]
        private ImageWithIndependentRoundedCorners borderCorners;

        [SerializeField]
        private Canvas[] canvases;
        [SerializeField]
        private Camera eventCamera;

        private Vector3 originalScale;

        public static NoteEntity InstantNewEntity(NoteEntity notePrefab, NoteData noteData)
        {
            NoteEntity noteEntity = Instantiate(notePrefab);
            noteEntity.PopulateData(noteData);
            noteEntity.SetId(noteData.id);
            return noteEntity;
        }

        void Start()
        {
/*            button.onClick.AddListener(() =>
            {
                if (!isCompleted) return;
                
                if (noteContainer.transform.localScale != Vector3.zero)
                {
                    isCompleted = false;
                    originalScale = noteContainer.transform.localScale;
                    noteContainer.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuart).OnComplete(() =>
                    {
                        isCompleted = true;
                    });
                } else
                {
                    isCompleted = false;
                    noteContainer.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        isCompleted = true;
                    });
                }
                
            });
            if (eventCamera == null)
            {
                eventCamera = Camera.main;
                foreach (Canvas canvas in canvases)
                {
                    canvas.worldCamera = eventCamera;
                }
            }*/
        }

        public void InitNoteData()
        {
            text.text = LocalizationManager.GetLocalizedText("NoteFirstText");
        }

        public NoteData GetNoteData()
        {
            NoteData noteData = new NoteData();
            noteData.id = GetId();
            noteData.name = GetEntityName();
            noteData.noteContent = text.text;
            noteData.noteTransformData = TransformData.TransformToTransformData(transform);
            noteData.noteContentRectTransformData = RectTransformData.RectTransformToRectTransformData(noteContainer);
            if (noteData.noteContentRectTransformData.localScale == Vector3.zero)
            {
                noteData.noteContentRectTransformData.localScale = originalScale;
            }
            noteData.boxWidth = GetBoxWidth();

            noteData.fontSize = GetFontSize();
            noteData.textBackgroundColor = GetTextBackgroundColor();
            noteData.textBorderRadius = GetTextBorderRadius();
            noteData.textColor = GetTextColor();
            noteData.borderWidth = GetTextBorderWidth();
            noteData.borderColor = GetBorderColor();
            return noteData;
        }

        public void PopulateData(NoteData data)
        {
            TransformData.TransformDataToTransfrom(data.noteTransformData, transform);
            RectTransformData.RectTransformDataToRectTransform(data.noteContentRectTransformData, noteContainer);
            text.text = data.noteContent;
            SetBoxWidth(data.boxWidth);
            SetFontSize(data.fontSize);
            SetTextBackgroundColor(data.textBackgroundColor);
            SetTextBorderRadius(data.textBorderRadius);
            SetTextColor(data.textColor);
            SetBorderColor(data.borderColor);
            SetTextBorderWidth(data.borderWidth);
        }

        public void SetHeight(float height)
        {
            Vector3 localPosition = noteContainer.localPosition;
            localPosition.y = height;
            noteContainer.localPosition = localPosition;
        }

        public float GetHeight()
        {
            return noteContainer.localPosition.y;
        }

        public void SetBoxWidth(float boxWidth)
        {
            imageBox.sizeDelta = new Vector2(boxWidth, imageBox.sizeDelta.y);
        }

        public float GetBoxWidth()
        {
            return imageBox.sizeDelta.x;
        }

        public void SetFontSize(int fontSize)
        {
            text.fontSize = fontSize;
        }

        public int GetFontSize()
        {
            return (int) text.fontSize;
        }

        public void SetText(string value)
        {
            text.text = value;
        }

        public string GetText()
        {
            return text.text;
        }

        public void SetTextBackgroundColor(Color color)
        {
            textBackground.color = color;
        }

        public Color GetTextBackgroundColor()
        {
            return textBackground.color;
        }

        public void SetTextBorderRadius(Vector4 r)
        {
            borderCorners.r = r;
            borderCorners.Refresh();
            UpdateBackgroundCorners();
        }

        public Vector4 GetTextBorderRadius()
        {
            return borderCorners.r;
        }

        public void SetTextColor(Color color)
        {
            text.color = color;
        }

        public Color GetTextColor()
        {
            return text.color;
        }

        public void SetTextBorderWidth(Vector4 width)
        {
            border.offsetMax = new Vector2(width.x, width.y);
            border.offsetMin = new Vector2(-width.z, -width.w);
            UpdateBackgroundCorners();
        }

        public Vector4 GetTextBorderWidth()
        {
            return new Vector4(border.offsetMax.x, border.offsetMax.y, -border.offsetMin.x, -border.offsetMin.y);
        }

        public void SetBorderColor(Color color)
        {
            borderBackground.color = color;
        }

        public Color GetBorderColor()
        {
            return borderBackground.color;
        }

        private void UpdateBackgroundCorners()
        {
            int remainDeg = Mathf.Max((int) (borderCorners.r.x - border.offsetMax.x), 0);
            imageCorners.r = new Vector4(remainDeg, remainDeg, remainDeg, remainDeg);
            imageCorners.Refresh();
            int padding = (int) imageCorners.GetRealR().x / 2;
            textHorizontalLayoutGroup.padding = new RectOffset(padding, padding, 3, 3);
        }
    }
}

