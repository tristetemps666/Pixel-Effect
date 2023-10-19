Shader "Hidden/DepthRender"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _transparent_tex ("Texture", 2D) = "white" {}
        _transparent_tex_pixelated ("Texture", 2D) = "white" {}
        _sizePixels("Size of the pixel effect", Range(10.0,100.0)) = 50.0
        _aspectRatio("camera aspect Ratio", Range(0.0,10.0)) = 1.0
        _should_pix("sould we pixelate ?", Range(0.0,10.0)) = 0.0
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

            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _transparent_tex;
            uniform sampler2D _transparent_tex_pixelated;

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

            sampler2D _MainTex;
            float _sizePixels;
            float _aspectRatio;
            float _should_pix;

            fixed4 frag (v2f i) : SV_Target
            {
                if(_should_pix == 1){
                    i.uv = floor(i.uv*_sizePixels)/_sizePixels;
                }

                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                col.rgb = 1.;
                float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
                col.rgb = depth;
                return col;
            }
            ENDCG
        }
    }
}
