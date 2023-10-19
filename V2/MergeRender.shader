Shader "Hidden/MergeRender"
{
    Properties
    {
        _cam_tex ("Texture de la cam", 2D) = "red" {}
        _cam_tex_pixelated_elements ("Texture des elemtns de la cam pixélisés", 2D) = "red" {}
        _depth_tex ("Texture profondeur nom pixélisée", 2D) = "white" {}
        _depth_tex_pixelated ("Texture profondeur pixélisée", 2D) = "white" {}
        _sizePixels("Size of the pixel effect", Range(10.0,100.0)) = 50.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { "Queue"="Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _depth_tex;
            uniform sampler2D _cam_tex;
            uniform sampler2D _cam_tex_pixelated_elements;
            uniform sampler2D _depth_tex_pixelated;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.vertex = v.vertex;
                o.uv = v.uv;
                return o;
            }

            float _sizePixels;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                float depth = tex2D(_depth_tex,i.uv);
                float depth_pixelated = tex2D(_depth_tex_pixelated,i.uv);

                // return (depth > depth_pixelated ) ? 1.0: 0.0;
                // return depth_pixelated;

                // return (depth*depth_pixelated > 0.0 ) ? 1.0: 0.0;


                // return tex2D(_depth_tex_pixelated,i.uv);

                
                if(depth < depth_pixelated){
                    i.uv = floor(i.uv*_sizePixels)/_sizePixels;
                    col = tex2D(_cam_tex_pixelated_elements,i.uv);
                }else{
                    col = tex2D(_cam_tex, i.uv);
                }
                // col *=0.;
                return col;
            }
            ENDCG
        }
    }
}
