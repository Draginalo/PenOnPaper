using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Blur")]
public class BlurSettings : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter blurrStrength = new ClampedFloatParameter(0.0f, 0.0f, 15.0f);
    public ClampedFloatParameter colorStrength = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter vignetteSpread = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter vignetteStrength = new ClampedFloatParameter(0.0f, 0.0f, 100.0f);

    public void SetBlurStrength(float strength)
    {
        blurrStrength = new ClampedFloatParameter(strength, blurrStrength.min, blurrStrength.max);
    }

    public void SetColorStrength(float strength)
    {
        colorStrength = new ClampedFloatParameter(strength, colorStrength.min, colorStrength.max);
    }

    public void SetVignetteStrength(float strength)
    {
        vignetteStrength = new ClampedFloatParameter(strength, vignetteStrength.min, vignetteStrength.max);
    }

    public void SetVignetteSpread(float strength)
    {
        vignetteSpread = new ClampedFloatParameter(strength, vignetteSpread.min, vignetteSpread.max);
    }

    public bool IsActive()
    {
        return (blurrStrength.value > 0.0f) && active;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
