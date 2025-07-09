using UnityEngine.Rendering.Universal;

public class ColorFadeRenderFeature : ScriptableRendererFeature
{
    private ColorFadeRenderPass colorFadeRenderPass;

    public override void Create()
    {
        colorFadeRenderPass = new ColorFadeRenderPass();
        name = "ColorFade";
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (colorFadeRenderPass.Setup(renderer))
        {
            renderer.EnqueuePass(colorFadeRenderPass);
        }
    }
}
