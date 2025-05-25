Shader "Unlit/PixelOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Radius ("Radius", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _Radius;

            // Set this constant to match your pixel scaling
            #define PIXEL_SCALE 5

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float na = 0;
                float r = _Radius;

                float2 scaledTexelSize = _MainTex_TexelSize.xy * PIXEL_SCALE;

                for (int nx = -r; nx <= r; nx++)
                {
                    for (int ny = -r; ny <= r; ny++)
                    {
                        if (nx * nx + ny * ny <= r * r)
                        {
                            float2 offset = float2(scaledTexelSize.x * nx, scaledTexelSize.y * ny);
                            fixed4 nc = tex2D(_MainTex, i.uv + offset);
                            na += ceil(nc.a);
                        }
                    }
                }

                na = clamp(na, 0, 1);
                fixed4 c = tex2D(_MainTex, i.uv);
                na -= ceil(c.a);

                return lerp(c, _Color, na);
            }
            ENDCG
        }
    }
}