using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorFadeRenderPass : ScriptableRenderPass
{
    private Material colorFadeMat;
    private ColorFadeSettings colorFadeSettings = null;

    private RenderTargetIdentifier source;
    private int colorFadeTexID;

    public bool Setup(ScriptableRenderer renderer)
    {
        colorFadeSettings = VolumeManager.instance.stack.GetComponent<ColorFadeSettings>();

        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        if (colorFadeSettings != null && colorFadeSettings.IsActive())
        {
            colorFadeMat = new Material(Shader.Find("Shader Graphs/ColorFade"));
            return true;
        }

        return false;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        if (colorFadeSettings == null || !colorFadeSettings.IsActive())
        {
            return;
        }

        colorFadeTexID = Shader.PropertyToID("_BlurTex");
        cmd.GetTemporaryRT(colorFadeTexID, cameraTextureDescriptor);

        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (colorFadeSettings == null || !colorFadeSettings.IsActive())
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("Color Fade Post Process");

        //colorFadeMat.SetColor("_ColorToFadeTo", colorFadeSettings.fadeColor);
        colorFadeMat.SetFloat("_lerpT", colorFadeSettings.lerpValue.value);

        source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        
        cmd.Blit(source, colorFadeTexID, colorFadeMat, 0);
        cmd.Blit(colorFadeTexID, source, colorFadeMat, 1);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(colorFadeTexID);
        base.FrameCleanup(cmd);
    }
}
