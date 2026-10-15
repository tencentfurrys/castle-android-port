Shader "Legacy Shaders/Transparent/VertexLit" {
Properties {
_Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,0)
 _Emission ("Emissive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.1,1)) = 0.7
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" { }
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    Cull Off
    Lighting Off
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #include "UnityCG.cginc"
    sampler2D _MainTex;
    fixed4 _Color;
    fixed4 _Emission;
    #ifdef ETC1_EXTERNAL_ALPHA
        sampler2D _AlphaTex;
    #endif
    struct appdata_t
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };
    struct v2f
    {
        float4 vertex : SV_POSITION;
        fixed3 color : COLOR0;
        float2 texcoord : TEXCOORD0;
        fixed alpha : COLOR1;
    };
    v2f vert (appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
        OUT.texcoord = IN.texcoord;
        float3 n = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, IN.normal));
        float diff = max(dot(n, normalize(float3(0.5, 0.8, 0.6))), 0.0);
        OUT.color = (UNITY_LIGHTMODEL_AMBIENT.rgb * 2.0 + diff) * _Color.rgb * IN.color.rgb + _Emission.rgb;
        OUT.alpha = _Color.a * IN.color.a;
        return OUT;
    }
    fixed4 frag (v2f IN) : SV_Target
    {
        fixed4 c = tex2D (_MainTex, IN.texcoord);
        #ifdef ETC1_EXTERNAL_ALPHA
        c.a = tex2D (_AlphaTex, IN.texcoord).r;
        #endif
        c.rgb *= IN.color;
        c.a *= IN.alpha;
        return c;
    }
    ENDCG
}
}
