using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{

    [SerializeField]
    public Shader m_depthShader;

    public Shader m_mergeShader;

    public RenderTexture m_camera_texture;
    public RenderTexture m_camera_pixel_part_texture;
    public RenderTexture m_depthRenderTexture;
    public RenderTexture m_depthUnpixelatedRenderTexture;
    public Mesh m_mesh;

    public float m_sizePixels;

    private CustomPostProcessPass m_customPass; // this one creates the pixelated depth texture
    private MergePostProcessPass m_customMergePass; // this one creates the pixelated depth texture

    private Material m_depthMaterial;
    private Material m_mergeMaterial;
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customPass);
        renderer.EnqueuePass(m_customMergePass);
    }

    public override void Create()
    {
        m_depthMaterial = CoreUtils.CreateEngineMaterial(m_depthShader);
        m_mergeMaterial = CoreUtils.CreateEngineMaterial(m_mergeShader);

        m_customPass = new CustomPostProcessPass(m_depthMaterial, m_mesh,m_sizePixels);
        m_customMergePass = new MergePostProcessPass(m_mergeMaterial, m_mesh,m_sizePixels,m_depthRenderTexture,m_depthUnpixelatedRenderTexture,m_camera_texture, m_camera_pixel_part_texture, m_mergeMaterial);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_depthMaterial);
        CoreUtils.Destroy(m_mergeMaterial);
    }

}
