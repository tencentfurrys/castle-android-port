Shader "Standard" {
Properties {
_Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Albedo", 2D) = "white" { }
 _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
 _Glossiness ("Smoothness", Range(0,1)) = 0.5
[Gamma]  _Metallic ("Metallic", Range(0,1)) = 0
 _MetallicGlossMap ("Metallic", 2D) = "white" { }
 _BumpScale ("Scale", Float) = 1
 _BumpMap ("Normal Map", 2D) = "bump" { }
 _Parallax ("Height Scale", Range(0.005,0.08)) = 0.02
 _ParallaxMap ("Height Map", 2D) = "black" { }
 _OcclusionStrength ("Strength", Range(0,1)) = 1
 _OcclusionMap ("Occlusion", 2D) = "white" { }
 _EmissionColor ("Color", Color) = (0,0,0,1)
 _EmissionMap ("Emission", 2D) = "white" { }
 _DetailMask ("Detail Mask", 2D) = "white" { }
 _DetailAlbedoMap ("Detail Albedo x2", 2D) = "grey" { }
 _DetailNormalMapScale ("Scale", Float) = 1
 _DetailNormalMap ("Normal Map", 2D) = "bump" { }
[Enum(UV0,0,UV1,1)]  _UVSec ("UV Set for secondary textures", Float) = 0
[HideInInspector]  _Mode ("__mode", Float) = 0
[HideInInspector]  _SrcBlend ("__src", Float) = 1
[HideInInspector]  _DstBlend ("__dst", Float) = 0
[HideInInspector]  _ZWrite ("__zw", Float) = 1
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    Cull Back
    ZWrite On
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #include "UnityCG.cginc"
    sampler2D _MainTex;
    fixed4 _Color;
    #ifdef ETC1_EXTERNAL_ALPHA
        sampler2D _AlphaTex;
    #endif
    struct appdata_t
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float2 texcoord : TEXCOORD0;
    };
    struct v2f
    {
        float4 vertex : SV_POSITION;
        float3 normal : TEXCOORD0;
        float2 texcoord : TEXCOORD1;
    };
    v2f vert (appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
        OUT.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, IN.normal));
        OUT.texcoord = IN.texcoord;
        return OUT;
    }
    fixed4 frag (v2f IN) : SV_Target
    {
        fixed4 albedo = tex2D (_MainTex, IN.texcoord) * _Color;
        #ifdef ETC1_EXTERNAL_ALPHA
        albedo.a = tex2D (_AlphaTex, IN.texcoord).r;
        #endif
        float diff = max(dot(IN.normal, normalize(float3(0.4, 0.7, 0.6))), 0.0);
        fixed3 col = albedo.rgb * (UNITY_LIGHTMODEL_AMBIENT.rgb * 2.0 + diff);
        return fixed4(col, 1.0);
    }
    ENDCG
}
}
