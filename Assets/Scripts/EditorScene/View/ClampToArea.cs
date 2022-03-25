using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ClampToArea : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentArea;

    private RectTransform thisRectTransform;

    private void Awake()
    {
        thisRectTransform = GetComponent<RectTransform>();
        if (!parentArea)
            parentArea = transform.root.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        Vector3 pos = thisRectTransform.position;
        Vector3 minRectInWorld = transform.TransformPoint(thisRectTransform.rect.min);
        Vector3 maxRectInWorld = transform.TransformPoint(thisRectTransform.rect.max);
        Vector3 minCanvasRectInWorld = parentArea.TransformPoint(parentArea.rect.min);
        Vector3 maxCanvasRectInWorld = parentArea.TransformPoint(parentArea.rect.max);

        Vector3 minPosition = pos + minCanvasRectInWorld - minRectInWorld;
        Vector3 maxPosition = pos + maxCanvasRectInWorld - maxRectInWorld;

        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);

        thisRectTransform.position = pos;
    }
}
