using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class CameraTransparentDepth : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera cam_transparent;
    private Camera cam_transparent_pixelated;
    private Camera cam_depth_unpixelated;
    private Camera cam_depth_pixelated;
    private Camera cam_merge;
    private Camera cam_merge_pixel_part;
    public RenderTexture transparent_object_textures;
    public RenderTexture transparent_pixelated_object_textures;
    public RenderTexture depth_unpixelated_objects_texture;
    public RenderTexture depth_pixelated_objects_texture;
    public RenderTexture merge_objects_texture;
    public RenderTexture merge_objects_pixelated_texture;

    public Shader DepthShader;
    private void Start() {
        if (cam_transparent == null)
            cam_transparent = new GameObject("tranparent Depth Camera").AddComponent<Camera>();

        if (cam_transparent_pixelated == null)
            cam_transparent_pixelated = new GameObject("tranparent Depth Pixelated Camera").AddComponent<Camera>();

        if (cam_depth_unpixelated == null)
            cam_depth_unpixelated = new GameObject("Depth unpixelated Camera").AddComponent<Camera>();
    
        if (cam_depth_pixelated == null)
            cam_depth_pixelated = new GameObject("Depth pixelated Camera").AddComponent<Camera>();

        if (cam_merge == null)
            cam_merge = new GameObject("Merge Camera").AddComponent<Camera>();
            
        if (cam_merge_pixel_part == null)
            cam_merge_pixel_part = new GameObject("Merge Camera only pixel part").AddComponent<Camera>();


        cam_transparent.CopyFrom(Camera.main);
        cam_transparent_pixelated.CopyFrom(Camera.main);
        cam_depth_unpixelated.CopyFrom(Camera.main);
        cam_depth_pixelated.CopyFrom(Camera.main);
        cam_merge.CopyFrom(Camera.main);
        cam_merge_pixel_part.CopyFrom(Camera.main);


        cam_transparent.transform.SetParent(Camera.main.transform);
        cam_transparent_pixelated.transform.SetParent(Camera.main.transform);
        cam_depth_unpixelated.transform.SetParent(Camera.main.transform);
        cam_depth_pixelated.transform.SetParent(Camera.main.transform);
        cam_merge.transform.SetParent(Camera.main.transform);
        cam_merge_pixel_part.transform.SetParent(Camera.main.transform);


        cam_transparent.depthTextureMode = DepthTextureMode.Depth; // Active la capture de la profondeur


    }

    // Update is called once per frame
    void Update()
    {
        cam_transparent.cullingMask = 1<<10;
        cam_transparent.targetTexture = transparent_object_textures;

        cam_transparent_pixelated.cullingMask = 1<<9;
        cam_transparent_pixelated.targetTexture = transparent_pixelated_object_textures;


        cam_depth_unpixelated.cullingMask = 1<<11;
        cam_depth_unpixelated.targetTexture = depth_unpixelated_objects_texture;

        cam_depth_pixelated.cullingMask = ~(1<<11);
        cam_depth_pixelated.targetTexture = depth_pixelated_objects_texture;

        
        // cam_depth_pixelated.cullingMask = ~(1<<11);
        cam_merge.targetTexture = merge_objects_texture;

        cam_merge_pixel_part.cullingMask = ~(1<<11);
        cam_merge_pixel_part.targetTexture = merge_objects_pixelated_texture;
        // cam_depth_unpixelated.RenderWithShader(DepthShader,"Opaque");

    }
}
