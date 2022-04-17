using DG.Tweening;
using UnityEngine;

namespace EAR.View
{
    public class ShowAndHide : MonoBehaviour
    {
        [SerializeField]
        private bool hideAtFirst;

        private Vector3 originalScale;
        private void Awake()
        {
            if (transform.localScale != Vector3.zero)
            {
                originalScale = transform.localScale;
            } else
            {
                originalScale = Vector3.one;
            }
            if (hideAtFirst)
            {
                Hide();
            }
        }

        public void Show()
        {
            transform.DOScale(originalScale, 0f);
        }

        public void Hide()
        {
            transform.DOScale(Vector3.zero, 0f);
        }
    }
}

