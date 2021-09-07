Shader "Unlit/TestShader"
{
    Properties
    {
        // Adds controllable attribute in Inspector
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("Color_name", Color) = (1,1,1,1)
        _ColorB ("Color_name", Color) = (1,1,1,1)
        // _Scale ("UV_Scale", Float) = 1
        // _Offset ("UV_Offset", FLoat) = 0
        
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

            float4 _ColorA;
            float4 _ColorB;
            float _Scale;
            float _Offset;

            struct Meshdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normals : NORMAL;
                float2 uv0 : TEXCOORD0;

            };

            struct Interpolators
            {
                //float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            Interpolators vert (Meshdata v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv0; //(v.uv0 + _Offset) * _Scale;
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                // lerp between two colors based on uv x coord
                float4 outColor = lerp( _ColorA, _ColorB, i.uv.x);
                return outColor;
            }
            ENDCG
        }
    }
}
