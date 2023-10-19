using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MergePostProcessPass : ScriptableRenderPass
{
    private Material m_mergeMat;
    private Mesh m_mesh;

    private float m_sizePixels;

    private RenderTexture m_camera_texture;
    private RenderTexture m_camera_pixelated_part_texture;
    private RenderTexture m_depthRenderTexture;
    private RenderTexture m_depthUnpixelatedRenderTexture;
    private RenderTextureDescriptor m_Descriptor;

    private bool should_pixelated;

    public MergePostProcessPass(Material depthMaterial, Mesh mesh, float pixelSize, RenderTexture depthRenderTexture, RenderTexture depthUnpixelatedRenderTexture, RenderTexture cam_tex, RenderTexture camera_pixelated_part_texture, Material merge_mat)
    {
        m_mergeMat = depthMaterial;
        m_mesh = mesh;
        m_sizePixels = pixelSize;

        m_depthRenderTexture = depthRenderTexture;
        m_depthUnpixelatedRenderTexture = depthUnpixelatedRenderTexture;
        m_camera_texture = cam_tex;
        m_camera_pixelated_part_texture = camera_pixelated_part_texture;

        m_mergeMat = merge_mat;

        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        Debug.Log("LA RENDER PASS MERGE S EXECUTE");
        CommandBuffer cmd = CommandBufferPool.Get(name: "MergePostProcessPass");

        Camera camera = renderingData.cameraData.camera;
        

        Debug.Log("nom cam pour le second render pass : " + camera.name);
        if (camera.name !=  "Main Camera") return;

        Vector3 scale = new Vector3(1, camera.aspect, 1);
        scale = 2f* new Vector3(1f,1f,1f);
        cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);

        if(m_camera_texture == null) Debug.Log("text vide rarw");

        m_mergeMat.SetTexture("_cam_tex", m_camera_texture);
        m_mergeMat.SetTexture("_cam_tex_pixelated_elements", m_camera_pixelated_part_texture);
        m_mergeMat.SetTexture("_depth_tex", m_depthUnpixelatedRenderTexture);
        m_mergeMat.SetTexture("_depth_tex_pixelated", m_depthRenderTexture);

        m_mergeMat.SetFloat("_sizePixels",m_sizePixels);


        if(m_mergeMat != null){
            cmd.DrawMesh(m_mesh,Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale), m_mergeMat);
            Debug.Log("ULTIME DRAW");

        }
        else {
            Debug.Log("c vide :(");
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
