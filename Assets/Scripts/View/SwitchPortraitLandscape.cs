using UnityEngine;
using EAR;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class SwitchPortraitLandscape : MonoBehaviour
{
    [SerializeField]
    private RectTransform referencedRectTransform;
    [Space]
    [SerializeField]
    private bool copyLocalPosition;
    [SerializeField]
    private bool copyAnchoredPosition = true;
    [SerializeField]
    private bool copySizeDelta = true;
    [SerializeField]
    private bool copyAnchorMin = true;
    [SerializeField]
    private bool copyAnchorMax = true;
    [SerializeField]
    private bool copyPivot;
    [SerializeField]
    private bool copyLocalScale;
    [SerializeField]
    private bool copyLocalRotation; 
    [Space]
    [SerializeField]
    private float aspectThreshold = 0.9f;

    private RectTransformData originalRectTransformData;
    private bool isPortrait;
    private RectTransform thisRectTransform;

    private void Awake()
    {
        originalRectTransformData = RectTransformData.RectTransformToRectTransformData(referencedRectTransform);
        thisRectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        while(true)
        {
            float aspect = (float)Screen.width / Screen.height;
            if (aspect < aspectThreshold && !isPortrait)
            {
                isPortrait = true;
                if (copyAnchoredPosition) referencedRectTransform.anchoredPosition = thisRectTransform.anchoredPosition;
                if (copyLocalPosition) referencedRectTransform.localPosition = thisRectTransform.localPosition;
                if (copySizeDelta) referencedRectTransform.sizeDelta = thisRectTransform.sizeDelta;
                if (copyAnchorMin) referencedRectTransform.anchorMin = thisRectTransform.anchorMin;
                if (copyAnchorMax) referencedRectTransform.anchorMax = thisRectTransform.anchorMax;
                if (copyPivot) referencedRectTransform.pivot = thisRectTransform.pivot;
                if (copyLocalScale) referencedRectTransform.localScale = thisRectTransform.localScale;
                if (copyLocalRotation) referencedRectTransform.localRotation = thisRectTransform.localRotation;
            }
            else if (aspect >= aspectThreshold && isPortrait)
            {
                isPortrait = false;
                if (copyAnchoredPosition) referencedRectTransform.anchoredPosition = originalRectTransformData.anchoredPosition;
                if (copyLocalPosition) referencedRectTransform.localPosition = originalRectTransformData.localPosition;
                if (copySizeDelta) referencedRectTransform.sizeDelta = originalRectTransformData.sizeDelta;
                if (copyAnchorMin) referencedRectTransform.anchorMin = originalRectTransformData.anchorMin;
                if (copyAnchorMax) referencedRectTransform.anchorMax = originalRectTransformData.anchorMax;
                if (copyPivot) referencedRectTransform.pivot = originalRectTransformData.pivot;
                if (copyLocalScale) referencedRectTransform.localScale = originalRectTransformData.localScale;
                if (copyLocalRotation) referencedRectTransform.localRotation = originalRectTransformData.localRotation;
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
