using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("ColorFade")]
public class ColorFadeSettings : VolumeComponent, IPostProcessComponent
{
    public Color fadeColor;
    //public float lerpValue;
    public ClampedFloatParameter lerpValue = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);

    public bool IsActive()
    {
        return (lerpValue.value > 0.0f) && active;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
