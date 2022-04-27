using UnityEngine;

public abstract class LocalizationEvent : MonoBehaviour
{
    void Start()
    {
        ApplyLocalization();
    }

    public abstract void ApplyLocalization();
}
