using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CRTRenderer), PostProcessEvent.AfterStack, "Custom/CRT")]
public class CRT : PostProcessEffectSettings
{
    public TextureParameter LUT = new TextureParameter();
    public IntParameter scaleX = new IntParameter { value = 720 };
    public IntParameter scaleY = new IntParameter { value = 720 };
    public FloatParameter blend = new FloatParameter { value = 0.25f };
}

public class CRTRenderer : PostProcessEffectRenderer<CRT>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/CRT"));
        sheet.properties.SetTexture("_LUT", settings.LUT);
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetInt("_ScaleX", settings.scaleX);
        sheet.properties.SetInt("_ScaleY", settings.scaleY);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
