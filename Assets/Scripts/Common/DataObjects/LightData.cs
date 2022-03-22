using System;
using UnityEngine;

[Serializable]
public class LightData
{
    public LightType lightType = LightType.Directional;
    public Color color = Color.black;
    public float intensity = 1f;
    public Vector3 direction = new Vector3(0, -1, 0);

    public LightData()
    {
    }

    public LightData(LightType lightType, Color color, float intensity, Vector3 direction)
    {
        this.lightType = lightType;
        this.color = color;
        this.intensity = intensity;
        this.direction = direction;
    }
}
