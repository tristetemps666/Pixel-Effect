using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessPass : ScriptableRenderPass
{
    private Material m_depthMat;
    private Mesh m_mesh;

    private float m_sizePixels;

    private RenderTexture depthUnpixelatedRenderTexture;
    private RenderTextureDescriptor m_Descriptor;

    private bool should_pixelated;

    public CustomPostProcessPass(Material depthMaterial, Mesh mesh, float pixelSize)
    {
        m_depthMat = depthMaterial;
        m_mesh = mesh;
        m_sizePixels = pixelSize;
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        Debug.Log("LA RENDER PASS S EXECUTE");
        CommandBuffer cmd = CommandBufferPool.Get(name: "CustomPostProcessPass");

        Camera camera = renderingData.cameraData.camera;
        if(camera.name.Contains("Main Camera") || camera.name.Contains("Merge Camera") || camera.name.Contains("SceneCamera")) return;
        // renderingData.cameraData.camera.cullingMask = ~ (1 << 11);
        
        float should_pix = camera.name.Contains("Depth unpixelated Camera")? 0f : 1f;
        Debug.Log(camera.name + "// should pix ? " +should_pix);

        Vector3 scale = new Vector3(1, camera.aspect, 1);
        scale = 2f* new Vector3(1f,1f,1f);
        cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
        m_depthMat.SetFloat("_aspectRatio", 1f);
        m_depthMat.SetFloat("_sizePixels",m_sizePixels);
        m_depthMat.SetFloat("_should_pix",should_pix);


        if(m_depthMat != null){
            cmd.DrawMesh(m_mesh,Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale), m_depthMat);
        }
        context.ExecuteCommandBuffer(cmd);

        // renderingData.cameraData.camera.activeTexture;

        CommandBufferPool.Release(cmd);


    }


    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_Descriptor = renderingData.cameraData.cameraTargetDescriptor;
        
    }
}
