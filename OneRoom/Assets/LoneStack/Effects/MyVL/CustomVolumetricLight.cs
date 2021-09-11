using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using LoneStack;

[System.Serializable]
public class CustomVolumetricLightSettings : LSEffectSettings
{
    public float distance = 10f;
    public int stepCount = 25;
    public float brightnessMultiplier = 1f;
    [ColorUsage(false, true)]
    public Color color;
}

[LSEffectPath("Custom/CustomVolumetricLight")]
[AlwaysIncludeShader("Hidden/VLightingShader")]
public class CustomVolumetricLight : LSEffect<CustomVolumetricLightSettings>
{
    Material mat;

    public override float Blend {
        set { mat.SetFloat(_Blend, value); }
    }

    protected override void Awake()
    {
        mat = new Material(Shader.Find("Hidden/VLightingShader"));
    }

    public override void EnqueueToBuffer(CommandBuffer cmdBuffer, LSContext context, ref RenderingData renderingData)
    {

        float wRatio = renderingData.cameraData.camera.scaledPixelWidth / (float)renderingData.cameraData.camera.scaledPixelHeight;
        float fovScale = Mathf.Tan(Mathf.Deg2Rad * renderingData.cameraData.camera.fieldOfView * .5f);

        Transform camT = renderingData.cameraData.camera.transform;

        cmdBuffer.Blit(context.source, context.destination);

        mat.SetVector("_CamPos", camT.position);
        mat.SetVector("_CamFwd", camT.forward);
        mat.SetVector("_CamUp", camT.up * fovScale);
        mat.SetVector("_CamRight", camT.right * fovScale * wRatio);

        mat.SetFloat("_Distance", settings.distance);
        mat.SetInt("_StepCount", settings.stepCount);
        mat.SetFloat("_BrightnessMultiplier", settings.brightnessMultiplier);

        mat.SetVector("_color", settings.color);

        // Executes once per render (there may be more or less than one render per frame)
        // Set your render-specific effect / shader parameters by accessing your effect's settings
        // and populate the command buffer
        cmdBuffer.Blit(context.source, context.destination, mat, 0);
    }

    public override void Update()
    {
        // Executes once per frame
        // useful for logic which is common to all renders but varies over time
    }
}
