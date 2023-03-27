Shader "Custom/CloudShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1, 1, 1, 1)
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float4 _TintColor;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                float4 c = tex2D(_MainTex, IN.uv_MainTex) * _TintColor;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
