using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    private Material blurMaterial;
    private Material vignetteMaterial;
    private BlurSettings blurSettings = null;

    private RenderTargetIdentifier source;
    private int blurTexID;

    public bool Setup(ScriptableRenderer renderer)
    {
        //Gets blurr settings from all volumes
        blurSettings = VolumeManager.instance.stack.GetComponent<BlurSettings>();

        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        if (blurSettings != null && blurSettings.IsActive())
        {
            blurMaterial = new Material(Shader.Find("PostProcessing/Blur"));
            vignetteMaterial = new Material(Shader.Find("Shader Graphs/ColorVignette"));
            return true;
        }

        return false;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        if (blurSettings == null || !blurSettings.IsActive())
        {
            return;
        }

        blurTexID = Shader.PropertyToID("_BlurTex");
        cmd.GetTemporaryRT(blurTexID, cameraTextureDescriptor);

        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (blurSettings == null || !blurSettings.IsActive())
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("Blur Post Process");

        //Set blur shader properties
        int gridSize = Mathf.CeilToInt(blurSettings.blurrStrength.value * 6.0f);

        if (gridSize % 2 == 0)
        {
            gridSize++;
        }

        blurMaterial.SetInteger("_GridSize", gridSize);
        blurMaterial.SetFloat("_Spread", blurSettings.blurrStrength.value);

        //Set vignette shader properties
        vignetteMaterial.SetFloat("_ColorStrength", blurSettings.colorStrength.value);
        vignetteMaterial.SetFloat("_VignetteExtent", blurSettings.vignetteSpread.value);
        vignetteMaterial.SetFloat("_VignetteStrength", blurSettings.vignetteStrength.value);

        source = renderingData.cameraData.renderer.cameraColorTargetHandle;

        //Execute blur effect with 2 passes
        cmd.Blit(source, blurTexID, blurMaterial, 0);
        cmd.Blit(blurTexID, source, blurMaterial, 1);

        //Execute vignette effect
        //cmd.Blit(source, blurTexID, vignetteMaterial, 0);
        cmd.Blit(blurTexID, source, vignetteMaterial, 0);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(blurTexID);
        base.FrameCleanup(cmd);
    }
}
