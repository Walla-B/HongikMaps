Shader "Unlit/TestShader"
{
    Properties
    {
        // Adds controllable attribute in Inspector
        _MainTex ("Texture", 2D) = "white" {}
        //_Color ("Color_name", color) = (1,1,1,1)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;

            struct Meshdata
            {
                float4 vertex : POSITION;
                // float4 color : COLOR;
                float3 normals : NORMAL;
                float2 uv0 : TEXCOORD0;

            };

            struct Interpolators
            {
                //float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 norma1 : TEXCOORD0;
            };

            Interpolators vert (Meshdata v)
            {
                Interpolators o;
                o.normal = v.normals;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return float4(i.normal, 1);
            }
            ENDCG
        }
    }
}
