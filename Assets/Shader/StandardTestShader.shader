// Shader "Category/Shader_name"
Shader "Custom/StandardTestShader"
{
    // All paremeters exposed in unity inspector
    Properties
    {
        // Var_name ("Name", data_type) = Default_value
        // Range(start, end) just constraints slider in "inspector", can go over/under this value.
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Saturation ("Saturation", Range(-5,5)) = 1
    }
    SubShader
    {
        // When to render object, how to render
        Tags { "RenderType"="Opaque" }
        LOD 200

        // CGPROGRMA ~ ENDCG : Actual shader code
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // All parameters defined in Properties defined again
        // half, float, fixed, int etc
        sampler2D _MainTex;
        half _Glossiness;  
        half _Metallic, _Saturation;
        fixed4 _Color;  //vector ->rgba

        struct Input
        {
            float2 uv_MainTex;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        // Program that runs for every pixel on the screen
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;
            float saturation = _Saturation;
            // Albedo comes from a texture tinted by color
            // Takes the pixel value of texture and multiply it with color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 c = tex2D (_MainTex, uv); // if not , "Color" in inspector won't affect anything.

            // Linear Interpolation, like in C#, lerp(start, end, value(0:start)(1:end) -> Can go over or under)
            // in this usage, (c.r+c.g+c.b)/3 is basically greyscale, c is original, and saturation as value
            o.Albedo = lerp((c.r+c.g+c.b)/3, c, saturation); 
            
            // o.Albedo = c.r; is Same as o.Albedo = float3(c.r, c.r, c.r);
            // o.Albedo = c.rrg / c.brg / c. ggb like these are valid
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
